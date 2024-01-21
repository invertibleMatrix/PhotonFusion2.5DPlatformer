using System;
using System.Collections;
using _Runtime.Gameplay.Player;
using Fusion;
using UnityEngine;

namespace _Runtime.Gameplay.Weapon.States
{
    public class NormalWeaponState : BaseWeaponState
    {
        private NormalWeaponSpecs _specs;

        public NormalWeaponState(WeaponController mediator, NormalWeaponSpecs specs) : base(mediator)
        {
            WeaponType = WeaponType.NORMAL;
            _specs     = specs;
        }

        public override IEnumerator Start()
        {
            _specs.RootWeaponObject.gameObject.SetActive(true);
            Mediator.SetWeaponType(WeaponType.NORMAL);
            yield break;
        }

        public override IEnumerator Exit()
        {
            _specs.RootWeaponObject.gameObject.SetActive(false);
            yield break;
        }

        public override void FireBullet(NetworkPrefabRef bulletPrefab)
        {
            base.FireBullet(bulletPrefab);
            Mediator.Runner.Spawn(bulletPrefab, _specs.FirePoint.position, _specs.FirePoint.rotation, Mediator.Object.InputAuthority, OnBeforeSpawned);
        }

        private void OnBeforeSpawned(NetworkRunner runner, NetworkObject obj)
        {
            obj.GetComponent<Bullet>().ResetDataBeforeSpawn();
        }
    }

    [Serializable]
    public class NormalWeaponSpecs : WeaponSpecs
    {
        public Transform FirePoint;
    }
}