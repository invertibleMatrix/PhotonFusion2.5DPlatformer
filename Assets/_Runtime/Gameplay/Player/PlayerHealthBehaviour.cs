using System;
using _Runtime.Gameplay.Player;
using AK.Events;
using Fusion;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _Runtime.Gameplay.Player
{
    public class PlayerHealthBehaviour : NetworkBehaviour
    {
        [SerializeField] private ServicesProvider _servicesProvider;
        [SerializeField] private PlayerController _localPlayer;
        [SerializeField] private Image            _fillImage;
        private                  Collider         _coll;

        [Networked(OnChanged = nameof(PlayerHealthValueChanged))]
        private int currentHealthAmount { get; set; }


        [Networked] private int lastAttackerID { get; set; }

        private const int MAX_HEALTH_AMOUNT = 100;

        private void OnEnable()
        {
        }

        public override void Spawned()
        {
            _coll               = GetComponent<Collider>();
            currentHealthAmount = MAX_HEALTH_AMOUNT;
            lastAttackerID      = -1;
        }

        public override void FixedUpdateNetwork()
        {
            KillPlayerIfOutsideMap();
        }

        private void KillPlayerIfOutsideMap()
        {
            if (Object.HasInputAuthority)
            {
                int        levelBoundary = LayerMask.GetMask("LevelBoundary");
                Collider[] result        = new Collider[1];
                int        numColliders  = Runner.GetPhysicsScene().OverlapBox(transform.position, _coll.bounds.size * .9f, result, Quaternion.identity, levelBoundary);

                if (numColliders > 0 && _localPlayer.PlayerIsAlive)
                {
                    Rpc_DamagePlayer(MAX_HEALTH_AMOUNT, -1);
                }
            }
        }


        private static void PlayerHealthValueChanged(Changed<PlayerHealthBehaviour> changed)
        {
            // Debug.Log($"PlayerHealthValueChanged called, Changed : {changed.Behaviour.name}");
            //Latest data
            var currentHealth = (int)changed.Behaviour.currentHealthAmount;

            //Prev data
            changed.LoadOld();
            var oldHealth = (int)changed.Behaviour.currentHealthAmount;

            if (currentHealth != oldHealth)
            {
                changed.Behaviour.UpdateUIVisualsToCurrentArg(currentHealth);

                //If we are not respawning/spawning, do damage
                if (currentHealth != MAX_HEALTH_AMOUNT)
                {
                    changed.Behaviour.PlayerGotHit(currentHealth);
                }
            }
        }


        // RPC Only received by Host
        [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
        public void Rpc_DamagePlayer(int damage, int attackerID)
        {
            // Debug.Log($"Got Hit By {attackerID}");
            DamagePlayer(damage, attackerID);
        }

        private void DamagePlayer(int dmg, int attackerID)
        {
            this.currentHealthAmount -= dmg;
            lastAttackerID           =  attackerID;
        }


        private void UpdateUIVisualsToCurrentArg(int healthAmount)
        {
            if (healthAmount >= 0)
            {
                var num = (float)healthAmount / MAX_HEALTH_AMOUNT;
                _fillImage.fillAmount = num;
            }
        }

        private void PlayerGotHit(int healthAmount)
        {
            var isLocalPlayer = _localPlayer.IsLocalPlayer();
            if (isLocalPlayer && healthAmount > 0)
            {
            }

            if (healthAmount <= 0)
            {
                _localPlayer.PlayerDied();

                _servicesProvider.PlayerSpawningHandler.CurrentSpawnedPlayers.TryGetValue(lastAttackerID, out NetworkObject attackerObj);
                if (attackerObj != null)
                {
                    PlayerController.Rpc_PlayerDied(Runner, Object, attackerObj);
                    lastAttackerID = -1;
                }
            }

            _localPlayer.StatsListener.OnHealthUpdate(healthAmount);
        }


        public void ResetPlayerToBeRespawned()
        {
            currentHealthAmount = MAX_HEALTH_AMOUNT;
            lastAttackerID      = -1;
        }
    }
}