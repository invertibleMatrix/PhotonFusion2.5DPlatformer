using System;
using System.Collections;
using UnityEngine;
using AK.StateMachine;

namespace Views
{
    [CreateAssetMenu(fileName = "UIViewTransition", menuName = "Views/Transitions/Basic UIView Transition")]
    public class UIViewTransition : Transition
    {
        [NonSerialized] public Transition NextTransition;


        public override IEnumerator Execute()
        {
            yield return base.Execute();

            UIViewState state = ToState as UIViewState;

            if (NextTransition != null)
            {
                state.NextTransition = NextTransition;
            }

            NextTransition = null;
        }
    }
}