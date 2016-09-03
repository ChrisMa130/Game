using UnityEngine;
using System.Collections;

namespace MG
{
    public class RevivePoint : MonoBehaviour
    {
        public Vector3 Pos;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (TimeController.Instance.IsOpTime())
                return;

            var player = other.GetComponent<Player>();
            if (player != null)
                player.SetRevivePoint(Pos);
        }
    }


}

