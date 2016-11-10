using System;
using UnityEngine;

namespace MG
{
    public class Diaries : TimeUnit
    {
        private int Id;
        void Start()
        {
            string name = transform.parent.name;
            int pos = name.LastIndexOf("_") + 1;
            Id = Convert.ToInt32(name.Substring(pos));

            if (GameData.Instance != null)
            {
                if (GameData.Instance.HasCollect(Id))
                    DestoryMe();
            }
        }
        void OnTriggerEnter2D(Collider2D other)
        {

            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                var player = other.gameObject.GetComponent<Player>();
                if (player != null)
                {
                    player.PickupDiarie(Id);
                    DestoryMe();
                }
            }
        }
    }

}
