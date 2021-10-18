using System;
using Photon.Pun;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace InGame
{
    public class PlayerControl : MonoBehaviourPunCallbacks
    {
        private PhotonView _photonView;
        private Rigidbody2D _rigidbody2D;

        private const int GroundLayer = 6;
        
        [Header("조작 UI")] 
        [SerializeField] private JoyStickControl joyStickControl;
        [SerializeField] private Button buttonSpace;
        [SerializeField] private Button buttonE;

        [Header("속도")] 
        [SerializeField] private float moveSpeed = 10;
        [SerializeField] private float jumpPower = 10;


        private bool _canPlayerJump;
        
        private void Start()
        {
            _photonView = GetComponent<PhotonView>();
            _rigidbody2D = GetComponent<Rigidbody2D>();

            if (_photonView.IsMine)
            {
                // JoyStick -> 이동
                this.UpdateAsObservable().Subscribe(_ => { Move(joyStickControl.InputDirection.x); });

                // 점프 막기
                this.OnCollisionEnter2DAsObservable()
                    .Where(other => other.gameObject.layer == GroundLayer)
                    .Subscribe(_ => { _canPlayerJump = true; });
                
                // Button Space 입력 -> 점프
                buttonSpace.OnClickAsObservable()
                    .Where(_ => _canPlayerJump)
                    .Subscribe(_ => Jump());

                // Button E 입력 -> 상호작용
                var buttonEStream = buttonE.OnClickAsObservable()
                    .Zip(this.OnCollisionStay2DAsObservable(), (unit, collision) => collision)
                    .First().Repeat();

                // 상호작용 -> 레버 작동
                buttonEStream
                    .Where(collision => collision.gameObject.CompareTag("Lever"))
                    .Subscribe(collision => { collision.gameObject.GetComponent<Trigger>().OnTriggerSwitch(); });
            }
        }

        public void Move(float x)
        {
            var velocity = new Vector2(x * moveSpeed, _rigidbody2D.velocity.y);
            _rigidbody2D.velocity = velocity;
        }

        public void Jump()
        {
            var velocity = _rigidbody2D.velocity;
            _rigidbody2D.velocity = new Vector2(velocity.x, jumpPower);
            _canPlayerJump = false;
        }
    }
}