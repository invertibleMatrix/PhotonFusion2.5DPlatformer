using Fusion;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Runtime.Gameplay.Player
{
    public partial class PlayerController
    {
        [SerializeField] private float _moveSpeed = 20f;
        [SerializeField] private float _jumpForce = 3000;

        [Networked] private NetworkBool _IsGrounded { get; set; }

        private Rigidbody _rb;
        private float     _horizontal;

        private void CheckIfGrounded()
        {
            Vector3 boxSize     = new Vector3(1, 2f, 1);
            Vector3 boxPosition = transform.position;

            int layerMask = LayerMask.GetMask("Ground");

            Collider[] results = new Collider[1];

            int numColliders = Runner.GetPhysicsScene().OverlapBox(boxPosition, boxSize, results, Quaternion.identity, layerMask);

            _IsGrounded = numColliders > 0;
        }

        private void CheckJumpInput(PlayerInputNetworkData input)
        {
            var pressed = input.NetworkButtons.GetPressed(_buttonsPrevious);
            if (pressed.WasPressed(_buttonsPrevious, ButtonType.JUMP) && _IsGrounded)
            {
                _rb.AddForce(Vector2.up * _jumpForce);
            }
        }

        private void Move(PlayerInputNetworkData data)
        {
            _rb.velocity = new Vector2(data.HorizontalInput * _moveSpeed, _rb.velocity.y);
        }
    }
}