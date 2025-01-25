using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
	public class MiniMapManager :MonoBehaviour
	{
		public static MiniMapManager instance;

		public List<MapTile> tilelist;

		public int curX;

		public int curY;

		public MapTile flashtile;

		public void Awake()
		{
			if (instance == null)
			{
				instance = this;
			}
		}

		public void Update()
		{
			if (curX != WorldManager.Instance.CurrentRoomX || curY != WorldManager.Instance.CurrentRoomY)
			{
				if (flashtile)
				{
					flashtile.spr.color = flashtile.start;
				}

				curX = WorldManager.Instance.CurrentRoomX;
				curY = WorldManager.Instance.CurrentRoomY;
				foreach (var tile in tilelist)
				{
					if (tile.GetX() == curX && tile.GetY() == curY)
					{
						flashtile = tile;
					}
				}
			}
			flashtile._Update();

		}
	}

