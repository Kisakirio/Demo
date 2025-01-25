
	using UnityEngine;

	public class GedGehog:AIBase
	{

		public override Type type => Type.GedGehog;

		private bool noticed;

		private bool firstloop = true;

		public float outlinerepeat;

		private bool ishit;

		public GedGehog(EnemyController enc)
		{
			base.enc = enc;
		}

		public override void Init()
		{
			firstloop = true;
			enc.phase = AIPhase.MAIN_1;
			noticed = false;
		}

		public override void AI()
		{
			//Debug.Log(enc.phase);
			if (!firstloop && !noticed &&
			    Mathf.Abs(enc._transform.position.x - player._transform.position.x) < TILESIZE * 5)
			{
				noticed = true;
			}

			AIPhase phase = enc.phase;
			switch (phase)
			{
				case AIPhase.MAIN_1:
				{
					float num1 = Random.Range(0.5f, 0.9f);
					if (enc.GetCounter(0) == 0)
					{
						num1 = 0;
					}
					enc.SetCounter(1,num1);
					if (noticed)
					{
						enc.ChangeDirByPlayer();
					}
					else if(!firstloop)
					{
						enc.ChangeDirByRandom();
						if (Random.Range(0, 1) > 0.6f)
						{
							enc.ChangeDirByPlayer();
						}

					}
					else
					{
						enc.ChangeDirByPlayer();
					}
					enc.SetCounter(0,1);
					enc.NextPhase();
					break;
				}
				case AIPhase.MAIN_2:
				{
					enc.animationPlayer.PlayAnimation(Animconstant._instance.stand);
					if (enc.time > enc.GetCounter(1) && enc.phase==AIPhase.MAIN_2)
					{
						float num2 = Random.Range(1.25f, 2f);
						enc.SetCounter(0,num2);
						enc.ChangePhase(AIPhase.PHASEA_1);
					}

					break;
				}
				case AIPhase.PHASEA_1:
				{
					enc.AIMove(5f);
					//enc.animationPlayer.PlayAnimation(Animconstant._instance.Run);
					if (enc.IsLookingPlayer(TILESIZE * 8, TILESIZE * 2) && enc.onGrounded()&& enc.HPPercent()<1)
					{
						enc.ChangePhase(AIPhase.PHASEB_1);
					}
					else if(enc.time>enc.GetCounter(1))
					{
						firstloop = false;
						float num1 = Random.Range(0.5f, 0.75f);
						enc.SetCounter(1,num1);
						enc.NextPhase();
					}
					break;
				}
				case AIPhase.PHASEA_2:
				{
					enc.animationPlayer.PlayAnimation(Animconstant._instance.stand);
					if (enc.time > enc.GetCounter(1))
					{
						enc.ChangePhase(AIPhase.MAIN_1);
					}
					break;
				}
				case AIPhase.PHASEB_1:
				{
					if (enc.onGrounded())
					{

						if (enc.time > 0.075f && enc.time < 10f)
						{
							enc.time = 10f;
							enc.animationPlayer.PlayAnimation(Animconstant._instance.attack1);
						}

						if (enc.time < 10f)
						{
							enc.AIStopMove();
						}

						if (enc.time > 10f && enc.time <= 100f&& !enc.IsHit(0))
						{
							if (enc.animationPlayer.CurrentAnimationTime() > 0.0425f)
							{
								enc.animationPlayer.SetAnimationSpeed(1f);
								enc.DoAttack(0,AttackType.enemyAttack1,20);
								enc.time = 100f;
							}
							else
							{
								enc.AIStopMove();
								outlinerepeat += Time.deltaTime;
								enc.animationPlayer.SetAnimationSpeed(0.0375f);
								if (outlinerepeat >= 0.18f)
								{
									outlinerepeat -= 0.18f;
									enc.DoOutlineEffect(1.44f, 45f, new Color(0.9f, 0.7625f, 0.2f, 0.8f));
								}
							}
						}
						else
						{
							enc.DisableDetectorByHit(0);
						}

						if (enc.time <= 100)
						{
							break;
						}

						if (enc.animationPlayer.CurrentAnimationTime() < 0.875f)
						{
							enc.AIMove(10f);
						}

						if (enc.animationPlayer.CurrentAnimationTime()>=0.999f)
						{
							noticed = false;
							enc.NextPhase();
							enc.ChangeDirByPlayer();
							enc.DisableDetector(0);
						}
					}

					break;
				}
				case AIPhase.PHASEB_2:
				{
					float num = 1f;
					if (enc.time > 0.3f)
					{
						enc.ChangePhase(AIPhase.MAIN_2);
						enc.ChangeDirByPlayer();
					}
					break;
				}
			}

		}
	}

