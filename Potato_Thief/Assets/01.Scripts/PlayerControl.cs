using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJG
{
    public class PlayerControl : MonoBehaviour
    {
        //public
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
        }

        // Update is called once per frame
        void Update()
        {
            Move(_playerMoveSpeed, Input.GetAxisRaw("Horizontal"));
            // Move(_playerMoveSpeed, _joystickControl.GetHorizontalValue());

            if (Input.GetKeyDown(jumpKey) && !_isPlayerJump) //스페이스 바가 눌러졌을때와 캐릭터가 점프하고 있지 않을떄
            {
                _isPlayerJump = true;
                Jump(jumpSpeed);
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
                    other.gameObject.GetComponent<Trigger>().InteractionBehavior();
                }
            }
        }
    }
}