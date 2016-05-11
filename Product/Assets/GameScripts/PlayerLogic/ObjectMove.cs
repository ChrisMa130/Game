using UnityEngine;
using System.Collections;

// 对移动的封装
namespace MG
{
    public class ObjectMove : MonoBehaviour
    {
        private Transform MyTrans;

        private float Speed { get; set; }

        void Start()
        {
            MyTrans = gameObject.transform;
        }

        public void MoveRight(float detlaTime)
        {
            
        }

        public void MoveLeft(float detlaTime)
        {
            
        }

        public void MoveUp(float detlaTime)
        {
            
        }

        public void MoveDown(float detlaTime)
        {
            
        }

        public void Jump(float detlaTime)
        {
            
        }
    }

}


