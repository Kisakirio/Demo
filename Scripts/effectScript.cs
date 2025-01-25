using UnityEngine;

namespace DefaultNamespace
{
	public class effectScript :MonoBehaviour
	{

		public SpriteRenderer spr;

		public float size;

		public float targetsize;

		public float time;

		public Transform t;

		public float speed;

		public void DisableMe(bool GameRunning)
		{
			SetSpriteFlip(false);
			base.gameObject.SetActive(value: false);
			t.transform.localScale = Vector3.one;
			spr.color = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue,0);
		}

		public void EnableMe()
		{
			t = spr.GetComponentInParent<Transform>();
			base.gameObject.SetActive(value: true);
			time = 0;
		}

		public void SetSpriteFlip(bool t)
		{
			spr.flipX = t;
		}

		public void SetColor(Color c)
		{
			spr.color = c;
		}


		public void _Update()
		{
			time += Time.deltaTime;
			if (size == 0f)
			{
				size= spr.bounds.size.y;
			}

			t.localScale= Vector3.Lerp(t.localScale, new Vector3(1.44f,1.44f,1), 1 - Mathf.Pow(0.6f, Time.deltaTime * 25));
			if (time > 0.03f)
			{
				Color color = spr.color;
				color.a = 0;
				spr.color = Color.Lerp(spr.color, color, 1-Mathf.Pow(0.6f,Time.deltaTime*speed));
			}
			if (spr.color.a <= 0.015f)
			{
				DisableMe(GameRunning: true);
			}
		}

	}
}
