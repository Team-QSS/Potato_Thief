using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

namespace InGame
{
    public class Repeater : MonoBehaviour
    {
        [SerializeField] private List<Trigger> triggers;
        [SerializeField] private List<Triggeree> triggerees;

        private void Start()
        {
            // 리피터 등록
            foreach (var trigger in triggers)
            {
                trigger.Repeater = this;
            }
        }

        // 전체 트리거 확인
        public void TriggerStatusCheck()
        {
            Debug.Log("[Repeater] Check Obstacle Status");
            if (triggers.Any(trigger => !trigger.Status))
            {
                DeactivateTriggerees();
                return;
            }

            ActivateTriggerees();
        }

        // 전체 장애물 활성화
        private void ActivateTriggerees()
        {
            foreach (var obstacle in triggerees)
            {
                Debug.Log("[Repeater] Active Obstacle Status");
                var pv = obstacle.GetComponent<PhotonView>();
                pv.RPC(nameof(obstacle.ActivateTriggeree), RpcTarget.All);
                obstacle.Status = true;
            }
        }

        // 전체 장애물 비활성화
        private void DeactivateTriggerees()
        {
            foreach (var obstacle in triggerees)
            {
                Debug.Log("[Repeater] Inactive Obstacle Status");
                var pv = obstacle.GetComponent<PhotonView>();
                pv.RPC(nameof(obstacle.DeactivateTriggeree), RpcTarget.All);
                obstacle.Status = false;
            }
        }
    }
}