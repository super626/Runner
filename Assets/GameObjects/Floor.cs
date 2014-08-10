using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Floor : MonoBehaviour {
	const int ROWS = 30;
	const int COLS = 17;

	public class GridRow {
		public MapCell[] m_cells;
		public GridRow(int z, GameLevel levelData)
		{
			m_cells = new MapCell[COLS];

			float x = -COLS * 0.5f;
			for (int i = 0; i < COLS; i++)
			{
				char iCell = levelData.GetCell((int)(x + i + 0.5f), z);
				m_cells[i] = MapCell.Create(iCell, x + i, z);
			}
		}

		public void ReleaseCells()
		{
			for (int i = 0; i < COLS; i++)
			{
				m_cells[i].Release();
			}
		}

		public float GetZ()
		{
			return m_cells[0].cellData.z;
		}
	}
	List<GridRow> m_rows = new List<GridRow>();
	int m_frontZ = 0;
	LevelRuntime m_level;

	public Floor()
	{
	}
	// Use this for initialization
	void Start () {
		ResetLevel (GameRuntime.curLevel);
	}


	public void ResetLevel(LevelRuntime level)
	{
		m_level = level;
		m_rows.Clear();
		for (int i = 0; i < ROWS; i++) 
		{
			m_rows.Add(new GridRow(i, m_level.LevelData));
		}
		m_frontZ = 0;
	}

	// Update is called once per frame
	void Update () {
		if (m_level == null)
			return;

		float startZ = m_level.HeadZ;
		while (startZ > m_rows[0].GetZ())
		{
			ReplaceRow();
		}
	}

	void ReplaceRow() {
		GridRow row = m_rows [0];
		m_rows.RemoveAt(0);
		m_rows.Add(new GridRow((int)row.GetZ() + ROWS, m_level.LevelData));
		row.ReleaseCells ();
		m_frontZ++;
	}

	GridRow GetRow(int z) {
		z -= m_frontZ;
		if (z < 0 || z >= m_rows.Count)
			return null;
		return m_rows[z];
	}

	public void HitCell(int x, int z, Snake snake) {
		GridRow row = GetRow(z);
		if (row == null)
			return;
		int ix = x + COLS / 2;
		if (ix < 0 || ix >= COLS)
			return;
		MapCell cell = row.m_cells[ix];
		cell.Hit(snake);
	}

	public MapCell GetCell(int x, int z)
	{
		GridRow row = GetRow(z);
		if (row == null)
			return null;
		int ix = x + COLS / 2;
		if (ix < 0 || ix >= COLS)
			return null;

		return row.m_cells[ix];
	}
}
