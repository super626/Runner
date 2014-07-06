using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class PoolParam : MonoBehaviour
{
	public string typeName;
	public float destoryTimer;
}

class GameObjectTable 
{
	public GameObjectTable(string name) {
		m_name = name;
	}
	
	public string m_name;
	public List<GameObject> m_listActive = new List<GameObject>();
	public List<GameObject> m_listFree = new List<GameObject>();
	
	public GameObject CreateNew(float fDestoryTime, Vector3 pos, Quaternion rot) {
		if (m_listFree.Count > 0)
		{
			GameObject go = m_listFree[m_listFree.Count - 1];
			go.transform.position = pos;
			go.transform.rotation = rot;
			m_listFree.RemoveAt(m_listFree.Count - 1);
			go.GetComponent<PoolParam>().destoryTimer = fDestoryTime;
			go.SetActive(true);
			m_listActive.Add(go);
			return go;
		}
		else 
		{
			GameObject go = (GameObject)Object.Instantiate(Resources.Load(m_name), pos, rot);
			PoolParam poolParam = go.AddComponent<PoolParam>();
			poolParam.typeName = m_name;
			poolParam.destoryTimer = fDestoryTime;
			m_listActive.Add(go);
			return go;
		}
	}
	
	public void Update(float dt)
	{
		for (int i = 0; i < m_listActive.Count; i++)
		{
			GameObject go = m_listActive[i];
			PoolParam pp = go.GetComponent<PoolParam>();
			if (pp.destoryTimer >= 0)
			{
				pp.destoryTimer -= dt;
				if (pp.destoryTimer <= 0)
					go.SetActive(false);
			}
			if (!go.activeSelf)
			{
				m_listActive.RemoveAt(i);
				i--;
				m_listFree.Add(go);
			}
		}
	}
}

public class GameObjectPool
{

	static Dictionary<string, GameObjectTable> m_objTables = new Dictionary<string, GameObjectTable>();
	// Use this for initialization
	public static void Init() {
	}

	public static GameObject CreateNew(string name) {
		return CreateNew(name, -1, Vector3.zero, Quaternion.identity);
	}

	public static GameObject CreateNew(string name, float destoryTime) {
		return CreateNew(name, destoryTime, Vector3.zero, Quaternion.identity);
	}

	public static GameObject CreateNew(string name, float destoryTime, Vector3 pos, Quaternion rot) {
		if (m_objTables.ContainsKey(name))
		{
			return m_objTables[name].CreateNew(destoryTime, pos, rot);
		}
		else 
		{
			GameObjectTable tb = new GameObjectTable(name);
			m_objTables[name] = tb;
			return tb.CreateNew(destoryTime, pos, rot);
		}
	}
	// Update is called once per frame
	public static void Update (float dt)
	{
		foreach (KeyValuePair<string,GameObjectTable> pair in m_objTables)
		{
			pair.Value.Update(dt);
		}
	}

	public static void SetDestroyTime(GameObject go, float time)
	{
		PoolParam pp = go.GetComponent<PoolParam>();
		if (pp == null)
			return;
		pp.destoryTimer = time;
	}
}

