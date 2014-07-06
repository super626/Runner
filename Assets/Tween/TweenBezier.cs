using UnityEngine;
using System.Collections;

public class TweenBezier : UITweener
{
	public Vector3 from = Vector3.zero;
	public Vector3 to = Vector3.zero;
	public Bezier bezier;
	Transform mTrans;

	/// <summary>
	/// Interpolate and update the alpha.
	/// </summary>
	
	override protected void OnUpdate (float factor, bool isFinished) 
	{ 
		mTrans.position = bezier.GetPointAtTime(factor); 
		Vector3 pos = mTrans.localPosition;
		pos.z = 0;
		mTrans.localPosition = pos;
	}
	
	/// <summary>
	/// Start the tweening operation.
	/// </summary>
	
	static public TweenBezier Begin (GameObject go, float duration, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
	{
		TweenBezier comp = UITweener.Begin<TweenBezier>(go, duration);
		comp.mTrans = go.transform;
		comp.from = p1;
		comp.to = p4;
		comp.bezier = new Bezier(p1, p2, p3, p4);
		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}
}

