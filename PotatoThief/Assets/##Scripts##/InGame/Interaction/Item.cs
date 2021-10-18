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
            base.ActivateTouch();

            _spriteRenderer.DOFade(0f, 1f);
            GameManager.Instance.GetItem(item);
            gameObject.SetActive(false);
        }
    }
}