using System;

namespace MG
{
    public static class SettingReader
    {
        public static void Load(string fileName, Action<TabFile> EnumeratorFunc)
        {
            if (string.IsNullOrEmpty(fileName) || EnumeratorFunc == null)
                return;

            var tabFile = TabFile.LoadFromFile(string.Format("Resources/Setting/{0}", fileName));

            if (tabFile == null)
                return;

            int maxRow = tabFile.GetHeight();
            if (maxRow < 2)
                return;

            for (int i = 2; i <= maxRow; i++)
            {
                EnumeratorFunc(tabFile);
            }
        }
    }
}

