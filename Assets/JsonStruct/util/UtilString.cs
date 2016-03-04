using System;
using UnityEngine;

public class UtilString
{
	public static string ToCamelCase(string str)
	{
		if (str == null)
			return null;
		
		if (str.Length > 1)
			return char.ToUpper(str[0]) + str.Substring(1);
		
		return str.ToUpper();
	}

	public static string TitleCaseString(string str, string split)
	{
		var s = str as String;
		if (s == null) return s;
		
		String[] words = s.Split(split.ToCharArray());
		for (int i = 0; i < words.Length; i++)
		{
			if (words[i].Length == 0) continue;
			
			Char firstChar = Char.ToUpper(words[i][0]); 
			String rest = "";
			if (words[i].Length > 1)
			{
				rest = words[i].Substring(1).ToLower();
			}
			words[i] = firstChar + rest;
		}
		return String.Join(split, words).ToString();
	}

	public static Vector3 ToVector(string str)
	{
		string[] ss = str.Split(',');
		var pos = Vector3.zero;
		if (ss.Length >= 1)
			pos.x = float.Parse(ss[0]);
		if (ss.Length >= 2)
			pos.y = float.Parse(ss[1]);
		if (ss.Length >= 3)
			pos.z = float.Parse(ss[2]);
		return pos;
	}
}
