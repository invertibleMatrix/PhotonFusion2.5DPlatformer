using System.Collections;
using System.Collections.Generic;
using _Runtime.Gameplay.Camera;
using _Runtime.Gameplay.Player;
using Fusion;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "ServicesProvider", menuName = "Gameplay/ServicesProvider")]
public class ServicesProvider : ScriptableObject
{
    [SerializeField] private NetworkRunner     _networkRunnerPrefab;
    [SerializeField] private GameObject        _networkObjectsPrefab;
    [SerializeField] private NetworkEventRelay _networkEventRelay;

    private NetworkRunner         _networkRunner;
    private PlayerSpawningHandler _playerSpawningHandler;

    public NetworkRunner         NetworkRunner         => _networkRunner;
    public NetworkEventRelay     NetworkEventRelay     => _networkEventRelay;
    public PlayerSpawningHandler PlayerSpawningHandler => _playerSpawningHandler;

    public GameplayCamera GameplayCamera;

    public void InitServices()
    {
        _networkRunner = Instantiate(_networkRunnerPrefab);
        GameObject networkGO = Instantiate(_networkObjectsPrefab);
        _playerSpawningHandler = networkGO.GetComponentInChildren<PlayerSpawningHandler>();
        _networkRunner.AddCallbacks(_networkEventRelay);
        _playerSpawningHandler.Runner = NetworkRunner;
    }

    public void ResetRunner()
    {
        if (_networkRunner)
        {
            Destroy(_networkRunner);
        }

        _networkRunner = Instantiate(_networkRunnerPrefab);
        _networkRunner.AddCallbacks(_networkEventRelay);
    }
}