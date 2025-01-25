
	using UnityEngine;

	public class MapTile: MonoBehaviour
	{
		public SpriteRenderer spr;

		[SerializeField]
		private byte x;

		[SerializeField]
		private byte y;

		private float time;

		[SerializeField]
		public Color start = new Color(0.72f, 0.58f, 0.68f);

		private bool canchange;



		public byte GetX()
		{
			return x;
		}

		public byte GetY()
		{
			return y;
		}

		public void _Update()
		{

			if (canchange)
			{
				spr.color = Color.white;
				time += Time.deltaTime;
				if (time > 0.2f)
				{
					canchange = false;
					time = 0;
				}
			}
			else
			{
				time += Time.deltaTime;
				spr.color = start;
				time += Time.deltaTime;
				if (time > 0.2f)
				{
					canchange = true;
					time = 0;
				}
			}
		}

	}

