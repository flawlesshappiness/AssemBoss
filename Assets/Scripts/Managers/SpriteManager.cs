using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteManager : MonoBehaviour {

	private SpriteRenderer r;

	//Colors
	private Color cTarget;
	private Color clearWhite = new Color(1f, 1f, 1f, 0f);

	//Alpha
	private float aTarget;

	//Lerps
	private Lerp<Color> cLerp;
	private Lerp<float> aLerp;

	//Awake
	void Awake()
	{
		r = GetComponent<SpriteRenderer>();
	}

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		if(cLerp != null)
		{
			SetColor(cLerp.GetLerp());
			if(cLerp.IsFinished()) cLerp = null;
		}

		if(aLerp != null)
		{
			SetAlpha(aLerp.GetLerp());
			if(aLerp.IsFinished()) aLerp = null;
		}
	}

	#region COLOR
	public void SetColor(Color c)
	{
		r.color = new Color(c.r, c.g, c.b, r.color.a);
	}

	public Color GetColor()
	{
		return r.color;
	}

	public Color GetTargetColor()
	{
		return cTarget;
	}

	public void FadeColor(Color start, Color end, float time)
	{
		cLerp = Lerp.Get(time, start, end);
		cTarget = end;
	}

	public void SetColorLerp(Lerp<Color> lerp)
	{
		cLerp = lerp;
	}
	#endregion
	#region ALPHA
	public void SetAlpha(float a)
	{
		r.color = new Color(r.color.r, r.color.g, r.color.b, a);
	}

	public float GetAlpha()
	{
		return r.color.a;
	}

	public float GetTargetAlpha()
	{
		return aTarget;
	}

	public void FadeAlpha(float start, float end, float time)
	{
		aLerp = Lerp.Get(time, start, end);
		aTarget = end;
	}

	public void SetAlphaLerp(Lerp<float> lerp)
	{
		aLerp = lerp;
	}
	#endregion
	#region SPRITE
	public Sprite GetSprite()
	{
		return r.sprite;
	}

	public void SetSprite(Sprite s)
	{
		r.sprite = s;
	}

	public void SetSpriteEnabled(bool enabled)
	{
		r.enabled = enabled;
	}

	public bool SpriteEnabled()
	{
		return r.enabled;
	}
	#endregion
}
