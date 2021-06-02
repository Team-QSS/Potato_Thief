using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    public List<GameObject> targetObjects = new List<GameObject>();
    public SpriteRenderer spriteRenderer;

    Color defaultColor = new Color(0.3098039f, 0.09411766f, 0.09411766f, 1f);
    Color activeColor = Color.black;

    public bool status;
    public int key;


    private void Start()
    {
        status = false;
        spriteRenderer.color = defaultColor;
    }

    public void TargetStatusChange()
    {
        bool countStatus = targetObjects[0].GetComponent<Lever>().status;

        for (int i = 1; i < targetObjects.Count; i++)
        {
            countStatus = countStatus && targetObjects[i].GetComponent<Lever>().status;
        }

        status = countStatus;
        ChangeState();
    }

    public void ChangeState() => 
        spriteRenderer.color = status ? activeColor : defaultColor;

}
