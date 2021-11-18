using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StreamObject : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Transform _transform;
    private Vector3 _curPos;

    [SerializeField] private int id;
    
    void Awake()
    {
        _spriteRenderer.color = _photonView.IsMine ? Color.blue : Color.red;
        id = _photonView.ViewID;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (_photonView.IsMine)
        {
            Move();
        }
        else
        {
            // IsMine이 아닌 것들은 부드럽게 위치 동기화
            var checkPosition = transform.position - _curPos;

            if (checkPosition.sqrMagnitude >= 100)
            {
                transform.position = _curPos;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, _curPos, Time.deltaTime * _speed);
            }
        }
    }

    void Move()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
            Debug.Log("[Move] up");
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
            Debug.Log("[Move] left");
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
            Debug.Log("[Move] down");
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
            Debug.Log("[Move] right");
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
            _curPos = (Vector3) stream.ReceiveNext();
        }
    }

    [PunRPC]
    void DestroyRPC()
    {
        Destroy(gameObject);
    }
}
