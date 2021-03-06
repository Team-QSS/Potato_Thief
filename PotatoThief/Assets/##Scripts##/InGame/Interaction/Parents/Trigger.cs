using Photon.Pun;

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

        [PunRPC] public void OnTriggerSwitch()   // 공유자원
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
            // 다른 플레어에서 이 함수가 호출되게 함 
        }

        // -> 활성화
        protected virtual void ActivateTrigger() { }

        // -> 비활성화
        protected virtual void DeactivateTrigger() { }
    }
}