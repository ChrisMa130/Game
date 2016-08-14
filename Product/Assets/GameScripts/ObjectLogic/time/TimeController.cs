using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MG
{
    public class TimeController : SingletonMonoBehaviour<TimeController>
    {
        /// <summary>
        /// 所有时间单位存储在这里，无论是否已经销毁
        /// </summary>
        private List<TimeUnit> Units = new List<TimeUnit>();

        public float TimeSpeed = 0.05f;

        private bool DoAction;

        public void AddUnit(TimeUnit unit)
        {
            Units.Add(unit);
        }

        void Update()
        {
            TimeSpeed -= Time.deltaTime;
            if (TimeSpeed > 0)
                return;
        }

    }
}

