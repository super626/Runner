using UnityEngine;
using System.Collections;

public class TweenCardinal : UITweener
{
	public Vector3 from = Vector3.zero;
	public Vector3 to = Vector3.zero;
	public Cardinal cardinal;
	Transform mTrans;
	
	/// <summary>
	/// Interpolate and update the alpha.
	/// </summary>
	
	override protected void OnUpdate (float factor, bool isFinished) 
	{ 
		mTrans.position = cardinal.GetPointAtTime(factor); 
		Vector3 pos = mTrans.localPosition;
		pos.z = 0;
		mTrans.localPosition = pos;
	}
	
	/// <summary>
	/// Start the tweening operation.
	/// </summary>
	
	static public TweenCardinal Begin (GameObject go, float duration, Vector3 p0, Vector3 p1, Vector3 m0, Vector3 m1)
	{
		TweenCardinal comp = UITweener.Begin<TweenCardinal>(go, duration);
		comp.mTrans = go.transform;
		comp.from = p0;
		comp.to = p1;
		comp.cardinal = new Cardinal(p0, p1, m0, m1);
		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}
}

