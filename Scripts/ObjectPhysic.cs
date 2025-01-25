using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPhysic : MonoBehaviour
{
	private float _gravity=-10f;
	private float groundGravity = .2f;

	public CharacterController2D _control;

	private Rigidbody2D _rigidbody;

	private Vector2 _CurMovement;

	private Vector2 _PreMovement;

	private Vector2 _targetMovement;

	public bool isGrounded;

	public float ai_move;

	public CharacterBase cb;

	public float time;

	public float[] counter = new float[10];

	private float moveY;

	public void Awake()
	{
		_control = GetComponent<CharacterController2D>();
	}


	public void HandlGravity()
	{


		float preVelocity = _CurMovement.y;
		_CurMovement.y+= _gravity * Time.deltaTime;
		_CurMovement.y = Mathf.Max(_CurMovement.y, -30f);
		_targetMovement.y = (preVelocity + _CurMovement.y) * .5f;

	}

	public virtual void _Update()
	{
		time += Time.deltaTime;
		if (cb.type == Type.Smog)
		{
			return;
		}
		HandlGravity();
		if (this is PlayerPhy)
		{
			_targetMovement.x = _CurMovement.x;
		}

		float num = ai_move;
		if (GetAIMove() !=0)
		{
			if (cb.GetCurrenAnimationTime() > 0.999)
			{
				cb.animationPlayer.PlayAnimation(Animconstant._instance.Run);
			}

			num = ai_move;
			if (num < 0.1 && num > -0.1)
			{
				num = 0;
			}
			if (cb.direction == Direction.LEFT)
			{
				num *= -1;
			}

			float smooth = GetSmooth(45f);
			ai_move = Mathf.Lerp(ai_move, 0, smooth);
			if (ai_move < 0 && ai_move >- 0.05f)
			{
				ai_move = 0;
			}

			if (ai_move > 0 && ai_move < 0.05f)
			{
				ai_move = 0;
			}

			_targetMovement.x = Mathf.Lerp(_targetMovement.x,num,GetSmooth(66));
		}
		else if(GetAIMove()<=0.01f&&this is EnemyPhy)
		{
			if (cb.animationPlayer.IsCurrentAnimationThis(Animconstant._instance.Run))
			{
				cb.animationPlayer.PlayAnimation(Animconstant._instance.stand);
			}
		}

		_control.Move(_targetMovement * Time.deltaTime, 1);

	}

	public void SetGraviyt(float gravity)
	{
		_gravity = gravity;
	}

	public float GetGravity()
	{
		return _gravity;
	}

	public void SetMoveSpeedY(float speedY)
	{
		_CurMovement.y = speedY;
	}

	public void SetMoveSpeedX(float speedX)
	{
		_CurMovement.x = speedX;
	}

	public void SetMoveSpeedXY(Vector2 speedXY)
	{
		_CurMovement = speedXY;
	}

	public float GetCurrentMoveY()
	{
		return _CurMovement.y;
	}
	public bool IsGround()
	{
		return _control.IsGrounded;
	}

	public void AIMove(float speed)
	{
		ai_move = speed;
	}
	public float GetAIMove()
	{
		return ai_move;
	}

	public float GetSmooth(float speed)
	{
		return 1 - Mathf.Pow(0.6f, Time.deltaTime * speed);
	}
}
