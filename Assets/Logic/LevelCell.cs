using UnityEngine;
using System.Collections;

public class LevelCell
{
	public char iType;
	public int ix;
	public int iz;
	public float x;
	public float z;
	public bool hasHit = false;
	
	public bool IsBlock()
	{
		return iType == GameLevel.CELL_BLOCK;
	}

	public virtual void Init()
	{
	}
	
	public virtual bool Hit(Snake snake)
	{
		if (!hasHit)
			return false;
		hasHit = true;
		return true;
	}
	
	public static LevelCell Create(char iType, float fx, float fz)
	{
		LevelCell cell = null;
		switch (iType)
		{
		case GameLevel.CELL_FLOOR:
			cell = new LevelCellFloor();
			break;
		case GameLevel.CELL_BLOCK:
			cell = new LevelCellBlock();
			break;
		case GameLevel.CELL_BLOCK_SIDE_LEFT:
			cell = new LevelCellBlockSideLeft();
			break;
		case GameLevel.CELL_BLOCK_SIDE_RIGHT:
			cell = new LevelCellBlockSideRight();
			break;
		case GameLevel.CELL_BLOCK_NARROW:
			cell = new LevelCellBlockNarrow();
			break;
		case GameLevel.CELL_GOLD:
			cell = new LevelCellGold();
			break;
		default:
			Debug.Log("unknown cell type");
			return null;
		}
		cell.iType = iType;
		cell.x = fx;
		cell.z = fz;
		cell.ix = (int)(fx + 0.5f);
		cell.iz = (int)(fz + 0.5f);
		cell.Init();
		return cell;
	}
}

public class LevelCellFloor : LevelCell
{
	public override void Init()
	{
	}	
	
	public override bool Hit(Snake snake)
	{
		hasHit = true;
		snake.SetSpeedTo(10, 3);
		return false;
	}
}

public class LevelCellBlock : LevelCell
{
	public override void Init()
	{
	}
	
	public override bool Hit(Snake snake)
	{
		if (hasHit)
			return false;
		hasHit = true;
		
		snake.SetSpeedTo(5, 10);
		return true;
	}
}

public class LevelCellBlockSideLeft : LevelCell
{
	public override void Init()
	{
	}
	
	public override bool Hit(Snake snake)
	{
		if (snake.GetState() == Snake.State.SIDE)
			return false;
		if (hasHit)
			return false;
		hasHit = true;
		return true;
	}
}

public class LevelCellBlockSideRight : LevelCell
{
	public override void Init()
	{
	}
	
	public override bool Hit(Snake snake)
	{
		if (snake.GetState() == Snake.State.SIDE)
			return false;
		if (hasHit)
			return false;
		hasHit = true;
		return true;
	}
}

public class LevelCellBlockNarrow : LevelCell
{
	public override void Init()
	{
	}
	
	public override bool Hit(Snake snake)
	{
		if (snake.GetState() == Snake.State.NARROW)
			return false;
		if (hasHit)
			return false;
		hasHit = true;
		return true;
	}
	
}

public class LevelCellGold : LevelCell
{
	public override void Init()
	{
	}
	
	public override bool Hit(Snake snake)
	{
		if (hasHit)
			return false;

		hasHit = true;
		return true;
	}
}
