using System.Collections.Generic;
using System.IO;
using SimpleJson;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MG
{
    public class LevelInfo
    {
        public string Name;
        public List<int> CollectItem;
    }

    public class GameSaveData
    {
        public LevelInfo CurrentLevel;
        public float X;
        public float Y;
        public float Z;
        public List<LevelInfo> PassLevel;

        public bool Load()
        {
            string dataPath = Application.persistentDataPath + "\\save.dat";
            string data = File.ReadAllText(dataPath);
            if (string.IsNullOrEmpty(data))
                return false;

            JsonObject obj = SimpleJson.SimpleJson.DeserializeObject(data) as JsonObject;
            if (obj == null)
                return false;

            var cl = LoadLevelInfo(obj);
            if (cl != null)
            {
                CurrentLevel = cl;
            }

            // 最后一次的储存点
            X = int.Parse(obj["X"].ToString());
            Y = int.Parse(obj["Y"].ToString());
            Z = int.Parse(obj["Z"].ToString());

            if (obj["PassLevel"] != null)
            {
                JsonArray pls = obj["PassLevel"] as JsonArray;
                PassLevel = new List<LevelInfo>();

                int count = pls.Count;
                for (int i = 0; i < count; i++)
                {
                    JsonObject pl = pls[i] as JsonObject;
                    var l = LoadLevelInfo(pl);
                    if (l != null)
                    {
                        PassLevel.Add(l);
                    }
                }
            }

            return true;
        }

        private LevelInfo LoadLevelInfo(JsonObject obj)
        {
            LevelInfo info = null;

            if (obj["CurrentLevel"] != null)
            {
                JsonObject cl = obj["CurrentLevel"] as JsonObject;
                info = new LevelInfo();
                info.Name = cl["Name"].ToString();

                if (cl["CollectItem"] != null)
                {
                    JsonArray ci = cl["CollectItem"] as JsonArray;
                    info.CollectItem = new List<int>();

                    int len = ci.Count;
                    for (int i = 0; i < len; i++)
                    {
                        int count = int.Parse(ci[i].ToString());
                        info.CollectItem.Add(count);
                    }
                }
            }

            return info;
        }

        public bool Save()
        {
            string t = SimpleJson.SimpleJson.SerializeObject(this);
            string dataPath = Application.persistentDataPath + "\\save.dat";

            File.WriteAllText(dataPath, t);

            return true;
        }

        public void SavePassLevelInfo()
        {
            if (PassLevel == null)
                PassLevel = new List<LevelInfo>();

            PassLevel.Add(CurrentLevel);
            CurrentLevel = null;
        }

        public void SaveCurrentLevelInfo()
        {
            if (CurrentLevel == null)
            {
                CurrentLevel = new LevelInfo();               
            }

            var sn = SceneManager.GetActiveScene();
            CurrentLevel.Name = sn.name;

            if (CurrentLevel.CollectItem == null)
            {
                CurrentLevel.CollectItem = new List<int>();
            }

            CurrentLevel.CollectItem.Clear();

            var player = GameMgr.Instance.PlayerLogic;
            for (int i = 0; i < GameDefine.CollectCount; i++)
            {
                int count = player.GetCollectCount(i);
                CurrentLevel.CollectItem.Add(count);
            }

            X = player.Position.x;
            Y = player.Position.y;
            Z = player.Position.z;
        }
    }
}

