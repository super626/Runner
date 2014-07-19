using UnityEngine;
using System.Collections;

public class SpriteCollect : MonoBehaviour {
	// Use this for initialization
	void Start () {
	}
	public void Init()
	{	
		GameObject target = GameObject.Find("Sprite:Gold");
		transform.parent = target.transform.parent;
		gameObject.layer =target.layer;
		transform.localScale = target.transform.localScale * 0.7f;
		float z = target.transform.position.z;
		Vector3 ptStart = GameUtils.ConvertWorldToUI(GameUtils.ConvertPos(transform.position));
		ptStart.z = z;
		Vector3 ptEnd = target.transform.position;
		Vector3 ptStartHandle = ptStart + new Vector3(1.0f, 1.0f, 0);
		Vector3 ptEndHandle = ptEnd + new Vector3(-0.0f, -0.0f, 0);

		TweenCardinal be = TweenCardinal.Begin(gameObject, 0.7f, ptStart, ptEnd, new Vector3(-1.0f, 0.0f, 0), new Vector3(-0.5f, 0.5f, 0));
		be.method = UITweener.Method.EaseIn;

		be.onFinished = delegate (UITweener tween) {
			gameObject.SetActive(false);
			GameObject pa = ParticleMan.PlayParticle("gfx/CollectSpark", target.transform.position);
			pa.transform.parent = target.transform.parent;
			pa.transform.localPosition = target.transform.localPosition;
			pa.transform.localScale = Vector3.one;
			pa.transform.localEulerAngles = new Vector3(0, 0, 0);
			GameRuntime.curLevel.AddGoldNum(1);
			GameRuntime.labelGold.text = GameRuntime.curLevel.GoldNum.ToString(); 
		};

		TweenScale sc = TweenScale.Begin(gameObject, 0.45f, target.transform.localScale * 1.5f);
		sc.method = UITweener.Method.EaseIn;
		sc.onFinished = delegate (UITweener tween) {
			TweenScale.Begin(gameObject, 0.22f, target.transform.localScale * 0.7f);
		};
	}


	// Update is called once per frame
	void Update () {
	}

	public static GameObject CreateNew(Vector3 pos)
	{
		GameObject go = GameObjectPool.CreateNew("ui/SpriteGold");
		go.transform.position = pos;
		SpriteCollect sc = go.GetComponent<SpriteCollect>();
		if (sc == null)
			sc = go.AddComponent<SpriteCollect>();
		sc.Init();
		return go;
	}
}
