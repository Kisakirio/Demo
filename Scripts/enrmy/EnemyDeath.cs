using DefaultNamespace;
using UnityEngine;
	public class EnemyDeath: MonoBehaviour
	{
		private float xspeed;

		private float yspeed;

		[SerializeField]
		private SpriteRenderer basesprite;

		[SerializeField]
		private SpriteRenderer outlinesprite;

		[SerializeField]
		private SpriteRenderer supportsprite;

		[SerializeField]
		private Transform effectT;

		[SerializeField]
		private Animator ani;

		private bool haveSupport;

		public void SpawnMe(float x, float y, bool flip, Vector3 efft, RuntimeAnimatorController c, bool sup)
		{

			ani.runtimeAnimatorController = c;
			effectT.position = efft;
			xspeed = x;
			yspeed = y;
			basesprite.flipX = flip;
			outlinesprite.flipX = flip;
			supportsprite.flipX = flip;
			haveSupport = sup;
			ani.Play("hurt", -1, 0f);
			base.gameObject.SetActive(value: true);
		}

		public void SetColor(Color c)
		{
			basesprite.color = c;
		}

		private void Update()
		{
			supportsprite.enabled = haveSupport;
			yspeed -= Time.deltaTime * (40f * 1.25f);
			base.transform.position += new Vector3(Time.deltaTime * xspeed * 2.14f, Time.deltaTime * yspeed * 2.14f, 0f);
			if (base.transform.position.y < CameraScript.instance.targetC.position.y -MainVar.instance.ROOM_HIGHT * 0.7f)
			{
				base.gameObject.SetActive(value: false);
			}
		}


	}
