using Photon.Pun;
using UnityEngine;

namespace InGame
{
    public class Obstacle : Touch, IPunObservable
    {
        [SerializeField] private ObstacleType obstacle;
        [PunRPC] protected override void ActivateTouch()
        {
            Debug.Log($"[Obstacle] Call Method ActivateTouch(Status : {Status})");
            if (Status) return;
            
            base.ActivateTouch();
            GameManager.Instance.HitByObstacle(obstacle, GameManager.Instance.myIndex);

            Status = true;
            Invoke(nameof(SetStatusFalse), 3);
        }

        private void SetStatusFalse() => Status = false;

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // 포톤으로 관측하여 전송 할 내용
                stream.SendNext(Status);
            }
            else
            {
                // 관측한 정보를 받을 내용
                Status = (bool) stream.ReceiveNext();
            }
        }
    }
}