using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeManager : MonoBehaviour {

	public Transform mainTrans;
	private Lerp<Vector3> lSize;
	private Vector3 size = new Vector3(1f, 1f, 1f);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(lSize != null)
		{
			mainTrans.localScale = lSize.GetLerp();
			if(lSize.IsFinished()) lSize = null;
		}
	}

	public void SetDefaultSize(Vector3 size)
	{
		this.size = size;
	}

	public void SetSize(Vector3 size)
	{
		lSize = null;
		mainTrans.localScale = size;
	}

	public void SetSizeToDefault()
	{
		mainTrans.localScale = size;
	}

	public void FadeSize(Vector3 end, float time)
	{
		FadeSize(mainTrans.localScale, end, time);
	}

	public void FadeSize(Vector3 start, Vector3 end, float time)
	{
		lSize = Lerp.Get(time, start, end);
	}

	public void FadeToDefault(float time)
	{
		lSize = Lerp.Get(time, mainTrans.localScale, size);
	}

	public Vector3 GetSizeMultiplied(float xMult, float yMult)
	{
		return new Vector3(size.x * xMult, size.y * yMult, size.z);
	}
}
