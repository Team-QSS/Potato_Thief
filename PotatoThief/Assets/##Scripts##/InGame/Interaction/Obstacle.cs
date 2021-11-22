using Photon.Pun;
using UnityEngine;

namespace InGame
{
    public class Obstacle : Touch
    {
        [SerializeField] private ObstacleType obstacle;
        [PunRPC]protected override void ActivateTouch()
        {
            if (Status) return;
            
            base.ActivateTouch();
            GameManager.Instance.HitByObstacle(obstacle, GameManager.Instance.myIndex);

            Status = true;
            Invoke(nameof(SetStatusFalse), 3);
        }

        private void SetStatusFalse() => Status = false; 
    }
}