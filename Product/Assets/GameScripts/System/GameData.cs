using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MG
{
    public class GameData : MonoBehaviour
    {
        class SaveBlack
        {
            public string LevelName;
            public string LevelASName;
            public List<int> CollectIds;
            public Vector3 BornPos;
        }

        private SaveBlack CurrentLevel;
        private List<SaveBlack> BlacksTable;

        // 如果这个开关为真，那么gamemgr在启动时会读取保存的数据
        public bool LoadSwitch { get; set; }

        void Start()
        {
            BlacksTable = new List<SaveBlack>();
            GameObject.DontDestroyOnLoad(this);
        }

        public void SaveColloctPickup(int id)
        {
            CurrentLevel.CollectIds.Add(id);
        }

        public void SavePos(Vector3 pos)
        {
            CurrentLevel.BornPos = pos;
        }

        public void AddSaveNewLevel(string levelName, string asName, Vector3 pos)
        {
            if (CurrentLevel != null)
            {
                BlacksTable.Add(CurrentLevel);
            }

            CurrentLevel = new SaveBlack();
            CurrentLevel.LevelName = levelName;
            CurrentLevel.LevelASName = asName;
            CurrentLevel.CollectIds = new List<int>();
            CurrentLevel.BornPos = pos;
        }
    }
}
