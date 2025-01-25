using System;
using System.Collections.Generic;
using player;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Playables;


public class Player : CharacterBase
{

	private Vector2 movement;
	public bool isJumping { private set; get; }
	private bool isMoving;
	private LayerMask layerMask;
	private Vector2 size;
	private bool onGround;
	[NonSerialized]
	public float justJumped=0f;


	public Camera timelinc;


	[NonSerialized]
	public int jumpCount=1;
	private Vector2 moveX;
	private Vector2 bottonTransform;
	private Vector2[] originPosition;
	private ContactFilter2D ContactFilter2D;
	private RaycastHit2D[] result;

	public float A_holdTime;

	public List<GameObject> particleSystem;

	public GameObject uiCanve;

	private bool  isdoubleJump;

	public bool isAttack { private set; get; }

	public bool canDbJump;

	public float m_HoldTime = 0;


	private Vector2 m_V1;

	private FSMSystem _playerfsm;

	public bool canAttak=true;

	public bool canDash;

	public bool canSlide;

	public bool isDash { private set; get; }

	public bool canAirAttack=true;

	public SpriteRenderer effectSprite;

	public Direction hitForm;

	public GameObject hitbox;

	private bool AttackVoiceCooldown;

	private bool FinishVoiceCooldown;

	private bool BurstVoiceCooldown;

	public GameObject load;

	public bool isSlide;

	public float justSlide;



	public bool isup => m_Controls.GamePlayer.Move.ReadValue<Vector2>().y>0;

	public bool isdown => m_Controls.GamePlayer.Move.ReadValue<Vector2>().y < 0;

	public int attackbuffer;

	public override void Start()
	{
		base.Start();
		SpawnMe();
	}


	public override void Awake()
	{

		base.Awake();
		InitAttackType();
		_playerfsm = new FSMSystem(this);
		m_Controls = new PlayControl();
		m_Controls.GamePlayer.Jump.performed += ctx => { isJumping = true; };
		m_Controls.GamePlayer.Jump.canceled += ctx => {
			isJumping = false;
			if(_control.velocity.y<=0)
			    jumpCount = 1;
		};
		m_Controls.GamePlayer.Attack.started += ctx =>
		{
			GetAttack();
		};
		m_Controls.GamePlayer.Attack.performed+= ctx =>
		{
			isAttack = true;
		};
		m_Controls.GamePlayer.Attack.canceled += ctx =>
		{
			isAttack = false;
			A_holdTime = 0;
			canAttak = true;
		};
		m_Controls.GamePlayer.Dash.performed += ctx =>
		{
			isDash = true;
			isSlide = true;
		};
		m_Controls.GamePlayer.Dash.canceled += ctx =>
		{
			isDash = false;
			isSlide = false;
		};

	}

	private SpriteRenderer GetChildSprite(Transform transform,string name)
	{
		foreach (Transform child in transform)
		{
			if (child.name == name)
				return child.GetComponent<SpriteRenderer>();
		}

		return null;
	}

	public void SpawnMe()
	{
		Vector3 position=new Vector3(0,0,0);
		position.x = SaveManager.instance.savedata.x;
		position.y = SaveManager.instance.savedata.y;
		_transform.position = position;

	}

	public void Filp()
	{
		if (GettMoveSpeedX()>.1f)
		{
			_transform.localRotation = Quaternion.Euler(0, 180, 0);
			effectSprite.flipX = true;
			effectSprite.transform.localRotation = Quaternion.Euler(0, 180, 0);
		}

		if (GettMoveSpeedX()<-.1f)
		{
			_transform.localRotation = Quaternion.Euler(0, 0, 0);
			effectSprite.flipX = false;
			effectSprite.transform.localRotation = Quaternion.Euler(0, 0, 0);
		}
	}


    public override void OnEnable()
	{

		m_Controls.Enable();
	}

	public override void OnDisable()
	{
		m_Controls.Disable();
	}

	protected override void  Update()
	{
		_phy._Update();
		_playerfsm.Update();
		base.Update();
		DamageManager.Instance._Update();
		EffectManager.instance._Update();

	}



	public void Filp(Vector2 Forward)
	{
		if (Forward.x > 0)
			_transform.localRotation = Quaternion.Euler(180, 0, 180);
		else if (Forward.x < 0)
			_transform.localRotation = Quaternion.Euler(0, 0, 0);
	}

	public bool IsFaceLeft()
	{
		return _transform.localRotation.y == 0;
	}



	public float GettMoveSpeedX()
	{
		moveX = m_Controls.GamePlayer.Move.ReadValue<Vector2>()*15f;
		return moveX.x;
	}

	public bool IsJumping()
	{
		if (_control.velocity.y < 0)
		{
			return false;
		}

		if ((isJumping || justJumped > 0) && jumpCount == 1)
		{
			jumpCount = 0;
			return true;
		}

		return false;
	}


	public override bool isPlayer()
	{
		return true;
	}


	private void InitAttackType()
	{
		combat.SetDetector(0,AttackType.attackgroundNORMAL1,1);
		combat.SetDetector(1,AttackType.attackgroundNORMAL1,1);

	}

	protected override void UpdateCombat()
	{
		combat.SendAllInfos();
		//Debug.Log(combatInfos.Count);
		if (combatInfos == null||combatInfos.Count==0)
		{
			return;
		}
		for (int i = 0; i < combatInfos.Count; i++)
		{
			if (combatInfos[i].attackReceive == this)
			{
				HP -= (int)combatInfos[i].damage;
				animationPlayer.SetFlashMode(2f);
				DamageScript a = DamageManager.Instance.CreateDamage((int)combatInfos[i].damage, this, _transform.position);
				hitForm = _transform.position.x - combatInfos[i].hitPosition.x > 0 ? Direction.LEFT : Direction.RIGHT;
				ChangeLogicState(PlayerState.HURT);
			}

		}

		if (HP <= 0)
		{
			Debug.Log(true);
			HP = 0;
			ChangeLogicState(PlayerState.DEATH);
		}
		ClearInfos();
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

	public void ChangeLogicState(PlayerState state)
	{
		_playerfsm.SetTargetState(state);
	}


	public void GetAttack()
	{
		IStateHanlder handle = _playerfsm._curStateHandle;
		PlayerAttackGround _attack=null;
		PlayerAirAttack _attack1 = null;
		if (_control.OnGround())
		{
			if (handle is PlayerAttackGround)
			{
				_attack = handle as PlayerAttackGround;
			}
			switch (_playerfsm._curState)
			{
				case PlayerState.IDLE:
				{
					SetCounter(0,0);
					SetCounter(1,0);
					SetCounter(3,0);
					ChangeLogicState(PlayerState.ATTACK1);
					break;
				}
				case PlayerState.WALK:
				{
					SetCounter(0,0);
					SetCounter(1,0);
					SetCounter(3,0);
					ChangeLogicState(PlayerState.ATTACK1);
					break;
				}
				case PlayerState.ATTACK1:
					if (_attack != null)
					{
						switch (_attack.logicattack)
						{
							case PlayerAttackGround.groundAttack.playerattack1:
							{
								if (attackbuffer == 0&& _playerfsm._curStateTime>=0.025f)
								{
									attackbuffer = 1;
								}
								break;
							}
							case PlayerAttackGround.groundAttack.playerattack2:
							{
								if (attackbuffer == 0&& _playerfsm._curStateTime>=0.025f)
								{
									attackbuffer = 1;
								}
								break;
							}
							case PlayerAttackGround.groundAttack.playerattack3:
							{
								if (attackbuffer == 0&& _playerfsm._curStateTime>=0.025f)
								{
									attackbuffer = 1;
								}
								break;
							}
							case PlayerAttackGround.groundAttack.playerattack4:
							{
								if (attackbuffer == 0&& _playerfsm._curStateTime>=0.025f)
								{
									attackbuffer = 1;
								}
								break;
							}
							case PlayerAttackGround.groundAttack.playerupattack:
							{
								if (attackbuffer == 0&& _playerfsm._curStateTime>=0.025f)
								{
									attackbuffer = 1;
								}
								break;
							}

						}

					}
					break;
			}
		}
		else
		{
			if (canAirAttack)
			{
				if (handle is PlayerAirAttack)
				{
					_attack1 = handle as PlayerAirAttack;
				}

				switch (_playerfsm._curState)
				{
				case PlayerState.DROP:
				{
					SetCounter(0, 0);
					SetCounter(1, 0);
					SetCounter(3, 0);
					ChangeLogicState(PlayerState.ATTACK2);
					break;
				}
				case (PlayerState.JUMP):
				{
					SetCounter(0, 0);
					SetCounter(1, 0);
					SetCounter(3, 0);
					ChangeLogicState(PlayerState.ATTACK2);
					break;
				}
				case PlayerState.ATTACK2:
				{
					if (_attack1 != null)
					{
						switch (_attack1.logicstate)
						{
							case PlayerAirAttack.airAttack.playerattack1:
							{
								if (attackbuffer == 0 && _playerfsm._curStateTime >= 0.025f)
								{
									attackbuffer = 1;
								}

								break;
							}
							case PlayerAirAttack.airAttack.playerattack2:
							{
								if (attackbuffer == 0 && _playerfsm._curStateTime >= 0.025f)
								{
									attackbuffer = 1;
								}

								break;

							}
							case PlayerAirAttack.airAttack.playerattack3:
							{
								if (attackbuffer == 0 && _playerfsm._curStateTime >= 0.025f)
								{
									attackbuffer = 1;
								}

								break;
							}
							case PlayerAirAttack.airAttack.playerdash:
							{
								if (attackbuffer == 0 && _playerfsm._curStateTime >= 0.025f)
								{
									attackbuffer = 1;
								}

								break;
							}
						}
					}

					break;
				}
				}
			}
		}
	}

	public void PlayAttackVoice(AllSound _voice)
	{
		if ((AttackVoiceCooldown && _voice == AllSound.bomb1) || (FinishVoiceCooldown && _voice==AllSound.finish1)|| (BurstVoiceCooldown && _voice==AllSound.burst1) )
		{
			return;
		}

		AllSound voice=AllSound.NONE;
		if (_voice != 0)
		{
			if (Random.Range(0, 1) <= 0.5f)
			{
				 voice = _voice;
			}
			else
			{
				voice = _voice++;
			}
			ObjectPooler.instance.PlaySound(voice,_transform.position);
		}
		switch (_voice)
		{
			case AllSound.finish1:
				FinishVoiceCooldown = true;
				Invoke("ResetVoiceCooldown_Finish",  Random.Range(20f, 40f));
				break;
			case AllSound.bomb1:
				AttackVoiceCooldown = true;
				Invoke("ResetVoiceCooldown_Attack",  Random.Range(16f, 32f));
				break;
			case AllSound.burst1:
				BurstVoiceCooldown = true;
				Invoke("ResetVoiceCooldown_Burst",  Random.Range(5f, 20f));
				break;
		}
	}

	public void ResetVoiceCooldown_Finish()
	{
		FinishVoiceCooldown = false;
	}
	public void ResetVoiceCooldown_Attack()
	{
		AttackVoiceCooldown= false;
	}

	public void ResetVoiceCooldown_Burst()
	{
		BurstVoiceCooldown= false;
	}


	public void SetJustJumped(float time)
	{
		_control.justJumped = time;
	}
	public void SetHitBox(bool start)
	{
		hitbox.SetActive(start);
	}


}

