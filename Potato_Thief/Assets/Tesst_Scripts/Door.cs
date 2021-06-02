using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    public List<GameObject> targetObjects = new List<GameObject>();
    public bool status;
    public int key;
    private void Start()
    {
        status = false;

        SpriteRenderer i = GetComponent<SpriteRenderer>();
        Color c = i.color;

        c.r = 0.3098039f;
        c.g = 0.09411766f;
        c.b = 0.09411766f;
        c.a = 1f;

        i.color = c;
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

    public void ChangeState()
    {
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
            c.r = 0.3098039f;
            c.g = 0.09411766f;
            c.b = 0.09411766f;
            c.a = 1f;
        }
        i.color = c;
    }
}
