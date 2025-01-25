using System;
using UnityEngine;

namespace DefaultNamespace
{
	public class FogManger:MonoBehaviour
	{
		private Transform t;

		public float Yoffset;

		private Vector3 positon;

		public static FogManger Instence;

		private SpriteRenderer spr;

		private float spriteWeigth;

		private void Start()
		{
			ResetFog();
			InvokeRepeating("Move",0,0.04f);

		}

		private void Awake()
		{
			if (Instence == null)
			{
				Instence = this;
			}
			t = GetComponent<Transform>();
			spr = GetComponent<SpriteRenderer>();
			spriteWeigth = spr.bounds.size.x;
			Debug.Log(spriteWeigth);
		}

		private void Move()
		{
			positon.x -= .5f;
			if (positon.x + spriteWeigth / 2 < (WorldManager.Instance.CurrentRoomX+1.5f) * MainVar.instance.ROOM_WEIGHT)
			{
				ResetFog();
			}
			t.position = positon;
		}

		public void ResetFog()
		{
			positon.y=(WorldManager.Instance.CurrentRoomY + 1) * MainVar.instance.ROOM_HIGHT + Yoffset;
			positon.x = (WorldManager.Instance.CurrentRoomX + 1) * MainVar.instance.ROOM_WEIGHT;
		}
	}
}
