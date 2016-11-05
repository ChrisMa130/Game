using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MG
{
    public class TabFile
    {
        private int ColCount;  // 列数
        private Dictionary<string, int> ColIndex = new Dictionary<string, int>();

        private Dictionary<int, List<string>> TabInfo = new Dictionary<int, List<string>>();

        // 直接从字符串分析
        public static TabFile LoadFromString(string content)
        {
            TabFile tabFile = new TabFile();
            tabFile.ParseString(content);

            return tabFile;
        }

        public static TabFile LoadFromFile(string fileName)
        {
			if (Path.HasExtension(fileName)) 
			{
				fileName = fileName.Remove(fileName.LastIndexOf('.'));
			}

#pragma warning disable 0618
            var asset = Resources.Load<TextAsset>(fileName);
#pragma warning restore 0618
            if (asset == null)
                return null;

            return LoadFromString(asset.text);
        }

        private bool ParseReader(TextReader oReader)
        {
            string sLine = "";
            char[] separator = { '\t' };
            int indexLine = 1;

            sLine = oReader.ReadLine();
            if (sLine == null)
            {
                return true;
            }

            string[] splitString = sLine.Split(separator);
            for (int i = 1; i <= splitString.Length; i++)
            {
                ColIndex[splitString[i - 1].Trim()] = i;
            }
            ColCount = splitString.Length;

            List<string> arrlist = new List<string>(splitString);

            TabInfo[indexLine] = arrlist;
            indexLine++;
            while (sLine != null)
            {
                sLine = oReader.ReadLine();
                if (sLine != null)
                {
                    string[] splitString1 = sLine.Split(separator);
                    List<string> arrlist1 = new List<string>(splitString1);
                    while (arrlist1.Count < ColCount)
                        arrlist1.Add("");  // 小于header列数, 补空

                    TabInfo[indexLine] = arrlist1;
                    indexLine++;
                }
            }
            return true;
        }

        private bool ParseString(string content)
        {
            using (StringReader oReader = new StringReader(content))
            {
                ParseReader(oReader);
            }

            return true;
        }

        public bool LoadByIO(string fileName)
        {
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);  // 不会锁死, 允许其它程序打开

            StreamReader oReader;
            try
            {
                oReader = new StreamReader(fileStream, System.Text.Encoding.Default);
            }
            catch
            {
                return false;
            }

            ParseReader(oReader);

            oReader.Close();
            return true;
        }

        public bool Save(string fileName)
        {
            bool result = false;
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<int, List<string>> item in TabInfo)
            {
                int i = 0;
                foreach (string str in item.Value)
                {
                    i++;
                    sb.Append(str);
                    if (i != item.Value.Count)
                    {
                        sb.Append('\t');
                    }
                    else
                    {
                        sb.Append('\r');
                        sb.Append('\n');
                    }
                }
            }

            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default))
                    {
                        sw.Write(sb);

                        result = true;
                    }
                }
            }
            catch (IOException e)
            {
                Debug.LogError("可能文件正在被Excel打开?" + e.Message);
                result = false;
            }

            return result;
        }

        public string GetString(int row, int column)
        {
            if (column == 0) // 没有此列
                return string.Empty;

            return TabInfo[row][column - 1].ToString();
        }

        public string GetString(int row, string columnName)
        {
            int column;
            if (!ColIndex.TryGetValue(columnName, out column))
                return string.Empty;

            return GetString(row, column);
        }

        public int GetInteger(int row, int column)
        {
            try
            {
                string field = GetString(row, column);
                return int.Parse(field);
            }
            catch
            {
                return 0;
            }
        }

        public int GetInteger(int row, string columnName, int defaultValue = 0)
        {
            try
            {
                string field = GetString(row, columnName);
                return int.Parse(field);
            }
            catch
            {
                return defaultValue;
            }
        }

		public long GetLong(int row, int column)
		{
			try
			{
				string field = GetString(row, column);
				return long.Parse(field);
			}
			catch
			{
				return 0;
			}
		}
		
		public long GetLong(int row, string columnName, long defaultValue = 0)
		{
			try
			{
				string field = GetString(row, columnName);
				return long.Parse(field);
			}
			catch
			{
				return defaultValue;
			}
		}

        public uint GetUInteger(int row, int column)
        {
            try
            {
                string field = GetString(row, column);
                return uint.Parse(field);
            }
            catch
            {
                return 0;
            }
        }

        public uint GetUInteger(int row, string columnName)
        {
            try
            {
                string field = GetString(row, columnName);
                return uint.Parse(field);
            }
            catch
            {
                return 0;
            }
        }
        public double GetDouble(int row, int column)
        {
            try
            {
                string field = GetString(row, column);
                return double.Parse(field);
            }
            catch
            {
                return 0;
            }
        }

        public double GetDouble(int row, string columnName)
        {
            try
            {
                string field = GetString(row, columnName);
                return double.Parse(field);
            }
            catch
            {
                return 0;
            }
        }

        public float GetFloat(int row, int column)
        {
            try
            {
                string field = GetString(row, column);
                return float.Parse(field);
            }
            catch
            {
                return 0;
            }
        }

        public float GetFloat(int row, string columnName)
        {
            try
            {
                string field = GetString(row, columnName);
                return float.Parse(field);
            }
            catch
            {
                return 0;
            }
        }

        public float GetFloat(int row, string columnName, float defalutValue)
        {
            try
            {
                string field = GetString(row, columnName);
                return float.Parse(field);
            }
            catch
            {
                return defalutValue;
            }
        }

        public bool GetBool(int row, int column)
        {
            int field = GetInteger(row, column);
            return field != 0;
        }

        public bool GetBool(int row, string columnName)
        {
            int field = GetInteger(row, columnName);
            return field != 0;
        }

        public T GetEnum<T>(int row, string columnName, T defalutValue)
        {
            string s = GetString(row, columnName);
            if (string.IsNullOrEmpty(s))
                return defalutValue;

            try {
				return (T)System.Enum.Parse (typeof(T), s);
			} catch (Exception) {
				Debug.LogErrorFormat("第{0}行，不存在该{1}{2}", row, columnName, s);
				return defalutValue;
			}
        }

        public bool HasColumn(string colName)
        {
            return ColIndex.ContainsKey(colName);
        }

        public int NewColumn(string colName = "")
        {
            if (!string.IsNullOrEmpty(colName))  // 无列命，不保存字符索引
                ColIndex.Add(colName, ColIndex.Count + 1);
            ColCount++;

            if (!TabInfo.ContainsKey(1))
                TabInfo[1] = new List<string>();
            TabInfo[1].Add(colName);

            for (int i = 2; i <= TabInfo.Count; i++)
            {
                TabInfo[i].Add("");
            }

            return ColCount;
        }

        public int NewRow()
        {
            List<string> list = new List<string>();
            for (int i = 0; i < ColCount; i++)
            {
                list.Add("");
            }
            TabInfo.Add(TabInfo.Count + 1, list);
            return TabInfo.Count;
        }

        public int GetHeight()
        {
            return TabInfo.Count;
        }

        public int GetWidth()
        {
            return ColCount;
        }

        public bool SetValue<T>(int row, int column, T value)
        {
            if (row > TabInfo.Count || column > ColCount || row <= 0 || column <= 0)  //  || column > ColIndex.Count
            {
                return false;
            }
            string content = Convert.ToString(value);
            if (row == 1)
            {
                foreach (KeyValuePair<string, int> item in ColIndex)
                {
                    if (item.Value == column)
                    {
                        ColIndex.Remove(item.Key);
                        ColIndex[content] = item.Value;
                        break;
                    }
                }
            }
            TabInfo[row].RemoveAt(column - 1);
            TabInfo[row].Insert(column - 1, content);
            return true;
        }

        public bool SetValue<T>(int row, string columnName, T value)
        {
            int column;
            if (!ColIndex.TryGetValue(columnName, out column))
                return false;

            return SetValue(row, column, value);
        }
    }

}

