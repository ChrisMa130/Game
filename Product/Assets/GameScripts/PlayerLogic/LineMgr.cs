using UnityEngine;

namespace MG
{
    public class LineMgr : MonoBehaviour
    {
        private GameObject Line;
        private Vector3 StartPos;
        private Vector3 EndPos;
        private LineRenderer LineRender;
        private bool ValidLine;

        void Start()
        {
            var o = Resources.Load("prefabs/line") as GameObject;
            Line = Instantiate(o);
            Line.name = "Line";

            LineRender = Line.GetComponent<LineRenderer>();
            LineRender.SetVertexCount(2);
            LineRender.SetWidth(0.02f, 0.02f);

            Line.SetActive(false);
        }

        public void Activate(float deltaTime)
        {
            // 处理存活时间
        }

        public void ApplyInput(GameInput input)
        {
            // 如果是鼠标按下
            if (input.Mouse1Down)
            {
                SetStartPos();
            }
            else if (input.Mouse1Up)
            {
                SetEndPos();
            }
            else if (input.Mouse1Move)
            {
                MovePos();
            }
        }

        bool CheckPosition()
        {
            // 检查当前点是否能画线

            return false;
        }

        void SetStartPos()
        {
            ResetLine();
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            LineRender.SetPosition(0, mousePos);
            LineRender.SetPosition(1, mousePos);

            StartPos = mousePos;

            if (!Line.activeInHierarchy)
                Line.SetActive(true);
        }

        void SetEndPos()
        {
            if (!ValidLine)
            {
                ResetLine();
                Line.SetActive(false);
            }

            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            LineRender.SetPosition(1, mousePos);

            EndPos = mousePos;
            SetCollider();
        }

        void MovePos()
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            LineRender.SetPosition(1, mousePos);

            float lineLength = Vector3.Distance(StartPos, mousePos);
            if (lineLength > GameDefine.LineMaxLimit || lineLength <= GameDefine.LineMinLimit)
            {
                LineRender.SetColors(Color.red, Color.red);
                ValidLine = false;
            }
            else
            {
                LineRender.SetColors(Color.white, Color.white);
                ValidLine = true;
            }
        }

        void ResetLine()
        {
            LineRender.SetVertexCount(2);
            LineRender.SetWidth(0.02f, 0.02f);
            LineRender.SetPosition(0, Vector2.zero);
            LineRender.SetPosition(0, Vector2.zero);
            Line.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }

        private void SetCollider()
        {
            if (StartPos == EndPos)
                return;

            BoxCollider2D col = Line.gameObject.GetComponent<BoxCollider2D>();
            float lineLength = Vector3.Distance(StartPos, EndPos); // length of line
            col.size = new Vector2(lineLength, 0.02f);
            Vector3 midPoint = (StartPos + EndPos)/ 2;
            col.transform.localPosition = midPoint;

            float angle = (Mathf.Abs(StartPos.y - EndPos.y) / Mathf.Abs(StartPos.x - EndPos.x));
            if ((StartPos.y < EndPos.y && StartPos.x > EndPos.x) || (EndPos.y < StartPos.y && EndPos.x > StartPos.x))
            {
                angle *= -1;
            }

            angle = Mathf.Rad2Deg * Mathf.Atan(angle);
            col.transform.Rotate(0, 0, angle);
            col.offset = new Vector2(0, 0);
        }
    }
}
