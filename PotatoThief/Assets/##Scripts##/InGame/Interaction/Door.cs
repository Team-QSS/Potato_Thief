using Photon.Pun;
using UnityEngine;

namespace InGame
{
    public class Door : Triggeree
    {
        private SpriteRenderer spriteRenderer;

        private readonly Color activeColor = Color.green;
        private readonly Color inactiveColor = Color.white;

        [PunRPC] private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        [PunRPC] public override void ActivateTriggeree()
        {
            Debug.Log("[Door] Active");
            spriteRenderer.color = activeColor;
        }

        [PunRPC] public override void DeactivateTriggeree()
        {
            Debug.Log("[Door] Inactive");
            spriteRenderer.color = inactiveColor;
        }
    }
}
