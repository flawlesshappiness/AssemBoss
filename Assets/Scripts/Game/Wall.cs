using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Wall : MonoBehaviour {

	public Texture texture;
	public bool repeat;
	public float repeatScale = 1f;

	//Awake
	void Awake()
	{
		if(repeat) Repeat();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Repeat()
	{
		var ren = GetComponent<MeshRenderer>();
		if(texture != null) ren.material.SetTexture("_MainTex", texture);
		ren.material.SetColor("_TintColor", Color.gray);

		var scale = transform.localScale;
		ren.material.SetTextureScale ("_MainTex", new Vector2 (scale.x * repeatScale, scale.y * repeatScale));
	}
}
