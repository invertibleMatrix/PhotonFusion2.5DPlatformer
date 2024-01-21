using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Runtime.Gameplay.Player;
using Fusion;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerSpawningHandler : SimulationBehaviour
{
    public event Action<PlayerRef, NetworkObject> OnPlayerJoinedEvent;
    public event Action<PlayerRef, NetworkObject> OnPlayerLeftEvent;

    public                   PlayerController               LocalPlayerController => GetLocalPlayer();
    [SerializeField] private NetworkPrefabRef               _playerNetworkPrefab = NetworkPrefabRef.Empty;
    [SerializeField] private ServicesProvider               _servicesProvider;
    public                   Dictionary<int, NetworkObject> CurrentSpawnedPlayers { get; private set; } = new Dictionary<int, NetworkObject>();

    private NetworkRunner _runner => _servicesProvider.NetworkRunner;

    private NetworkObject _localPlayerBehaviour;

    private void OnEnable()
    {
        _servicesProvider.NetworkEventRelay.OnPlayerJoinedEvent += OnPlayerJoined;
        _servicesProvider.NetworkEventRelay.OnPlayerLeftEvent   += OnPlayerLeft;
    }

    private void OnDisable()
    {
        _servicesProvider.NetworkEventRelay.OnPlayerJoinedEvent -= OnPlayerJoined;
        _servicesProvider.NetworkEventRelay.OnPlayerLeftEvent   -= OnPlayerLeft;
    }


    public void SpawnPlayers()
    {
        foreach (PlayerRef player in Runner.ActivePlayers)
        {
            SpawnPlayer(player);
        }
    }

    private void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        DespawnPlayer(player);
    }

    private void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        SpawnPlayer(player);
    }


    private void SpawnPlayer(PlayerRef player)
    {
        if (_runner.IsServer)
        {
            if (!CurrentSpawnedPlayers.ContainsKey(player.PlayerId))
            {
                Vector2 vec2 = Random.insideUnitCircle;
                // var spawnPosition = new Vector3(vec2.x * 10, vec2.y * 5, 0);
                var spawnPosition = Vector3.zero;
                var playerObject  = _runner.Spawn(_playerNetworkPrefab, spawnPosition, Quaternion.identity, player);
                _runner.SetPlayerObject(player, playerObject);
                CurrentSpawnedPlayers.Add(player.PlayerId, playerObject);
                OnPlayerJoinedEvent?.Invoke(player, playerObject);
            }
        }

        CacheLocalPlayerIfNull();
    }

    private void DespawnPlayer(PlayerRef player)
    {
        //Despawn a player and removes it from our dic
        if (CurrentSpawnedPlayers != null && CurrentSpawnedPlayers.ContainsKey(player.PlayerId))
        {
            CurrentSpawnedPlayers.TryGetValue(player.PlayerId, out var networkObject);
            CurrentSpawnedPlayers.Remove(player);
            OnPlayerLeftEvent?.Invoke(player, networkObject);
        }

        if (Runner.TryGetPlayerObject(player, out var playerNetworkObject))
        {
            Runner.Despawn(playerNetworkObject);
        }

        Runner.SetPlayerObject(player, null);
    }

    private void CacheLocalPlayerIfNull()
    {
        if (_localPlayerBehaviour == null)
        {
            _localPlayerBehaviour = CurrentSpawnedPlayers.Values.FirstOrDefault(x => x == x.Runner.LocalPlayer);
        }
    }

    private PlayerController GetLocalPlayer()
    {
        if (_localPlayerBehaviour != null)
        {
            return _localPlayerBehaviour.GetComponent<PlayerController>();
        }

        return null;
    }
}