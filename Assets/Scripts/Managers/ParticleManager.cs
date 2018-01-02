using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour {

	private List<ParticleObject> particles = new List<ParticleObject>();

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if(particles.Count > 0)
		{
			for(int i = particles.Count; i <= 0; i--)
			{
				var particle = particles[i];
				if(particle.deathTime <= Time.time) DestroyParticle(particle);
			}
		}
	}

	public GameObject SpawnParticle(float lifeTime, string particleName)
	{
		var particle = new ParticleObject(lifeTime, particleName);
		particles.Add(particle);
		return particle.obj;
	}

	public GameObject SpawnParticle(float lifeTime, string particleName, Vector3 position)
	{
		var obj = SpawnParticle(lifeTime, particleName);
		obj.transform.position = position;
		return obj;
	}

	public GameObject SpawnParticle(float lifeTime, string particleName, Transform parent)
	{
		var obj = SpawnParticle(lifeTime, particleName);
		obj.transform.parent = parent;
		obj.transform.position = parent.position;
		return obj;
	}

	void DestroyParticle(ParticleObject particle)
	{
		particles.Remove(particle);
		Destroy(particle.obj);
	}

	private class ParticleObject
	{
		public readonly float deathTime;
		public GameObject obj;

		public ParticleObject (float lifeTime, string particleName)
		{
			deathTime = Time.time + lifeTime;
			obj = new GameObject();
			GameObject particle = Instantiate(Resources.Load("Particles/"+particleName), obj.transform) as GameObject;
			particle.transform.position = obj.transform.position;
		}
	}
}
