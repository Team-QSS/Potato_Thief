using UnityEngine;

namespace InGame
{
    public class ObstacleObject : Touch
    {
        [SerializeField] private ObstacleType obstacle;
        protected override void ActivateTouch()
        {
            base.ActivateTouch();
            GameManager.Instance.HitByObstacle(obstacle);
        }
    }
}