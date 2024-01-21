using _Runtime.Gameplay.Weapon.States;
using Fusion;
using UnityEngine;

namespace _Runtime.Gameplay.Player
{
    public struct PlayerInputNetworkData : INetworkInput
    {
        public float          HorizontalInput;
        public Quaternion     PivotRotationAngle;
        public NetworkButtons NetworkButtons;
        public WeaponType     WeaponType;
    }
}