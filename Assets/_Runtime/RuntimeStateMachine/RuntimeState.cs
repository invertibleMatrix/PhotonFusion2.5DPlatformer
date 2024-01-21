using System;
using System.Collections;

namespace AK.RTStateMachine
{
    public abstract class RuntimeState<TMediator>
    {
        public TMediator Mediator;

        protected RuntimeState(TMediator mediator)
        {
            Mediator = mediator;
        }

        public virtual IEnumerator Start()
        {
            yield break;
        }

        public virtual IEnumerator Update()
        {
            yield break;
        }

        public virtual IEnumerator Exit()
        {
            yield break;
        }
    }
}