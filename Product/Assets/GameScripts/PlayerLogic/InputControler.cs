using UnityEngine;
using System.Collections;
using System.ComponentModel;
using System.Deployment.Internal;

// 对键盘或者鼠标等收入设备的封装

namespace MG
{
    public class GameInput : MonoBehaviour
    {
        public bool Right       { get; private set; }
        public bool Left        { get; private set; }
        public bool Up          { get; private set; }
        public bool Down        { get; private set; }
        public bool PauseTime   { get; private set; }
        public bool Jump        { get; private set; }

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
                CheckPause
            };
        }

        void LateUpdate()
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
            return true;
        }

        private bool CheckUp()
        {
            if (!Input.GetKey(KeyCode.W))
                return false;

            Up = true;
            return true;
        }

        private bool CheckDown()
        {
            if (!Input.GetKey(KeyCode.S))
                return false;

            Down = true;
            return true;
        }

        private bool CheckLeft()
        {
            if (!Input.GetKey(KeyCode.A))
                return false;

            Left = true;
            return true;
        }

        private bool CheckJump()
        {
            if (!Input.GetButtonDown("Jump"))
                return false;

            Jump = true;
            return true;
        }

        private bool CheckPause()
        {
            if (!Input.GetKey(KeyCode.E))
                return false;

            PauseTime = true;
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
        }
    }
}
