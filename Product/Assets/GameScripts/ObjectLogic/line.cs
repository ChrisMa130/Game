using UnityEngine;
using System.Collections;

namespace MG
{
    public class line : MonoBehaviour
    {
        void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (GameMgr.Instance.LineMgr.LineAngle != 45)
                    return;

                var player = coll.gameObject.GetComponent<Player>();
                if (player != null)
                {
                    player.OnTheLine = true;
                }
            }

        }

        void OnCollisionExit2D(Collision2D coll)
        {
            if (coll.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                var player = coll.gameObject.GetComponent<Player>();
                if (player != null)
                {
                    player.OnTheLine = false;
                }
            }
        }
    }
}

