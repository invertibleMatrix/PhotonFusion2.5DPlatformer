using UnityEngine;

namespace AK.Events
{
    [CreateAssetMenu(fileName = "e_", menuName = "Events/Game Event With String")]
    public class GameEventWithString : GameEventWithParam<string>
    {
        public override void Raise(string t)
        {
            base.Raise(t);
        }
    }
}