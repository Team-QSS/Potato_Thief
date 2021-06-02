using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Obstacle
{
    public SpriteRenderer spriteRenderer;

    Color defaultColor = new Color(0.5f, 1f, 0.5f, 1f);
    Color activeColor = Color.black;

    private void Start() =>
        spriteRenderer.color = defaultColor;

    public void SendSignal()
    {
        object[] sendData = new object[] { GetKey(), !GetStatus() };
        MapManager.mapMaker.SendMessage(sendData);
    }

    override public void SetStatus(bool status)
    {
        base.SetStatus(status);
        spriteRenderer.color = status ? activeColor : defaultColor;
    }
}
