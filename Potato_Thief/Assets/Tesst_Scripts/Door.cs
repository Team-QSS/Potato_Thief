using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Obstacle
{
    public List<GameObject> targetObjects = new List<GameObject>();
    public SpriteRenderer spriteRenderer;

    Color defaultColor = new Color(0.3098039f, 0.09411766f, 0.09411766f, 1f);
    Color activeColor = Color.black;

    private void Start() => 
        spriteRenderer.color = defaultColor;

    public void TargetStatusChange()
    {
        bool countStatus = true;

        foreach (GameObject objects in targetObjects)
        {
            if (objects.GetComponent<Obstacle>().GetStatus() == false)
            {
                countStatus = false;
                break;
            }
        }
        
        SetStatus(countStatus);
        ChangeState();
    }

    public void ChangeState() =>
        spriteRenderer.color = GetStatus() ? activeColor : defaultColor;

}
