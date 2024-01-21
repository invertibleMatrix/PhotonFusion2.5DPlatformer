using System.Collections;
using System.Collections.Generic;
using AK.StateMachine;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "BootState", menuName = "State Machine/States/BootState")]
public class BootState : State
{
    public  int            SceneIndex;
    private AsyncOperation _sceneLoadingOperation;

    public override IEnumerator Init(IState listener)
    {
        yield return base.Init(listener);
    }

    public override IEnumerator Execute()
    {
        yield return base.Execute();
        _sceneLoadingOperation = SceneManager.LoadSceneAsync(SceneIndex, LoadSceneMode.Additive);
    }

    public override IEnumerator Tick()
    {
        yield return base.Tick();
        while (!_sceneLoadingOperation.isDone)
        {
            yield return null;
        }

        Scene scene = SceneManager.GetSceneByBuildIndex(SceneIndex);
        SceneManager.SetActiveScene(scene);
        End();
    }

    public override IEnumerator Exit()
    {
        yield return base.Exit();
    }
}