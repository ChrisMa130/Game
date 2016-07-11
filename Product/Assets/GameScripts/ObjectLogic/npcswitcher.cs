using UnityEngine;
using System.Collections;

namespace MG
{
    public class npcswitcher : MonoBehaviour
    {
        public GameObject[] Portals;

        public void OnSwitch()
        {
            if (Portals == null)
            {
                return;
            }

            int len = Portals.Length;
            for (int i = 0; i < len; i++)
            {
                var portal = Portals[i].GetComponent<NpcPortal>();
                portal.DestoryAllNpc();
            }
        }
    }
}

