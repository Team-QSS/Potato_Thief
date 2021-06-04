using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJG
{


    public class Trigger : Interaction
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void InteractionBehavior()
        {
            if (state)
            {
                Deactivate();
            }
            else
            {
                Activate();
            }
        }
    }
}