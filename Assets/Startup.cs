using UnityEngine;
using System.Collections;

public class Startup : MonoBehaviour {

	void Awake() {
		GameRuntime.Init ();
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		GameRuntime.Update();
	}
}
