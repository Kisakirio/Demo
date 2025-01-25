
using UnityEngine;

namespace player
{
	public class PlayerDeath : IStateHanlder
	{
		private Player player;
		private float targetX;
		private GameObject gm;
		public void SetSystem(FSMSystem fsm)
		{
			if (fsm is FSMSystem playerstate)
			{
				player = playerstate.playerc_perfer;
			}
		}

		public void Enter(FSMSystem fsm)
		{
			player.m_Controls.GamePlayer.Disable();
			targetX = player.direction == Direction.LEFT ? 60 : -60;
			player.SetCounter(0,0);
			player.SetCounter(1,0);
		}

		public void _Update(FSMSystem fsm)
		{
			player.PlayAnimation(Animconstant._instance.playerDeath);
			player.animationPlayer.SetAnimationSpeed(0.5f);
			targetX = Mathf.Lerp(targetX, 0, 1 - Mathf.Pow(0.6f , Time.deltaTime * 12.5f));
			player.SetMoveSpeedX(targetX);
			if (player.GetCurrenAnimationTime() > 0.999f)
			{

				SaveManager.instance.LoadGame();
				player.HP = (int)player.maxHP / 2;
				EventManager.instance.MovePlayerByTempPos();
				player.load.SetActive(true);
				player.AddCounter(1,Time.deltaTime);
				player.uiCanve.gameObject.SetActive(false);
			}

			if (player.GetCounter(1) > 1.5f)
			{
				player.isDeath = true;
				DisableLoad();
				player.load.SetActive(false);
				player.ChangeLogicState(PlayerState.IDLE);
			}

		}

		public void Exit(FSMSystem fsm)
		{
			player.m_Controls.GamePlayer.Enable();
			player.isDeath = false;

		}

		private void DisableLoad()
		{
			player.load.SetActive(false);
			player.uiCanve.gameObject.SetActive(true);
		}

	}
}
