using Photon.Pun;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace InGame
{
    public class PressurePlate : Trigger
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        private readonly Color activeColor = Color.green;
        private readonly Color inactiveColor = Color.white;

        [PunRPC] public void Start()
        {
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }

            pv.RPC(nameof(TriggerSubscribe), RpcTarget.All);
        }

        [PunRPC] private void TriggerSubscribe()
        {
            var collisionStream = this.OnCollisionEnter2DAsObservable()
                .Merge(this.OnCollisionExit2DAsObservable());

            collisionStream
                .Where(other => CollisionCheck() && IsPlayerCollision(other))
                .Subscribe(_ => { OnTriggerSwitch(); }).AddTo(this);
        }

        private bool CollisionCheck()
        {
            Debug.Log("[Pressure Plate] Collision Occurred");
            return true;
        }
        private bool IsPlayerCollision(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Debug.Log("[Pressure Plate] Player Collision");
                return true;
            }
            Debug.Log("[Pressure Plate] Other Collision");
            return false;
        }
        
        [PunRPC] protected override void ActivateTrigger()
        {
            spriteRenderer.color = activeColor;
            Debug.Log("[Pressure Plate] Active");
        }

        [PunRPC] protected override void DeactivateTrigger()
        {
            spriteRenderer.color = inactiveColor;
            Debug.Log("[Pressure Plate] Inactive");
        }
    }
}