using UnityEngine;

namespace InGame
{
    public class Door : Triggeree
    {
        private SpriteRenderer spriteRenderer;

        private readonly Color activeColor = Color.green;
        private readonly Color inactiveColor = Color.white;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public override void ActivateTriggeree()
        {
            spriteRenderer.color = activeColor;
        }

        public override void DeactivateTriggeree()
        {
            spriteRenderer.color = inactiveColor;
        }
    }
}
