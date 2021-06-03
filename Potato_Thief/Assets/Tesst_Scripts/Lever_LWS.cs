using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever_LWS : Obstacle_LWS
{
    public SpriteRenderer spriteRenderer;

    Color defaultColor = new Color(0.5f, 1f, 0.5f, 1f);
    Color activeColor = Color.black;

    private void Start() => 
        spriteRenderer.color = defaultColor;

    public void SendSignal()
    {
        object[] sendData = new object[] { 
            new ObstacleObject[] { ObstacleObject.door }, // Broadcasting Targets
            GetKey(), // Key
            !GetStatus()    // Status
        };
        MapManager_LWS.mapMaker.SendMessage(sendData);
    }

    override public void SetStatus(bool status)
    {
        base.SetStatus(status);
        spriteRenderer.color = status ? activeColor : defaultColor;
    }
}
