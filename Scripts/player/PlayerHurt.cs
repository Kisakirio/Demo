
	using UnityEngine;

	public class PlayerHurt: IStateHanlder
	{
		private Player player;

		private Direction dir;

		private float speed;
		public void SetSystem(FSMSystem fsm)
		{
			if (fsm is FSMSystem playerstate)
			{
				player = playerstate.playerc_perfer;
			}
		}

		public void Enter(FSMSystem fsm)
		{
			dir = player.hitForm;
			speed = dir == Direction.LEFT ? 24 * 1 : 24 * -1;
			player.animationPlayer.SetAnimationSpeed(0.3125f);
			player.animationPlayer.PlayAnimation(Animconstant._instance.hurt);
			ObjectPooler.instance.PlaySound(AllSound.PLAYERDAMAGEN,player._transform.position,1,0.5f);
			int a = Random.Range(0, 6);
			if (a == 0)
			{
				ObjectPooler.instance.PlaySound(AllSound.PLAYERDAMAGE1,player._transform.position);
			}
			else if(a==1)
			{
				ObjectPooler.instance.PlaySound(AllSound.PLAYERDAMAGE2,player._transform.position);
			}
			else if(a==2)
			{
				ObjectPooler.instance.PlaySound(AllSound.PLAYERDAMAGE3,player._transform.position);
			}

		}

		public void _Update(FSMSystem fsm)
		{

			speed = Mathf.Lerp(speed, 0, 1 - Mathf.Pow(0.6f, 25f*Time.deltaTime));
			player.SetMoveSpeedX(speed);
		}

		public void Exit(FSMSystem fsm)
		{
			player.SetHitBox(true);
			player.animationPlayer.SetAnimationSpeed(1f);
		}
	}

