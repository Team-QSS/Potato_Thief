using Photon.Pun;

namespace InGame
{
    public class ClearPoint : Touch
    {
        [PunRPC]protected override void ActivateTouch()
        {
            if (Status) return;
            
            base.ActivateTouch();
            GameManager.Instance.TouchAtClearPoint();
            
            Status = true;
        }
    }
}