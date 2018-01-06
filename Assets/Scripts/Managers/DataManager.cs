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
		if(File.Exists(path)) File.Delete(path);
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

	public string GetPath()
	{
		return Paths.GetBossDirectory() + "/" + name + ".boss";
	}
}

#region DATA ATTACK
[Serializable]
public abstract class DataAttack {
	public string name { get; set; }
	public string type { get; set; }
	public float timeStart { get; set; }
	public float timeEnd { get; set; }

	public DataAttack ()
	{
		name = "New attack";
	}

	public abstract BossAttack AddComponent(GameObject g);
}

[Serializable]
public class DataAttackJump : DataAttack {
	public float jumpSpeed { get; set; }
	public float fallSpeed { get; set; }
	public float jumpTime { get; set; }
	public float moveSpeed { get; set; }
	public string approachToPlayer { get; set; }

	public DataAttackJump ()
	{
		jumpSpeed = 0.07f;
		fallSpeed = 6f;
		jumpTime = 5f;
		moveSpeed = 0.05f;
		approachToPlayer = Approach.TOWARDS.ToString();
	}

	public override BossAttack AddComponent (GameObject g)
	{
		var c = g.AddComponent<BossAttackJump>();
		c.data = this;
		return c;
	}
}

[Serializable]
public class DataAttackShoot : DataAttack {
	public float speedMove { get; set; }
	public float speedRotation { get; set; }
	public int projectileAmount { get; set; }
	public string projectileDirection { get; set; }
	public float spawnDelay { get; set; } //Time between each spawn
	public float scale { get; set; }

	public DataAttackShoot ()
	{
		speedMove = 0.1f;
		speedRotation = 0.1f;
		projectileAmount = 1;
		projectileDirection = ProjectileDirection.FORWARDS.ToString();
		spawnDelay = 0.1f;
		scale = 1f;
	}

	public override BossAttack AddComponent (GameObject g)
	{
		var c = g.AddComponent<BossAttackProjectile>();
		c.data = this;
		return c;
	}
}
#endregion

[Serializable]
public class DataPosition {
	public string posType { get; set; } //"CENTER" or "CREATOR"
	public float xOffset { get; set; }
	public float yOffset { get; set; }
}
#endregion