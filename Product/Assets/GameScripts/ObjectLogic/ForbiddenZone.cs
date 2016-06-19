using UnityEngine;

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
            if (line.CurrentOpLine == null)
                return;

            var anotherC2d = line.CurrentOpLine.GetComponent<Collider2D>();
            if (c2d.bounds.Intersects(anotherC2d.bounds))
            {
                line.InForbiddenZone = true;
            }
            else
            {
                line.InForbiddenZone = false;
            }
        }
    }
}


