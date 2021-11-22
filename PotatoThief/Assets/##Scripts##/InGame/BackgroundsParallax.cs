using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace InGame
{
    public class BackgroundsParallax : MonoBehaviour
    {
        [SerializeField] private Transform[] imageTransform;
        [SerializeField] private float[] imageParallaxSpeed;

        private Transform cameraTransform;
        private Vector3 lastCameraPosition;

        private void Start()
        {
            cameraTransform = Camera.main.transform;
            lastCameraPosition = cameraTransform.position;

            if (imageTransform.Length != imageParallaxSpeed.Length)
            {
                Debug.LogError("배경과 그에 맞는 속도의 수가 맞지 않음");
            }

            this.LateUpdateAsObservable().Subscribe(_ =>
            {
                var deltaMovement = cameraTransform.position - lastCameraPosition;
                for (var i = 0; i < imageTransform.Length; i++)
                {
                    var move = deltaMovement * imageParallaxSpeed[i];
                    move.y = 0;
                    imageTransform[i].position += move;
                }
                lastCameraPosition = cameraTransform.position;
            }).AddTo(gameObject);
        }
    }
}