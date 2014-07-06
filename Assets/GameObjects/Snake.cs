using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
class TweenFlash : UITweener 
{
	Material mMaterial;
	Vector4 mColor;
	override protected void OnUpdate (float factor, bool isFinished) { 
		float f = (Mathf.Sin(factor * 10 - Mathf.PI / 2) + 1) * 0.5f + 1;
		if (isFinished)
			f = 1;
		mMaterial.SetVector("_Color", f * mColor);
	}
	
	/// <summary>
	/// Start the tweening operation.
	/// </summary>
	
	static public TweenFlash Begin (GameObject go, Material material, float duration, Vector4 color)
	{
		TweenFlash comp = UITweener.Begin<TweenFlash>(go, duration);
		comp.mMaterial = material;
		comp.mColor = color;
		
		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}
}

public class Snake : MonoBehaviour {
	const float FH = 0.01f;
	public int HP = 1000;

	enum Dir {
		NONE, LEFT, RIGHT, UP, DOWN
	};

	public enum  State{
		NORMAL, SIDE, NARROW
	}

	GameObject m_head;
	GameObject m_neck;
	Dir m_curDir = Dir.NONE;
	Dir m_tendDir = Dir.NONE;
	List<GameObject> m_bodys = new List<GameObject>();

	bool m_isTurning = false;
	float m_turnOrgZ = 0;
	float m_turnOrgX = 0;
	bool m_turnHalf = false;
	int m_logicX = 0;
	float m_hitRadius;

	float m_fShrinkTimer;
	State m_state;
	bool m_bSideLeft;
	float m_speed = 10;
	float m_targetSpeed = 10;
	float m_deltaSpeed = 0;

	public void SetSpeedTo(float targetSpeed, float delta)
	{
		m_targetSpeed = targetSpeed;
		m_deltaSpeed = delta;
	}

	public float GetSpeed()
	{
		return m_speed;
	}

	public State GetState()
	{
		return m_state;
	}
	// Use this for initialization
	void Start () {
		m_head = ResManager.CreateGridObject(new Vector3(m_logicX, FH, GetStartZ()), ResManager.GRID_HEAD);
		m_neck = ResManager.CreateGridObject(new Vector3(m_logicX, FH, GetStartZ() - 1), ResManager.GRID_BODY);
		m_neck.transform.localScale = new Vector3(1, 1, 0);
		m_state = State.NORMAL;
	}
	
	// Update is called once per frame
	void Update () {
		if (m_isTurning)
			StepTurn();
		if (!m_isTurning)
		{
			StepUp ();
			//Debug.Log(m_curDir);
			if (m_tendDir == Dir.LEFT || m_tendDir == Dir.RIGHT)
			{
				m_curDir = m_tendDir;
				m_tendDir = Dir.NONE;
				Turn();
			}
			else if (m_tendDir == Dir.UP)
			{
				m_curDir = m_tendDir;
				m_tendDir = Dir.NONE;
				Shrink();
			}
			else if (m_tendDir == Dir.DOWN)
			{
				m_curDir = m_tendDir;
				m_tendDir = Dir.NONE;
				Side();
			}
		}
		if (m_state == State.NARROW)
			StepShrink();
		else if (m_state == State.SIDE)
			StepSide();

		float z = Camera.main.transform.position.z;
		for (int i = 0; i < m_bodys.Count; i++) {
			if (m_bodys[i].transform.position.z + 1 < z)
			{
				ResManager.ReturnGridObject(m_bodys[i]);
				m_bodys.RemoveAt(i);
				i--;
			}
		}
		TestHit();

		float maxDelta = m_targetSpeed - m_speed;
		float speedDelta = m_deltaSpeed * Time.smoothDeltaTime;
		if (Mathf.Abs(maxDelta) > speedDelta)
			maxDelta = speedDelta * Mathf.Sign(maxDelta);
		m_speed += maxDelta;


		if (Input.GetMouseButtonDown(0))
			OnMouseDown();
		if (Input.GetMouseButtonUp(0))
			OnMouseUp();
		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
	}

	float GetStartZ()
	{
		return GameRuntime.GetStartZ() + 7;
	}

	GameObject CreateBodyObject(Vector3 neckPos)
	{
		GameObject obj = ResManager.CreateGridObject(neckPos, ResManager.GRID_BODY);
		if (m_state == State.NARROW)
		{
			obj.transform.localScale = new Vector3(0.5f, 1, 1);
		}
		if (m_state == State.SIDE)
		{
			obj.transform.localScale = new Vector3(0.5f, 1, 1);
		}
		else
		{
			obj.transform.localScale = new Vector3(1, 1, 1);
		}
		return obj;
	}

	void StepUp() {
		float startZ = GetStartZ();
		Vector3 neckPos = m_neck.transform.localPosition;

		float fScaleX = 1.0f;
	
		if (m_state == State.NARROW)
		{
			fScaleX = 0.5f;
		}
		if (m_state == State.SIDE)
		{
			fScaleX = 0.5f;
			neckPos.x = (float)m_logicX + (m_bSideLeft ? -0.25f : 0.25f);

		}
		else
		{

		}

		while (startZ - neckPos.z > 1)
		{
			Vector3 scale = m_neck.transform.localScale;
			scale.z = 1;
			m_neck.transform.localScale = scale;
			m_bodys.Add(m_neck);
			neckPos.z += 1;
			m_neck = CreateBodyObject(neckPos);
			scale = m_neck.transform.localScale;
			scale.x = fScaleX;
			m_neck.transform.localScale = scale;
		}
		Vector3 scaleNeck = m_neck.transform.localScale;
		scaleNeck.z = startZ - neckPos.z;
		m_neck.transform.localScale = scaleNeck;

		m_head.transform.position = new Vector3(neckPos.x, FH, startZ + 0.5f);

		m_head.transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
	}

	static float GetTurnShapeRotate(Dir dir, bool postHalf) {
		if (dir == Dir.LEFT) {
			return postHalf ? 180 : 0;
		} else {
			return postHalf ? 90 : -90;
		}
	}

	static bool GetTurnShapeInverse(Dir dir, bool postHalf) {
		if (dir == Dir.LEFT) {
			return postHalf ? true : false;
		} else {
			return postHalf ? false : true;
		}
	}

	void StepTurn() {
		float turnProgress = GetStartZ() - m_turnOrgZ;

		if (!m_turnHalf)
		{
			if (turnProgress > 0.5f)
			{
				GameObject neck = ResManager.CreateGridObject(new Vector3(m_turnOrgX, FH, m_turnOrgZ + 0.5f), ResManager.GRID_TURN);
				neck.transform.eulerAngles = new Vector3(0, GetTurnShapeRotate(m_curDir, false), 0);
				m_bodys.Add(neck);
				m_turnHalf = true;
				m_neck.transform.position = new Vector3(m_logicX, FH, m_turnOrgZ + 0.5f);
				m_neck.transform.eulerAngles = new Vector3(0, GetTurnShapeRotate(m_curDir, true), 0);
			}
			else
			{
				m_neck.GetComponent<MeshFilter>().mesh.triangles = ResManager.GetProgressIndices(turnProgress * 2, GetTurnShapeInverse(m_curDir, false));
				float sin45 = Mathf.Sin(Mathf.PI / 4);
				float theta = turnProgress * Mathf.PI + Mathf.PI * 0.25f;
				//theta = Mathf.PI * 0.75f;
				float x = m_turnOrgX + (m_logicX - m_turnOrgX) * (0.5f - sin45 * Mathf.Cos(theta));
				float z = m_turnOrgZ + sin45 * Mathf.Sin(theta);
				m_head.transform.position = new Vector3(x, FH, z);
				m_head.transform.eulerAngles = new Vector3(0, (m_logicX - m_turnOrgX) *  Mathf.Rad2Deg * turnProgress * Mathf.PI, 0);
			}
		}

		if (m_turnHalf)
		{
			if (turnProgress > 1)
			{
				GameObject neck = ResManager.CreateGridObject(new Vector3(m_logicX, FH, m_turnOrgZ + 0.5f), ResManager.GRID_TURN);
				neck.transform.eulerAngles = new Vector3(0, GetTurnShapeRotate(m_curDir, true), 0);
				m_bodys.Add(neck);
				m_isTurning = false;
				ResManager.ReturnGridObject(m_neck);
				m_neck = ResManager.CreateGridObject(new Vector3(m_logicX, FH, m_turnOrgZ + 1), ResManager.GRID_BODY);
				m_curDir = Dir.NONE;
			}
			else
			{
				m_neck.GetComponent<MeshFilter>().mesh.triangles = ResManager.GetProgressIndices(turnProgress * 2 - 1, GetTurnShapeInverse(m_curDir, true));
				float sin45 = Mathf.Sin(Mathf.PI / 4);
				float theta = (turnProgress - 0.5f) * Mathf.PI + Mathf.PI * 0.25f;
				//theta = Mathf.PI * 0.75f;
				float x = m_turnOrgX + (m_logicX - m_turnOrgX) * (0.5f + sin45 * Mathf.Sin(theta));
				float z = m_turnOrgZ + 1 - sin45 * Mathf.Cos(theta);
				m_head.transform.position = new Vector3(x, FH, z);
				m_head.transform.eulerAngles = new Vector3(0, (m_logicX - m_turnOrgX) *  Mathf.Rad2Deg * (1 - turnProgress) * Mathf.PI, 0);

			}
		}
	}

	void Turn() {
		if (m_curDir != Dir.LEFT && m_curDir != Dir.RIGHT)
			return;
		m_turnOrgZ = GetStartZ();
		m_turnOrgX = m_logicX;
		m_isTurning = true;
		m_bodys.Add(m_neck); 
		m_neck = ResManager.CreateGridObject(new Vector3(m_turnOrgX, FH, m_turnOrgZ + 0.5f), ResManager.GRID_TURN_PROGRESS);
		m_neck.GetComponent<MeshFilter>().mesh.triangles = ResManager.GetProgressIndices(0, true);
		m_turnHalf = false;
		switch (m_curDir)
		{
		case Dir.LEFT:
			m_logicX -= 1;
			break;
		case Dir.RIGHT:
			m_logicX += 1;
			break;
		default:
			break;
		}
		m_neck.transform.eulerAngles = new Vector3(0, GetTurnShapeRotate(m_curDir, false), 0);

		//m_head.transform.position = new Vector3(m_logicX, FH, m_turnOrgZ + 0.5f);
	}

	void Shrink() {
		m_bodys.Add(m_neck); 
		m_neck = ResManager.CreateGridObject(new Vector3(m_logicX, FH, GetStartZ()), ResManager.GRID_TO_SMALL);
		m_head.transform.localScale = new Vector3(0.5f, 1, 1);
		m_state = State.NARROW;
		m_fShrinkTimer = 0.5f;
		m_curDir = Dir.NONE;
	}

	void StepShrink() {
		m_fShrinkTimer -= Time.smoothDeltaTime;
		if (m_fShrinkTimer <= 0)
		{
			m_bodys.Add(m_neck); 
			m_state = State.NORMAL;
			m_neck = ResManager.CreateGridObject(new Vector3(m_logicX, FH, GetStartZ()), ResManager.GRID_TO_SMALL_INVERT);
			m_head.transform.localScale = new Vector3(1, 1, 1);
		}
	}

	void Side() {
		m_bodys.Add(m_neck);
		m_bSideLeft = GameRuntime.currentLevel.ForwardSideIsLeft(m_logicX, (int)GetStartZ());
		m_neck = ResManager.CreateGridObject(new Vector3(m_logicX, FH, GetStartZ()), 
		                                     m_bSideLeft ? ResManager.GRID_TO_SIDE_LEFT : ResManager.GRID_TO_SIDE_RIGHT);
		m_head.transform.localScale = new Vector3(0.5f, 1, 1);
		m_state = State.SIDE;
		m_fShrinkTimer = 0.5f;
		m_curDir = Dir.NONE;
	}
	
	void StepSide() {
		m_fShrinkTimer -= Time.smoothDeltaTime;
		if (m_fShrinkTimer <= 0)
		{
			m_bodys.Add(m_neck); 
			m_state = State.NORMAL;
			m_neck = ResManager.CreateGridObject(new Vector3(m_logicX, FH, GetStartZ()), 
			                                     m_bSideLeft ? ResManager.GRID_TO_SIDE_LEFT_INVERT : ResManager.GRID_TO_SIDE_RIGHT_INVERT);
			m_head.transform.localScale = new Vector3(1, 1, 1);
		}
	}


	bool m_isDown = false;
	Vector3 m_mousePos;
	void OnMouseDown()
	{
		if (Input.GetMouseButtonDown(0))
		{
			m_isDown = true;
			m_mousePos = Input.mousePosition;
		}
	}

	void OnMouseUp()
	{
		if (m_isDown && m_state == State.NORMAL)
		{
			Vector3 deltaPos = Input.mousePosition - m_mousePos;
			float absX = Mathf.Abs(deltaPos.x);
			float absY = Mathf.Abs(deltaPos.y);

			if (absX > absY)
			{
				if (absX > 5)
				{
					if (deltaPos.x > 0 && m_logicX < GameLevel.MAP_MAXX - 1)
						m_tendDir = Dir.RIGHT;
					if (deltaPos.x < 0 && m_logicX > GameLevel.MAP_MINX + 1)
						m_tendDir = Dir.LEFT;
				}
			}
			else 
			{
				if (absY > 5)
				{
					m_tendDir = deltaPos.y > 0 ? Dir.UP : Dir.DOWN;
				}
			}
		}

		m_isDown = false;
	}


	void TestHit()
	{
		GameRuntime.floor.HitCell((int)m_logicX, (int)GetStartZ(), this);
	}

}
