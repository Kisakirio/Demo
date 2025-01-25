using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
	[SerializeField]
	public ObjectPhysic _phy;

	[SerializeField]
	public AnimationPlayer animationPlayer;

	private Animator _animator;

	[HideInInspector]
	public CharacterController2D _control;

    [HideInInspector]
	public Rigidbody2D _rigidbody;

	[SerializeField]
	public Transform _transform;

	public Combat combat;

	protected List<CombatInfo> combatInfos=new List<CombatInfo>();

	public int HP=1000;

	public int maxHP = 1000;

	public float hitstun;

	public PlayControl m_Controls;

	public float shake;





	public Direction direction;


	public Type type;

	public bool isDeath;

	public void RenewHP()
	{
		maxHP = SaveManager.instance.GetMaxHP();
		if (HP > maxHP)
		{
			HP = maxHP;
		}
	}

	public bool onGrounded()
	{
		return _phy.IsGround();
	}

	public virtual bool isPlayer()
	{
		return false;
	}

	public virtual bool Isdead()
	{
		return HP <= 0;
	}

	public virtual void Awake()
	{
		combat = GetComponentInChildren<Combat>();
		combat.Init();
		_control = GetComponent<CharacterController2D>();
		_rigidbody = GetComponent<Rigidbody2D>();
	}

	public virtual void Start()
	{
		SaveManager.instance.LoadGame();
		RenewHP();
	}

	protected virtual void Update()
	{
		UpdateCombat();
	}
	public virtual void OnEnable()
	{

	}
	public virtual void OnDisable()
	{

	}

	public void Init()
	{

	}

	protected virtual void UpdateCombat()
	{
		if (!Isdead())
		{

		}
	}
	public void PlayAnimation(string animstate)
	{
		animationPlayer.PlayAnimation(animstate);
	}

	public bool IsCurrentAnimationThis(string name)
	{
		return animationPlayer.IsCurrentAnimationThis(name);
	}
	public void SetMoveSpeedY(float speedY)
	{
		_phy.SetMoveSpeedY(speedY);
	}

	public void SetMoveSpeedX(float speedX)
	{
		_phy.SetMoveSpeedX(speedX);
	}

	public void SetMoveSpeedXY(Vector2 speedXY)
	{
		_phy.SetMoveSpeedXY(speedXY);
	}
	public void SetGraviyt(float gravity)
	{
		_phy.SetGraviyt(gravity);
	}

	public float GetGravity()
	{
		return _phy.GetGravity();
	}

	public float CalculateGravity(float Maxhight,float MaxupTime)
	{
		return -2 * Maxhight / Mathf.Pow(MaxupTime, 2);
	}

	public float CalculateVelocity(float Maxhight,float MaxupTime )
	{
		return 2 * Maxhight / MaxupTime;
	}


	public string GetCurrentAnimationName()
	{
		return animationPlayer.GetCurrentAnimationName();
	}

	public bool IsCurrentAnimationAlmostFinished()
	{
		return animationPlayer.IsCurrentAnimationAlmostFinshed();
	}

	public bool IsCurrentAnimationAlmostFinished(float time)
	{
		return animationPlayer.IsCurrentAnimationAlmostFinshed(time);
	}

	public float GetCurrenAnimationTime()
	{
		return animationPlayer.CurrentAnimationTime();
	}

	public void GetInfos(CombatInfo ci)
	{
		combatInfos.Add(ci);
	}

	public void ClearInfos()
	{
		combatInfos.Clear();
	}

	public void DisableDetectorByHit(int index)
	{
		if (combat.GetDetector(index).IsHit)
		{
			combat.DisableDetector(index);
		}
	}

	public void DisableDetector(int index)
	{
		combat.DisableDetector(index);
	}

	public bool IsHitBoxStart(int index)
	{
		return combat.IsHitBoxStart(index);
	}

	public List<CombatInfo> GetCombatInfo()
	{
		return combat.GetCombatInfo();
	}

	public void ChangeDirection(Direction dir)
	{
		if (direction != dir)
		{
			direction = dir;
		}
	}

	public float HPPercent()
	{
		return (float)HP / (float)(maxHP);
	}

	public void DoAttack(int index,AttackType attackType, float damage)
	{
		combat.EnableDetector(index,attackType,damage);
	}

	public effectScript DoOutlineEffect(float size, float _speed, Color c)
	{
		effectScript effectScript = EffectManager.instance.CreateEffect(this, animationPlayer.transform.position,animationPlayer.outlinesprite);
		if (effectScript != null)
		{
			effectScript.SetSpriteFlip(animationPlayer.basesprite.flipX);
			effectScript.targetsize=size;
			effectScript.speed=_speed;
			effectScript.SetColor(c);
		}
		return effectScript;
	}

	public  bool IsHit(int index)
	{
		return combat.IsHit(index);
	}
}
