using System;
using DefaultNamespace;
using UnityEngine;

namespace player
{
	public class BackGroundData:MonoBehaviour
	{
		[SerializeField]
		public SpriteRenderer bgRender;

		[SerializeField]
		public Transform t;

		public byte layer;

		public CameraScript _camera;

		private float spriteHeight;

		private float spriteWeigth;

		private float scweight = 45.8f;

		private float scheight = 25.7f;


		[SerializeField]
		public float speed;

		private void Awake()
		{
			spriteWeigth = bgRender.bounds.size.x;
			spriteHeight = bgRender.bounds.size.y;
		}

		private void LateUpdate()
		{
			if (bgRender == null)
			{
				return;
			}

			if (WorldManager.Instance.currentRoomType == RoomType.NONE)
			{
				return;
			}

			float num1 = _camera.t.position.x;
			float num2 = _camera.t.position.y;

			float num3 = WorldManager.Instance.CurrentRoomEdgeL;
			float num4 = WorldManager.Instance.CurrentRoomEdgeR;

			float num5 = num4 - num3 - MainVar.instance.ROOM_WEIGHT;
			float num7;
			float num9 = num2 + (scheight - spriteWeigth) / 2;
			if (speed < -50f)
			{
				num9 = 19.2f;
			}
			if (num5 <= 0||WorldManager.Instance.currentRoomType==RoomType.RoomOnlyY)
			{

				int num12;
				for (num12 = 5- 5 + 18; num12 > 5; num12 -= MainVar.instance.MAPSIZEX + 1)
				{
				}
				num7 = num1 + (float)(-num12) * ((spriteWeigth - scweight )/ 32f);
			}
			else
			{
				float num6 = num4 - num1 - MainVar.instance.ROOM_WEIGHT / 2;
				num7 = num1 - (spriteWeigth - scweight) / 2;
				float num8 = num6 / num5 * speed;
				num7 = num7 + (spriteWeigth - scweight) * num8;
			}

			float num15 = 0;
			if (spriteHeight >= scheight)
			{
				float num10 = WorldManager.Instance.CurrentRoomEdgeT;
				float num11 = WorldManager.Instance.CurrentRoomEdgeB;
				float num12 = num10 - num11 - scheight;
				if (num12 > 0 && WorldManager.Instance.currentRoomType == RoomType.RoomOnlyY||WorldManager.Instance.currentRoomType==RoomType.RoomFree)
				{
					num9 = num2 - (spriteHeight - scheight) / 2f;
					float num13 = (num10 - (scheight / 2f+num2)) / num12;
					num15 += (spriteHeight - scheight) * (1-num13);
				}
				else
				{
					num9 = num2;
				}
			}

			Vector3 position = new Vector3(num7, num9+num15, 0f);
			t.position = position;


		}
	}
}
