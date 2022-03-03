using DG.Tweening;
using Photon.Pun;
using UnityEngine;

namespace InGame
{
    public class Item : Touch
    {
        private SpriteRenderer _spriteRenderer;

        [SerializeField] private ItemType item;

        [PunRPC] private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        [PunRPC] protected override void ActivateTouch()
        {
            Debug.Log($"[Obstacle] Call Method ActivateTouch(Status : {Status})");
            if(Status) return;
            
            base.ActivateTouch();
            _spriteRenderer.DOFade(0f, 1f);
            Invoke(nameof(Disable), 1f);
            GameManager.Instance.GetItem(item, GameManager.Instance.myIndex);
            
            Status = true;
        }

        [PunRPC] private void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}