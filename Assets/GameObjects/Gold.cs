using UnityEngine;

public class Gold : MonoBehaviour {

	Material material;
	bool m_fly = false;
	// Use this for initialization
	void Start () {
		material = GetComponent<MeshRenderer>().material;
		m_fly = false;
	}
	
	// Update is called once per frame
	void Update () {
		material.SetFloat("_TurnX", GameRuntime.turnX);

		if (m_fly)
		{
			transform.localPosition = transform.localPosition - Vector3.up * 10 * Time.smoothDeltaTime;
		}
	}

	public void Fly() {
		m_fly = true;
	}

	public void Stay()
	{
		m_fly = false;
	}
}
