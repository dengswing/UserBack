/************************** 
     * 文件名:AutoSetTextureUISprite.cs; 
     * 文件描述:自动设置Assetbundle名字为文件夹名_文件名.unity3d; 
     * 创建日期:2015/05/04; 
     * Author:陈鹏; 
     ***************************/  

using UnityEngine;  
using System.Collections;  
using UnityEditor;  

public class AutoSetTextureUISprite :AssetPostprocessor   
{  

	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) 
	{
		//foreach (var str in importedAssets)
		//{
		//	//Debug.Log("Reimported Asset: " + str);
	
		//	if(!str.EndsWith(".cs"))
		//	{
		//		AssetImporter importer=AssetImporter.GetAtPath(str);
		//		importer.assetBundleName=str;

  //          }

		//}
		//foreach (var str in deletedAssets) 
		//{
		//	//Debug.Log("Deleted Asset: " + str);
		//	if(!str.EndsWith(".cs"))
		//	{
		//		AssetImporter importer=AssetImporter.GetAtPath(str);
		//		importer.assetBundleName=str;
  //          }
  //      }
		
		//for (var i=0; i<movedAssets.Length; i++)
		//{
		//	//Debug.Log("Moved Asset: " + movedAssets[i] + " from: " + movedFromAssetPaths[i]);
		//}
	}
}  