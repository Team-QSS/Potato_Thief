using DG.Tweening;
using Photon.Pun;
using UnityEngine;

namespace InGame
{
    public class MovingPlane : Triggeree
    {
        [SerializeField] private PhotonView pv;
        [SerializeField] private float movingDuration = 1;
        public Vector3 destPosition;
        public Vector3 defaultPosition;

        [PunRPC] private void Start()
        {
            defaultPosition = transform.position;
        }

        [PunRPC] public override void ActivateTriggeree()
        {
            Debug.Log("[Moving Plate] Active");
            base.ActivateTriggeree();
            transform.DOMove(transform.position + destPosition, movingDuration);
        }

        [PunRPC] public override void DeactivateTriggeree()
        {
            Debug.Log("[Moving Plate] Inactive");
            base.DeactivateTriggeree();
            transform.DOMove(defaultPosition, movingDuration);
        }

        private void Reset()
        {
            defaultPosition = transform.position;
        }
    }
}