using Photon.Pun;
using UnityEngine;

namespace InGame
{
    public class Lever : Trigger
    {
        private SpriteRenderer spriteRenderer;

        private readonly Color activeColor = Color.green;
        private readonly Color inactiveColor = Color.white;

        [PunRPC] private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        [PunRPC] protected override void ActivateTrigger()
        {
            Debug.Log("[Lever] Active");
            spriteRenderer.color = activeColor;
        }

        [PunRPC] protected override void DeactivateTrigger()
        {
            Debug.Log("[Lever] Inactive");
            spriteRenderer.color = inactiveColor;
        }
    }
}