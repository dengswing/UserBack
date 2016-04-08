using UnityEngine;
using UnityEditor;
using System.Collections;

public class BuildAssetbundle : Editor
{
	[MenuItem("BuildAssetbundle/BuildAll")]
	static void Build()
	{
		BuildPipeline.BuildAssetBundles(Application.dataPath+"/../Assetbundle",BuildAssetBundleOptions.UncompressedAssetBundle);
	}
}
