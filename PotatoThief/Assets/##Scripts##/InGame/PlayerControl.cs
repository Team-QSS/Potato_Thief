using System;
using JetBrains.Annotations;
using Photon.Pun;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace InGame
{
    public class PlayerControl : MonoBehaviourPunCallbacks, IPunObservable
    {
        [SerializeField] private PhotonView playerPhotonView;
        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        
        private const int GroundLayer = 6;

        // [Header("조작 UI")] 
        private JoyStickControl _joyStickControl;
        private Button _buttonSpace;
        private Button _buttonE;
        
        [Header("속도")] 
        [SerializeField] private float moveSpeed = 10;
        [SerializeField] private float jumpPower = 10;

        [Header("애니메이션")] 
        [SerializeField] private RuntimeAnimatorController[] controllers;

        private bool _canPlayerJump;
        private static readonly int IsWalk = Animator.StringToHash("IsWalk");
        private static readonly int Jump1 = Animator.StringToHash("Jump");
        private bool _isWalk;
        
        private void Awake()
        {
            Debug.Log($"[PlayerControl] Awake : IsMine = {playerPhotonView.IsMine.ToString()}");
            if (!PhotonNetwork.IsMasterClient)
            {
                transform.position = GameManager.Instance.OtherPlayerSpawnPosition;
            }
            if (playerPhotonView.IsMine)
            {
                GameManager.Instance.player = gameObject;
            }
            else
            {
                GameManager.Instance.otherPlayer = gameObject;
            }
            Camera.main.GetComponent<CameraControl>()
                .SetTarget(GameManager.Instance.player, GameManager.Instance.otherPlayer);
        }
        
        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            playerPhotonView = GetComponent<PhotonView>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            SetPlayerColor(playerPhotonView.IsMine);
            if (!playerPhotonView.IsMine)
            {
                return;
            }
            _joyStickControl = GameManager.Instance.joyStickControl;
            _buttonSpace = GameManager.Instance.buttonSpace;
            _buttonE = GameManager.Instance.buttonE;
            
            // JoyStick -> 이동
            _joyStickControl.InputDirection
                .Where(_ => GameManager.Instance.Players[GameManager.Instance.myIndex].canMove)
                .Subscribe(dir =>
                {
                    Move(dir.x);
                    _spriteRenderer.flipX = dir.x < 0;
                    if (_isWalk != (dir.x != 0))
                    {
                        _isWalk = (dir.x != 0);
                        _animator.SetBool(IsWalk, _isWalk);
                    }
                }).AddTo(_joyStickControl);

            // 점프 막기
            this.OnCollisionEnter2DAsObservable()
                .Where(other => other.gameObject.layer == GroundLayer)
                .Subscribe(_ => { _canPlayerJump = true; }).AddTo(gameObject);

            // Button Space 입력 -> 점프
            _buttonSpace.OnClickAsObservable()
                .Where(_ => _canPlayerJump && GameManager.Instance.Players[GameManager.Instance.myIndex].canMove)
                .Subscribe(_ =>
                {
                    playerPhotonView.RPC(nameof(Jump), RpcTarget.All);
                    _animator.SetTrigger(Jump1);
                }).AddTo(_buttonSpace);

            // Button E 입력 -> 상호작용
            var buttonEStream = _buttonE.OnClickAsObservable()
                .Zip(this.OnCollisionStay2DAsObservable(), (unit, collision) => collision)
                .Where(_ => GameManager.Instance.Players[GameManager.Instance.myIndex].canMove)
                .First().Repeat();

            // 상호작용 -> 레버 작동
            buttonEStream
                .Where(collision => collision.gameObject.CompareTag("Lever"))
                .Subscribe(collision =>
                {
                    playerPhotonView.RPC("CallOnTriggerSwitch", RpcTarget.All, collision); 
                    // animation
                })
                .AddTo(_buttonE);
        }

        private void CallOnTriggerSwitch(Collider2D collision)
        {
            collision.gameObject.GetComponent<Trigger>().OnTriggerSwitch();
        }

        public void Move(float x)
        {
            var velocity = new Vector2(x * moveSpeed, _rigidbody2D.velocity.y);
            _rigidbody2D.velocity = velocity;
        }

        [PunRPC]
        public void Jump()
        {
            var velocity = _rigidbody2D.velocity;
            _rigidbody2D.velocity = new Vector2(velocity.x, jumpPower);
            _canPlayerJump = false;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
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

        private void SetPlayerColor(bool isMine)
        {
            _animator.runtimeAnimatorController = controllers[isMine ? 0 : 1];
        }
    }
}