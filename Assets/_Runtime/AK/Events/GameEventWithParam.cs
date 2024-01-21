using UnityEngine;

namespace AK.Events
{
    public delegate void GameEventHandlerWithParam<T>(T t);

    public class GameEventWithParam<T> : ScriptableObject
    {
        public event GameEventHandlerWithParam<T> Handler;

        public virtual void Raise(T t)
        {
            Handler?.Invoke(t);
        }
    }
}