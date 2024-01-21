using System;
using System.Collections.Generic;
using _Runtime.Gameplay.Player;
using Cinemachine;
using Fusion;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Runtime.Gameplay.Camera
{
    public class GameplayCamera : MonoBehaviour
    {
        [SerializeField] private ServicesProvider         _servicesProvider;
        [SerializeField] private CinemachineTargetGroup   _cinemachineTargetGroup;
        [SerializeField] private CinemachineImpulseSource _cinemachineImpulseSource;
        [SerializeField] private CinemachineVirtualCamera _cam;

        private PlayerSpawningHandler _playerSpawningHandler => _servicesProvider.PlayerSpawningHandler;

        
        
        private void Start()
        {
            _servicesProvider.GameplayCamera = this;
            SetCameraTargetsForExistingPlayers();
        }

        private void SetCameraTargetsForExistingPlayers()
        {
            if (_playerSpawningHandler.CurrentSpawnedPlayers?.Count > 0)
            {
                foreach (var item in _playerSpawningHandler.CurrentSpawnedPlayers)
                {
                    AddNetworkObjectToDefaultGroup(item.Value);
                }
            }
        }

        private void OnEnable()
        {
            RegisterEvents();
        }

        private void OnDisable()
        {
            UnregisterEvents();
        }

        private void OnPlayerLeft(PlayerController playerController)
        {
            _cinemachineTargetGroup.RemoveMember(playerController.transform);
        }

        private void OnPlayerJoined(PlayerController playerController)
        {
            _cinemachineTargetGroup.AddMember(playerController.transform, 1, 10f);
        }


        private void AddNetworkObjectToDefaultGroup(NetworkObject networkObject)
        {
            _cinemachineTargetGroup.AddMember(networkObject.transform, 1, 10f);
        }

        private void RegisterEvents()
        {
            PlayerController.OnAnyPlayerControllerSpawned   += OnPlayerJoined;
            PlayerController.OnAnyPlayerControllerDeSpawned += OnPlayerLeft;
            PlayerController.OnAnyPlayerDied                += OnPlayerLeft;
        }

        private void UnregisterEvents()
        {
            PlayerController.OnAnyPlayerControllerSpawned   -= OnPlayerJoined;
            PlayerController.OnAnyPlayerControllerDeSpawned -= OnPlayerLeft;
            PlayerController.OnAnyPlayerDied                -= OnPlayerLeft;
        }

        public void ShakeCamera(Vector2 force)
        {
            _cinemachineImpulseSource.GenerateImpulse(force);
        }
    }
}