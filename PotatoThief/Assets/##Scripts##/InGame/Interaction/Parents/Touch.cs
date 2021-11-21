using UniRx;
using UniRx.Triggers;

namespace InGame
{
    public class Touch : Interaction
    {
        protected virtual void Awake()
        {
            TouchSubscribe();
        }

        protected virtual void TouchSubscribe()
        {
            this.OnCollisionEnter2DAsObservable()
                .Where(other => other.gameObject.CompareTag("Player"))
                .Subscribe(_ =>
                {
                    ActivateTouch(); 
                    // 다른 플레어에서 이 함수가 호출되게 함 
                });
        }

        protected virtual void ActivateTouch() { }

        protected virtual void DeactivateTouch() { }
    }
}