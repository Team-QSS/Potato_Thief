using UnityEngine;

namespace InGame
{
    public class ClearPoint : Touch
    {
        protected override void ActivateTouch()
        {
            base.ActivateTouch();
            GameManager.Instance.TouchAtClearPoint();
        }
    }
}