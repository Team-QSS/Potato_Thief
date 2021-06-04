using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Repeater : MonoBehaviour
{
    // Start is called before the first frame update

    public List<Trigger> triggers = new List<Trigger>();
    public List<Obstacle> obstacles = new List<Obstacle>();

    void Start()
    {
        foreach (var trigger in triggers)
        {
            trigger.SetReapeater(this);
        }

        foreach (var obstacle in obstacles)
        {
            obstacle.SetReapeater(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Check()
    {
        foreach (var trigger in triggers) //상호작용한 것들을 for문으로 확인한다
        {
            if (trigger.state == false) //상호작용한 것중에 꺼진게 있으면
            {
                foreach (var obstacle in obstacles) //모든 목표를 다 끈다
                {
                    obstacle.Deactivate();
                }
                return;
            }
        }

        foreach (var obstacle in obstacles) //그게 아니라면 다 작동시킨다.
        {
            obstacle.Activate();
        }
    }
}
