using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using LuaInterface;

namespace LuaFramework
{
    public class PanelManager : Manager
    {
        private Transform parent;

        Transform Parent
        {
            get
            {
                if (parent == null)
                {
                    GameObject go = GameObject.FindWithTag("GuiCamera");
                    if (go != null) parent = go.transform;
                }
                return parent;
            }
        }


#if ASYNC_MODE

        /// <summary>
        /// ������壬������Դ������
        /// </summary>
        /// <param name="type"> �������</param>
        public void CreatePanel(string name, LuaFunction func = null)
        {
            string assetName = name + "Panel";
            string abName = name.ToLower() + AppConst.ExtName;
            ResManager.LoadPrefab(abName, assetName, delegate (UnityEngine.Object[] objs)
            {
                if (objs.Length == 0) return;
                var prefab = objs[0] as GameObject;
                ShowPanel(prefab, assetName, func);
            });
        }

        /// <summary>
        /// ������壬������Դ������
        /// </summary>
        /// <param name="type"> Asset�µ�·��</param>
        public void CreatePathPanel(string path, LuaFunction func)
        {
            ResManager.LoadPathPrefab<GameObject>(path, delegate (UnityEngine.Object[] objs)
            {
                if (objs.Length == 0) return;
                var prefab = objs[0] as GameObject;
                var assetName = ResManager.GetAssetName(path);
                ShowPanel(prefab, assetName, func);
            });
        }

        void ShowPanel(GameObject prefab, string assetName, LuaFunction func)
        {
            if (Parent.FindChild(assetName) != null || prefab == null)
            {
                return;
            }

            GameObject go = Instantiate(prefab) as GameObject;
            go.name = assetName;
            go.layer = LayerMask.NameToLayer("Default");
            go.transform.SetParent(Parent);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
            go.AddComponent<LuaBehaviour>();

            if (func != null) func.Call(go);
            Debug.LogWarning("CreatePanel::>> " + assetName + " " + prefab);
        }
#else
        /// <summary>
        /// ������壬������Դ������
        /// </summary>
        /// <param name="type"></param>
        public void CreatePanel(string name, LuaFunction func = null) {
            string assetName = name + "Panel";
            GameObject prefab = ResManager.LoadAsset<GameObject>(name, assetName);
            if (Parent.FindChild(name) != null || prefab == null) {
                return;
            }
            GameObject go = Instantiate(prefab) as GameObject;
            go.name = assetName;
            go.layer = LayerMask.NameToLayer("Default");
            go.transform.SetParent(Parent);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
            go.AddComponent<LuaBehaviour>();

            if (func != null) func.Call(go);
            Debug.LogWarning("CreatePanel::>> " + name + " " + prefab);
        }
#endif
    }
}