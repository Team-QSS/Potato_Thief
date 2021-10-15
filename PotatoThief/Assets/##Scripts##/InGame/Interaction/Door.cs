using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGame
{
    public class Door : Obstacle
    {
        private SpriteRenderer spriteRenderer;

        private readonly Color activeColor = Color.green;
        private readonly Color inactiveColor = Color.white;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public override void ActivateObstacle()
        {
            spriteRenderer.color = activeColor;
        }

        public override void DeactivateObstacle()
        {
            spriteRenderer.color = inactiveColor;
        }
    }
}
