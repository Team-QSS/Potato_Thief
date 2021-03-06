using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using YJM;

namespace KJG
{
    public class PlayerControl : MonoBehaviourPunCallbacks
    {
        //public
        public PhotonView _photonView;
        public float moveSpeed = 10;
        public float jumpSpeed = 10;
        public float skyMoveSpeed = 5;
        public KeyCode jumpKey = KeyCode.Space;
        public GameObject joystick;
        public KeyCode interactionKey = KeyCode.E;

        //private
        private Rigidbody2D _rigidbody2D;
        private bool _isPlayerJump = false;
        private float _playerMoveSpeed;
        private JoystickControl _joystickControl;

        // Start is called before the first frame update
        void Start()
        {
            _rigidbody2D = transform.GetComponent<Rigidbody2D>();
            _playerMoveSpeed = moveSpeed;
            _joystickControl = joystick.GetComponent<JoystickControl>();
            _photonView = GetComponent<PhotonView>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_photonView.IsMine)
            {
                Move(_playerMoveSpeed, Input.GetAxisRaw("Horizontal"));
                Move(_playerMoveSpeed, _joystickControl.GetHorizontalValue());

                if (Input.GetKeyDown(jumpKey) && !_isPlayerJump) //스페이스 바가 눌러졌을때와 캐릭터가 점프하고 있지 않을떄
                {
                    _isPlayerJump = true;
                    Jump(jumpSpeed);
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                _isPlayerJump = false;
            }
        }

        void Move(float speed, float x)
        {
            Vector2 v = new Vector2(x * speed, _rigidbody2D.velocity.y);
            _rigidbody2D.velocity = v;
        }

        void Jump(float jumpspeed)
        {
            Vector2 v = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y + jumpspeed);
            _rigidbody2D.velocity = v;
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Interaction"))
            {
                if (Input.GetKeyDown(interactionKey))
                {
                    other.gameObject.GetComponent<Trigger>().OnTriggerActivate();
                }
            }
        }
        
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            // IPunObservable 인터페이스의 구현 메소드
        
            if (stream.IsWriting)
            {
                // 포톤으로 관측하여 전송 할 내용
                stream.SendNext(transform.position);
            }
            else
            {
                // 관측한 정보를 받을 내용
                gameObject.transform.position = (Vector3) stream.ReceiveNext();
            }
        }
    }
}