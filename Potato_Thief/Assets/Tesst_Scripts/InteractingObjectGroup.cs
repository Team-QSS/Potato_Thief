using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractingObjectGroup
{
    public ObstacleObject obstacleType;
    public int obstacleKey;

    public InteractingObjectGroup(ObstacleObject obstacleType, int obstacleKey)
    {
        this.obstacleType = obstacleType;
        this.obstacleKey = obstacleKey;
    }
}

