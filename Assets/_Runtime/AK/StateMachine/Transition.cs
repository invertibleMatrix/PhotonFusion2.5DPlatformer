using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AK.StateMachine
{
    [CreateAssetMenu(fileName = "Transition", menuName = "State Machine/Transitions/Basic Transition")]
    public class Transition : ScriptableObject
    {
        public State ToState;
        public bool  PausePreviousState;
        public bool  HidesPreviousStateWhenPausingIt;

        public virtual IEnumerator Execute()
        {
            yield break;
        }
    }
}