using System;
using System.Collections;
using AK.Events;
using UnityEngine;

namespace AK.StateMachine
{
    [CreateAssetMenu(fileName = "GameStateBase", menuName = "State Machine/States/Game State Base")]
    public class GameStateBase : State, IGameStateTransitioner
    {
        [SerializeField] protected Transition RestartStateTransition;

        [SerializeField] protected GameObject LevelPrefab;
        [NonSerialized]  protected GameObject _levelObject;


        bool ITransitioner.TransitionTo(Transition transition)
        {
            return _Listener.TransitionTo(this, transition);
        }

        Transition IGameStateTransitioner.RestartTransition()
        {
            return RestartStateTransition;
        }


        public virtual void RestartGameState()
        {
            _Listener.TransitionTo(this, RestartStateTransition);
        }


        public override IEnumerator Init(IState listener)
        {
            yield return base.Init(listener);
            yield return _Listener.CleanupAllPausedStates(this);

            if (LevelPrefab != null)
            {
                _levelObject = Instantiate(LevelPrefab);
            }
        }

        public override IEnumerator Execute()
        {
            yield return base.Execute();
            RegisterEvents();
        }

        public override IEnumerator Exit()
        {
            Reset();
            UnRegisterEvents();
            yield return base.Exit();
        }

        public override IEnumerator Pause(bool hideView)
        {
            UnRegisterEvents();
            yield return base.Pause(hideView);
        }

        public override IEnumerator Resume()
        {
            RegisterEvents();
            yield return base.Resume();
        }

        public override IEnumerator Cleanup()
        {
            Reset();
            UnRegisterEvents();
            return base.Cleanup();
        }

        protected virtual void Reset()
        {
            if (_levelObject != null)
            {
                Destroy(_levelObject);
                _levelObject = null;
            }
        }

        protected virtual IEnumerator Show()
        {
            yield break;
        }

        protected virtual IEnumerator Hide(bool shouldDestroy)
        {
            if (shouldDestroy)
            {
                Destroy(_levelObject);
            }

            yield break;
        }

        protected virtual void RegisterEvents()
        {
        }

        protected virtual void UnRegisterEvents()
        {
        }
    }
}