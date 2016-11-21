using System;
using System.Collections.Generic;

// 游戏的各类配置文件

namespace MG
{
    public class GameConfig
    {
        // 书稿对应的文字描述
        private Dictionary<int, string> DocsDesc = new Dictionary<int, string>();
        private Dictionary<string, string> LevelsInfo = new Dictionary<string, string>();

        public void Load()
        {
            LoadDiaries();
            LoadLevels();
        }

        private void LoadDiaries()
        {
            SettingReader.Load("DiariesText", (l, i) =>
            {
                int id = l.GetInteger(i, "Id");
                string t = l.GetString(i, "Text");

                DocsDesc.Add(id, t);
            });
        }

        private void LoadLevels()
        {
            SettingReader.Load("Levels", (l, i) =>
            {
                string name = l.GetString(i, "Name");
                string nikeName = l.GetString(i, "NikeName");

                LevelsInfo.Add(name, nikeName);
            });
        }

        public void TraversalDiarieId(Action<int> func)
        {
            DocsDesc.Forecah((i, s) =>
            {
                func(i);
            });
        }

        public void Traversallevels(Action<string, string> func)
        {
            LevelsInfo.Forecah(func);
        }

        public string GetDiarieText(int id)
        {
            return DocsDesc.GetValue(id);
        }
    }
}


