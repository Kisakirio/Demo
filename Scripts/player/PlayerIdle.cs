using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : IStateHanlder
{
	private Player player;
	public void SetSystem(FSMSystem fsm)
	{

		if (fsm is FSMSystem playerfsm)
		{
			player = playerfsm.playerc_perfer;

		}
	}

	public void Enter(FSMSystem fsm)
	{
		player.SetGraviyt(-10f);
		player.canAirAttack = true;
		player.canDash = true;
		player.SetMoveSpeedX(0);
		Vector2 vector2 =new Vector2(1f,2.74f);
		Vector2 vector1 = new Vector2(0, 1.45f);
		player._control.m_Boxcollider.size=vector2;
		player._control.m_Boxcollider.offset = vector1;
	}

	public void _Update(FSMSystem fsm)
	{
		player.PlayAnimation(Animconstant._instance.stand);
	}

	public void Exit(FSMSystem fsm)
	{

	}
}
