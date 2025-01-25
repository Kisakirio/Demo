using DefaultNamespace;
using UnityEngine;

	public class Smog: AIBase
	{
		public override Type type => Type.Smog;

		private EnemyController enc;

		private Vector3 seekPoint1=Vector3.zero;

		private Vector3 seekPoint2=Vector3.zero;

		private byte direction;

		private Vector3 targetpos;

		private Vector3 _t;

		public Smog(EnemyController _enc)
		{
			enc = _enc;
		}

		public override void Init()
		{
			enc.animationPlayer.basesprite.color= Color.white;
			direction = enc.id;
			if (direction <= 1)
			{

				seekPoint1 = enc._transform.position + new Vector3(0,enc.moveoffset * TILESIZE , 0);
				seekPoint2 = enc._transform.position - new Vector3(0, enc.moveoffset * TILESIZE, 0);
			}
			else if (direction <= 3)
			{
				seekPoint1 = enc._transform.position + new Vector3(enc.moveoffset * TILESIZE, 0, 0);
				seekPoint2 = enc._transform.position - new Vector3(enc.moveoffset*TILESIZE, 0, 0);
			}
			ChangeDir();
		}

		private void ChangeDir()
		{
			if (direction == 1)
			{
				targetpos = seekPoint1;
				direction = 0;
			}
			else if (direction == 0)
			{
				targetpos = seekPoint2;
				direction = 1;

			}
			if (direction == 2)
			{
				targetpos = seekPoint1;
				direction = 3;
			}
			else if (direction == 3)
			{
				targetpos = seekPoint2;
				direction = 2;
			}
		}

		private void LockToX()
		{
			Vector3 position = enc._transform.position;
			position.x = Mathf.Lerp(position.x, targetpos.x, GetSmooth(1.65f));
			enc._transform.position = position;
		}

		private void LockToY()
		{
			Vector3 position = enc._transform.position;
			position.y = Mathf.Lerp(position.y, targetpos.y, GetSmooth(1.65f));
			enc._transform.position = position;
		}

		private float GetSmooth(float num)
		{
			return 1 - Mathf.Pow(0.6f, Time.deltaTime * num);
		}
		private void DoAttack()
		{

			enc.DoAttack(0, AttackType.enemyAttack1, 20);
			Transform t=ObjectPooler.instance.CreateSmog();
			t.position = enc._transform.position+new Vector3(0f,2f,0);
			enc.SetCounter(1,1);
		}

		public override void AI()
		{
			//Debug.Log(enc.GetCounter(2));
			if (enc.HPPercent() <= 0.5f)
			{
				enc.animationPlayer.ReduceAlpha(2.25f);
				if (enc.animationPlayer.basesprite.color.a <= 0.05f)
				{
					DoAttack();
					enc.DespawnMe();
				}
				return;
			}

			if (enc.GetCounter(2) > 0f)
			{
				enc.SubCounter(2, Time.deltaTime);
				if (enc.GetCounter(1) == 0)
				{
					DoAttack();
				}

				if (enc.GetCounter(2) <= 0f&& enc.IsHitBoxStart(0))
				{
					enc.DisableDetector(0);
					//Debug.Log(enc.IsHitBoxStart(0));
					enc.SetCounter(1,0);
				}

				return;
			}

			if (Vector3.Distance(enc._transform.position, targetpos) < 2f)
			{
				_t = targetpos;
				enc.FlipDir();
				ChangeDir();
				enc.SetCounter(2, 0.225f);
			}
			if (direction <= 1)
			{
				LockToY();
			}
			else if (direction <= 3)
			{
				LockToX();
			}
		}
	}

