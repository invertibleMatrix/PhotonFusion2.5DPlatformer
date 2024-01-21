using _Runtime.Gameplay.Player;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Runtime.Gameplay.UI
{
    public class GameplayHUD : MonoBehaviour, ILocalPlayerStatsListener
    {
        [SerializeField] private TMP_Text _healthText;
        [SerializeField] private TMP_Text _totalKillsText;
        [SerializeField] private TMP_Text _totalDeathsText;
        [SerializeField] private TMP_Text _selectedWeaponText;

        [SerializeField] private Image _bloodImage;


        public void OnHealthUpdate(int currentHealth)
        {
            _healthText.text = $"HEALTH: {currentHealth}";
            ShowBloodEffectWhenHit();
        }

        public void OnKillsUpdate(int totalKills)
        {
            _totalKillsText.text = $"KILLS: {totalKills}";
        }

        public void OnDeathsUpdate(int totalDeaths)
        {
            _totalDeathsText.text = $"DEATHS: {totalDeaths}";
        }

        public void OnWeaponChange(int i)
        {
            _selectedWeaponText.text = $"WEAPON # {i}";
        }

        private void ShowBloodEffectWhenHit()
        {
            var startingColor = _bloodImage.color;
            startingColor.a   = 1;
            _bloodImage.color = startingColor;
            _bloodImage.DOFade(0f, 0.1f);
        }
    }
}