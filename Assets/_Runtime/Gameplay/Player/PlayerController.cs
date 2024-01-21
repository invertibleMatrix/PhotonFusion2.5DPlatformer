using System;
using _Runtime.Gameplay.UI;
using AK.Events;
using Fusion;
using UnityEngine;
using Views;

namespace _Runtime.Gameplay.Player
{
    public partial class PlayerController : NetworkBehaviour, IBeforeUpdate
    {
        [SerializeField] private ServicesProvider _servicesProvider;
        [SerializeField] private GameplayHUD      _gameplayHUD;

        public static event Action<PlayerController> OnAnyPlayerControllerSpawned;
        public static event Action<PlayerController> OnAnyPlayerControllerDeSpawned;
        public static event Action<PlayerController> OnAnyPlayerDied;

        private PlayerHealthBehaviour     _healthBehaviour;
        private WeaponController          _playerWeaponController;
        public  ILocalPlayerStatsListener StatsListener => _localPlayerStatsListener;
        private ILocalPlayerStatsListener _localPlayerStatsListener;
        private int                       _totalDeaths;
        private int                       _totalKills;
        private Collider                  _bodyCollider;

        [Networked] public NetworkBool PlayerIsAlive  { get; private set; }
        public             bool        AcceptAnyInput => Object.IsValid && PlayerIsAlive; // && PlayerIsAlive && !GameManager.MatchIsOver;

        private void OnEnable()
        {
            RegisterEvents();
        }

        private void OnDisable()
        {
            UnregisterEvents();
        }

        private void Awake()
        {
            GetComponents();
        }

        public override void Spawned()
        {
            InitVisuals();
            if (Runner.IsServer)
            {
                PlayerIsAlive = true;
            }

            OnAnyPlayerControllerSpawned?.Invoke(this);

            Invoke(nameof(SetKinematicFalse), 1f);
        }

        private void SetKinematicFalse()
        {
            _rb.isKinematic = false;
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            base.Despawned(runner, hasState);
            OnAnyPlayerControllerDeSpawned?.Invoke(this);
        }

        private void GetComponents()
        {
            _rb                     = GetComponent<Rigidbody>();
            _playerWeaponController = GetComponent<WeaponController>();
            _healthBehaviour        = GetComponent<PlayerHealthBehaviour>();
            _bodyCollider           = GetComponent<Collider>();
            _gameplayHUD            = GetComponent<GameplayHUD>();
            if (_gameplayHUD != null)
            {
                _localPlayerStatsListener = _gameplayHUD;
            }
        }


        public void BeforeUpdate()
        {
            if (Object.HasInputAuthority && AcceptAnyInput)
            {
                _horizontal = Input.GetAxisRaw("Horizontal");
            }
        }

        public override void FixedUpdateNetwork()
        {
            CheckForTimerActions();
            CheckForInput();
        }

        private void CheckForTimerActions()
        {
            CheckIfRespawning();
        }

        public void PlayerDied()
        {
            _totalDeaths++;
            _rb.isKinematic = true;
            PlayerIsAlive   = false;
            RespawnTimer    = TickTimer.CreateFromSeconds(Runner, 5);
            OnAnyPlayerDied?.Invoke(this);
            _localPlayerStatsListener.OnDeathsUpdate(_totalDeaths);

            if (IsLocalPlayer())
            {
                GenericFloaterBuilder.Show("Respawning...");
            }
        }


        [Rpc]
        public static void Rpc_PlayerDied(NetworkRunner runner, NetworkObject deadPlayer, NetworkObject killer)
        {
            var killerController = killer.GetComponent<PlayerController>();
            killerController.UpdateTotalKills();
        }

        public void UpdateTotalKills()
        {
            _totalKills++;
            _localPlayerStatsListener.OnKillsUpdate(_totalKills);
        }


        public bool IsLocalPlayer()
        {
            return Runner.LocalPlayer == Object.HasInputAuthority;
        }

        private void RegisterEvents()
        {
        }


        private void UnregisterEvents()
        {
        }
    }
}

public enum ButtonType
{
    SHOOT,
    JUMP
}