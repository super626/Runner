using UnityEngine;
using System.Collections;

public class LevelRuntime
{
	private GameLevel m_levelData;
	private float m_headZ;
	private Snake m_snake;
	private int m_goldNum;

	public GameLevel LevelData { get { return m_levelData;  } }
	public float HeadZ { get { return m_headZ; } }
	public Snake Snake { get { return m_snake; } }
	public int GoldNum { get { return m_goldNum; } }

	public void AddGoldNum(int amount) 
	{
		m_goldNum += amount;
	}

	public void Init()
	{
		m_levelData = new GameLevel();
		m_levelData.LoadFromSource(new Level1());
		m_snake = GameObject.Find("SnakeObject").GetComponent<Snake>();
		m_headZ = 0;
		m_goldNum = 0;
	}

	public void Update(float dt) {
		m_headZ += m_snake.GetSpeed() * dt;
	}

	static bool bLastLeft = true;
	
	public bool ForwardSideIsLeft(int x, int z)
	{
		bLastLeft = !bLastLeft;
		
		for (int i = 0; i < 10; i++)
		{
			char cell = m_levelData.GetCell(x, z + i);
			
			if (cell == GameLevel.CELL_BLOCK_SIDE_LEFT)
			{
				bLastLeft = false;
				break;
			}
			else if (cell == GameLevel.CELL_BLOCK_SIDE_RIGHT)
			{
				bLastLeft = true;
				break;
			}
		}
		
		return bLastLeft;
	}

}

