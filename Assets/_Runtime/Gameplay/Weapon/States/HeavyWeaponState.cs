using System;
using System.Collections;
using _Runtime.Gameplay.Player;
using Fusion;
using UnityEngine;

namespace _Runtime.Gameplay.Weapon.States
{
    public class HeavyWeaponState : BaseWeaponState
    {
        private HeavyWeaponSpecs _specs;

        public HeavyWeaponState(WeaponController mediator, HeavyWeaponSpecs specs) : base(mediator)
        {
            WeaponType = WeaponType.HEAVY;
            _specs     = specs;
        }

        public override IEnumerator Start()
        {
            _specs.RootWeaponObject.gameObject.SetActive(true);
            Mediator.SetWeaponType(WeaponType.HEAVY);
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
            Mediator.Runner.Spawn(bulletPrefab, _specs.FirePoint1.position, _specs.FirePoint1.rotation, Mediator.Object.InputAuthority, OnBeforeSpawned);
            Mediator.Runner.Spawn(bulletPrefab, _specs.FirePoint2.position, _specs.FirePoint2.rotation, Mediator.Object.InputAuthority, OnBeforeSpawned);
        }

        private void OnBeforeSpawned(NetworkRunner runner, NetworkObject obj)
        {
            obj.GetComponent<Bullet>().ResetDataBeforeSpawn();
        }
    }


    [Serializable]
    public class HeavyWeaponSpecs : WeaponSpecs
    {
        public Transform FirePoint1;
        public Transform FirePoint2;
    }
}