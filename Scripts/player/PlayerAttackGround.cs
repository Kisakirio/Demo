using System;
using System.Collections.Generic;
using PathologicalGames;
using Unity.VisualScripting;
using UnityEngine;

namespace player
{
	public class PlayerAttackGround : IStateHanlder
	{
		public enum groundAttack
		{
			playerattack1 = 1,
			playerattack2 = 2,
			playerattack3 = 3,
			playerattack4 = 4,
			playerattack4_strong = 5,
			playerupattack=6,
		}

		private string upAttack = Animconstant._instance.playerattack_upwardAttack;

		private bool allowNext;

		private Player player;

		private bool isupAttack;

		private bool changeAirAttack;

		private bool hasChecked;

		private List<CombatInfo> ci;

		private float speedY;


		public float STARTTIME=0.05f;
		private FSMSystem _fsm;
		public groundAttack logicattack;
		public void SetSystem(FSMSystem fsm)
		{
			if (fsm is FSMSystem playerstate)
			{
				_fsm = fsm;
				player = playerstate.playerc_perfer;
			}
		}

		public void Enter(FSMSystem fsm) {
			logicattack = groundAttack.playerattack1;
			if (player.isup){
				isupAttack = true;
				player.canAttak = false;
				logicattack = groundAttack.playerupattack;
			}
		}

		public void _Update(FSMSystem fsm)
		{

			//Debug.Log(logicattack);
			switch (logicattack)
			{
				case groundAttack.playerupattack:
				{
					player.animationPlayer.PlayAnimation(upAttack);
					AddCounter(1,Time.deltaTime);
					if (GetCounter(0) == 0 && GetCounter(1) >= STARTTIME && player.GetCurrenAnimationTime() > 0.25f)
					{
						SetCounter(0,1);
						SetCounter(2,0);
						SetCounter(3,0);
						SetCounter(5,0);
						SetCounter(6,0);
						SetCounter(7,0);
						player.PlayAttackVoice(AllSound.bomb1);

					}

					if (GetCounter(0) != 1 || GetCounter(1) < STARTTIME)
					{
						break;
					}

					if (GetCounter(6) == 0)
					{
						player.particleSystem[1].SetActive(true);
						player.SetMoveSpeedY(player.CalculateVelocity(5,.34f));
						player.SetGraviyt(player.CalculateGravity(5,.34f));
						SetCounter(6,1);
					}

					float num = 0.125f;
					if (player.animationPlayer.CurrentAnimationTime() > 0.35f + num * GetCounter(2) && GetCounter(2) < 4&&!player.IsHitBoxStart(1))
					{
						if (GetCounter(2) == 0)
						{
							ObjectPooler.instance.PlaySound(AllSound.PLAYERUPATTACK, player._transform.position,1, 0.5f);

						}
						player.DoAttack(1,AttackType.attackgroundNORMAL1,8);
						Debug.Log(player.animationPlayer.CurrentAnimationTime());

						AddCounter(2,1);
					}
					else
					{
						if (player.IsHit(1))
						{
							SetCounter(3,0.1f);
							SetCounter(7,1);
							ObjectPooler.instance.PlaySound(AllSound.PLAYERMELEEHIT1, player._transform.position,1,0.5f);
							if (GetCounter(5) == 0)
							{
								player.animationPlayer.StartHitstun();
								Transform _t = ObjectPooler.instance.CreateWeakHit();
								ci = player.GetCombatInfo();
								_t.position = HitTargetTransform(ci);
								SetCounter(5, 1);
							}
						}
						if (GetCounter(7) == 1)
						{
							if (GetCounter(3) > 0)
							{
								SubCounter(3, Time.deltaTime);
								player.animationPlayer.SetAnimationSpeed(0.16f);
								player.shake = 0.1f;
								player.animationPlayer.StartHitstun();
								if (GetCounter(8) == 0)
								{
									speedY = player._phy.GetCurrentMoveY();
									SetCounter(8,1);
								}
								//Debug.Log(speedY);
								player.SetMoveSpeedY(0f);
							}
							else
							{
								player.animationPlayer.SetAnimationSpeed(1f);
								player.SetMoveSpeedY(speedY);
								player.shake = 0;
								SetCounter(7,0);
							}
						}

						player.DisableDetectorByHit(1);
					}

					if (player.animationPlayer.CurrentAnimationTime()<0.999f)
					{
						break;
					}

					player.shake = 0;
					player.particleSystem[1].SetActive(false);
					if (player.attackbuffer == 0)
					{
						player.ChangeLogicState(PlayerState.DROP);
					}
					else
					{
						player.ChangeLogicState(PlayerState.ATTACK2);
					}
					break;
				}
				case groundAttack.playerattack1:
				{
					if (GetCounter(0) == 0)
					{
						SetCounter(0,1);
						player.PlayAnimation(Animconstant._instance.attack1);
						ObjectPooler.instance.PlaySound(AllSound.PLAYERATTACK1, player._transform.position,1,0.25f);

					}
					AddCounter(1,Time.deltaTime);
					if (GetCounter(1) < STARTTIME || player.GetCurrenAnimationTime() <.35f)
					{
						break;
					}

					if (!(player.IsHitBoxStart(0))&&GetCounter(3)==0)
					{
						player.DoAttack(0,AttackType.attackgroundNORMAL1,25);


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
						if (player.attackbuffer == 0)
						{
							player.ChangeLogicState(PlayerState.IDLE);
						}
						else if(player.attackbuffer!=0&&!player.isup)
						{
							logicattack = groundAttack.playerattack2;
							SetCounter(0,0);
							SetCounter(1,0);
							SetCounter(3,0);
							player.DisableDetector(0);
							player.attackbuffer = 0;
						}
						else if(player.attackbuffer!=0&& player.isup)
						{
							SetCounter(0,0);
							SetCounter(1,0);
							SetCounter(3,0);
							logicattack = groundAttack.playerupattack;
							player.DisableDetector(0);
						}
					}
					break;

				}
				case groundAttack.playerattack2:
				{
					if (GetCounter(0) == 0)
					{
						SetCounter(0, 1);
						SetCounter(4, 0);
						SetCounter(5,0);
						player.PlayAnimation(Animconstant._instance.playerattack2);
						ObjectPooler.instance.PlaySound(AllSound.PLAYERATTACK1, player._transform.position,1,0.25f);
						LayerMask layermask = LayerMask.GetMask("EnemyBase");
						Vector3 dir = player.IsFaceLeft() ? Vector3.left : Vector3.right;
						RaycastHit2D m_raycasthit = Physics2D.Raycast(player.transform.position+new Vector3(0,1.35f,0), dir, 3f, layermask);

						if (m_raycasthit)
						{
							player.animationPlayer.SetAnimationSpeed(0.4f);
							SetCounter(3,1);
						}
					}

					AddCounter(1,Time.deltaTime);
					if (GetCounter(1) < STARTTIME)
					{
						break;
					}
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

					if (player.GetCurrenAnimationTime() > 0.8f)
					{
						player.shake = 0;
					}
					if (player.GetCurrenAnimationTime() > 0.999f)
					{
						if (player.attackbuffer == 0)
						{
							player.ChangeLogicState(PlayerState.IDLE);
						}
						else if(player.attackbuffer!=0&&!player.isup)
						{
							logicattack = groundAttack.playerattack3;
							SetCounter(0,0);
							SetCounter(1,0);
							SetCounter(3,0);
							SetCounter(5,0);
							player.animationPlayer.SetAnimationSpeed(1f);
							player.DisableDetector(0);
							player.attackbuffer = 0;
						}
						else if(player.attackbuffer!=0&& player.isup)
						{
							SetCounter(0,0);
							SetCounter(1,0);
							SetCounter(3,0);
							logicattack = groundAttack.playerupattack;
						}
					}
					break;
				}
				case groundAttack.playerattack3:
				{
					if (GetCounter(0) == 0)
					{
						SetCounter(6,0);
						SetCounter(7,0);
						SetCounter(0,1);
						player.animationPlayer.SetAnimationSpeed(1f);
						player.PlayAnimation(Animconstant._instance.playerattack3);
						ObjectPooler.instance.PlaySound(AllSound.PLAYERATTACK1, player._transform.position,1,0.25f);
					}
					AddCounter(1,Time.deltaTime);
					if (GetCounter(1) < STARTTIME)
					{
						break;
					}
					holdTime();
					if (!player.IsHitBoxStart(0)&&GetCounter(3)==0)
					{
						player.DoAttack(0,AttackType.attackgroundNORMAL3,30);
						SetCounter(3,1);
					}
					else
					{
						if (player.IsHit(0))
						{
							ObjectPooler.instance.PlaySound(AllSound.PLAYERMELEEHIT1, player._transform.position);
							Transform _t=ObjectPooler.instance.CreateWeakHit();
							ci = player.GetCombatInfo();
							_t.position = HitTargetTransform(ci);
						}
						player.DisableDetectorByHit(0);
					}

					if (GetCounter(6) == 0 && player.GetCurrenAnimationTime() >= 0.125f)
					{
						SetCounter(6,1);
						/*playbgmagain*/
						ObjectPooler.instance.PlaySound(AllSound.PLAYERATTACK1, player._transform.position,1,0.25f);
					}

					if (GetCounter(7) == 0 && player.GetCurrenAnimationTime() >=0.375f)
					{
						player.DoAttack(0,AttackType.attackgroundNORMAL3,21);

						SetCounter(7,1);
					}
					else
					{

						player.DisableDetectorByHit(0);
					}

					if (player.GetCurrenAnimationTime() > 0.999f)
					{
						SetCounter(0,0);
						SetCounter(1,0);
						SetCounter(3,0);

						if (GetCounter(2)>.2f)
						{
							player.DisableDetector(0);
							logicattack = groundAttack.playerattack4_strong;
						}
						else if(player.attackbuffer!=0&&!player.isup)
						{
							logicattack = groundAttack.playerattack4;
							player.DisableDetector(0);
							player.attackbuffer = 0;
						}
						else if(player.attackbuffer!=0&& player.isup)
						{
							SetCounter(0,0);
							SetCounter(1,0);
							SetCounter(3,0);
							logicattack = groundAttack.playerupattack;
						}
						else
						{

							player.ChangeLogicState(PlayerState.IDLE);
						}

					}
					break;
				}
				case groundAttack.playerattack4:
				{

					if (GetCounter(0) == 0)
					{
						SetCounter(0,1);
						SetCounter(3,0);
						SetCounter(9,0);
						player.PlayAnimation(Animconstant._instance.playerattack4);
						/*******playersound*********/
						player.PlayAttackVoice(AllSound.finish1);


					}
					AddCounter(1,Time.deltaTime);
					if (GetCounter(1) > STARTTIME&& player.GetCurrenAnimationTime()>0.05f&& GetCounter(0)==0)
					{
						SetCounter(9,1);
						/*playbgm*/
					}
					if (!(GetCounter(1) >= STARTTIME) || player.GetCurrenAnimationTime()<0.25f)
					{
						break;
					}

					if (GetCounter(3) == 0 && !player.IsHitBoxStart(0))
					{
						ObjectPooler.instance.PlaySound(AllSound.PLAYERATTACK2, player._transform.position,1.33f);
						player.DoAttack(0,AttackType.attackgroundNORMAL4,35);
						SetCounter(3,1);
					}
					else
					{
						if (player.IsHit(0))
						{
							ObjectPooler.instance.PlaySound(AllSound.PLAYERMELEEHIT1, player._transform.position, 1);
							Transform _t=ObjectPooler.instance.CreateStrongHit();
							ci = player.GetCombatInfo();
							_t.position = HitTargetTransform(ci);
						}
						player.DisableDetectorByHit(0);
					}

					if (player.GetCurrenAnimationTime() > 0.999f)
					{
						player.ChangeLogicState(PlayerState.IDLE);
					}
					break;
				}
				case groundAttack.playerattack4_strong:
				{
					if (GetCounter(0) == 0)
					{
						SetCounter(0,1);
						SetCounter(3,0);
						player.PlayAnimation(Animconstant._instance.playerattack4_strong);
						/*******playersound*********/

					}
					AddCounter(1,Time.deltaTime);
					if (GetCounter(1) < STARTTIME|| player.GetCurrenAnimationTime()<0.25f)
					{
						break;
					}
					if (GetCounter(3) == 3 && !player.IsHitBoxStart(0)&&player.GetCurrenAnimationTime()>.8f)
					{
						player.DoAttack(0,AttackType.attackgroundNORMAL4H,40);
						SetCounter(3,4);
					}
					else
					{
						player.DisableDetectorByHit(0);
						if (player.GetCurrenAnimationTime() >= 0.425f && GetCounter(3) == 0f)
						{
							//////////顿帧+特效
							//Debug.Log("freeeze");
							ObjectPooler.instance.PlaySound(AllSound.PLAYERATTACKSWINGR, player._transform.position);
							player.animationPlayer.FreezeThisFrame();
							Transform t=ObjectPooler.instance.CreateStarGlow();
							t.position = player.IsFaceLeft() ? player.transform.position + new Vector3(3.4f, 1.5f, 0) : player.transform.position + new Vector3(-3.4f, 1.5f, 0);
							SetCounter(3,1);
						}
						else if(player.GetCurrenAnimationTime()>=0.465f&& GetCounter(3)==1f)
						{
							/////////////声音+特效
							player.PlayAttackVoice(AllSound.finish1);
							ObjectPooler.instance.PlaySound(AllSound.PLAYERATTACK3, player._transform.position);
							SetCounter(3,2);
						}
						else if(player.GetCurrenAnimationTime()>=21/36f&& GetCounter(3)==2)
						{
							SetCounter(3,3);
							ObjectPooler.instance.PlaySound(AllSound.PLAYERSTRONGHIT2, player._transform.position,1,0.25f);
							Transform _t= ObjectPooler.instance.CreateStrongH();
							_t.position = player.IsFaceLeft() ? player.transform.position + new Vector3(-3f, 0f, 0) : player.transform.position + new Vector3(3f, 0f, 0);

							//////////击中特效
						}
					}

					if (player.GetCurrenAnimationTime() >= .999f)
					{
						player.ChangeLogicState(PlayerState.IDLE);
					}
					break;
				}

			}


		}

		private void UpAttack()
		{
			if (player.IsCurrentAnimationThis(Animconstant._instance.playerattack_upwardAttack))
			{

				player.particleSystem[1].SetActive(true);
				player.SetMoveSpeedY(player.CalculateVelocity(5,.34f));
				player.SetGraviyt(player.CalculateGravity(5,.34f));

			}

		}

		public void Exit(FSMSystem fsm)
		{
			player.attackbuffer = 0;
			//attackCount = 0;
			//hasAdd = false;
			hasChecked = false;
			player.DisableDetector(0);
			player.DisableDetector(1);
			player.animationPlayer.SetAnimationSpeed(1f);
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

		public void holdTime()
		{
			if (player.isAttack)
			{
				AddCounter(2,Time.deltaTime);
			}
			else
			{
				SetCounter(2,0);
			}
		}

		public Vector3 HitTargetTransform(List<CombatInfo> ci)
		{
			if (ci.Count > 0)
			{
				Vector3 tragetposition = ci[0].hitPosition;
				Debug.Log(tragetposition);
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
