using Photon.Pun;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace InGame
{
    public class Touch : Interaction
    {
        public PhotonView pv;
        protected virtual void Awake()
        {
            TouchSubscribe();
        }

        protected virtual void TouchSubscribe() // 공유자원
        {
            this.OnCollisionEnter2DAsObservable()
                .Where(IsPlayerCollision)
                .Subscribe(_ =>
                {
                    Debug.Log("[Touch] Call Method virtual ActivateTouch()");
                    pv.RPC(nameof(ActivateTouch), RpcTarget.All);
                });
        }

        private static bool IsPlayerCollision(Collision2D other)
        {
            Debug.Log("[Touch] Collision Occurred");
            if (other.gameObject.CompareTag("Player"))
            {
                Debug.Log("[Touch] Player Collision");
                return true;
            }

            Debug.Log("[Touch] Other Collision");
            return false;
        }

        [PunRPC] protected virtual void ActivateTouch() { }

        [PunRPC] protected virtual void DeactivateTouch() { }
    }
}