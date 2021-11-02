namespace InGame
{
    // 장애물 해제 스위치들 ex)발판, 레버
    public class Trigger : Interaction
    {
        private Repeater repeater;

        public Repeater Repeater
        {
            set => repeater = value;
        }

        public void OnTriggerSwitch()
        {
            Status = !Status;
            if (Status)
            {
                ActivateTrigger();
            }
            else
            {
                DeactivateTrigger();
            }

            repeater.TriggerStatusCheck();
        }

        // -> 활성화
        protected virtual void ActivateTrigger() { }

        // -> 비활성화
        protected virtual void DeactivateTrigger() { }
    }
}