using System;
using _Runtime.Gameplay.Weapon;
using _Runtime.Gameplay.Weapon.States;
using AK.RTStateMachine;
using Fusion;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Runtime.Gameplay.Player
{
    public class WeaponController : NetworkBehaviour, IBeforeUpdate
    {
        [SerializeField] private ServicesProvider _servicesProvider;

        [SerializeField] private Vector2           _shootingCamShake  = new(-0.3f, 0f);
        [SerializeField] private float             _delayBetweenShots = 0.18f;
        [SerializeField] private HeavyWeaponSpecs  _heavyWeaponSpecs;
        [SerializeField] private NormalWeaponSpecs _normalWeaponSpecs;
        [SerializeField] private NetworkPrefabRef  _bulletPrefab = NetworkPrefabRef.Empty;
        [SerializeField] private Transform         _pivotToRotate;
        [SerializeField] private PlayerController  _playerController;

        [Networked] private TickTimer      _shootCooldown        { get; set; }
        [Networked] private NetworkButtons _buttonsPrevious      { get; set; }
        [Networked] private Quaternion     _currentPivotRotation { get; set; }
        [Networked] public  NetworkBool    IsHoldingShootingKey  { get; private set; }

        private RuntimeStateMachine<BaseWeaponState, WeaponController> _weaponStateMachine;

        private BaseWeaponState _normalWeaponState;
        private BaseWeaponState _heavyWeaponState;
        private WeaponType      _selectedWeapon = WeaponType.NORMAL;

        public Quaternion LocalPivotRotation { get; private set; }

        private void Awake()
        {
            _weaponStateMachine = new RuntimeStateMachine<BaseWeaponState, WeaponController>();
            _normalWeaponState  = new NormalWeaponState(this, _normalWeaponSpecs);
            _heavyWeaponState   = new HeavyWeaponState(this, _heavyWeaponSpecs);
            _weaponStateMachine.StartStateMachine(_normalWeaponState, this);
        }

        public override void Spawned()
        {
#if UNITY_EDITOR
            //Dev usage in editor
            var o = this.gameObject;
            o.name = $"{o.name} ID:{Object.InputAuthority.PlayerId} IsLocal:{IsLocalPlayer()}";
#endif
        }

        public void BeforeUpdate()
        {
            if (IsLocalPlayer())
            {
                Ray     camRay   = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector3 planePos = camRay.GetPoint(Vector3.Distance(transform.position, UnityEngine.Camera.main.transform.position));

                Vector3 currentMousePosDirection = planePos - transform.position;
                currentMousePosDirection.z = 0;

                var angle = Mathf.Atan2(currentMousePosDirection.y - _pivotToRotate.localPosition.y, currentMousePosDirection.x) * Mathf.Rad2Deg;
                LocalPivotRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }

        public override void FixedUpdateNetwork()
        {
            if (Runner.TryGetInputForPlayer<PlayerInputNetworkData>(Object.InputAuthority, out var input))
            {
                if (_playerController.AcceptAnyInput)
                {
                    CheckFireInput(input);
                    CheckForWeaponSelectionInput(input);
                    _currentPivotRotation = input.PivotRotationAngle;
                    _buttonsPrevious      = input.NetworkButtons;
                }
                else
                {
                    _buttonsPrevious = default;
                }
            }

            _pivotToRotate.transform.rotation = _currentPivotRotation;
        }

        private void CheckFireInput(PlayerInputNetworkData inputData)
        {
            var pressed = inputData.NetworkButtons.GetPressed(_buttonsPrevious);

            IsHoldingShootingKey = pressed.WasReleased(_buttonsPrevious, ButtonType.SHOOT);

            if (pressed.WasReleased(_buttonsPrevious, ButtonType.SHOOT) && _shootCooldown.ExpiredOrNotRunning(Runner) == true)
            {
                if (IsLocalPlayer())
                {
                    // playerCamera.ShakeCamera(shootingCamShake);
                }

                _shootCooldown = TickTimer.CreateFromSeconds(Runner, _delayBetweenShots);
                _weaponStateMachine._currentState.FireBullet(_bulletPrefab);
            }
        }

        private void CheckForWeaponSelectionInput(PlayerInputNetworkData inputData)
        {
            if (_selectedWeapon != inputData.WeaponType && inputData.WeaponType != WeaponType.NONE)
            {
                switch (inputData.WeaponType)
                {
                    case WeaponType.NORMAL:
                        _weaponStateMachine.Transition(new RuntimeTransition<BaseWeaponState>(_normalWeaponState));
                        TryBroadcastWeaponChange(WeaponType.NORMAL);
                        break;
                    case WeaponType.HEAVY:
                        _weaponStateMachine.Transition(new RuntimeTransition<BaseWeaponState>(_heavyWeaponState));
                        TryBroadcastWeaponChange(WeaponType.HEAVY);
                        break;
                }
            }
        }

        private void TryBroadcastWeaponChange(WeaponType weaponType)
        {
            _servicesProvider.PlayerSpawningHandler.CurrentSpawnedPlayers.TryGetValue(Object.InputAuthority, out NetworkObject obj);
            if (obj != null)
            {
                Rpc_WeaponChanged(Runner, obj, weaponType);
            }
        }


        [Rpc]
        private static void Rpc_WeaponChanged(NetworkRunner runner, NetworkObject networkObject, WeaponType weaponType)
        {
            networkObject.GetComponent<WeaponController>().UpdateWeaponVisuals(weaponType);
        }

        public void SetWeaponType(WeaponType weaponType)
        {
            _selectedWeapon = weaponType;
            _playerController.StatsListener.OnWeaponChange((int)weaponType);
        }

        private void UpdateWeaponVisuals(WeaponType weaponType)
        {
            switch (weaponType)
            {
                case WeaponType.NORMAL:
                    _normalWeaponSpecs.RootWeaponObject.gameObject.SetActive(true);
                    _heavyWeaponSpecs.RootWeaponObject.gameObject.SetActive(false);
                    break;
                case WeaponType.HEAVY:
                    _normalWeaponSpecs.RootWeaponObject.gameObject.SetActive(false);
                    _heavyWeaponSpecs.RootWeaponObject.gameObject.SetActive(true);
                    break;
            }

            _selectedWeapon = weaponType;
        }

        private bool IsLocalPlayer()
        {
            return Runner.LocalPlayer == Object.HasInputAuthority;
        }
    }
}