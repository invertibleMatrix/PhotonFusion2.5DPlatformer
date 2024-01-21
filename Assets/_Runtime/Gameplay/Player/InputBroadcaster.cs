using System;
using Fusion;
using UnityEngine;

namespace _Runtime.Gameplay.Player
{
    public class InputBroadcaster : NetworkBehaviour
    {
        [SerializeField] private ServicesProvider _servicesProvider;

        private void OnEnable()
        {
            RegisterEvents();
        }

        private void OnDisable()
        {
            UnregisterEvents();
        }


        private void OnInput(NetworkRunner runner, NetworkInput input)
        {
        }

        private void RegisterEvents()
        {
            _servicesProvider.NetworkEventRelay.OnInputEvent += OnInput;
        }


        private void UnregisterEvents()
        {
            _servicesProvider.NetworkEventRelay.OnInputEvent -= OnInput;
        }
    }
}