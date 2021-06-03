using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 장애물 ex)벽, 잠긴 문
public class Obstacle : Interaction
{
    [SerializeField] private List<Trigger> triggers;
    
    public virtual void Deactive(){}
    public virtual void Active(){}
}
