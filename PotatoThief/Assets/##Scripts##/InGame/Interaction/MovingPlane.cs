using DG.Tweening;
using UnityEngine;

namespace InGame
{
    public class MovingPlane : Triggeree
    {
        [SerializeField] private Vector3 moveDirection;
        private Vector3 defaultPosition;

        private void Start()
        {
            defaultPosition = transform.position;
        }

        public override void ActivateTriggeree()
        {
            base.ActivateTriggeree();
            transform.DOMove(transform.position + moveDirection, 3);
        }

        public override void DeactivateTriggeree()
        {
            base.DeactivateTriggeree();
            transform.DOMove(defaultPosition, 3);
        }
    }
}