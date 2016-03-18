using System;
using System.Collections.Generic;
using System.IO;

namespace StructGenerate
{
    /// <summary>
    /// 读取php文件
    /// </summary>
    internal class ReaderPhp
    {
        public Dictionary<string, string> ReadPhpToStructTable(string sPath)
        {
            return ReadAndParsePhp(sPath);
        }

        Dictionary<string, string> ReadAndParsePhp(string sPath)
        {
            var _rstream = new StreamReader(sPath, System.Text.Encoding.UTF8);
            string allContent = _rstream.ReadToEnd();
            _rstream.Close();

            allContent = allContent.Replace(ReadConst.LineSpace, ReadConst.LineBlank);
            StringReader reader = new StringReader(allContent);
            string readText = reader.ReadLine();
            string sPrev;
            var tableList = new Dictionary<string, string>();

            while (readText != null)
            {
                sPrev = readText;
                readText = reader.ReadLine();
                if (String.IsNullOrEmpty(readText)) continue;
                var index = readText.IndexOf(ReadConst.splitTable);
                if (index == -1) continue;

                var length = readText.Length;
                var end = length - readText.LastIndexOf(ReadConst.singleQuotes);
                index += ReadConst.splitTable.Length;

                string sDataBase = readText.Substring(index + 1, length - index - 1 - end);
                sDataBase = sDataBase.Replace(ReadConst.singleQuotes, ReadConst.LineBlank).Trim();

                index = sPrev.IndexOf(ReadConst.singleQuotes);
                length = sPrev.Length;
                end = length - sPrev.LastIndexOf(ReadConst.singleQuotes);
                string sTableName = sPrev.Substring(index + 1, length - index - 1 - end);

                sTableName = sTableName.Trim();
                tableList.Add(sDataBase, sTableName);
            }

            return tableList;
        }

        bool IndexOfTableName(string sValue)
        {
            return (sValue.IndexOf(ReadConst.splitTable) != -1);
        }
    }

}