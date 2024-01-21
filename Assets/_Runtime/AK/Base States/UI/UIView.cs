using System;
using System.Collections;
using AK.StateMachine;
using UnityEngine;

namespace Views
{
    public class UIView : MonoBehaviour
    {
        [NonSerialized] public UIViewState UIViewState;

        [SerializeField] protected UIViewConfig  UIViewConfig;
        [SerializeField] protected RectTransform Container;
        [SerializeField] protected Canvas        Canvas;

        public virtual IEnumerator Show()
        {
            Container.gameObject.SetActive(true);
            yield break;
        }

        public virtual IEnumerator Hide()
        {
            Container.gameObject.SetActive(false);
            yield break;
        }

        public virtual IEnumerator Pause()
        {
            Container.gameObject.SetActive(false);
            yield break;
        }

        public virtual void TransitionTo(Transition transition)
        {
            UIViewState.TransitionTo(transition);
        }
    }
}