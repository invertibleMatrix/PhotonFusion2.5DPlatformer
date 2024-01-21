using Fusion;
using UnityEngine;
using Views;

namespace _Runtime.Gameplay.Player
{
    public partial class PlayerController
    {
        [Networked] public TickTimer RespawnTimer { get; private set; }

        private void CheckIfRespawning()
        {
            if (PlayerIsAlive == false)
            {
                if (RespawnTimer.Expired(Runner))
                {
                    RespawnPlayer();
                }
            }
        }

        private void RespawnPlayer()
        {
            transform.position = Vector3.zero;
            _healthBehaviour.ResetPlayerToBeRespawned();
            _rb.isKinematic = false;
            PlayerIsAlive   = true;
            OnAnyPlayerControllerSpawned?.Invoke(this);
            if (IsLocalPlayer())
            {
                ShowStartToast();
            }
        }

        private void ShowStartToast()
        {
            GenericFloaterBuilder.Show("Start!");
        }
    }
}