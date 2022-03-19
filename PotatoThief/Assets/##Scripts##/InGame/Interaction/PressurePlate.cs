using Photon.Pun;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Windows.Speech;

namespace InGame
{
    public class PressurePlate : Trigger
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        private int _objectCount;
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

        [PunRPC]
        private void TriggerSubscribe()
        {
            var collisionExitStream = this.OnCollisionExit2DAsObservable();
            var collisionEnterStream = this.OnCollisionEnter2DAsObservable();

            collisionExitStream
                .Where(other => CollisionCheck() && IsPlayerCollision(other))
                .Subscribe(_ =>
                {
                    Debug.Log("[Pressure Plate] Exit Stream");
                    _objectCount--;
                    IsStatusChange(); // 1 -> 0 : 상태 반전
                }).AddTo(this);

            collisionEnterStream
                .Where(other => CollisionCheck() && IsPlayerCollision(other))
                .Subscribe(_ =>
                {
                    Debug.Log("[Pressure Plate] Enter Stream");
                    IsStatusChange(); // 0 -> 1 : 상태 반전
                    _objectCount++;
                });
            /*
            var collisionStream = 
                this.OnCollisionEnter2DAsObservable().
                    Merge(this.OnCollisionExit2DAsObservable());
            
            collisionStream
                .Where(other => CollisionCheck() && IsPlayerCollision(other))
                .Subscribe(_ => {  OnTriggerSwitch(); }).AddTo(this);
            */
        }

        private void IsStatusChange()
        {
            if (_objectCount == 0)
            {
                OnTriggerSwitch();
            }
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