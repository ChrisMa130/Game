using UnityEngine;
using System.Collections;

namespace MG
{
    public class ForbiddenZone : MonoBehaviour
    {
        Collider2D c2d;

        void Start()
        {
            c2d = GetComponent<Collider2D>();
        }

        void Update()
        {
            if (GameMgr.Instance.LineMgr == null)
                return;

            var line = GameMgr.Instance.LineMgr;
//            if (c2d.bounds.Contains(line.EndPos))
//                Debug.Log("穿过了");
            if (line.CurrentOpLine == null)
                return;

            var anotherC2d = line.CurrentOpLine.GetComponent<Collider2D>();
            if (c2d.bounds.Intersects(anotherC2d.bounds))
            {
                line.InForbiddenZone = true;
                Debug.Log("在里面");
            }
            else
            {
                line.InForbiddenZone = false;
                Debug.Log("不再里面");
            }
        }
    }
}


