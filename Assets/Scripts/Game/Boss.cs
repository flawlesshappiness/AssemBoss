using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Boss : MonoBehaviour {

	public Health health;
	public SpriteManager mgSprite;
	public MovementManager mgMovement;
	public JumpManager mgJump;
	public ParticleManager mgParticle;
	public PrefabManager mgPrefab;
	public SizeManager mgSize;

	public LevelManager mgLevel;
	public Transform middleTrans;
	private Player player;

	private bool enabled;
	private new string name;

	private List<BossAttack> firstAttacks = new List<BossAttack>();
	private Dictionary<DataAttack, BossAttack> attacks = new Dictionary<DataAttack, BossAttack>();

	//Awake
	void Awake()
	{
		
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!enabled) return;
	}

	//Setup after load
	public void Setup()
	{
		foreach(KeyValuePair<DataAttack, BossAttack> kv in attacks)
		{
			kv.Value.SetData(kv.Key);
		}
	}

	public void EnableBoss()
	{
		enabled = true;
		GetRandomAttack(firstAttacks).Enable();
	}

	public void OnDamage()
	{
		mgSprite.FadeColor(Color.red, Color.white, 0.5f);
		mgLevel.UpdateBossHealth(health.Get());
	}

	public void OnDeath()
	{
		mgParticle.SpawnParticle("BossExplode", 2f, transform.position);
		gameObject.SetActive(false);

		mgLevel.UpdateBossHealth(health.Get());
		mgLevel.BossDeath();
	}

	#region ATTACKS
	public void AddAttack(DataAttack da)
	{
		var ba = da.AddComponent(gameObject);
		attacks.Add(da, ba);
		if(da.firstAttack.value) firstAttacks.Add(ba);
	}

	public BossAttack GetAttack(DataAttack da)
	{
		return attacks[da];
	}

	public BossAttack GetRandomAttack()
	{
		return GetRandomAttack(attacks.Values.ToList());
	}

	public BossAttack GetRandomAttack(List<BossAttack> list)
	{
		return list[Random.Range(0, list.Count)];
	}
	#endregion

	public Player GetPlayer()
	{
		return mgLevel.GetPlayer();
	}

	public Direction GetDirectionToPlayer()
	{
		var ppos = mgLevel.GetPlayerPosition();
		var pos = transform.position;
		return (ppos.x > pos.x) ? Direction.RIGHT : Direction.LEFT;
	}

	public void FacePlayer(bool towards)
	{
		var d = GetDirectionToPlayer();
		if(towards)
		{
			if(mgMovement.GetCurrentDirection() != d) mgMovement.MoveDirection(d);
		}
		else
		{
			if(mgMovement.GetCurrentDirection() == d) mgMovement.MoveOppositeDirection(d);
		}
	}
}
