using UnityEngine;
using System.Collections;

public class ParticleMan
{
	public static GameObject PlayParticle(string name, Vector3 pos)
	{
		GameObject go = GameObjectPool.CreateNew(name, -1, pos, Quaternion.Euler(new Vector3(-90, 0, 0)));
		ParticleSystem ps = go.GetComponent<ParticleSystem>();
		if (ps != null)
		{
			ps.Clear();
			ps.Play();
			if (!ps.loop)
			{
				GameObjectPool.SetDestroyTime(go, ps.duration + ps.startLifetime);
			}
		}
		return go;
	}

}

