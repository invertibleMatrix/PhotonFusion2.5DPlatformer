using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace Views
{
    public class GenericFloater : MonoBehaviour
    {
        #region Serialized Variables

        [SerializeField] private Image           FloaterBg;
        [SerializeField] private TextMeshProUGUI FloaterText;
        [SerializeField] private RectTransform   ContainerPanel;

        #endregion

        #region Private Variables

        private Vector2  _initialPosition;
        private int      _moveDirection = 1;
        private string   _textToDisplay;
        private Sequence _floaterMoveSequence;
        private Sequence _floaterHideSequence;

        #endregion

        public void Init(float spawnPositionY, string textToDisplay)
        {
            _moveDirection   = 1;
            _textToDisplay   = textToDisplay;
            _initialPosition = new Vector2(0, spawnPositionY);
            StartDisplaySequence();
        }

        private void StartDisplaySequence()
        {
            _floaterMoveSequence?.Kill();
            _floaterMoveSequence = DOTween.Sequence();

            FloaterText.text = _textToDisplay;
            ContainerPanel.gameObject.SetActive(true);
            ContainerPanel.anchoredPosition = _initialPosition;

            _floaterMoveSequence
                .Append(ContainerPanel.DOAnchorPosY(_initialPosition.y + (_moveDirection * 120f), 2f).SetEase(Ease.OutSine)
                    .OnKill(Hide))
                .Join(FloaterBg.DOFade(1, 0))
                .Join(FloaterText.DOFade(1, 0));
            _floaterMoveSequence.Play();
        }

        private void Hide()
        {
            _floaterHideSequence?.Kill();
            _floaterHideSequence = DOTween.Sequence();
            _floaterHideSequence.Append(FloaterBg.DOFade(0, .5f))
                .Join(FloaterText.DOFade(0, .5f).OnKill
                (() => { GenericFloaterBuilder.AddToPool(this); }
                ));
            _floaterHideSequence.Play();
        }
    }
}