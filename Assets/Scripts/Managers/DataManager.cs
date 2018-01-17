using System;
using System.Runtime.Serialization;
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
		FileStream file = null;
		try
		{
			if(File.Exists(path))
			{
				BinaryFormatter bf = new BinaryFormatter();
				file = File.Open(path, FileMode.Open);
				T data = (T)bf.Deserialize(file);
				file.Close();
				//
				return data;
			}
		}
		catch(SerializationException e)
		{
			print(e.Message);
		}
		catch(ArgumentException e)
		{
			print(e.Message);
		}
		finally
		{
			if(file != null) file.Close();
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
	public DataValue<String> name = new DataValue<string>("New boss");
	public DataValue<int> health = new DataValue<int>(50);
	public DataValue<float> sizeMult = new DataValue<float>(1f);
	public DataPlayer player = new DataPlayer();
	public DataAttack[] attacks { get; set; } //Use GetType() to get actual type

	public string GetPath()
	{
		return Paths.GetBossDirectory() + "/" + name.value + ".boss";
	}
}

[Serializable]
public class DataPlayer {
	public DataValue<int> health = new DataValue<int>(5);
}

#region DATA ATTACK
[Serializable]
public abstract class DataAttack {
	public DataValue<string> name = new DataValue<string>("New attack");
	public DataValue<string> type = new DataValue<string>("");
	public DataValue<float> timeStart = new DataValue<float>(0.0f);
	public DataValue<float> timeEnd = new DataValue<float>(0.0f);
	public DataValue<bool> activeAttack = new DataValue<bool>(true); //Can attack be used by boss at random?

	public abstract BossAttack AddComponent(GameObject g);
}

[Serializable]
public class DataAttackJump : DataAttack {
	public DataValue<float> jumpSpeed = new DataValue<float>(0.07f);
	public DataValue<float> fallSpeed = new DataValue<float>(6f);
	public DataValue<float> jumpTime = new DataValue<float>(5f);
	public DataValue<float> moveSpeed = new DataValue<float>(0.05f);
	public DataValue<string> approachToPlayer = new DataValue<string>(Approach.TOWARDS.ToString());
	//On [condition] do [attack]

	public override BossAttack AddComponent (GameObject g)
	{
		var c = g.AddComponent<BossAttackJump>();
		c.data = this;
		return c;
	}
}

[Serializable]
public class DataAttackJumpToPlayer : DataAttack {
	

	public override BossAttack AddComponent (GameObject g)
	{
		var c = g.AddComponent<BossAttackJump>();
		c.data = this;
		return c;
	}
}

[Serializable]
public class DataAttackShoot : DataAttack {
	public DataValue<float> speedMove = new DataValue<float>(0.1f);
	public DataValue<float> speedRotation = new DataValue<float>(0.1f);
	public DataValue<int> projectileAmount = new DataValue<int>(1);
	public DataValue<string> projectileDirection = new DataValue<string>(ProjectileDirection.FORWARDS.ToString());
	public DataValue<float> spawnDelay = new DataValue<float>(0.1f); //Time between each spawn
	public DataValue<float> scale = new DataValue<float>(1f);

	public override BossAttack AddComponent (GameObject g)
	{
		var c = g.AddComponent<BossAttackProjectile>();
		c.data = this;
		return c;
	}
}

[Serializable]
public class DataAttackSequence : DataAttack {
	public List<DataAttack> attacks = new List<DataAttack>();

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

#region VALUES
[Serializable]
public class DataInt {
	public int value = 0;
	public DataInt (int v)
	{
		value = v;
	}
}

[Serializable]
public class DataFloat {
	public float value = 0f;
	public DataFloat (float v)
	{
		value = v;
	}
}

[Serializable]
public class DataBool {
	public bool value = false;
	public DataBool (bool v)
	{
		value = v;
	}
}

[Serializable]
public class DataString {
	public string value = "";
	public DataString (string v)
	{
		value = v;
	}
}

[Serializable]
public class DataValue<T> {
	public T value;
	public DataValue (T v)
	{
		value = v;
	}
}
#endregion
#endregion