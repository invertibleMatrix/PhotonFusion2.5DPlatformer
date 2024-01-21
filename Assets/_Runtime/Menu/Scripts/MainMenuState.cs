using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Runtime.Gameplay;
using AK.StateMachine;
using Fusion;
using UnityEngine;
using Views;

[CreateAssetMenu(fileName = "MainMenuState", menuName = "Views/States/Main Menu State")]
public class MainMenuState : UIViewState
{
    [SerializeField] protected Transition       RestartStateTransition;
    [SerializeField] private   ServicesProvider _servicesProvider;

    private NetworkRunner _networkRunner => _servicesProvider.NetworkRunner;
    private MainMenuView  _menuView      => _view as MainMenuView;

    public async void OnHostClicked(string playerName)
    {
        await StartGameMode(GameMode.Host);
    }

    public async void OnJoinedClicked(string playerName)
    {
        await StartGameMode(GameMode.Client);
    }

    private async Task StartGameMode(GameMode mode)
    {
        var startGameArgs = new StartGameArgs()
        {
            GameMode    = mode,
            PlayerCount = 4,
            SessionName = "room",
        };

        var result = await _networkRunner.StartGame(startGameArgs);
        if (result.Ok)
        {
            End();
            GenericFloaterBuilder.Show("Starting Game...");
        }
        else
        {
            if (result.ShutdownReason == ShutdownReason.ServerInRoom)
            {
                GenericFloaterBuilder.Show("Server Already Running, Join game instead!");
            }

            Debug.LogError($"Failed to Start: {result.ShutdownReason}");
            _servicesProvider.ResetRunner();
            _menuView.ResetView();
        }
    }
}