using System;
using UnityEngine;

namespace MG
{
    public class LineMgr : MonoBehaviour
    {
        private GameObject Line;

        private Vector3 StartPos;
        private Vector3 EndPos;

        private bool ValidLine;

        private GameObject CurrentOpLine;
        private LineRenderer CurrentLineRenderer;

        public void Activate(float deltaTime)
        {
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

        void CreateLine()
        {
            var o = Resources.Load("prefabs/line") as GameObject;
            CurrentOpLine = Instantiate(o);
            CurrentLineRenderer = CurrentOpLine.GetComponent<LineRenderer>();
            CurrentLineRenderer.SetVertexCount(2);
            CurrentLineRenderer.SetWidth(0.02f, 0.02f);
        }

        void SetStartPos()
        {
            if (CurrentOpLine == null)
            {
                CreateLine();
            }

            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            CurrentLineRenderer.SetPosition(0, mousePos);
            CurrentLineRenderer.SetPosition(1, mousePos);

            StartPos = mousePos;

            CurrentOpLine.SetActive(true);
        }

        void SetEndPos()
        {
            if (!ValidLine)
            {
                GameObject.DestroyImmediate(CurrentOpLine);
                return;
            }

            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            CurrentLineRenderer.SetPosition(1, mousePos);

            EndPos = mousePos;
            SetCollider();

            if (Line != null)
            {
                var s = Line.AddComponent<autodestory>();
                s.DestoryTime = GameDefine.DisableLineLiveTime;
                var b2d = Line.GetComponent<BoxCollider2D>();
                if (b2d != null)
                {
                    GameObject.DestroyImmediate(b2d);
                }
            }

            Line = CurrentOpLine;
            CurrentOpLine = null;
        }

        void MovePos()
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            CurrentLineRenderer.SetPosition(1, mousePos);

            float lineLength = Vector3.Distance(StartPos, mousePos);
            if (lineLength > GameDefine.LineMaxLimit || lineLength <= GameDefine.LineMinLimit)
            {
                CurrentLineRenderer.SetColors(Color.red, Color.red);
                ValidLine = false;
            }
            else
            {
                CurrentLineRenderer.SetColors(Color.white, Color.white);
                ValidLine = true;
            }
        }

        private void SetCollider()
        {
            if (StartPos == EndPos)
                return;

            BoxCollider2D col = CurrentOpLine.gameObject.GetComponent<BoxCollider2D>();
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
