using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InGame
{
    public class Repeater : MonoBehaviour
    {
        [SerializeField] private List<Trigger> triggers;
        [SerializeField] private List<Obstacle> obstacles;

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
            if (triggers.Any(trigger => !trigger.Status))
            {
                DeactivateObstacles();
                return;
            }

            ActivateObstacles();
        }

        // 전체 장애물 활성화
        private void ActivateObstacles()
        {
            foreach (var obstacle in obstacles)
            {
                obstacle.ActivateObstacle();
                obstacle.Status = true;
            }
        }

        // 전체 장애물 비활성화
        private void DeactivateObstacles()
        {
            foreach (var obstacle in obstacles)
            {
                obstacle.DeactivateObstacle();
                obstacle.Status = false;
            }
        }
    }
}