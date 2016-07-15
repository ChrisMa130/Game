using UnityEngine;
using System.Collections;

namespace MG
{
    public class PlatformBase : MonoBehaviour
    {
        public virtual void TurnOn(GameObject obj) { }
        public virtual void TurnOff(GameObject obj) { }
        public virtual void TurnStay(GameObject obj) { }
    }
}



