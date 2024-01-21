using System;
using UnityEngine;
using System.Collections;

namespace AK.RTStateMachine
{
    public class RuntimeStateMachine<TBaseState, TMediator> where TBaseState : RuntimeState<TMediator>
    {
        public TBaseState _currentState;

        TBaseState                    _bootState;
        TBaseState                    _previousState;
        RuntimeTransition<TBaseState> _currentTransition;

        Coroutine _stateMachineRoutine;

        public virtual void StartStateMachine<TRunner>(TBaseState state, TRunner runner) where TRunner : MonoBehaviour
        {
            _bootState           = state;
            _stateMachineRoutine = runner.StartCoroutine(Tick());
        }

        public void Transition(RuntimeTransition<TBaseState> transition)
        {
            _currentTransition = transition;
        }


        protected void SetState(TBaseState state)
        {
            _previousState = _currentState;
            _currentState  = state;
        }

        private IEnumerator Tick()
        {
            while (true)
            {
                if (_currentState == null)
                {
                    _currentState = _bootState;
                    if (_currentState != null)
                    {
                        yield return _currentState.Start();
                    }
                }
                else
                {
                    if (_currentTransition != null && _currentTransition.ToState != null)
                    {
                        yield return _currentState.Exit();

                        yield return _currentTransition.Start();
                        SetState(_currentTransition.ToState);
                        _currentTransition = null;

                        yield return _currentState.Start();
                    }

                    yield return _currentState.Update();
                }

                yield return null;
            }
        }
    }
}