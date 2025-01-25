
	using System.Collections.Generic;
	using UnityEngine;

	public class PlayerSlide : IStateHanlder
	{
		private Player player;
		private float slidDirection;
		private float STARTTIME=0.05f;
		private float slidSpeed ;
		private Vector2 topLeft;
		public Vector2 bottomLeft;
		public Vector2 bottomRight;
		private RaycastHit2D m_rayCastHit;
		public void SetSystem(FSMSystem fsm)
		{
			if (fsm is FSMSystem playerstate)
			{
				player = playerstate.playerc_perfer;
			}
		}

		public void Enter(FSMSystem fsm)
		{
			slidDirection = player.transform.rotation.y == 0 ? -1 : 1;
			slidSpeed = 27.5f * slidDirection;
			player.SetCounter(0,0);
			player.SetCounter(1,0);
			Vector2 vector2 =new Vector2(1f,2f);
			Vector2 vector1 = new Vector2(0, 1.08f);
			player._control.m_Boxcollider.size=vector2;
			player._control.m_Boxcollider.offset = vector1;
		}

		public void _Update(FSMSystem fsm)
		{
			player.PlayAnimation(Animconstant._instance.playerSlide);
			if (player.GetCounter(0) == 0)
			{
				player.PlayAttackVoice(AllSound.burst1);
				player.SetCounter(0,1);
				player.SetCounter(1,0);
				player.SetCounter(2,0);
				player.SetCounter(3,0);
				player.SetCounter(4,0.35f);
				player.SetCounter(5,0);
				player.particleSystem[0].SetActive(true);
				ObjectPooler.instance.PlaySound(AllSound.slide, player._transform.position,1,0.25f);
			}
			if (player.GetCounter(0) != 1&& player.GetCounter(1)<=STARTTIME)
			{
				return;
			}
			player.AddCounter(1,Time.deltaTime);
			InitPos();
			player.SetCounter(5,0);
			LayerMask layermask = LayerMask.GetMask("Tileset");
			for (int i = 0; i < 8; i++)
			{

				Vector2 start = new Vector2(topLeft.x + i * (bottomRight.x - bottomLeft.x) / (8 - 1), topLeft.y);

				m_rayCastHit = Physics2D.Raycast(start, Vector3.up, 0.2f, layermask);
				if (m_rayCastHit)
				{
					Debug.Log(true);
					player.SetCounter(5,1);
				}
			}

			if (player.GetCounter(4) > 0)
			{
				player.SubCounter(4,Time.deltaTime);
				player.SetMoveSpeedX(slidSpeed);
			}
			if (!(player.IsHitBoxStart(0))&&player.GetCounter(3)==0)
			{
				player.DoAttack(0,AttackType.attackgroundNORMAL1,30);
				player.AddCounter(3,1f);
			}
			else
			{
				if (player.IsHit(0))
				{
					ObjectPooler.instance.PlaySound(AllSound.slide_hit, player._transform.position);
					Transform _t = ObjectPooler.instance.CreateWeakHit();
					_t.position = HitTargetTransform(player.GetCombatInfo());
					player.shake = 0.1f;
					player.SetCounter(4,0);
				}
				player.DisableDetectorByHit(0);
			}

			if ((player.GetCurrenAnimationTime() > 0.999f&&player.IsCurrentAnimationThis(Animconstant._instance.playerSlide)&& player.GetCounter(5)==0))
			{
				player.SetMoveSpeedX(0);
				player.canDash = false;
				player.particleSystem[0].SetActive(false);
				player.ChangeLogicState(PlayerState.IDLE);

			}
		}

		public void Exit(FSMSystem fsm)
		{
			player.justSlide = 0;
			player.shake = 0;
			player.DisableDetector(0);
			player.canDash = false;
			Vector2 vector2 =new Vector2(1f,2.74f);
			Vector2 vector1 = new Vector2(0, 1.45f);
			player._control.m_Boxcollider.size=vector2;
			player._control.m_Boxcollider.offset = vector1;
		}

		public void InitPos()
		{

				Bounds bounds = player._control.m_Boxcollider.bounds;
				bounds.Expand(-2f * 0.02f);
				topLeft = new Vector2(bounds.min.x, bounds.max.y);
				bottomLeft = bounds.min;
				bottomRight = new Vector2(bounds.max.x, bounds.min.y);

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

