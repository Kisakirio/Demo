
	using UnityEngine;

	public class RobotDog:AIBase
	{
		public override Type type => Type.RobotDog;

		private EnemyController enc;

		private bool firstloop=true;

		private bool noticed;

		private float targetlose;

		private int shoottimes;

		private bool r=true;

		private float hpdetect = 1f;

		public RobotDog(EnemyController _enc)
		{
			enc = _enc;
		}
		public override void Init()
		{
			firstloop = true;
			enc.phase = AIPhase.MAIN_1;
			noticed = false;
			r = true;
			targetlose = 0;
		}

		public override void AI()
		{
			//Debug.Log(enc.phase);
			switch (enc.phase)
			{
				case AIPhase.MAIN_1:
				{
					if (!noticed)
					{
						if (enc.HPPercent() < hpdetect)
						{
							enc.ChangePhase(AIPhase.MAIN_3);
							enc.ChangeDirByPlayer();
							noticed = true;
							firstloop = false;
							enc.SetCounter(4, 2f);
							break;
						}
					}
					if (enc.time < 0.1f)
					{
							enc.time = 0.1f;
							if (!firstloop)
							{
								if (enc.CheckDisBetweenPlayerX() > 8 && !noticed)
								{
									enc.FlipDir();
								}
								else
								{
									enc.ChangeDirByPlayer();
									if (!noticed && Random.Range(0, 100) % 11 <= 5)
									{
										enc.FlipDir();
									}
								}
							}
					}

					if (enc.time < 2 && enc.time >= 0.1f && !firstloop)
					{
							enc.AIMove(5);
					}

					if (enc.time > 0.1f)
					{
							float num = 2.75f;
							if (enc.IsLookingPlayer(100f, 3f))
							{
								firstloop = false;
								enc.ChangePhase(AIPhase.PHASEA_1);
							}

							if (enc.time > num && !firstloop)
							{
								firstloop = false;
								enc.ChangePhase(AIPhase.MAIN_3);
								enc.SetCounter(0,0);
							}
					}
					if (noticed)
					{
						firstloop =false;
						enc.ChangePhase(AIPhase.MAIN_2);
					}


					break;
				}
				case AIPhase.MAIN_2:
				{
					if (enc.time > 0.125f||r)
					{
						if (enc.GetCounter(2) >= 0.3f && enc.GetCounter(3) <= 0f)
						{
							enc.SubCounter(2, 0.3f);
							if (enc.IsLookingPlayer(200f, TILESIZE * 3.5f)||r)
							{
								if (enc.CheckDisBetweenPlayerY() >2.5f && enc.CheckDisBetweenPlayerX() < 2f)
								{
									targetlose++;
									if (targetlose >= 2)
									{
										noticed = false;
										targetlose = 0;
										enc.ChangePhase(AIPhase.PHASEC_1);
										break;
									}
								}
								if (enc.onGrounded())
								{
									enc.ChangeDirByPlayer();
								}

								enc.time = 0f;
							}
							else
							{
								enc.SetCounter(3, 1f);
							}
						}
						if (enc.GetCounter(3) <= 0f)
						{
							enc.AddCounter(2, Time.deltaTime);
							enc.AIMove(12f);
						}
						else
						{
							Debug.Log(true);
							enc.AIStopMove();
							enc.SubCounter(3, Time.deltaTime);
							if (enc.GetCounter(3) <= 0f)
							{
								enc.FlipDir();
								enc.time = 0f;
							}
						}
					}
					enc.AddCounter(4, Time.deltaTime);
					if (enc.GetCounter(4) >= 1.115f && enc.CheckDisBetweenPlayerX() < 10 && enc.IsLookingPlayer(100f, TILESIZE * 0.25f))
					{
						targetlose = 0;
						enc.SetCounter(4, 0f);
						enc.ChangePhase(AIPhase.PHASEB_1);
					}
					break;
				}
				case AIPhase.MAIN_3:
				{
					if (enc.time > 0.25f+enc.GetCounter(0)*0.15 )
					{
						enc.AddCounter(0,1);
						if (enc.IsLookingPlayer(100f,7f))
						{
							enc.ChangePhase(AIPhase.PHASEA_1);
						}
					}
					float num2 = 1.35f;

					if (enc.time > num2)
					{
						enc.ChangePhase(AIPhase.MAIN_1);
					}
					break;
				}
				case AIPhase.PHASEB_1:
				{
					if (enc.time > 0.1f)
					{
						enc.animationPlayer.PlayAnimation(Animconstant._instance.attack1);
						if (enc.GetCounter(4) != 1)
						{
							enc.SetCounter(4, 1);
							//enc.PlaySound("electric_bark");
							ObjectPooler.instance.PlaySound(AllSound.eletricbark,enc._transform.position);
						}
					}
					if (enc.time > 0.2f && enc.animationPlayer.IsCurrentAnimationAlmostFinshed())
					{
						enc.SetCounter(5, 0f);
						enc.animationPlayer.PlayAnimation(Animconstant._instance.attack1_loop);
						enc.NextPhase();
					}

					break;
				}
				case AIPhase.PHASEB_2:
				{
					enc.AddCounter(6,Time.deltaTime);
					if (enc.time < 0.03f|| enc.GetCounter(6)<=0.06f)
					{
						break;
					}
					enc.SubCounter(6,0.06f);
					if (shoottimes % 8 <= 3)
					{
						BulletScripts bulletScripts = ObjectPooler.instance.CreateBullet();
						float num = enc.direction == Direction.LEFT ? -2 : 2;
						Vector3 start = enc._transform.position;
						start.x += num;
						start.y += 1.6f;
						bulletScripts.Init(start,enc.direction,16);
					}

					shoottimes++;
					if (shoottimes > 21f)
					{
						shoottimes = 0;
						enc.NextPhase();
						enc.animationPlayer.PlayAnimation(Animconstant._instance.attack1_over);
					}

					break;
				}
				case AIPhase.PHASEB_3:
				{
					if (enc.time > 0.03f && enc.animationPlayer.IsCurrentAnimationAlmostFinshed())
					{
						enc.AIStopMove();
						enc.animationPlayer.PlayAnimation(Animconstant._instance.stand);
						enc.NextPhase();
					}
					break;
				}
				case AIPhase.PHASEB_4:
				{
					if (enc.time <= 0.3f)
					{
						break;
					}

					if (enc.IsLookingPlayer(100f, TILESIZE * 5))
					{
						enc.AIJump(20);
						enc.NextPhase();
					}
					else
					{
						enc.ChangePhase(AIPhase.PHASEA_2);
					}
					break;
				}
				case AIPhase.PHASEB_5:
				{
					enc.AIMove(12f);
					if (enc.time > 0.03f && enc.onGrounded())
					{
						enc.ChangePhase(AIPhase.PHASEA_2);
					}
					break;
				}
				case AIPhase.PHASEA_1:
				{
					if (!noticed)
					{
						noticed = true;
						enc.SetCounter(4,2);
					}
					enc.ChangePhase(AIPhase.MAIN_2);

					break;
				}
				case AIPhase.PHASEA_2:
				{
					if (enc.time > 1f)
					{
						hpdetect = enc.HPPercent();
						enc.ChangePhase(AIPhase.MAIN_1);
					}
					enc.ChangeDirByPlayer();
					break;
				}
				case AIPhase.PHASEC_1:
				{
					if (enc.time > 0.9f)
					{
						enc.ChangeDirByRandom();
						enc.NextPhase();
					}
					break;
				}
				case AIPhase.PHASEC_2:
				{
					if (enc.time > 0.9f)
					{
						noticed = false;
						enc.ChangePhase(AIPhase.MAIN_1);
					}
					break;
				}
			}
		}
	}

