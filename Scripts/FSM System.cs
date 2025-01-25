using System.Collections.Generic;
using player;
using UnityEngine;

public class FSMSystem
{
	public Dictionary<PlayerState, IStateHanlder> AddStateHandle = new Dictionary<PlayerState, IStateHanlder>();
	public PlayerState lastState;
	public PlayerState _curState;
	public PlayerState targetState;

	public IStateHanlder _curStateHandle;

	public float _curStateTime;

	public CharacterBase cb;

	public Player playerc_perfer;

	public EnemyController emc_perfer;

	public FSMSystem(CharacterBase _cb)
	{
		if (_cb.isPlayer())
		{
			playerc_perfer = _cb as Player;
		}
		else
		{
			emc_perfer=_cb as EnemyController;
		}
		Init();
	}

	public void initFSM()
	{
		lastState = PlayerState.IDLE;
		_curState = PlayerState.IDLE;
		targetState = PlayerState.IDLE;
		_curStateTime = 0;
		_curStateHandle = AddStateHandle[PlayerState.IDLE];


	}

	public void Init()
	{
		PInit();
		initFSM();
	}

	public void AddState(PlayerState logicState, IStateHanlder state)
	{
		AddStateHandle.Add(logicState, state);
	}



	public void Update()
	{
		//Debug.Log(_curState);
		CurStateTime();
		ChangeLogicState();
		if (_curState != targetState)
		{
			ProcessSwitchState();
			_curStateTime = 0;
		}


		_curStateHandle._Update(this);

	}

	protected void ProcessSwitchState()
	{
		if (_curStateHandle != null)
		{
			_curStateHandle.Exit(this);
		}

		lastState = _curState;
		_curState = targetState;
		CurStateTime();
		SetCurStateHandler();
		_curStateHandle.SetSystem(this);
		_curStateHandle.Enter(this);

	}

	public void SetTargetState(PlayerState state)
	{
		targetState = state;
	}

	public void SetCurrentState(PlayerState state)
	{
		_curState = state;
	}

	protected void SetCurStateHandler()
	{
		if (!AddStateHandle.TryGetValue(_curState, out _curStateHandle))
			Debug.Log("Cannot Find Match of curStateHandle bycurstate !");
	}

	public void CurStateTime()
	{
		_curStateTime += Time.deltaTime;
	}

	public PlayerState GetCurrentState()
	{
		return _curState;
	}




	public void PInit()
	{

		PlayerIdle handler = new PlayerIdle();
		PlayerJump handler1 = new PlayerJump();
		PlayerWalk handler2 = new PlayerWalk();
		PlayerDrop handler3 = new PlayerDrop();
		PlayerDoubleJump handler4 = new PlayerDoubleJump();
		PlayerAttackGround handler5 = new PlayerAttackGround();
		PlayerAirAttack handle6 = new PlayerAirAttack();
		PlayerHurt handle7 = new PlayerHurt();
		PlayerDeath handle8 = new PlayerDeath();
		PlayerSlide handle9 = new PlayerSlide();
		AddState(PlayerState.IDLE, handler);
		AddState(PlayerState.JUMP, handler1);
		AddState(PlayerState.WALK, handler2);
		AddState(PlayerState.DROP, handler3);
		AddState(PlayerState.DOUBLEJUMP, handler4);
		AddState(PlayerState.ATTACK1, handler5);
		AddState(PlayerState.ATTACK2, handle6);
		AddState(PlayerState.HURT,handle7);
		AddState(PlayerState.DEATH,handle8);
		AddState(PlayerState.SLIDE,handle9);

	}

	protected void ChangeLogicState()
	{
		playerc_perfer.justSlide += Time.deltaTime;
		switch (_curState)
		{
			case PlayerState.IDLE:
				if (playerc_perfer.isJumping&& playerc_perfer.isdown&& playerc_perfer.onGrounded()&& playerc_perfer.justSlide>=1f&& _curStateTime>0.02f)
				{
					SetTargetState(PlayerState.SLIDE);
					break;
				}

				if (playerc_perfer.IsJumping() && playerc_perfer.onGrounded()&& playerc_perfer.justSlide>0.25f)
				{
					SetTargetState(PlayerState.JUMP);
					break;
				}

				if (playerc_perfer.GettMoveSpeedX() != 0)
				{
					SetTargetState(PlayerState.WALK);
				}

				if (!playerc_perfer.onGrounded() && playerc_perfer._control.velocity.y <= 0)
				{

					SetTargetState(PlayerState.DROP);
				}

				break;


			case PlayerState.JUMP:
				if (playerc_perfer._control.velocity.y <= 0)
				{
					SetTargetState(PlayerState.DROP);
				}

				if ((playerc_perfer.isDash && playerc_perfer.canDash)&& SaveManager.instance.savedata.canDash)
				{
					SetTargetState(PlayerState.ATTACK2);
				}

				break;
			case PlayerState.DROP:
				if (playerc_perfer.onGrounded())
				{
					playerc_perfer.canDbJump = true;
					SetTargetState(PlayerState.IDLE);
				}

				if (playerc_perfer.isJumping && playerc_perfer.jumpCount == 1 && playerc_perfer.canDbJump)
				{
					SetTargetState(PlayerState.DOUBLEJUMP);
				}

				if (playerc_perfer.isDash && playerc_perfer.canDash&& _curStateTime>0.02f&& SaveManager.instance.savedata.canDash)
				{
					SetTargetState(PlayerState.ATTACK2);
				}

				break;
			case PlayerState.WALK:
				if (playerc_perfer.GettMoveSpeedX() == 0)
				{
					SetTargetState(PlayerState.IDLE);
				}
				if (playerc_perfer.isJumping&& playerc_perfer.isdown&& playerc_perfer.onGrounded()&& playerc_perfer.justSlide>=1f)
				{
					SetTargetState(PlayerState.SLIDE);
					break;
				}

				if (playerc_perfer.IsJumping())
				{
					SetTargetState(PlayerState.JUMP);
				}

				if (!playerc_perfer._control.m_colliderState.below)
				{
					SetTargetState(PlayerState.DROP);
				}

				break;
			case PlayerState.DOUBLEJUMP:
				if (playerc_perfer._control.velocity.y <= 0)
				{
					SetTargetState(PlayerState.DROP);
				}

				break;
			case PlayerState.ATTACK1:
				if (!playerc_perfer.IsCurrentAnimationAlmostFinished(1))
				{
					SetTargetState(PlayerState.ATTACK1);
				}

				break;
			case PlayerState.ATTACK2:
				if (!playerc_perfer.IsCurrentAnimationAlmostFinished(1))
				{
					SetTargetState(PlayerState.ATTACK2);
				}

				break;
			case PlayerState.HURT:
			{
				if (!playerc_perfer.IsCurrentAnimationAlmostFinished(1))
				{
					SetTargetState(PlayerState.HURT);
				}
				else
				{
					SetTargetState(PlayerState.IDLE);
				}


				break;
			}
		}

	}
}
