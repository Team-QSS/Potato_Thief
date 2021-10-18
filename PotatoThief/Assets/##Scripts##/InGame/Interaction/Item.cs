using System;
using DG;
using DG.Tweening;
using UnityEngine;

namespace InGame
{
    public class Item : Touch
    {
        private SpriteRenderer _spriteRenderer;

        [SerializeField] private ItemType item;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected override void ActivateTouch()
        {
            if(Status) return;
            
            base.ActivateTouch();
            _spriteRenderer.DOFade(0f, 1f);
            Invoke(nameof(Disable), 1f);
            GameManager.Instance.GetItem(item);
            
            Status = true;
        }

        private void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}