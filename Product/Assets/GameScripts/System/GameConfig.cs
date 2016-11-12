using System;
using System.Collections.Generic;

// 游戏的各类配置文件

namespace MG
{
    public class GameConfig
    {
        // 书稿对应的文字描述
        public Dictionary<int, string> DocsDesc = new Dictionary<int, string>();

        public void Load()
        {
            LoadDiaries();
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

        public void TraversalDiarieId(Action<int> func)
        {
            DocsDesc.Forecah((i, s) =>
            {
                func(i);
            });
        }

        public string GetDiarieText(int id)
        {
            return DocsDesc.GetValue(id);
        }

    }
}


