using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class M {
	
	public static T GetInterface<T>(GameObject g)
	{
		return g.GetComponents<Component>().OfType<T>().FirstOrDefault();
	}

	public static IEnumerable<T> GetInterfaces<T>(GameObject g)
	{
		return g.GetComponents<Component>().OfType<T>();
	}

	public static T GetEnum<T>(string s)
	{
		try
		{
			return (T)System.Enum.Parse(typeof(T), s);
		}
		catch(ArgumentException e)
		{
			return Activator.CreateInstance<T>();
		}
	}

	public static T GetRandomEnum<T>()
	{
		Array values = Enum.GetValues(typeof(T));
		var random = new System.Random();
		return (T)values.GetValue(random.Next(values.Length));
	}

	public static List<string> GetListOfEnum(System.Type type)
	{
		return Enum.GetNames(type).ToList();
	}

	public static bool IsLastSibling(Transform t)
	{
		bool found = false;
		foreach(Transform child in t.parent)
		{
			if(child == t) found = true;
			else found = false;
		}
		return found;
	}

	public static bool IsFirstSibling(Transform t)
	{
		return t.GetSiblingIndex() == 0;
	}
}
