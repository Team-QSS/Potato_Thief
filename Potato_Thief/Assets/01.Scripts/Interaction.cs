using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJG
{
    public class Interaction : MonoBehaviour
    {
        // Start is called before the first frame update
        public bool state;

        //private
        public Repeater repeater;

        void Start()
        {
            state = false;
        }

        public void SetReapeater(Repeater repeater)
        {
            this.repeater = repeater;
        }

        public virtual void InteractionBehavior()
        {
        }

        public virtual void Deactivate()
        {
        }

        public virtual void Activate()
        {
        }


    }
}
