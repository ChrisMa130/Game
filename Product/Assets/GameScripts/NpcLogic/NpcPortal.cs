using UnityEngine;
using System.Collections;

namespace MG
{
    public class NpcPortal : MonoBehaviour
    {
        public int ReviveCount;

        public Npc[] NpcTable;

        void Start()
        {
            if (ReviveCount > 0)
                NpcTable = new Npc[ReviveCount];
        }

        void Update()
        {

        }
    }
}


