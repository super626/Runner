using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLevel
{
	public const char CELL_FLOOR = ' ';
	public const char CELL_BLOCK = '#';
	public const char CELL_BLOCK_SPLIT = '\'';
	public const char CELL_BLOCK_NARROW = '\"';
	public const char CELL_GOLD = 'O';

	public const int MAP_MINX = -4;
	public const int MAP_MAXX = 4;

	public string Name;
	public GameLevel ()
	{
	}

	List<string> Map = new List<string>();
	int ReadMap(string[] lines, int startIndex) {
		int i = startIndex;
		Map.Clear();
		string line = "";
		while (i < lines.Length)
		{
			if (lines[i].Trim() == "End")
				return i;
			if (lines[i].StartsWith("x"))
			{
				int iRep = Convert.ToInt32(lines[i].Substring(1));
				for (int j = 0; j < iRep; j++)
					Map.Insert(0, line);
			}
			else 
			{
				line = lines[i];
				Map.Insert(0, line);
			}
			i++;
		}
		return i;
	}

	public bool LoadFromAsset(string fileName)
	{
		return LoadFromString(FileUtils.ReadTextAsset("levels/" + fileName));
	}

	public bool LoadFromFile(string fileName)
	{
		return LoadFromString(FileUtils.ReadTextFile(fileName));
	}

	public bool LoadFromString(string text) {
		String[] lines = text.Split(new char[] {'\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < lines.Length; i++)
		{
			string s = lines[i];
			String[] p = s.Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);
			if (p.Length == 0)
				continue;

			if (p[0] == "Name") {
				if (p.Length > 1)
					Name = p[1];
			} else if (p[0] == "Map") {
				i = ReadMap(lines, i + 1);
			}

		}
		return true;
	}

	public char GetCell(int x, int z) {
		z = z % Map.Count;
		if (z < 0)
			z += Map.Count;

		if (x < 0 || x >= Map[z].Length)
			return CELL_FLOOR;
		return Map[z][x];
	}

    public void SetCell(int x, int z, char cell)
    {
        x -= MAP_MINX; 
        if (z < 0 || z >= Map.Count || x < 0 || x >= GetWidth())
            return;
        string oldLine = Map[z];
        Map[z] = oldLine.Substring(0, z) + cell + oldLine.Substring(z + 1);
    }

    public int GetWidth() {
        return MAP_MAXX - MAP_MINX + 1;
    }

    public int GetHeight() {
        return Map.Count;
    }

    public void Resize(int newHeight)
    {
        if (Map.Count < newHeight)
        {
            for (int i = Map.Count; i < newHeight; i++)
            {
                Map.Add("         ");
            }
        }
        else
        {
            Map.RemoveRange(newHeight, Map.Count - newHeight);
        }
    }
}

