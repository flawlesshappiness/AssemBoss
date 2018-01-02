using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public GameObject SpawnPrefabGame(string prefab)
	{
		return GameObject.Instantiate(Resources.Load("Prefabs/Game/"+prefab)) as GameObject;
	}

	public GameObject SpawnPrefabUI(string prefab)
	{
		return GameObject.Instantiate(Resources.Load("Prefabs/UI/"+prefab)) as GameObject;
	}
}
