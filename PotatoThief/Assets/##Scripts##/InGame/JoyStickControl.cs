using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InGame
{
    // JoyStick에 넣는 거
    public class JoyStickControl : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private RectTransform _thisTransform;
        private RectTransform _pointTransform;

        private Vector2 _pointPosition;  // point 원래 자리
        private float _pointRange;  // point 이동 범위
        private float _vectorRate;  // 입력 위치 > 입력 방향으로 변환할때 사용하는 값 (1 / _pointRange)

        private Vector2 _inputPosition;  // 입력 위치(point기준 로컬 좌표)

        public Vector2 InputDirection => _inputPosition * _vectorRate;  // 입력 방향

        public void OnBeginDrag(PointerEventData eventData)
        {
        }

        public void OnDrag(PointerEventData eventData)
        {
            // 터치한 위치 좌표 > UI상 위치 좌표로 변환
            _inputPosition = eventData.position - _pointPosition;
            // 조이스틱 범위를 넘어가지 않게 조정함
            _inputPosition = _inputPosition.magnitude > _pointRange
                ? _inputPosition.normalized * _pointRange : _inputPosition;

            _pointTransform.anchoredPosition = _inputPosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // 손 떼면 원래 자리로 돌려놓기
            _pointTransform.anchoredPosition = Vector2.zero;
        }

        private void Start()
        {
            _thisTransform = GetComponent<RectTransform>();
            _pointTransform = transform.GetChild(0).GetComponent<RectTransform>();

            _pointPosition = _pointTransform.position;
            _pointRange = _thisTransform.sizeDelta.x * 0.5f;
            _vectorRate = 1 / _pointRange;
        }
    }
}