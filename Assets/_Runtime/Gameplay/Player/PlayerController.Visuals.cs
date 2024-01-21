using UnityEngine;
using UnityEngine.UI;

namespace _Runtime.Gameplay.Player
{
    public partial class PlayerController
    {
        [SerializeField] private GameObject _localVisual;
        [SerializeField] private GameObject _nonLocalVisual;


        private void InitVisuals()
        {
            bool isLocal = IsLocalPlayer();
            _localVisual.SetActive(isLocal);
            _nonLocalVisual.SetActive(!isLocal);
        }
    }
}