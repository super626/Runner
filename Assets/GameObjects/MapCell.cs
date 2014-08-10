using UnityEngine;
using System.Collections;

public class MapCell
{
	public GameObject grid;
	public GameObject decal;
	public GameObject bonus;
	public LevelCell cellData;

	public bool IsBlock()
	{
		return cellData.iType == GameLevel.CELL_BLOCK;
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
		cellData.Hit (snake);
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
		cell.cellData = LevelCell.Create (iType, fx, fz);
		cell.Init();
		return cell;
	}
}

public class MapCellFloor : MapCell
{
	public override void Init()
	{
		grid = ResManager.CreateGridObject(new Vector3(cellData.x, 0, cellData.z), ResManager.GRID_FLOOR);
	}	

	public override void Hit(Snake snake)
	{
		cellData.Hit (snake);
	}
}

public class MapCellBlock : MapCell
{
	public override void Init()
	{
		grid = ResManager.CreateGridObject(new Vector3(cellData.x, 0, cellData.z), ResManager.GRID_BLOCK);
	}

	public override void Hit(Snake snake)
	{
		if (!cellData.Hit(snake))
			return;

		grid.GetComponent<MeshFilter>().mesh = ResManager.m_gridDarkBlockMesh;
		if (decal != null)
			ResManager.ReturnDecalObject(decal);
		decal = ResManager.CreateDecalObject(new Vector3(cellData.x + 0.5f, 0.705f, cellData.z + 0.5f), ResManager.DECAL_HIT);
		decal.transform.localScale = new Vector3(0.5f, 1, 0.5f);
		GameRuntime.ShakeCamera((new Vector3(0.2f, 0.4f, 1.0f)).normalized, 0.4f, 0.1f, 20);
		snake.SetSpeedTo(5, 10);

	}
}

public class MapCellBlockSideLeft : MapCell
{
	public override void Init()
	{
		grid = ResManager.CreateGridObject(new Vector3(cellData.x, 0, cellData.z), ResManager.GRID_BLOCK_SIDE_LEFT);
	}

	public override void Hit(Snake snake)
	{
		if (!cellData.Hit(snake))
			return;
		GameRuntime.ShakeCamera((new Vector3(0.2f, 0.4f, 1.0f)).normalized, 0.5f, 0.2f, 20);
	}
}

public class MapCellBlockSideRight : MapCell
{
	public override void Init()
	{
		grid = ResManager.CreateGridObject(new Vector3(cellData.x, 0, cellData.z), ResManager.GRID_BLOCK_SIDE_RIGHT);
	}
	
	public override void Hit(Snake snake)
	{
		if (!cellData.Hit(snake))
			return;

		GameRuntime.ShakeCamera((new Vector3(0.2f, 0.4f, 1.0f)).normalized, 0.5f, 0.2f, 20);
	}
}

public class MapCellBlockNarrow : MapCell
{
	public override void Init()
	{
		grid = ResManager.CreateGridObject(new Vector3(cellData.x, 0, cellData.z), ResManager.GRID_BLOCK_NARROW);
	}

	public override void Hit(Snake snake)
	{
		if (!cellData.Hit(snake))
			return;
		
		GameRuntime.ShakeCamera((new Vector3(0.2f, 0.4f, 1.0f)).normalized, 0.5f, 0.2f, 20);
	}

}

public class MapCellGold : MapCell
{
	public override void Init()
	{
		grid = ResManager.CreateGridObject(new Vector3(cellData.x, 0, cellData.z), ResManager.GRID_FLOOR);
		bonus = GameObjectPool.CreateNew("objects/GoldBonus");
		bonus.transform.localPosition = new Vector3(cellData.x + 0.5f, 0.5f, cellData.z);
		bonus.GetComponent<Gold>().Stay();
	}

	public override void Hit(Snake snake)
	{
		if (!cellData.Hit(snake))
			return;

		bonus.GetComponent<Gold>().Fly();
		GameObject par = ParticleMan.PlayParticle("gfx/GoldSpark", new Vector3(cellData.x + 0.5f, 0.0f, cellData.z + 0.5f));
		AdjustPos.AttachTo(par);
		SpriteCollect.CreateNew(new Vector3(cellData.x + 0.5f, 0.0f, cellData.z + 0.5f));
	}
}