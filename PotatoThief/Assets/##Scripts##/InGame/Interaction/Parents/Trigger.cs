using Photon.Pun;
using UnityEngine;

namespace InGame
{
    // 장애물 해제 스위치들 ex)발판, 레버
    public class Trigger : Interaction
    {
        [SerializeField] protected PhotonView pv;
        private Repeater repeater;

        public Repeater Repeater
        {
            set => repeater = value;
        }

        public void  OnTriggerSwitch()   // 공유자원
        {
            Debug.Log("[Trigger] Call Method OnTriggerSwitch()");
            Status = !Status;
            Debug.Log($"[Trigger] Status : {Status}");
            
            if (Status)
            {
                // pv.RPC(nameof(ActivateTrigger), RpcTarget.All);
                ActivateTrigger();
            }
            else
            {
                // pv.RPC(nameof(DeactivateTrigger), RpcTarget.All);
                DeactivateTrigger();
            }
            // pv.RPC(nameof(repeater.TriggerStatusCheck), RpcTarget.All);
            
            repeater.TriggerStatusCheck();
            // 다른 플레어에서 이 함수가 호출되게 함 
        }

        // -> 활성화
        [PunRPC] protected virtual void ActivateTrigger() { }

        // -> 비활성화
        [PunRPC] protected virtual void DeactivateTrigger() { }
    }
}