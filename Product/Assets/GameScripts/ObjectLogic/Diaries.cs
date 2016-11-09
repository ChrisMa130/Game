using System;
using UnityEngine;
using System.Collections;

namespace MG
{
    public class Diaries : TimeUnit
    {
        void OnTriggerEnter2D(Collider2D other)
        {
            string name = transform.parent.name;
            int pos = name.LastIndexOf("_") + 1;
            int id = Convert.ToInt32(name.Substring(pos));
            Debug.Log("Id = " + id);
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                var player = other.gameObject.GetComponent<Player>();
                if (player != null)
                {
                    player.PickupDiarie(id);
                    DestoryMe();
                }
            }
        }
    }

}
