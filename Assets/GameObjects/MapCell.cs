using UnityEngine;
using System.Collections;

public class MapCell
{
	public GameObject grid;
	public GameObject decal;
	public GameObject bonus;
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
	public virtual void Release()
	{
		ResManager.ReturnGridObject(grid);
		ResManager.ReturnDecalObject(decal);;
		if (bonus)
			bonus.SetActive(false);
	}

	public virtual void Init()
	{
	}

	public virtual void Hit(Snake snake)
	{
		hasHit = true;
	}

	public static MapCell Create(char iType, float fx, float fz)
	{
		MapCell cell = null;
		switch (iType)
		{
		case GameLevel.CELL_FLOOR:
			cell = new MapCellFloor();
			break;
		case GameLevel.CELL_BLOCK:
			cell = new MapCellBlock();
			break;
		case GameLevel.CELL_BLOCK_SIDE_LEFT:
			cell = new MapCellBlockSideLeft();
			break;
		case GameLevel.CELL_BLOCK_SIDE_RIGHT:
			cell = new MapCellBlockSideRight();
			break;
		case GameLevel.CELL_BLOCK_NARROW:
			cell = new MapCellBlockNarrow();
			break;
		case GameLevel.CELL_GOLD:
			cell = new MapCellGold();
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

public class MapCellFloor : MapCell
{
	public override void Init()
	{
		grid = ResManager.CreateGridObject(new Vector3(x, 0, z), ResManager.GRID_FLOOR);
	}	

	public override void Hit(Snake snake)
	{
		hasHit = true;
		snake.SetSpeedTo(10, 3);
	}
}

public class MapCellBlock : MapCell
{
	public override void Init()
	{
		grid = ResManager.CreateGridObject(new Vector3(x, 0, z), ResManager.GRID_BLOCK);
	}

	public override void Hit(Snake snake)
	{
		if (hasHit)
			return;
		hasHit = true;
		
		grid.GetComponent<MeshFilter>().mesh = ResManager.m_gridDarkBlockMesh;
		if (decal != null)
			ResManager.ReturnDecalObject(decal);
		decal = ResManager.CreateDecalObject(new Vector3(x + 0.5f, 0.705f, z + 0.5f), ResManager.DECAL_HIT);
		decal.transform.localScale = new Vector3(0.5f, 1, 0.5f);
		GameRuntime.ShakeCamera((new Vector3(0.2f, 0.4f, 1.0f)).normalized, 0.4f, 0.1f, 20);
		snake.SetSpeedTo(5, 10);

	}
}

public class MapCellBlockSideLeft : MapCell
{
	public override void Init()
	{
		grid = ResManager.CreateGridObject(new Vector3(x, 0, z), ResManager.GRID_BLOCK_SIDE_LEFT);
	}

	public override void Hit(Snake snake)
	{
		if (snake.GetState() == Snake.State.SIDE)
			return;
		if (hasHit)
			return;
		hasHit = true;
		GameRuntime.ShakeCamera((new Vector3(0.2f, 0.4f, 1.0f)).normalized, 0.5f, 0.2f, 20);
	}
}

public class MapCellBlockSideRight : MapCell
{
	public override void Init()
	{
		grid = ResManager.CreateGridObject(new Vector3(x, 0, z), ResManager.GRID_BLOCK_SIDE_RIGHT);
	}
	
	public override void Hit(Snake snake)
	{
		if (snake.GetState() == Snake.State.SIDE)
			return;
		if (hasHit)
			return;
		hasHit = true;
		GameRuntime.ShakeCamera((new Vector3(0.2f, 0.4f, 1.0f)).normalized, 0.5f, 0.2f, 20);
	}
}

public class MapCellBlockNarrow : MapCell
{
	public override void Init()
	{
		grid = ResManager.CreateGridObject(new Vector3(x, 0, z), ResManager.GRID_BLOCK_NARROW);
	}

	public override void Hit(Snake snake)
	{
		if (snake.GetState() == Snake.State.NARROW)
			return;
		if (hasHit)
			return;
		hasHit = true;
		
		GameRuntime.ShakeCamera((new Vector3(0.2f, 0.4f, 1.0f)).normalized, 0.5f, 0.2f, 20);
	}

}

public class MapCellGold : MapCell
{
	public override void Init()
	{
		grid = ResManager.CreateGridObject(new Vector3(x, 0, z), ResManager.GRID_FLOOR);
		bonus = GameObjectPool.CreateNew("objects/GoldBonus");
		bonus.transform.localPosition = new Vector3(x + 0.5f, 0.5f, z);
		bonus.GetComponent<Gold>().Stay();
	}

	public override void Hit(Snake snake)
	{
		if (hasHit)
			return;
		bonus.GetComponent<Gold>().Fly();
		GameObject par = ParticleMan.PlayParticle("gfx/GoldSpark", new Vector3(x + 0.5f, 0.0f, z + 0.5f));
		AdjustPos.AttachTo(par);
		SpriteCollect.CreateNew(new Vector3(x + 0.5f, 0.0f, z + 0.5f));
		hasHit = true;
	}
}