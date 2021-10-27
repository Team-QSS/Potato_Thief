using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace InGame
{
    public class PressurePlate : Trigger
    {
        private SpriteRenderer spriteRenderer;

        private readonly Color activeColor = Color.green;
        private readonly Color inactiveColor = Color.white;

        public void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            TriggerSubscribe();
        }

        private void TriggerSubscribe()
        {
            var collisionStream = this.OnCollisionEnter2DAsObservable()
                .Merge(this.OnCollisionExit2DAsObservable());

            collisionStream
                .Where(other => other.gameObject.CompareTag("Player"))
                .Subscribe(_ => { OnTriggerSwitch(); });
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