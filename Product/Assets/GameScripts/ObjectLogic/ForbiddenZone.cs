using UnityEngine;
using System.Collections;

namespace MG
{
    public class ForbiddenZone : MonoBehaviour
    {
        void OnMouseDown()
        {
            GameMgr.Instance.LineMgr.CanDraw = false;
        }

        void OnMouseOver()
        {
            if (GameMgr.Instance.LineMgr.CanDraw)
                GameMgr.Instance.LineMgr.CanDraw = false;
        }
        void OnMouseExit()
        {
            GameMgr.Instance.LineMgr.CanDraw = true;
        }
    }

}


