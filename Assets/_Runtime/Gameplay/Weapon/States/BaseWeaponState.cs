using System;
using System.Collections;
using _Runtime.Gameplay.Player;
using AK.RTStateMachine;
using Fusion;
using UnityEngine;

namespace _Runtime.Gameplay.Weapon.States
{
    [Serializable]
    public class BaseWeaponState : RuntimeState<WeaponController>
    {
        protected WeaponType WeaponType;

        public BaseWeaponState(WeaponController mediator) : base(mediator)
        {
        }


        public virtual void FireBullet(NetworkPrefabRef bulletPrefab)
        {
        }
    }

    public enum WeaponType
    {
        NONE,
        NORMAL,
        HEAVY
    }
}