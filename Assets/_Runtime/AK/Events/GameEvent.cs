using UnityEngine;

namespace AK.Events
{
    public delegate void GameEventHandler();

    [CreateAssetMenu(fileName = "e_", menuName = "Events/Game Event - Basic")]
    public class GameEvent : ScriptableObject
    {
        public event GameEventHandler Handler;

        public void Invoke()
        {
            Handler?.Invoke();
        }
    }
}