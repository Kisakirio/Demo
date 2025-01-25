using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalk : IStateHanlder
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
	}

	public void _Update(FSMSystem fsm)
	{

           player.SetMoveSpeedX(player.GettMoveSpeedX());
           player.Filp();
           player.PlayAnimation(Animconstant._instance.Run);
	}

	public void Exit(FSMSystem fsm)
	{
		player.SetMoveSpeedX(0f);
	}
}
