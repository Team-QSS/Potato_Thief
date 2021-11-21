using Photon.Pun;
using UnityEngine;

namespace InGame
{
    // 상호작용 - Obstacle, Trigger가 상속받음
    public class Interaction : MonoBehaviourPunCallbacks
    {
        public bool Status { get; set; }
    }
}
