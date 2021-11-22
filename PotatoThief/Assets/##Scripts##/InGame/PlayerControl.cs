using Photon.Pun;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace InGame
{
    public class PlayerControl : MonoBehaviourPunCallbacks, IPunObservable
    {
        [SerializeField] private PhotonView _photonView;
        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        
        private const int GroundLayer = 6;
        
        [Header("조작 UI")] 
        [SerializeField] private JoyStickControl joyStickControl;
        [SerializeField] private Button buttonSpace;
        [SerializeField] private Button buttonE;

        [Header("속도")] 
        [SerializeField] private float moveSpeed = 10;
        [SerializeField] private float jumpPower = 10;
        
        private bool _canPlayerJump;
        private static readonly int IsWalk = Animator.StringToHash("IsWalk");
        private static readonly int Jump1 = Animator.StringToHash("Jump");
        private bool _isWalk;

        private void Start()
        {
            _photonView = GetComponent<PhotonView>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();

            if (!_photonView.IsMine) return;
            
            // JoyStick -> 이동
            joyStickControl.InputDirection
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
                }).AddTo(joyStickControl);

            // 점프 막기
            this.OnCollisionEnter2DAsObservable()
                .Where(other => other.gameObject.layer == GroundLayer)
                .Subscribe(_ => { _canPlayerJump = true; }).AddTo(gameObject);

            // Button Space 입력 -> 점프
            buttonSpace.OnClickAsObservable()
                .Where(_ => _canPlayerJump && GameManager.Instance.Players[GameManager.Instance.myIndex].canMove)
                .Subscribe(_ =>
                {
                    _photonView.RPC(nameof(Jump), RpcTarget.All);
                    _animator.SetTrigger(Jump1);
                }).AddTo(buttonSpace);

            // Button E 입력 -> 상호작용
            var buttonEStream = buttonE.OnClickAsObservable()
                .Zip(this.OnCollisionStay2DAsObservable(), (unit, collision) => collision)
                .Where(_ => GameManager.Instance.Players[GameManager.Instance.myIndex].canMove)
                .First().Repeat();

            // 상호작용 -> 레버 작동
            buttonEStream
                .Where(collision => collision.gameObject.CompareTag("Lever"))
                .Subscribe(collision =>
                {
                    _photonView.RPC("CallOnTriggerSwitch", RpcTarget.All, collision); 
                    // animation
                })
                .AddTo(buttonE);
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
    }
}