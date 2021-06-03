using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractedObstacle_LWS : Obstacle_LWS
{
    public List<GameObject> targetObjects = new List<GameObject>();

    abstract public void TargetStatusChange();
}