using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;


public class EnemyController : CharacterBase
{
	[SerializeField]
	public float AreaPosX;

	[SerializeField]
	public float AreaPosY;

	private EnemyDeath _death;
	protected override void Update()
    {
	    _phy._Update();
		base.Update();
		if (CheckIsOutRoom()&& time>0.05f)
		{
			Debug.Log(base.gameObject.name);
			DespawnMe();
		}
    }
    public AIBase aibase;

    public AIPhase phase;

    public float time;

    public float dtime;

    public byte id;

    public float  moveoffset;

    public Vector3 SpawnPosition;

    private bool isOutRoom;

    public void FindEnemyDate()
    {
	    AIBase[] array = new AIBase[]
	    {
		    new GedGehog(this),
		    new RobotDog(this),
		    new Smog(this),
	    };
	    foreach (AIBase aibase1 in array)
	    {
		    if (aibase1.type == type)
		    {
			    aibase = aibase1;
		    }
	    }
    }




    public void Start()
    {
	    InitStart();
	    InitAttackType();

    }

    protected override void UpdateCombat()
    {
	    dtime = Time.deltaTime;
	    time += dtime;



		if (!Isdead())
		{
			HandleInfo();
		}
		else
		{
			_death = ObjectPooler.instance.CreateEnemyDeath();
			if (_death)
			{
				float num = Random.Range(2.75f, 6.25f);
				float num2 = Random.Range(10f, 16f);
				if (_transform.position.x < EventManager.instance.mainCharacter._transform.position.x)
				{
					num *= -1;
				}
				_death.SpawnMe(num,num2,animationPlayer.basesprite.flipX,_transform.position,animationPlayer._animator.runtimeAnimatorController,false);
				_death.gameObject.transform.position = _transform.position;
				_death.transform.localScale = animationPlayer.gameObject.transform.localScale;
			}
			base.gameObject.SetActive(false);
		}

		if (shake > 0)
		{
			animationPlayer.StartHitstun();
			shake -= dtime;

		}

		if (hitstun > 0&& phase!=AIPhase.PHASEB_1&& type!=Type.GedGehog)
		{
			AIStopMove();
			animationPlayer.PlayAnimation(Animconstant._instance.hurt);
			hitstun -= dtime;
			//ChangePhase(AIPhase.MAIN_1);
			time = 0;
			return;
		}

		if (EventManager.instance.mainCharacter.isDeath)
		{
			ReSpawnMe();
		}
		aibase.AI();
	}

	private void HandleInfo()
	{
		combat.SendAllInfos();
		if (combatInfos == null||combatInfos.Count==0)
		{
			return;
		}

		for (int i = 0; i < combatInfos.Count; i++)
		{
			if (combatInfos[i].attackReceive == this)
			{
				shake = 0.25f;
				hitstun = 1f;
				HP -= (int)combatInfos[i].damage;
				animationPlayer.SetFlashMode(1.25f);
				DamageScript a = DamageManager.Instance.CreateDamage((int)combatInfos[i].damage, this, _transform.position);
				AIStopMove();
			}

		}
		ClearInfos();
	}

	public override bool isPlayer()
	{
		return false;
	}

	public void InitStart()
	{
		phase = AIPhase.MAIN_1;
		FindEnemyDate();
		aibase.Init();
	}

	public void EnableMe()
	{
		base.gameObject.SetActive(true);
	}

	public void ChangeDirByPlayer()
	{
		if (_transform.localPosition.x <= EventManager.instance.mainCharacter._transform.position.x)
		{
			ChangeDirection(Direction.RIGHT);
		}
		else
		{
			ChangeDirection(Direction.LEFT);
		}
	}

	public void ChangeDirByRandom()
	{
		if (Random.Range(0, 100) % 2 == 0)
		{
			ChangeDirection(Direction.RIGHT);
		}
		else
		{
			ChangeDirection(Direction.LEFT);
		}
	}

	public void ChangePhase(AIPhase _phase)
	{
		phase = _phase;
		time = 0;
	}

	public void NextPhase()
	{
		ChangePhase(phase += 1);
	}

	public void AIMove(float speed)
	{
		_phy.AIMove(speed);
	}

	public void AIStopMove()
	{
		_phy.AIMove(0.01f);
	}
	public void SetCounter(int x,float y)
	{
		_phy.counter[x] = y;
	}

	public void AddCounter(int x, float y)
	{
		_phy.counter[x] += y;
	}

	public void SubCounter(int x, float y)
	{
		_phy.counter[x] -= y;
	}

	public float GetCounter(int x)
	{
		return _phy.counter[x];
	}

	public bool CheckIsOutRoom()
	{
		if (_transform.position.x >= WorldManager.Instance.CurrentRoomEdgeL &&
		    _transform.position.x <= WorldManager.Instance.CurrentRoomEdgeR &&
		    _transform.position.y <= WorldManager.Instance.CurrentRoomEdgeT &&
		    _transform.position.y >= WorldManager.Instance.CurrentRoomEdgeB)
		{
			return false;
		}

		return true;
	}

	public bool IsLookingPlayer(float width, float height)
	{
		Vector3 vector = EventManager.instance.mainCharacter._transform.position;
		if (Mathf.Abs(_transform.position.x - vector.x) > width)
		{
			return false;
		}

		if (Mathf.Abs(_transform.position.y - vector.y) > height)
		{
			return false;
		}

		if (direction == Direction.LEFT && vector.x >= base.transform.position.x)
		{
			return false;
		}

		if (direction == Direction.RIGHT && vector.x <= base.transform.position.x)
		{
			return false;
		}

		return true;
	}

	public float CheckDisBetweenPlayerX()
	{
		return Mathf.Abs(_transform.position.x - EventManager.instance.mainCharacter._transform.position.x)/MainVar.instance.TILESIZE;
	}

	public float CheckDisBetweenPlayerY()
	{
		return Mathf.Abs(_transform.position.y - EventManager.instance.mainCharacter._transform.position.y)/MainVar.instance.TILESIZE;
	}

	private void InitAttackType()
	{
		combat.SetDetector(0,AttackType.enemyAttack1,1);
		combat.SetDetector(1,AttackType.bodyattack,10);
		combat.EnableDetector(1,AttackType.bodyattack,10);

	}

	public void DisableDetectorByHit(int index)
	{
		if (combat.GetDetector(index).IsHit)
		{
			combat.DisableDetector(index);
		}
	}

	public void FlipDir()
	{
		if (direction == Direction.LEFT)
		{
			ChangeDirection(Direction.RIGHT);
		}
		else
		{
			ChangeDirection(Direction.LEFT);
		}
	}

	public void DespawnMe()
	{
		base.gameObject.SetActive(false);
	}

	public void ReSpawnMe()
	{
		InitStart();
		time = 0;
		base.gameObject.SetActive(true);
		HP = maxHP;
		base._transform.position = SpawnPosition;


	}

	public void AIJump(float velocity)
	{
		_phy.SetMoveSpeedY(velocity);
		_phy.SetGraviyt(-70f);
	}

}
