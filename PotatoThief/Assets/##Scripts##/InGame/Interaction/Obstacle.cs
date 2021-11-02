using UnityEngine;

namespace InGame
{
    public class Obstacle : Touch
    {
        [SerializeField] private ObstacleType obstacle;
        protected override void ActivateTouch()
        {
            if (Status) return;
            
            base.ActivateTouch();
            GameManager.Instance.HitByObstacle(obstacle);

            Status = true;
            Invoke(nameof(SetStatusFalse), 3);
        }

        private void SetStatusFalse() => Status = false; 
    }
}