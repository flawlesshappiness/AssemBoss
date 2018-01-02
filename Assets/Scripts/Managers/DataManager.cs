using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// Saves the given data object to file.
	/// </summary>
	/// <param name="data">The data object.</param>
	public void SaveToFile(DataObject data, string path)
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(path);
		bf.Serialize(file, data);
		file.Close();
	}

	/// <summary>
	/// Loads data from save file.
	/// </summary>
	public T LoadFromFile<T>(string path)
	{
		if(File.Exists(path))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(path, FileMode.Open);
			T data = (T)bf.Deserialize(file);
			file.Close();
			//
			return data;
		}
		return default(T);
	}

	public void DeleteFile(string path)
	{
		File.Delete(path);
	}
}

[Serializable]
public abstract class DataObject {
	
}

#region DATA BOSS
[Serializable]
public class DataBoss : DataObject {
	public string name = "New boss";
	public int health = 50;
	public DataAttack[] attacks { get; set; } //Use GetType() to get actual type

	public DataBoss ()
	{
		
	}
}

#region DATA ATTACK
[Serializable]
public abstract class DataAttack {
	public string name { get; set; }
	public float timeStart { get; set; }
	public float timeEnd { get; set; }
}

[Serializable]
public class DataAttackJump : DataAttack {
	public float jumpSpeed { get; set; }
	public float fallSpeed { get; set; }
	public float jumpTime { get; set; }
}

[Serializable]
public class DataAttackShoot : DataAttack {
	public DataProjectile[] projectiles { get; set; }
	public float spawnSpeed { get; set; }
}
#endregion

#region DATA PROJECTILE
[Serializable]
public abstract class DataProjectile {
	public float speed { get; set; }
	public DataPosition pos { get; set; }
}
#endregion

[Serializable]
public class DataPosition {
	public string posType { get; set; } //"CENTER" or "CREATOR"
	public float xOffset { get; set; }
	public float yOffset { get; set; }
}
#endregion