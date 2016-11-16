using UnityEngine;
using System.Collections;

namespace MG
{
    public class KeyHold : TimeUnit
    {
        public OpenDoor Door;

        public void OpenDoor()
        {
            Door.OpenTheDoor();
        }

    }

}

