using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace player
{
	public class PlayerAirAttack:IStateHanlder
	{
		private Player player;
		private float dashTime;
		private float dashSpeed;
		private float dashDirection;
		private bool isDash;
		private float STARTTIME=0.05f;
		private List<CombatInfo> ci;
		public enum airAttack
		{
			playerattack1 = 1,
			playerattack2 = 2,
			playerattack3 = 3,
			playerattack4 = 4,
			playerattack4_strong = 5,
			playerdash=6,
		}

		public airAttack logicstate;
		public void SetSystem(FSMSystem fsm)
		{
			if (fsm is FSMSystem playerstate)
			{
				player = playerstate.playerc_perfer;
			}
		}

		public void Enter(FSMSystem fsm)
		{
			dashDirection = player.transform.rotation.y == 0 ? -1 : 1;
			dashTime = .333f;
			dashSpeed =dashDirection* 37.5f;
			isDash = player.isDash;
			logicstate = airAttack.playerattack1;
			if (isDash)
			{
				logicstate = airAttack.playerdash;
				player.SetGraviyt(0);
				player.SetMoveSpeedY(0);
				SetCounter(0,0);
				SetCounter(1,0);
			}
		}

		public void _Update(FSMSystem fsm)
		{
			//Debug.Log(logicstate);
			switch (logicstate)
			{
				case airAttack.playerdash:
				{
					player.PlayAnimation(Animconstant._instance.playerDash);
					if (GetCounter(0) == 0)
					{
						player.PlayAttackVoice(AllSound.burst1);
						SetCounter(0,1);
						SetCounter(1,0);
						SetCounter(2,0);
						SetCounter(3,0);
						SetCounter(4,0.35f);
						player.particleSystem[0].SetActive(true);
						Debug.Log("111");
						ObjectPooler.instance.PlaySound(AllSound.dash, player._transform.position,1,0.25f);
					}
					if (GetCounter(0) != 1&& GetCounter(1)<=STARTTIME)
					{
						break;
					}
					AddCounter(1,Time.deltaTime);
					if (GetCounter(4) > 0)
					{
						SubCounter(4,Time.deltaTime);
						player.SetMoveSpeedX(dashSpeed);
						player.SetMoveSpeedY(0);
					}
					Debug.Log(GetCounter(4));
					if (!(player.IsHitBoxStart(0))&&GetCounter(3)==0)
					{
						player.DoAttack(0,AttackType.attackgroundNORMAL1,30);
						AddCounter(3,1f);
					}
					else
					{
						if (player.IsHit(0))
						{
							ObjectPooler.instance.PlaySound(AllSound.PLAYERMELEEHIT1, player._transform.position);
							Transform _t = ObjectPooler.instance.CreateWeakHit();
							_t.position = HitTargetTransform(player.GetCombatInfo());
							player.shake = 0.1f;
							player.SetMoveSpeedY(3);
							SetCounter(4,0);
						}
						player.DisableDetectorByHit(0);
					}

					if (player.GetCurrenAnimationTime() > 0.999f || GetCounter(4) <= 0)
					{
						player.SetMoveSpeedX(0);
						player.canDash = false;
						player.particleSystem[0].SetActive(false);
						player.ChangeLogicState(PlayerState.DROP);

					}

					break;
				}
				case airAttack.playerattack1:
				{
					player.SetMoveSpeedX(0);
					if (GetCounter(0) == 0f)
					{
						player.PlayAnimation(Animconstant._instance.playerAirAttack1);
						SetCounter(0, 1f);
						SetCounter(1, 0f);
						SetCounter(2, 0f);
						SetCounter(3, 0f);
						SetCounter(4, 0f);
						ObjectPooler.instance.PlaySound(AllSound.PLAYERATTACK1, player._transform.position, 1, 0.25f);
					}
					AddCounter(1,Time.deltaTime);
					if (GetCounter(0) != 1&& GetCounter(1)<=STARTTIME)
					{
						break;
					}

					if (GetCounter(4) == 1)
					{
						player.SetMoveSpeedY(0f);
					}

					if (!(player.IsHitBoxStart(0))&&GetCounter(3)==0&& player.GetCurrenAnimationTime()>0.25f)
					{
						player.DoAttack(0,AttackType.attackgroundNORMAL1,15);

						AddCounter(3,1f);
					}
					else
					{
						if (player.IsHit(0))
						{
							ObjectPooler.instance.PlaySound(AllSound.PLAYERMELEEHIT1, player._transform.position);
							Transform _t = ObjectPooler.instance.CreateWeakHit();
							_t.position = HitTargetTransform(player.GetCombatInfo());
							SetCounter(4,1);
						}
						player.DisableDetectorByHit(0);
					}

					if (player.GetCurrenAnimationTime() < 0.999f)
					{
						break;
					}
					if (GetCounter(4) == 0)
					{
						player.ChangeLogicState(PlayerState.DROP);
					}
					else if(player.attackbuffer!=0&& GetCounter(4)!=0)
					{
						SetCounter(0,0);
						SetCounter(1,0);
						SetCounter(2,0);
						SetCounter(3,0);
						logicstate = airAttack.playerattack2;
						player.attackbuffer = 0;
					}
					else
					{
						player.ChangeLogicState(PlayerState.DROP);
					}

					break;
				}
				case airAttack.playerattack2:
				{
					player.SetMoveSpeedY(0);
					player.PlayAnimation(Animconstant._instance.playerAirAttack2);
					if (GetCounter(0) == 0)
					{
						SetCounter(0, 1f);
						SetCounter(1, 0f);
						SetCounter(2, 0f);
						SetCounter(3, 0f);
						SetCounter(4, 0f);
						ObjectPooler.instance.PlaySound(AllSound.PLAYERATTACK1, player._transform.position, 1, 0.25f);
					}
					AddCounter(1,Time.deltaTime);
					if (GetCounter(0) ==0&&GetCounter(1)<STARTTIME)
					{
						break;
					}
					if (!(player.IsHitBoxStart(0))&&GetCounter(3)==0)
					{
						player.DoAttack(0,AttackType.attackgroundNORMAL1,20);
						AddCounter(3,1f);
					}
					else
					{
						if (player.IsHit(0))
						{
							ObjectPooler.instance.PlaySound(AllSound.PLAYERMELEEHIT1, player._transform.position);
							Transform _t = ObjectPooler.instance.CreateWeakHit();
							_t.position = HitTargetTransform(player.GetCombatInfo());
						}
						player.DisableDetectorByHit(0);
					}

					if (player.GetCurrenAnimationTime() > 0.999f)
					{
						if (player.attackbuffer != 0)
						{
							player.DisableDetector(0);
							SetCounter(0, 0);
							SetCounter(1, 0);
							logicstate = airAttack.playerattack3;
							player.attackbuffer = 0;
						}
						else
						{
							player.ChangeLogicState(PlayerState.DROP);
						}
					}

					break;
				}
				case airAttack.playerattack3:
				{
					player.SetMoveSpeedY(0);
					player.PlayAnimation(Animconstant._instance.playerAirattack3);
					if (GetCounter(0) == 0)
					{
						SetCounter(0,1);
						SetCounter(1,0);
						SetCounter(2,0);
						SetCounter(3,0);
						SetCounter(4,0);
						SetCounter(5,0);
						ObjectPooler.instance.PlaySound(AllSound.PLAYERATTACK1, player._transform.position, 1f, 0.25f);
					}

					if (GetCounter(0) != 1 && GetCounter(1) <= STARTTIME)
					{
						break;
					}
					AddCounter(1,Time.deltaTime);

					float  num1 = 0.2f;
					int num2 = 1;
					num2 += 2;
					if (player.GetCurrenAnimationTime()>=0.2+num1*GetCounter(4)&& GetCounter(4)<num2&&!player.IsHitBoxStart(0))
					{
						if (GetCounter(3) == 1)
						{
							player.shake = 0.1f;
							player.animationPlayer.StartHitstun();
						}
						player.DoAttack(0,AttackType.attackgroundNORMAL2,8);
						AddCounter(4,1);
					}
					else
					{

						if (player.IsHit(0)&& GetCounter(5)==0)
						{
							ObjectPooler.instance.PlaySound(AllSound.PLAYERMELEEHIT1, player._transform.position);
							Transform _t = ObjectPooler.instance.CreateWeakHit();
							ci = player.GetCombatInfo();
							_t.position = HitTargetTransform(ci);
							SetCounter(5,1);
						}
						player.DisableDetectorByHit(0);
					}

					if (player.GetCurrenAnimationTime() >= 0.999)
					{
						SetCounter(0,0);
						SetCounter(1,0);
						player.ChangeLogicState(PlayerState.DROP);
					}

					break;
				}
				case airAttack.playerattack4:
				{


					break;
				}
			}

		}

		public void Exit(FSMSystem fsm)
		{
			player.attackbuffer = 0;
			if (logicstate != airAttack.playerdash)
			{
				player.canAirAttack = false;
			}

			player.DisableDetector(0);
			player.canDash = false;
			player.shake = 0;
		}

		public void SetCounter(int x,float y)
		{
			player.SetCounter(x,y);
		}

		public void AddCounter(int x, float y)
		{
			player.AddCounter(x,y);
		}

		public void SubCounter(int x, float y)
		{
			player.SubCounter(x,y);
		}

		public float GetCounter(int x)
		{
			return player.GetCounter(x);
		}
		public Vector3 HitTargetTransform(List<CombatInfo> ci)
		{
			if (ci.Count > 0)
			{
				Vector3 tragetposition = ci[0].hitPosition;
				bool flag = ci[0].hitPosition.x > EventManager.instance.mainCharacter.transform.position.x;
				for (int i = 0; i < ci.Count; i++)
				{
					if (flag)
					{
						if (ci[i].hitPosition.x > tragetposition.x)
						{
							tragetposition = ci[i].hitPosition;
						}
					}
					else
					{
						if (ci[i].hitPosition.x < tragetposition.x)
						{
							tragetposition = ci[i].hitPosition;
						}
					}
				}

				return tragetposition;
			}
			return Vector3.zero;
		}

	}
}

