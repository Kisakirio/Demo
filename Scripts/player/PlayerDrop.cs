
public class PlayerDrop : IStateHanlder
{
	private Player player;
	public void SetSystem(FSMSystem fsm)
	{
		if (fsm is FSMSystem playerstate)
		{
			player = playerstate.playerc_perfer;
		}

	}

	public void Enter(FSMSystem fsm)
	{
		player.SetGraviyt(-10*10f);
		player.SetMoveSpeedY(0);
		player.PlayAnimation(Animconstant._instance.playerDrop);

	}

	public void _Update(FSMSystem fsm)
	{
		var x= player.GettMoveSpeedX();
		player.SetMoveSpeedX(x);
		player.Filp();


	}

	public void Exit(FSMSystem fsm)
	{
		player.PlayAnimation(Animconstant._instance.playerland);
	}
}

