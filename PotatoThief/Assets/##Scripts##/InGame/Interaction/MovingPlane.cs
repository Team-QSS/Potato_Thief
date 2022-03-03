using DG.Tweening;
using Photon.Pun;
using UnityEngine;

namespace InGame
{
    public class MovingPlane : Triggeree
    {
        [SerializeField] private PhotonView pv;
        [SerializeField] private Vector3 moveDirection;
        [SerializeField] private float tweenDuration = 1;
        private Vector3 defaultPosition;

        [PunRPC] private void Start()
        {
            defaultPosition = transform.position;
        }

        [PunRPC] public override void ActivateTriggeree()
        {
            Debug.Log("[Moving Plate] Active");
            base.ActivateTriggeree();
            transform.DOMove(transform.position + moveDirection, tweenDuration);
        }

        [PunRPC] public override void DeactivateTriggeree()
        {
            Debug.Log("[Moving Plate] Inactive");
            base.DeactivateTriggeree();
            transform.DOMove(defaultPosition, tweenDuration);
        }
    }
}