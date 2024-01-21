using System.Collections.Generic;
using _Runtime.Gameplay.Player;
using Fusion;
using UnityEngine;

namespace _Runtime.Gameplay.Weapon
{
    public class Bullet : NetworkBehaviour
    {
        [SerializeField] private LayerMask  _playerHitBoxLayerMask;
        [SerializeField] private int        _bulletDamage   = 10;
        [SerializeField] private float      _bulletLifeTime = 0.8f;
        [SerializeField] private float      _moveSpeed      = 95;
        [SerializeField] private GameObject _bulletVisual;

        [Networked(OnChanged = nameof(OnVisibleStateChanged))]
        private NetworkBool _isBulletGraphicVisible { get; set; }

        [Networked] private TickTimer   _currentLifetime { get; set; }
        [Networked] private NetworkBool _didHitSomething { get; set; }

        private          Vector3                 _directionToMoveTo;
        private          bool                    _creatorIsTheLocalPlayer;
        private          Collider                _thisCollider;
        private readonly List<LagCompensatedHit> _playerCompensatedHits = new();

        public void ResetDataBeforeSpawn()
        {
            _directionToMoveTo      = transform.up;
            _isBulletGraphicVisible = true;
            _didHitSomething        = false;
        }

        public override void Spawned()
        {
            if (_thisCollider == null)
            {
                _thisCollider = GetComponent<Collider>();
            }

            _currentLifetime = TickTimer.CreateFromSeconds(Runner, _bulletLifeTime);
        }

        private static void OnVisibleStateChanged(Changed<Bullet> changed)
        {
            var isCurrentlyVisible = changed.Behaviour._isBulletGraphicVisible;
            if (isCurrentlyVisible != changed.Behaviour._bulletVisual.activeSelf)
            {
                changed.Behaviour.SetVisualActiveState(isCurrentlyVisible);
            }
        }

        private void SetVisualActiveState(bool visible)
        {
            _bulletVisual.SetActive(visible);
        }

        public override void FixedUpdateNetwork()
        {
            if (!_didHitSomething)
            {
                CheckIfWeHitGround();
                CheckIfWeHitOtherPlayer();
            }

            if (_currentLifetime.ExpiredOrNotRunning(Runner) == false && !_didHitSomething)
            {
                transform.Translate(_directionToMoveTo * _moveSpeed * Runner.DeltaTime, Space.World);
            }

            if ((_currentLifetime.Expired(Runner) || _didHitSomething))
            {
                _isBulletGraphicVisible = false;

                Runner.Despawn(Object);
            }
        }

        private void CheckIfWeHitGround()
        {
            const string GROUND  = "Ground";
            Collider[]   results = new Collider[1];
            int numColliders = Runner.GetPhysicsScene()
                .OverlapBox(transform.position, _thisCollider.bounds.size * .9f, results, Quaternion.identity, LayerMask.GetMask(GROUND));
            _didHitSomething = numColliders > 0;
        }

        private void CheckIfWeHitOtherPlayer()
        {
            Runner.LagCompensation.OverlapBox(transform.position, _thisCollider.bounds.size * .9f, Quaternion.identity,
                Object.InputAuthority, _playerCompensatedHits, _playerHitBoxLayerMask);
            if (_playerCompensatedHits.Count <= 0) return;
            foreach (var item in _playerCompensatedHits)
            {
                if (item.Hitbox == null) continue;
                var playerThatIHit = item.Hitbox.GetComponentInParent<PlayerController>();
                bool didNotHitOurOwnPlayer = playerThatIHit.Object.InputAuthority.PlayerId !=
                                             Object.InputAuthority.PlayerId;

                if (!didNotHitOurOwnPlayer || !playerThatIHit.PlayerIsAlive)
                {
                    continue;
                }

                playerThatIHit.GetComponent<PlayerHealthBehaviour>().Rpc_DamagePlayer(_bulletDamage, Object.InputAuthority.PlayerId);
                _didHitSomething = true;
                break;
            }
        }
    }
}