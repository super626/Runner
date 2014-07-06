using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLevel
{
	public const char CELL_FLOOR = ' ';
	public const char CELL_BLOCK = '#';
	public const char CELL_BLOCK_SIDE_LEFT = '<';
	public const char CELL_BLOCK_SIDE_RIGHT = '>';
	public const char CELL_BLOCK_NARROW = '^';
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

	public bool LoadFromSource(LevelSource source) {
		source.Data.Clear();
		source.Generate();
		ReadMap(source.Data.ToArray(), 0);
		return true;
	}


	public char GetCell(int x, int z) {
		z = z % Map.Count;
		if (z < 0)
			z += Map.Count;
		x -= MAP_MINX;
		if (x < 0 || x >= Map[z].Length)
			return CELL_FLOOR;
		return Map[z][x];
	}

	static bool bLastLeft = true;

	public bool ForwardSideIsLeft(int x, int z)
	{
		bLastLeft = !bLastLeft;

		for (int i = 0; i < 10; i++)
		{
			char cell = GetCell(x, z + i);

			if (cell == CELL_BLOCK_SIDE_LEFT)
			{
				bLastLeft = false;
				break;
			}
			else if (cell == CELL_BLOCK_SIDE_RIGHT)
			{
				bLastLeft = true;
				break;
			}
		}

		return bLastLeft;
	}
}

