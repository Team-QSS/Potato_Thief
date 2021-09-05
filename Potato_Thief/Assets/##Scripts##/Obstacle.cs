using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KJG;

namespace YJM
{
    // 장애물 ex)벽, 잠긴 문
    public class Obstacle : Interaction
    {
        // -> 활성화
        public virtual void ActivateObstacle() { }

        // -> 비활성화
        public virtual void DeactivateObstacle() { }
    }
}
