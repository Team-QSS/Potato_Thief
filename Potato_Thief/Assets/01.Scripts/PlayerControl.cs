using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //public
    public float moveSpeed = 10;
    public float jumpSpeed = 10;
    public KeyCode jumpKey = KeyCode.Space;

    //private
    private Rigidbody2D _rigidbody2D;
    private bool _isPlayerJump = false;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = transform.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move(moveSpeed);

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

    void Move(float speed)
    {
        Vector2 v = new Vector2(Input.GetAxisRaw("Horizontal") * speed, _rigidbody2D.velocity.y);
        _rigidbody2D.velocity = v;
    }

    void Jump(float jumpSpeed)
    {
        Vector2 v = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y + jumpSpeed);
        _rigidbody2D.velocity = v;
    }
}
