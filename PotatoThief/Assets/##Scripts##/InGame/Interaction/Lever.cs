using UnityEngine;

namespace InGame
{
    public class Lever : Trigger
    {
        private SpriteRenderer spriteRenderer;

        private readonly Color activeColor = Color.green;
        private readonly Color inactiveColor = Color.white;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected override void ActivateTrigger()
        {
            spriteRenderer.color = activeColor;
        }

        protected override void DeactivateTrigger()
        {
            spriteRenderer.color = inactiveColor;
        }
    }
}