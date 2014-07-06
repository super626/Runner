using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {

	Vector3 eulerAngle = new Vector3(0, 0, 0);
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		eulerAngle.y = -GameRuntime.turnX;
		transform.localEulerAngles = eulerAngle;
	}
}
