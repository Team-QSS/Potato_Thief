using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 상호작용 - Obstacle, Trigger가 상속받음
public class Interaction : MonoBehaviour
{
    private bool status;

    public bool Status
    {
        get => status;
        set => status = value;
    }
}
