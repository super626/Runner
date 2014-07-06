using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AdjustPos: MonoBehaviour {
	public Vector3 posTrue;
	public void Start() {
		InitPos();
	}

	public void Update() {
		transform.position = GameUtils.ConvertPos(posTrue);
	}

	public void InitPos() {
		posTrue = transform.position;
	}

	public static void AttachTo(GameObject go)
	{
		AdjustPos comp = go.GetComponent<AdjustPos>();
		if (comp == null)
			comp = go.AddComponent<AdjustPos>();
		comp.InitPos();
	}
}

