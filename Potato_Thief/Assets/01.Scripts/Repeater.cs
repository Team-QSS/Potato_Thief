using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KJG;

namespace YJM
{
    public class Repeater : MonoBehaviour
    {
        [SerializeField] private List<Trigger> triggers;
        [SerializeField] private List<Obstacle> obstacles;
        // private bool status;
        private void Start()
        {
            foreach (var trigger in triggers)
            {
                trigger.Repeater = this;
            }
        }

        public void TriggerStatusCheck()
        {
            foreach (var trigger in triggers)
            {
                if (!trigger.Status)
                {
                    DeactivateObstacles();
                    return;
                }
            }

            ActivateObstacles();
        }

        private void ActivateObstacles()
        {
            foreach (var obstacle in obstacles)
            {
                obstacle.ActivateObstacle();
                obstacle.Status = true;
                Debug.Log(obstacle);
            }
        }

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
