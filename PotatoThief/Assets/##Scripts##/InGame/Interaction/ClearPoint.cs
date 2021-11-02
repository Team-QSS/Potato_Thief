namespace InGame
{
    public class ClearPoint : Touch
    {
        protected override void ActivateTouch()
        {
            if (Status) return;
            
            base.ActivateTouch();
            GameManager.Instance.TouchAtClearPoint();
            
            Status = true;
        }
    }
}