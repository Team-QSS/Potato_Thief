using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow)) transform.Translate(Vector2.up * Time.deltaTime);
        if (Input.GetKey(KeyCode.LeftArrow)) transform.Translate(Vector2.left * Time.deltaTime);
        if (Input.GetKey(KeyCode.DownArrow)) transform.Translate(Vector2.down * Time.deltaTime);
        if (Input.GetKey(KeyCode.RightArrow)) transform.Translate(Vector2.right * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            collision.gameObject.GetComponent<Lever>().SendSignal();
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 8)
        {
            print("hit");
            collision.gameObject.GetComponent<Lever>().SendSignal();
        }
    }
}
