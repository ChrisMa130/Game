using UnityEngine;
using System.Collections;
using System.ComponentModel;
using System.Deployment.Internal;
using UnityEngineInternal;

// 对键盘或者鼠标等收入设备的封装

namespace MG
{
    public class GameInput : MonoBehaviour
    {
        public bool Right       { get; private set; }
        public bool Left        { get; private set; }
        public bool Up          { get; private set; }
        public bool UpUp        { get; private set; }
        public bool Down        { get; private set; }
        public bool PauseTime   { get; private set; }
        public bool Jump        { get; private set; }
        public bool HasInput    { get; private set; }
        public bool Mouse1Down  { get; private set; }
        public bool Mouse1Up    { get; private set; }
        public bool Mouse1Move  { get; private set; }
        public bool MouseInput  { get; private set; }
        public bool TimebackDown { get; private set; }
        public bool Timebackup  { get; private set; }

        public delegate bool InputChecker();
        private InputChecker[] CheckerList;

        void Start()
        {
            CheckerList = new InputChecker[]
            {
                CheckRight,
                CheckUp,
                CheckDown,  
                CheckLeft,
                CheckJump,
                CheckPause,
                CheckMouse1Down,
                CheckMouse1Up,
                CheckMouse1Move,
                CheckUpUp,
                CheckTimebackDown,
                CheckTimebackUp
            };
        }

        public void Activate()
        {
            RecvInput();
        }

        private void RecvInput()
        {
            // Unity中的mono->forecah有内存泄漏，最好别用。尤其是在这种每帧都要执行的函数中。
            Reset();
            for (int i = 0; i < CheckerList.Length; i++)
            {
                CheckerList[i]();
            }
        }

        private bool CheckRight()
        {
            if (!Input.GetKey(KeyCode.D))
                return false;

            Right = true;
            HasInput = true;
            return true;
        }

        private bool CheckUp()
        {
            if (!Input.GetKey(KeyCode.W))
                return false;

            Up = true;
            HasInput = true;
            return true;
        }

        private bool CheckDown()
        {
            if (!Input.GetKey(KeyCode.S))
                return false;

            Down = true;
            HasInput = true;
            return true;
        }

        private bool CheckLeft()
        {
            if (!Input.GetKey(KeyCode.A))
                return false;

            Left = true;
            HasInput = true;
            return true;
        }

        private bool CheckJump()
        {
            if (!Input.GetButtonDown("Jump"))
                return false;

            Jump = true;
            HasInput = true;
            return true;
        }

        private bool CheckPause()
        {
            if (!Input.GetKeyDown(KeyCode.F))
                return false;

            PauseTime = true;
            HasInput = true;
            return true;
        }

        private bool CheckMouse1Down()
        {
            if (!Input.GetMouseButtonDown(0))
                return false;

            Mouse1Down = true;
            MouseInput = true;
            return true;
        }

        private bool CheckMouse1Up()
        {
            if (!Input.GetMouseButtonUp(0))
                return false;

            Mouse1Up = true;
            MouseInput = true;
            return true;
        }

        private bool CheckMouse1Move()
        {
            if (!Input.GetMouseButton(0))
                return false;

            Mouse1Move = true;
            MouseInput = true;
            return true;
        }

        private bool CheckUpUp()
        {
            if (!Input.GetKeyUp(KeyCode.W))
                return false;

            UpUp = true;
            HasInput = true;
            return true;
        }

        private bool CheckTimebackDown()
        {
            if (!Input.GetKeyDown(KeyCode.Q))
                return false;

            TimebackDown = true;
            HasInput = true;
            return true;
        }

        private bool CheckTimebackUp()
        {
            if (!Input.GetKeyUp(KeyCode.Q))
                return false;

            TimebackDown = true;
            HasInput = true;
            return true;
        }

        private void Reset()
        {
            Right       = false;
            Left        = false;
            Up          = false;
            Down        = false;
            PauseTime   = false;
            Jump        = false;
            HasInput    = false;
            Mouse1Down  = false;
            Mouse1Up    = false;
            Mouse1Move  = false;
            MouseInput  = false;
            UpUp        = false;
            TimebackDown    = false;
        }
    }
}
