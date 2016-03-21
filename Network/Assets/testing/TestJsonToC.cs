using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Networks;
using Networks.parser;
using System.Collections.Generic;
using System.Text;

using System;
using System.IO;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections;


class TestJsonToC : MonoBehaviour
{
     List<string> result = new List<string>();
        void Start()
        {

            string json = File.ReadAllText(Application.dataPath + "/Resources/JsonTable/sg_Test.json");
                 var data = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                string outKey = string.Empty;
                GenInfo genInfo = new GenInfo();
                genInfo.KeyName = "Root";
                this.Gen(data, genInfo, out outKey);

                this.GenCode(genInfo);



                Debug.Log(result);

            
            Console.Read();
        }


       

        public void GenCode(GenInfo info)
        {

            string code = string.Empty;
            code += "public class " + info.KeyName;
            code += "{";
            foreach (var item in info.Items)
            {
                GenCode(item);
                if (item.KeyType == "field" || item.IsLeaf == true)
                {
                    if (item.Items.Count == 0)
                        code += "public string " + item.KeyName + " { get; set; } ";
                    else
                        code += "public " + item.KeyName + " " + item.KeyName + " { get; set; } ";
                }
                else if (item.KeyType == "list")
                {
                    code += "public List<" + item.KeyName + "> " + item.KeyName + " { get; set; } ";
                }
            }
            code += "}";

            if (info.Items.Count > 0 && info.KeyType != "list")
                result.Add(code);
        }

        public void Gen(object obj, GenInfo genInfo, out string childrenKey)
        {

            childrenKey = string.Empty;
            if (this.TryConvert2Dic(obj))
            {
                genInfo.KeyType = "field";

                Dictionary<string, object> dic = (Dictionary<string, object>)(obj);
                foreach (var dicItem in dic)
                {
                    GenInfo gen = new GenInfo();
                    gen.KeyName = dicItem.Key;
                    genInfo.Items.Add(gen);
                    Gen(dicItem.Value, gen, out childrenKey);
                }
            }
            else if (this.TryConvert2Array(obj))
            {

                genInfo.KeyType = "list";
                ArrayList array = (ArrayList)obj;
                GenInfo gen = new GenInfo();
                gen.KeyName = genInfo.KeyName;
                genInfo.Items.Add(gen);
                Gen(array[0], gen, out childrenKey);
                //gen.KeyName = childrenKey;
            }
            else
            {
                genInfo.IsLeaf = true;
            }
        }

        public bool TryConvert2Dic(object obj)
        {
            try
            {
                Dictionary<string, object> dic = (Dictionary<string, object>)(obj);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }




        public bool TryConvert2Array(object obj)
        {
            try
            {
                ArrayList array = (ArrayList)(obj);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }


    public class GenInfo
    {
        public string KeyType { get; set; }
        public string KeyName { get; set; }
        public bool IsLeaf { get; set; }

        public List<GenInfo> Items { get; set; }
        public GenInfo() { this.Items = new List<GenInfo>(); }

    }