using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MG
{
    public class SaveBlack
    {
        public string LevelName;
        public string LevelASName;
        public List<int> CollectIds;
        public Vector3 BornPos;
    }

    public class GameData : SingletonMonoBehaviour<GameData>
    {
        private SaveBlack CurrentLevel;
        private List<SaveBlack> BlacksTable;

        private List<int> DiariesList;

        void Start()
        {
            DiariesList = new List<int>();
            BlacksTable = new List<SaveBlack>();
            GameObject.DontDestroyOnLoad(this);
        }

        public void SaveCollectPickup(int id)
        {
            CurrentLevel.CollectIds.Add(id);
        }

        public void SavePos(Vector3 pos)
        {
            CurrentLevel.BornPos = pos;
        }

        public bool HasCollect(int id)
        {
            foreach (var aId in CurrentLevel.CollectIds)
            {
                if (aId == id)
                    return true;
            }

            return false;
        }

        public void AddNewLevel(string levelName, string asName, Vector3 pos)
        {
            var data = GetLevelData(levelName);
            if (data != null)
                return;

            CurrentLevel = new SaveBlack();
            CurrentLevel.LevelName = levelName;
            CurrentLevel.LevelASName = asName;
            CurrentLevel.CollectIds = new List<int>();
            CurrentLevel.BornPos = pos;

            BlacksTable.Add(CurrentLevel);
        }

        public SaveBlack GetLevelData(string name)
        {
            SaveBlack ret = null;

            foreach (var black in BlacksTable)
            {
                if (black.LevelName.Equals(name))
                {
                    ret = black;
                    break;
                }
            }

            return ret;
        }

        public bool ChangeCurrentLevel(string name)
        {
            var data = GetLevelData(name);
            if (data == null)
                return false;

            CurrentLevel = data;

            return false;
        }
    }
}
