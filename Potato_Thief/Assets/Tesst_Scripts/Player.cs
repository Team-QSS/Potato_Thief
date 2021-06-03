using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 2f;
    void Update()
    {
        if (Input.GetKey(KeyCode.W)) transform.Translate(Vector2.up * Time.deltaTime * speed);
        if (Input.GetKey(KeyCode.A)) transform.Translate(Vector2.left * Time.deltaTime * speed);
        if (Input.GetKey(KeyCode.S)) transform.Translate(Vector2.down * Time.deltaTime * speed);
        if (Input.GetKey(KeyCode.D)) transform.Translate(Vector2.right * Time.deltaTime * speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
            collision.gameObject.GetComponent<Lever>().SendSignal();
    }
}
