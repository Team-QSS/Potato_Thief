using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public int key = 0;
    public bool status;

    Color defaultColor = new Color(0.5f, 1f, 0.5f, 1f);
    Color activeColor = Color.black;

    private void Start()
    {
        status = false;
        spriteRenderer.color = defaultColor;
    }

    public void SendSignal()
    {
        object[] sendData = new object[] { key, !status};
        MapMaker.mapMaker.SendMessage(sendData);
    }

    public void SetStatus(bool status)
    {
        this.status = status;
        spriteRenderer.color = status ? activeColor : defaultColor;
    }
}
