using System.Collections;
using System.Collections.Generic;
using AK.StateMachine;
using UnityEngine;

[CreateAssetMenu(fileName = "ShooterGameState", menuName = "State Machine/States/ShooterGameState")]
public class ShooterGameState : GameStateBase
{
    [SerializeField] protected ServicesProvider ServicesProvider;

    protected PlayerSpawningHandler PlayerSpawningHandler => ServicesProvider.PlayerSpawningHandler;

    public override IEnumerator Init(IState listener)
    {
        yield return base.Init(listener);
    }

    public override IEnumerator Execute()
    {
        yield return base.Execute();
        PlayerSpawningHandler.SpawnPlayers();
    }

    public override IEnumerator Exit()
    {
        yield return base.Exit();
    }

    public override IEnumerator Tick()
    {
        yield return base.Tick();
    }
}