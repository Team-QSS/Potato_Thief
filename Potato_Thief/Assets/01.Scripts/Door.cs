using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJG
{
    public class Door : Obstacle
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
            Debug.Log("Door 꺼짐");
            state = false;
            transform.position = _pos;
        }

        public override void Activate()
        {
            Debug.Log("Door 켜짐");
            state = true;
            transform.position = new Vector2(_pos.x, _pos.y - 1);
        }
    }
}