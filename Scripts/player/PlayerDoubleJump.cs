using UnityEngine;

public class PlayerDoubleJump:IStateHanlder
{
	private Player player;


	private float MaxupTime = .4f;
	private float Minhight = 4f;
	private  float Maxhight =6f;
	private float gravity;
	private float startPosition;
	public void SetSystem(FSMSystem fsm)
	{
		if (fsm is FSMSystem playerstate)
		{
			player = playerstate.playerc_perfer;
		}
	}
	public void Enter(FSMSystem fsm)
	{
		gravity= -2 * Maxhight / Mathf.Pow(MaxupTime, 2);
		player.SetGraviyt(gravity);
		player.SetMoveSpeedY(2 * Maxhight / MaxupTime);
		player.m_HoldTime = 0;
		startPosition = player._rigidbody.position.y;
		player.justJumped = .29f;
		player.jumpCount = 0;

	}

	public void _Update(FSMSystem fsm)
	{
		var x= player.GettMoveSpeedX();
		if (x != 0)
		{
			player.SetMoveSpeedX(x);
			player.Filp();
		}

		if (player.isJumping)
		{
			player.m_HoldTime += Time.deltaTime;
		}
		else
		{

			if (player.m_HoldTime is <= .15f and > 0 &&Minhight - player._rigidbody.position.y + startPosition>=0)
			{
				player.jumpCount++;
				player.m_HoldTime = 0;
				player.SetGraviyt(Mathf.Pow(player._control.velocity.y,2)/ -(2 * (Minhight - player._rigidbody.position.y + startPosition)));
			}
			else if(player.m_HoldTime>.15f)
			{
				player.jumpCount++;
				player.m_HoldTime = 0;
				player.SetGraviyt(1.5f*gravity);
			}
		}

		player.PlayAnimation(Animconstant._instance.playerDoubleJump);
		player.justJumped -= Time.deltaTime;

	}

	public void Exit(FSMSystem fsm)
	{
		player.justJumped = 0;
		player.canDbJump = false;
	}
}
