using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

public abstract class LevelSource
{
	protected List<string> m_Data = new List<string>();
	protected Dictionary<string, int> m_Labels = new Dictionary<string, int>();
	protected Stack<int> m_LoopBegins = new Stack<int>();
	public List<string> Data { get { return m_Data; } }
	public abstract void Generate();
	protected float m_GoldRate = 1;
	protected void Text(string s)
	{
		StringBuilder sb = new StringBuilder(s);
		for (int i = 0; i < sb.Length; i++)
		{
			switch (sb[i])
			{
			case GameLevel.CELL_GOLD:
				if (GameRuntime.random.NextDouble() > m_GoldRate)
				{
					sb[i] = GameLevel.CELL_FLOOR;
				}
				break;
			}
		}
		m_Data.Add(sb.ToString());
	}

	protected void Text(string s, int n)
	{
		for (int i = 0; i < n; i++)
		{
			Text(s);
		}
	}

	protected void Label(string name)
	{
		m_Labels[name] = m_Data.Count;
	}

	protected void RepeatLabel(string name, int n)
	{
		int begin = m_Labels[name];
		int end = m_Data.Count;
		for (int i = 0; i < n - 1; i++)
		{
			for (int j = begin; j < end; j++)
			{
				m_Data.Add(m_Data[j]);
			}
		}
	}

	protected void Loop()
	{
		m_LoopBegins.Push(m_Data.Count);
	}
	
	protected void Repeat(int n)
	{
		int begin = m_LoopBegins.Pop();
		int end = m_Data.Count;
		for (int i = 0; i < n - 1; i++)
		{
			for (int j = begin; j < end; j++)
			{
				m_Data.Add(m_Data[j]);
			}
		}
	}
}

