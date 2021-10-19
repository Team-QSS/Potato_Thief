using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace InGame
{
    public class Touch : Interaction
    {
        protected virtual void Awake()
        {
            TouchSubscribe();
        }

        protected virtual void TouchSubscribe()
        {
            this.OnCollisionEnter2DAsObservable()
                .Where(other => other.gameObject.CompareTag("Player"))
                .Subscribe(_ => { ActivateTouch(); });
        }

        protected virtual void ActivateTouch() { }

        protected virtual void DeactivateTouch() { }
    }
}