using System.Collections.Generic;
using System.IO;
using SimpleJson;
using UnityEngine;

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


            return false;
        }

        public bool Save()
        {
            return false;
        }

    }
}

