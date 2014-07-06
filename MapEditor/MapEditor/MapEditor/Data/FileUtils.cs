using UnityEngine;
using System.Collections;
using System.IO;

public class FileUtils : MonoBehaviour
{
	public static string ReadTextFile(string FileName) 
	{
		StreamReader sr = new StreamReader(FileName);
		string text = sr.ReadToEnd();
		return text;
	}

	public static string ReadTextAsset(string FileName) 
	{
		TextAsset t = Resources.Load<TextAsset>(FileName);
		return t.text;
	}
}

