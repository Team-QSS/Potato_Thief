using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interaction
{
    // Start is called before the first frame update
    
    private Vector2 _pos = new Vector2();
    void Start()
    {
        
        _pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Deactivate()
    {
        state = false;
        repeater.Check();
        Debug.Log("Lever 꺼짐");
        transform.position = _pos;
    }
    
    public override void Activate()
    {
        state = true;
        repeater.Check();
        Debug.Log("Lever 켜짐");
        transform.position = new Vector2(_pos.x, _pos.y - 1);
    }
}
