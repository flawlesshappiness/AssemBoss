using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		
	}

	public ParticleSystem SpawnParticle(string particleName, float time)
	{
		var particle = Instantiate(Resources.Load("Particles/"+particleName)) as GameObject;
		var p = particle.GetComponent<ParticleSystem>();
		Destroy(particle, time);
		return p;
	}

	public ParticleSystem SpawnParticle(string particleName, float life, Vector3 position)
	{
		var p = SpawnParticle(particleName, life);
		var obj = p.gameObject;
		obj.transform.position = position;
		return p;
	}

	public ParticleSystem SpawnParticle(string particleName, float life, Transform parent)
	{
		var p = SpawnParticle(particleName, life);
		var obj = p.gameObject;
		obj.transform.parent = parent;
		obj.transform.position = parent.position;
		return p;
	}
}
