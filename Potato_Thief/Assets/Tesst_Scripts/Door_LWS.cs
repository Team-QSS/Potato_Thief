using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_LWS : InteractedObstacle_LWS
{
    public SpriteRenderer spriteRenderer;

    Color defaultColor = new Color(0.3098039f, 0.09411766f, 0.09411766f, 1f);
    Color activeColor = Color.black;

    private void Start() => 
        spriteRenderer.color = defaultColor;

    public void ChangeState() =>
        spriteRenderer.color = GetStatus() ? activeColor : defaultColor;

    override public void TargetStatusChange()
    {
        print("Call TargetStatusChange : " + GetKey());
        bool countStatus = true;

        foreach (GameObject objects in targetObjects)
        {
            if (objects.GetComponent<Obstacle_LWS>().GetStatus() == false)
            {
                countStatus = false;
                break;
            }
        }

        SetStatus(countStatus);
        ChangeState();
    }

}
