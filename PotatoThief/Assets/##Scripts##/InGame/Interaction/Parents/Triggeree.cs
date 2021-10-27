namespace InGame
{
    // 장애물 ex)벽, 잠긴 문
    public class Triggeree : Interaction
    {
        // -> 활성화
        public virtual void ActivateTriggeree() { }

        // -> 비활성화
        public virtual void DeactivateTriggeree() { }
    }
}
