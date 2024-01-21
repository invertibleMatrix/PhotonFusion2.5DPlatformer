using System.Collections;
using System.Collections.Generic;
using AK.StateMachine;
using UnityEngine;

[CreateAssetMenu(fileName = "AppStartState", menuName = "State Machine/States/AppStartState")]
public class AppStartState : State
{
    public override IEnumerator Init(IState listener)
    {
        yield return base.Init(listener);
    }

    public override IEnumerator Execute()
    {
        yield return base.Execute();
        End();
    }

    public override IEnumerator Tick()
    {
        yield return base.Tick();
    }
}