using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public GameObject leverBody;
    public int key = 0;
    public bool status;

    private void Start()
    {
        status = false;

        SpriteRenderer i = GetComponent<SpriteRenderer>();
        Color c = i.color;

        c.r = 0.5f;
        c.g = 1f;
        c.b = 0.5f;
        c.a = 1f;

        i.color = c;
    }

    public void SendSignal()
    {
        print("key : " + key);
        object[] sendData = new object[] { key, !status};
        MapMaker.mapMaker.SendMessage(sendData);
    }

    public void SetStatus(bool status)
    {
        this.status = status;

        // Effect
        SpriteRenderer i = GetComponent<SpriteRenderer>();
        Color c = i.color;

        if (status)
        {
            c.r = 0;
            c.g = 0;
            c.b = 0;
            c.a = 1f;
        }
        else
        {
            c.r = 0.5f;
            c.g = 1f;
            c.b = 0.5f;
            c.a = 1f;
        }

        i.color = c;
    }
}
