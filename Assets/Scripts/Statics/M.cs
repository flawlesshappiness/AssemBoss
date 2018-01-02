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
}
