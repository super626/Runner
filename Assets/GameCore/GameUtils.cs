//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1008
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using UnityEngine;
using System.Collections;

public class GameUtils
{
	public static Vector3 ConvertPos (Vector3 pos)
	{
		Vector3 posCamera = Camera.main.transform.position;

		float dist = pos.z - posCamera.z;
		float cd = Mathf.Clamp(dist - 15, 0, 50) * (1.57f / 50);
		pos.y -= (float)Mathf.Sin(cd * cd) * 7;

		float cd2 = Mathf.Clamp(dist, 3, 50) * (1.57f / 50);
		pos.x -= (float)Math.Sin(cd2 * cd2) * GameRuntime.turnX;
		
		return pos;
	}

	public class Shaker
	{
		private Vector3 m_dir;
		private float m_totalTime;
		private float m_time = 0;
		private float m_amount;
		private float m_rate;
		private float m_start = UnityEngine.Random.Range(-Mathf.PI, Mathf.PI);
		public Shaker(Vector3 dir, float time, float amount, float rate)
		{
			m_dir = dir;
			m_totalTime = time;
			m_time = 0;
			m_amount = amount;
			m_rate = rate;
		}
		public void Update(float dt)
		{
			m_time += dt;
		}
		public Vector3 GetOffset()
		{
			if (m_time >= m_totalTime)
				return Vector3.zero;
			float fAmount = m_amount * Mathf.Cos(Mathf.PI * 0.5f * m_time / m_totalTime);
			float fShake = Mathf.Sin(Mathf.Sin(m_start + Mathf.PI * m_rate * m_time));
			return fAmount * fShake * m_dir;
		}
	}

	public static Vector3 ConvertWorldToUI(Vector3 posWorld)
	{
		Vector3 posScreen = Camera.main.WorldToScreenPoint(posWorld);
		return UICamera.mainCamera.ScreenToWorldPoint(posScreen);
	}
}

