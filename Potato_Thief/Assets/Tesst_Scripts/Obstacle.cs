using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private int key = 0;
    private bool status;

    private void Start()
    {
        status = false;
    }

    public void SetKey(int key) => this.key = key;
    
    virtual public void SetStatus(bool status) => this.status = status;
    
    public int GetKey() { return key; }

    public bool GetStatus() { return status; }
}
