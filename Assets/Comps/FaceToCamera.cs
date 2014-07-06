using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FaceToCamera: MonoBehaviour {

	public void Start() {
	}

	public void Update() {
		Transform cam = Camera.main.transform;
		transform.LookAt(cam, cam.up);
	//	transform.position = GameUtils.ConvertPos(posTrue);
	}

	public static void AttachTo(GameObject go)
	{
		FaceToCamera comp = go.GetComponent<FaceToCamera>();
		if (comp == null)
			comp = go.AddComponent<FaceToCamera>();
	}
}

