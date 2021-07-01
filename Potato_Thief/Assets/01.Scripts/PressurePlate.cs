using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YJM;

namespace KJG
{
    public class PressurePlate : Trigger
    {
        private SpriteRenderer spriteRenderer;
        
        private readonly Color activeColor = Color.green;
        private readonly Color inactiveColor = Color.white;
        
        void Start()
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

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                OnTriggerActivate();
                
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                OnTriggerActivate();
            }
        }
        
        
    }
}