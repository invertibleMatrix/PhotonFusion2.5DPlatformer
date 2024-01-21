using System.Collections;
using UnityEngine;

namespace AK.StateMachine
{
    [CreateAssetMenu(fileName = "Transitioner", menuName = "State Machine/Transitions/Transitioner")]
    public class Transitioner : ScriptableObject
    {
        public ITransitioner TransitionListener;

        public bool TransitionTo(Transition transition)
        {
            if (TransitionListener != null)
            {
                return TransitionListener.TransitionTo(transition);
            }
            else
            {
                return false;
            }
        }
    }
}