using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace InGame
{
    public class CameraControl : MonoBehaviour
    {
        private Camera _camera;

        private const float ScreenRatio = 9f / 16f;
        private const float DefaultSize = 5f;
        private const float DefaultPosX = -10;
        private const float Half = 0.5f;

        [SerializeField] private Transform myCharacterTransform;
        [SerializeField] private Transform anotherCharacterTransform;

        public void SetTarget(Transform my, Transform another)
        {
            myCharacterTransform = my;
            anotherCharacterTransform = another;
        }

        public void SetTarget(GameObject my, GameObject another)
        {
            myCharacterTransform = my.transform;
            anotherCharacterTransform = another.transform;
        }

        private void Start()
        {
            _camera = GetComponent<Camera>();

            this.UpdateAsObservable().Subscribe(_ =>
            {
                var my = myCharacterTransform.position;
                var another = anotherCharacterTransform.position;
                
                transform.position = CameraPosition(my, another);
                _camera.orthographicSize = CameraSize(my, another);
            });
        }

        // 카메라 위치 : 두 캐릭터 위치의 중점
        private static Vector3 CameraPosition(Vector3 my, Vector3 another)
        {
            var xMid = Mathf.Lerp(my.x, another.x, Half);
            var yMid = Mathf.Lerp(my.y, another.y, Half);

            return new Vector3(xMid, yMid, DefaultPosX);
        }

        // 카메라 크기 : 최소(기본) 크기, 두 캐릭터 위치의 x,y 사이 거리를 나타낼 카메라 크기 중 최대값
        private static float CameraSize(Vector3 my, Vector3 another)
        {
            var xSize = ConvertWidthToCameraSize(Mathf.Abs(my.x - another.x) + DefaultSize);
            var ySize = ConvertHeightToCameraSize(Mathf.Abs(my.y - another.y) + DefaultSize);
            
            return Mathf.Max(xSize, ySize, DefaultSize);
        }
        
        // 화면비가 16 : 9 일때, 입력한 가로 길이에 대한 Camera Size값을 반환하는 함수
        private static float ConvertWidthToCameraSize(float width) => width * Half * ScreenRatio;

        // 화면비가 16 : 9 일때, 입력한 세로 길이에 대한 Camera Size값을 반환하는 함수
        private static float ConvertHeightToCameraSize(float height) => height * Half;
    }
}