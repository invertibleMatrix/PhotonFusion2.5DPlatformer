using System;
using System.Collections;
using UnityEngine;

namespace AK.StateMachine
{
    public class State : ScriptableObject
    {
        [SerializeField] public Transition NextTransition;

        protected IState _Listener;

        //when the state is initialized
        public virtual IEnumerator Init(IState listener)
        {
            _Listener = listener;
            yield break;
        }

        //when the state starts execution
        public virtual IEnumerator Execute()
        {
            yield break;
        }

        //update the state
        public virtual IEnumerator Tick()
        {
            yield break;
        }

        //state will end
        public virtual IEnumerator Exit()
        {
            yield break;
        }

        public virtual IEnumerator Resume()
        {
            yield break;
        }

        public virtual IEnumerator Pause(bool hideView)
        {
            yield break;
        }

        public virtual IEnumerator Cleanup()
        {
            yield break;
        }

        protected virtual void End()
        {
            _Listener.TransitionTo(this, NextTransition);
        }

        protected virtual void Back()
        {
            _Listener.Back(this);
        }
    }
}