using Photon.Pun;
using UnityEngine;

namespace InGame
{
    // 상호작용 - Obstacle, Trigger가 상속받음
    public class Interaction : MonoBehaviourPunCallbacks, IPunObservable
    {
        public bool Status
        {
            get => status;
            set => status = value;
        }

        [SerializeField] private bool status;
        
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
