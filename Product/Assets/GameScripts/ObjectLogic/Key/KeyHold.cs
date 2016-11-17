using UnityEngine;
using System.Collections;

namespace MG
{
    public class KeyHold : TimeUnit
    {
        public Sprite open;
        public Sprite closed;

        public OpenDoor Door;

        void Update()
        {


        }

        public void OpenDoor()
        {
            Door.OpenTheDoor();
        }

    }

}

