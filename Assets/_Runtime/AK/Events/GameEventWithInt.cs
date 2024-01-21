using UnityEngine;

namespace AK.Events
{
    [CreateAssetMenu(fileName = "e_", menuName = "Events/Game Event With Int")]
    public class GameEventWithInt : GameEventWithParam<int>
    {
        public override void Raise(int t)
        {
            base.Raise(t);
        }
    }
}