using System.Collections;

namespace AK.StateMachine
{
    public interface IState
    {
        bool        TransitionTo(State           state, Transition transition);
        void        Back(State                   state);
        IEnumerator CleanupAllPausedStates(State state);
    }
}