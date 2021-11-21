using Photon.Pun;
using UniRx;
using UniRx.Triggers;

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
                .Where(other => other.gameObject.CompareTag("Player"))
                .Subscribe(_ =>
                {
                    // ActivateTouch();
                    pv.RPC("ActivateTouch", RpcTarget.All);
                });
        }

        [PunRPC] protected virtual void ActivateTouch() { }

        protected virtual void DeactivateTouch() { }
    }
}