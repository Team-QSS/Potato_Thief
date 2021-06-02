using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : Obstacle
{
    public List<GameObject> targetObjects = new List<GameObject>();
    public SpriteRenderer spriteRenderer;

    Color defaultColor = new Color(0.3098039f, 0.09411766f, 0.09411766f, 1f);
    Color activeColor = Color.black;

    private void Start() => spriteRenderer.color = defaultColor;

    public void TargetStatusChange()
    {
        bool countStatus = targetObjects[0].GetComponent<Obstacle>().GetStatus();

        for (int i = 1; i < targetObjects.Count; i++)
            countStatus = countStatus && targetObjects[i].GetComponent<Obstacle>().GetStatus();

        SetStatus(countStatus);
        ChangeState();
    }

    public void ChangeState() =>
        spriteRenderer.color = GetStatus() ? activeColor : defaultColor;

}
