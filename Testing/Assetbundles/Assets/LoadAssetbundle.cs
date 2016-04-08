﻿using UnityEngine;
using System.Collections;

public class LoadAssetbundle : MonoBehaviour {
	
	void OnGUI()
	{
		if(GUILayout.Button("LoadAssetbundle"))
		{
			//首先加载Manifest文件;
			AssetBundle manifestBundle=AssetBundle.CreateFromFile(Application.dataPath
			                                                      +"/../Assetbundle/Assetbundle");
			if(manifestBundle!=null)
			{
				AssetBundleManifest manifest=(AssetBundleManifest)manifestBundle.LoadAsset("AssetBundleManifest");
			
				//获取依赖文件列表;
				string[] cubedepends=manifest.GetAllDependencies("assets/myresources/cube0.prefab");
				AssetBundle[] dependsAssetbundle=new AssetBundle[cubedepends.Length];

				for(int index=0;index<cubedepends.Length;index++)
				{
					//加载所有的依赖文件;
					dependsAssetbundle[index]=AssetBundle.CreateFromFile(Application.dataPath
					                                                     +"/../Assetbundle/"
					                                                     +cubedepends[index]);





				}

				//加载我们需要的文件;"
				AssetBundle cubeBundle=AssetBundle.CreateFromFile(Application.dataPath
				                                                  +"/../Assetbundle/assets/myresources/cube0.prefab" );
				GameObject cube=cubeBundle.LoadAsset("Cube0") as GameObject;
				if(cube!=null)
				{
					Instantiate(cube);
				}
			}


		}
	}


}
