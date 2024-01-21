namespace AK.StateMachine
{
    public interface ITransitioner
    {
        bool TransitionTo(Transition transition);
    }

    public interface IGameStateTransitioner : ITransitioner
    {
        Transition RestartTransition();
    }
}