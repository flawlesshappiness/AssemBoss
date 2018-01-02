using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Lerp<T> {
	private float _time { get; set; }
	private float _startTime { get; set; }
	private float _endTime { get; set; }
	private T _startValue { get; set; }
	private T _endValue { get; set; }
	private Func<T, T, float, T> _funcLerp;

	public Lerp(float time, T start, T end, Func<T, T, float, T> lerpFunc)
	{
		_time = time;
		_startTime = Time.time;
		_endTime = _startTime + time;
		_startValue = start;
		_endValue = end;
		_funcLerp = lerpFunc;
	}

	public float GetPerc()
	{
		return TimePerc(_startTime, _endTime);
	}

	float TimePerc(float start, float end)
	{
		return (Time.time - start) / (end - start);
	}

	public bool IsFinished()
	{
		return GetPerc() >= 1.0f;
	}

	public T GetLerp()
	{
		return _funcLerp(_startValue, _endValue, GetPerc());
	}

	public void Reset()
	{
		_startTime = Time.time;
		_endTime = _startTime + _time;
	}
}

public class Lerp
{
	public static Lerp<float> Get(float time, float start, float end)
	{
		return new Lerp<float>(time, start, end, Mathf.Lerp);
	}

	public static Lerp<Color> Get(float time, Color start, Color end)
	{
		return new Lerp<Color>(time, start, end, Color.Lerp);
	}

	public static Lerp<Vector3> Get(float time, Vector3 start, Vector3 end)
	{
		return new Lerp<Vector3>(time, start, end, Vector3.Lerp);
	}
}
