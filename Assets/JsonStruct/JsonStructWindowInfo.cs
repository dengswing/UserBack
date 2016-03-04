using UnityEngine;
using System.Collections;
using System.Globalization;
using System.IO;
using LitJson;

public class JsonStructWindowInfo
{
	private string path;
	public string Path
	{
		get { return this.path; }
	}

	private string name;
	public string Name 
	{
		get 
		{
			if (!string.IsNullOrEmpty(this.name))
				return this.name;

			var fileName = System.IO.Path.GetFileNameWithoutExtension(this.path);
			var ti = CultureInfo.CurrentCulture.TextInfo;
			return this.name = ti.ToTitleCase(fileName);
		}
	}

	private string json;
	public string Json
	{
		get 
		{ 
			if (!string.IsNullOrEmpty(this.json))
				return this.json;

			var sr = new StreamReader(path);
			var jsonReader = new JsonReader(sr);
			var jsonData = JsonMapper.ToObject(jsonReader);
			var keys = new string[jsonData.Keys.Count];
			jsonData.Keys.CopyTo(keys, 0);
			var key = keys[0];

			return this.json = jsonData[key].ToJson().Normalize();
		}
	}

	private string codes;
	public string Codes 
	{
		get { return this.codes; }
	}

	public JsonStructWindowInfo(string path)
	{
		this.path = path;
	}
}
