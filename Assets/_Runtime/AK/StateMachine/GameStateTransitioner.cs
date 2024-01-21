using UnityEngine;

namespace AK.StateMachine
{
    [CreateAssetMenu(fileName = "Transitioner", menuName = "State Machine/Transitions/GameStateTransitioner")]
    public class GameStateTransitioner : Transitioner
    {
        public IGameStateTransitioner GameStateTransitionListener;

        public Transition Restart()
        {
            return GameStateTransitionListener.RestartTransition();
        }
    }
}