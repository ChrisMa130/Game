using System;
using UnityEngine;

namespace MG
{
    public class LineMgr : MonoBehaviour
    {
        private GameObject Line;
        private GameObject ForbiddenLine;

        private Vector3 StartPos;
        private Vector3 EndPos;

        private Vector3 LastStartPos;
        private Vector3 LastEndPos;

        private bool ValidLine;

        private GameObject CurrentOpLine;
        private LineRenderer CurrentLineRenderer;

        public bool CanDraw { get; set; }

        void Start()
        {
            CanDraw = true;
        }

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
			CurrentLineRenderer.SetWidth(GameDefine.LineSize, GameDefine.LineSize);
        }

        void SetStartPos()
        {
            if (!CanDraw)
                return;

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
            if (!ValidLine || !CanDraw)
            {
                if (ForbiddenLine != null)
                {
                    GameObject.DestroyImmediate(ForbiddenLine);
                    ForbiddenLine = null;
                }
                    
                GameObject.DestroyImmediate(CurrentOpLine);
                return;
            }

            SetCollider(CurrentOpLine, StartPos, EndPos);

            if (Line != null)
            {
                // 当前线段消失
                // 给领域加一个自动消失的脚本
                var s = ForbiddenLine.AddComponent<autodestory>();
                s.DestoryTime = GameDefine.DisableLineLiveTime;
                ForbiddenLine = null;
                GameObject.DestroyImmediate(Line);
                Line = null;
            }

            LastEndPos = StartPos;
            LastStartPos = EndPos;

            Line = CurrentOpLine;

            CreateForbiddenZone();

            CurrentOpLine = null;
            CurrentLineRenderer = null;
        }

        void CreateForbiddenZone()
        {
            var o = Resources.Load("prefabs/ForbiddenZone") as GameObject;
            ForbiddenLine = Instantiate(o);

            var renderer = ForbiddenLine.GetComponent<LineRenderer>();

            renderer.SetVertexCount(2);
            var size = GameDefine.LineSize + GameDefine.ForbiddenLineAddWidth;
            renderer.SetWidth(size, size);

            var start = CalcLinePos(StartPos, EndPos, GameDefine.ForbiddenLineAddLen);
            var end = CalcLinePos(EndPos, StartPos, GameDefine.ForbiddenLineAddLen);

            SetCollider(ForbiddenLine, start, end);

            renderer.SetPosition(0, start);
            renderer.SetPosition(1, end);
        }

        void MovePos()
        {
            if (!CanDraw || CurrentLineRenderer == null)
                return;

            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            if (mousePos == StartPos)
                return;

            float angle = GetAngle(StartPos, mousePos);
            var len = mousePos.x - StartPos.x;
            float theTa = Mathf.Round(angle/45.0f)*(45.0f);
            if ((int) Mathf.Abs(theTa) == 90)
            {
                mousePos.x = StartPos.x;
            }
            else
            {
                mousePos.y = StartPos.y + len * Mathf.Tan(theTa);
            }
            CurrentLineRenderer.SetPosition(1, mousePos);

            EndPos = mousePos;

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

        private void SetCollider(GameObject obj, Vector3 start, Vector3 end)
        {
            if (start == end || CurrentOpLine == null)
                return;

            BoxCollider2D col = obj.GetComponent<BoxCollider2D>();
            float lineLength = Vector3.Distance(start, end); // length of line
			col.size = new Vector2(lineLength, GameDefine.LineSize);
            Vector3 midPoint = (start + end) / 2;
            col.transform.localPosition = midPoint;

            float angle = GetAngle(start, end);
            col.transform.Rotate(0, 0, angle);
            col.offset = new Vector2(0, 0);
        }

        private Vector3 CalcLinePos(Vector3 A, Vector3 B, float len)
        {
            var lenAB = Mathf.Sqrt((B.y - A.y)*((B.y - A.y)) + (B.x - A.x)*(B.x - A.x));// 计算2点的长度

            float dy = B.y - A.y;
            float dx = B.x - A.x;

            float sinT = 0f, cosT = 0f;
            float x = 0f, y = 0f;

            if (!GameDefine.FloatIsZero(dy) && !GameDefine.FloatIsZero(dx))
            {
                sinT = dy/lenAB;
                cosT = dx/lenAB;
                y = B.y + len*sinT;
                x = B.x + len*cosT;
            }
            else if (GameDefine.FloatIsZero(dy) && dx > 0.0f)
            {
                y = B.y;
                x = B.x + len;
            }
            else if (dy > 0.0f && GameDefine.FloatIsZero(dx))
            {
                y = B.y + len;
                x = B.x;
            }
            else if (GameDefine.FloatIsZero(dy) && dx < 0.0f)
            {
                y = B.y;
                x = B.x - len;
            }
            else if (dy < 0.0f && GameDefine.FloatIsZero(dx))
            {
                y = B.y - len;
                x = B.x;
            }

            return new Vector3(x, y, 0);
        }

        private float GetAngle(Vector3 a, Vector3 b)
        {
            float angle = (Mathf.Abs(a.y - b.y) / Mathf.Abs(a.x - b.x));
            if ((a.y < b.y && a.x > b.x) || (b.y < a.y && b.x > a.x))
            {
                angle *= -1;
            }

            return Mathf.Rad2Deg * Mathf.Atan(angle);
        }
    }
}
