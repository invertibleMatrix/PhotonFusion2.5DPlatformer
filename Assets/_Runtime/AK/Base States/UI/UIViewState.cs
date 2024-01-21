using System;
using System.Collections;
using UnityEngine;
using AK.StateMachine;
using State = AK.StateMachine.State;
using Transition = AK.StateMachine.Transition;

namespace Views
{
    public class UIViewState : State
    {
        [SerializeField] protected ResourcesPrefab<GameObject> ViewPrefab;

        [NonSerialized] protected UIView _view;
        [NonSerialized] protected bool?  hidden = null;


        public override IEnumerator Init(IState listener)
        {
            yield return base.Init(listener);
            DestroyView();
            ViewPrefab.LoadAsset((view) =>
            {
                _view             = Instantiate(view).GetComponent<UIView>();
                _view.UIViewState = this;
            });

            yield return new WaitUntil(() => _view != null);
        }

        public override IEnumerator Execute()
        {
            yield return base.Execute();
            yield return _view.Show();
        }

        public override IEnumerator Pause(bool hideView)
        {
            yield return base.Pause(hideView);
            if (hideView)
            {
                yield return _view.Hide();
            }

            hidden = hideView;
        }

        public override IEnumerator Resume()
        {
            yield return base.Resume();
            if (hidden.HasValue && hidden.Value)
            {
                yield return _view.Show();
            }

            hidden = null;
        }

        public override IEnumerator Exit()
        {
            if (_view != null)
            {
                yield return _view.Hide();
                DestroyView();
            }

            yield return base.Exit();
        }

        public override IEnumerator Cleanup()
        {
            DestroyView();
            yield return base.Cleanup();
        }

        protected virtual void DestroyView()
        {
            if (_view != null)
            {
                Destroy(_view.gameObject);
            }

            _view = null;
        }

        public virtual void TransitionTo(Transition transition)
        {
            _Listener.TransitionTo(this, transition);
        }
    }
}