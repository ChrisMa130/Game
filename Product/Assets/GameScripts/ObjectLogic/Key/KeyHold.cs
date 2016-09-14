using UnityEngine;
using System.Collections;

namespace MG
{
    public class KeyHold : MonoBehaviour
    {
        public OpenDoor Door;

        public void OpenDoor()
        {
            Door.OpenTheDoor();
        }

    }

}

