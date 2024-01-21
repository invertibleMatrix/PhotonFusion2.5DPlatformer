using _Runtime.Gameplay.Weapon.States;
using Fusion;
using UnityEngine;

namespace _Runtime.Gameplay.Player
{
    public partial class PlayerController
    {
        [Networked] private NetworkButtons _buttonsPrevious { get; set; }

        private void CheckForInput()
        {
            if (Runner.TryGetInputForPlayer<PlayerInputNetworkData>(Object.InputAuthority, out var input))
            {
                if (AcceptAnyInput)
                {
                    CheckIfGrounded();
                    CheckJumpInput(input);
                    Move(input);


                    _buttonsPrevious = input.NetworkButtons;
                }
                else
                {
                    _rb.velocity = Vector2.zero;
                }
            }
        }

        public PlayerInputNetworkData GetSetPlayerNetworkInput()
        {
            PlayerInputNetworkData data = new PlayerInputNetworkData();
            data.HorizontalInput    = _horizontal;
            data.PivotRotationAngle = _playerWeaponController.LocalPivotRotation;
            data.NetworkButtons.Set(ButtonType.SHOOT, Input.GetButton("Fire1"));
            data.NetworkButtons.Set(ButtonType.JUMP,  Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W));

            if (Input.GetKey(KeyCode.Alpha1))
            {
                data.WeaponType = WeaponType.NORMAL;
            }

            if (Input.GetKey(KeyCode.Alpha2))
            {
                data.WeaponType = WeaponType.HEAVY;
            }

            return data;
        }
    }
}