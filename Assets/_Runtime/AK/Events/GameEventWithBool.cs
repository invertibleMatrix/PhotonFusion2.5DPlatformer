using UnityEngine;

namespace AK.Events
{
    [CreateAssetMenu(fileName = "e_", menuName = "Events/Game Event With Bool")]
    public class GameEventWithBool : GameEventWithParam<bool>
    {
        public override void Raise(bool t)
        {
            base.Raise(t);
        }
    }
}