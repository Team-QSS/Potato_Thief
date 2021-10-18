using System;
using DG.Tweening;
using UnityEngine;

namespace InGame
{
    public class MovingPlane : Obstacle
    {
        [SerializeField] private Vector3 moveDirection;
        private Vector3 defaultPosition;

        private void Start()
        {
            defaultPosition = transform.position;
        }

        public override void ActivateObstacle()
        {
            base.ActivateObstacle();
            transform.DOMove(transform.position + moveDirection, 3);
        }

        public override void DeactivateObstacle()
        {
            base.DeactivateObstacle();
            transform.DOMove(defaultPosition, 3);
        }
    }
}