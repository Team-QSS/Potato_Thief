using Photon.Pun;
using UnityEngine;

namespace InGame
{
    public class ClearPoint : Touch
    {
        [PunRPC] protected override void ActivateTouch()
        {
            Debug.Log($"[Obstacle] Call Method ActivateTouch(Status : {Status})");
            if (Status) return;
            
            base.ActivateTouch();
            GameManager.Instance.TouchAtClearPoint(GameManager.Instance.myIndex);
            
            Status = true;
        }
    }
}