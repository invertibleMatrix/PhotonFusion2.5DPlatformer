using System.Collections;

namespace AK.RTStateMachine
{
    public class RuntimeTransition<TBaseState>
    {
        public TBaseState ToState;

        public RuntimeTransition(TBaseState toState)
        {
            ToState = toState;
        }

        public virtual IEnumerator Start()
        {
            yield break;
        }
    }
}