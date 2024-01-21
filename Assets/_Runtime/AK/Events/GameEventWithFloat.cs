using UnityEngine;

namespace AK.Events
{
    [CreateAssetMenu(fileName = "e_", menuName = "Events/Game Event With Float")]
    public class GameEventWithFloat : GameEventWithParam<float>
    {
        public override void Raise(float t)
        {
            base.Raise(t);
        }
    }
}