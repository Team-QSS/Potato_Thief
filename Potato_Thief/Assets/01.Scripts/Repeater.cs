using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Repeater : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> interactionObjects;
    public List<GameObject> goalObjects;

    private List<Interaction> _interactions = new List<Interaction>();
    private List<Interaction> _goals = new List<Interaction>();
    
    void Start()
    {
        foreach (var interactionObject in interactionObjects)
        {
            Debug.Log(interactionObject);
        }
        
        foreach (var goalObject in goalObjects)
        {
            Debug.Log(goalObject);
        }
        
        foreach (var interactionObject in interactionObjects)
        {
            Interaction inter = interactionObject.GetComponent<Interaction>();
            _interactions.Add(inter);
            inter.SetReapeater(GetComponent<Repeater>());
        }

        foreach (var goalObject in goalObjects)
        {
            Interaction goal = goalObject.GetComponent<Interaction>();
            _goals.Add(goal);
            goal.SetReapeater(GetComponent<Repeater>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Check()
    {
        foreach (var interaction in _interactions) //상호작용한 것들을 for문으로 확인한다
        {
            if (interaction.state == false) //상호작용한 것중에 꺼진게 있으면
            {
                foreach (var goal in _goals) //모든 목표를 다 끈다
                {
                    goal.Deactivate();
                }
                return;
            }
        }

        foreach (var goal in _goals) //그게 아니라면 다 작동시킨다.
        {
            goal.Activate();
        }
    }
}
