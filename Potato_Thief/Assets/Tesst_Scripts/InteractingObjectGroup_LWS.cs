using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractingObjectGroup_LWS
{
    public ObstacleObject obstacleType;
    public int obstacleKey;

    public InteractingObjectGroup_LWS(ObstacleObject obstacleType, int obstacleKey)
    {
        this.obstacleType = obstacleType;
        this.obstacleKey = obstacleKey;
    }
}

