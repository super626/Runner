using UnityEngine;
[System.Serializable]

public class Cardinal : System.Object
{
	public Vector3 p0;
	public Vector3 p1;
	public Vector3 m0;
	public Vector3 m1;
	private Vector3 A;
	private Vector3 B;
	private Vector3 C;
	private Vector3 D;

	// Init function v0 = 1st point, v1 = handle of the 1st point , v2 = handle of the 2nd point, v3 = 2nd point
	// handle1 = v0 + v1
	// handle2 = v3 + v2
	public Cardinal( Vector3 v0, Vector3 v1, Vector3 t0, Vector3 t1 )
	{
		p0 = v0;
		p1 = v1;
		m0 = t0;
		m1 = t1;
		SetConstant();
	}
	
	// 0.0 >= t <= 1.0
	public Vector3 GetPointAtTime( float t )
	{
		float t2 = t * t;
		float t3 = t * t * t;
		return A * t3 + B * t2 + C * t + D;
	}
	
	private void SetConstant()
	{
		A = m0 + m1 + 2 * p0 - 2 * p1;
		B = 3 * p1 - 3 * p0 - m1 - 2 * m0;
		C = m0;
		D = p0;
	}
}