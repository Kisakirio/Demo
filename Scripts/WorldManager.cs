using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{	//[ExecuteInEditMode]
	public class WorldManager:MonoBehaviour
	{
		public static WorldManager Instance;

		public AreaMapData areaMapData;

		public byte area;

		public short CurrentRoomX;

		public short CurrentRoomY;

		public RoomType currentRoomType;

		public RoomType[,] roonType = new RoomType[MainVar.instance.MAPSIZEX,MainVar.instance.MAPSZIEY];

		public bool updataroomtype;

		public float CurrentRoomEdgeL;

		public float CurrentRoomEdgeR;

		public float CurrentRoomEdgeT;

		public float CurrentRoomEdgeB;

		public bool gotCurrentRoomInfo;

		public RoomIn roomin;

		[SerializeField]
		private List<EnemyController> emlist;


		[System.Serializable]
		public class MapData
		{
			public  short x;
			public  short y;
			public RoomType roomType;
		}

		public void GetRoomWithPosition(float x, float y,out short atRoomX,out short atRoomY)
		{
			int num = Mathf.FloorToInt(x);
			int num2 = Mathf.FloorToInt(y);
			atRoomX = (short)((float)num / MainVar.instance.ROOM_WEIGHT);
			atRoomY = (short)((float)num2 / MainVar.instance.ROOM_HIGHT);
		}

		[ContextMenu("SaveMap")]
		public void SaveMap()
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			String text = "/Map/MapData" + area + ".dat";
			FileStream fileStream = File.Create(Application.dataPath + text);
			binaryFormatter.Serialize(fileStream,areaMapData.mapList);
			fileStream.Close();

		}

		public void  LoadMap(byte area)
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			String text = "/Map/MapData" + area + ".dat";
			if (File.Exists(Application.dataPath + text))
			{
				FileStream fileStream = File.Open(Application.dataPath + text, FileMode.Open);
				List<MapData> list = (List<MapData>)binaryFormatter.Deserialize(fileStream);
				foreach (MapData mapData in list)
				{
					CreateMapData(mapData.x,mapData.y,mapData.roomType);
				}
			}

		}


		public void CreateMapData(short x, short y, RoomType roomType)
		{
			for (int i = 0; i < areaMapData.mapList.Count; i++)
			{
				if (areaMapData.mapList[i].x == x && areaMapData.mapList[i].y == y)
				{
					Debug.LogError("[WorldManager] Room X " + x + " Y " + y + " already exist! Create Failed.");
				}
			}

			MapData _mapdata = new MapData();
			_mapdata.x = x;
			_mapdata.y = y;
			_mapdata.roomType = roomType;
			areaMapData.mapList.Add(_mapdata);
		}


		public void ChangeCurrentRoom(short x, short y)
		{
			CurrentRoomX = x;
			CurrentRoomY = y;
			gotCurrentRoomInfo = false;

		}

		public void CheckPlayerPosition()
		{
			if (!CameraScript.instance.targetC)
			{
				return;
			}

			bool flag = false;
			short atroomX;
			short atroomY;
			var position = CameraScript.instance.targetC.position;
			GetRoomWithPosition(position.x,position.y,out atroomX, out atroomY);
			if (CurrentRoomX == atroomX && CurrentRoomY == atroomY)
			{
				return;
			}
			Debug.Log(atroomY);

			if (atroomX == Convert.ToInt16(CurrentRoomEdgeL/MainVar.instance.ROOM_WEIGHT)-1 || atroomY == Convert.ToInt16(CurrentRoomEdgeT/MainVar.instance.ROOM_HIGHT) ||atroomX == Convert.ToInt16(CurrentRoomEdgeR/MainVar.instance.ROOM_WEIGHT) || atroomY == Convert.ToInt16(CurrentRoomEdgeB/MainVar.instance.ROOM_HIGHT)-1)
			{
				roomin.StartMe();
				FogManger.Instence.ResetFog();
				SaveManager.instance.SaveGame();
				flag = true;
			}

			ChangeCurrentRoom(atroomX,atroomY);
			GetCurrentRoom();
			if (flag)
			{
				CheckEnemy();
			}
		}

		public void CheckEnemy()
		{
			foreach (var em in emlist)
			{

				if (em.AreaPosX >= Convert.ToInt16(CurrentRoomEdgeL / MainVar.instance.ROOM_WEIGHT) &&
				    em.AreaPosX <= Convert.ToInt16(CurrentRoomEdgeR / MainVar.instance.ROOM_WEIGHT)-1 &&
				    em.AreaPosY <= Convert.ToInt16(CurrentRoomEdgeT / MainVar.instance.ROOM_HIGHT)-1 &&
				    em.AreaPosY >= Convert.ToInt16(CurrentRoomEdgeB / MainVar.instance.ROOM_HIGHT))
				{
					em.ReSpawnMe();

				}
			}
		}


		public void Update()
		{
			CheckPlayerPosition();
			if (!gotCurrentRoomInfo)
			{
				GetCurrentRoom();
			}


		}

		public void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
			}
			LoadMap(area);
		}

		public void GetCurrentRoom()
		{
			foreach (MapData mapData in areaMapData.mapList)
			{
				if (mapData.x != CurrentRoomX || mapData.y != CurrentRoomY)
				{
					continue;
				}

				currentRoomType = mapData.roomType;
				CurrentRoomEdgeL = GetRoomEdgeX(-1);
				CurrentRoomEdgeR = GetRoomEdgeX(1);
				CurrentRoomEdgeT = GetRoomEdgeY(1);
				CurrentRoomEdgeB = GetRoomEdgeY(-1);
			}

			gotCurrentRoomInfo = false;

		}

		public RoomType GetNextRoom(short startX,short startY,short nextRoomX,short nextRoomY)
		{
			if (startX + nextRoomX < 0 || startY + nextRoomY < 0||startX + nextRoomX>MainVar.instance.MAPSIZEX||startY+nextRoomY>MainVar.instance.MAPSZIEY)
			{
				return RoomType.NONE;
			}
			UpdataRoomTypeCache();
			return roonType[startX + nextRoomX, startY + nextRoomY];
		}


		public RoomType GetRoomType(short x, short y)
		{
			UpdataRoomTypeCache();
			if (x < 0 || y < 0 || x > MainVar.instance.MAPSIZEX || y > MainVar.instance.MAPSZIEY)
			{
				return RoomType.NONE;
			}

			return roonType[x, y];
		}

		public void UpdataRoomTypeCache()
		{
			if (updataroomtype)
			{
				return;
			}

			foreach (MapData mapData in areaMapData.mapList)
			{
				short x = mapData.x;
				short y = mapData.y;
				roonType[x, y] = mapData.roomType;
			}

			updataroomtype = true;
		}
		public float GetRoomEdgeX(short x)
		{
			float num = CurrentRoomX * MainVar.instance.ROOM_WEIGHT;
			int num2 = CurrentRoomX+x;
			RoomType nextroom=GetNextRoom((short)num2,CurrentRoomY,0,0);
			while (nextroom==currentRoomType&&(currentRoomType==RoomType.RoomOnlyX||currentRoomType==RoomType.RoomFree))
			{
				num2 += x;
				num += x * MainVar.instance.ROOM_WEIGHT;
				nextroom = GetNextRoom((short)num2, CurrentRoomY, 0, 0);
			}

			if (x > 0)
			{
				num += x * MainVar.instance.ROOM_WEIGHT;
			}

			return num;
		}
		public float GetRoomEdgeY(short y)
		{
			float num = CurrentRoomY * MainVar.instance.ROOM_HIGHT;
			int num2 = CurrentRoomY+y;
			RoomType nextroom=GetNextRoom(CurrentRoomX,(short)num2,0,0);
			while (nextroom==currentRoomType&&(currentRoomType==RoomType.RoomOnlyY|| currentRoomType==RoomType.RoomFree))
			{
				num2 += y;
				num += y * MainVar.instance.ROOM_HIGHT;
				nextroom = GetNextRoom(CurrentRoomX, (short)num2, 0, 0);
			}

			if (y > 0)
			{
				num += y * MainVar.instance.ROOM_HIGHT;
			}

			return num;
		}
	}
}
