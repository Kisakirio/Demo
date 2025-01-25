using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraScript : MonoBehaviour
{
	public static CameraScript instance;

	public Camera mCamera;

	public Transform targetC;

	public bool AutoCamera=true;

	public Transform t;

	public Vector3 targetposition;

	[SerializeField]
	private WorldManager wm;

	private float camSpeedx = 15f;

	private float camSpeedY=1f;

	public void Awake()
	{
		if (instance == null)
		{
			t = base.transform;
			instance = this;
		}
	}

	public void LateUpdate()
	{

		if (AutoCamera)
		{
			//Debug.Log(wm.currentRoomType);
			float num1 = -1f;
			float num2 = -1f;
			if (wm.GetNextRoom(wm.CurrentRoomX, wm.CurrentRoomY, 1, 0)!=wm.currentRoomType)
			{
				num1 = (wm.CurrentRoomX + .5f) * MainVar.instance.ROOM_WEIGHT;
			}

			if(wm.GetNextRoom(wm.CurrentRoomX, wm.CurrentRoomY, -1, 0)!=wm.currentRoomType)
			{
				num2= (wm.CurrentRoomX + .5f) * MainVar.instance.ROOM_WEIGHT;
			}
			if (wm.currentRoomType == RoomType.RoomOnlyX||wm.currentRoomType==RoomType.RoomFree)
			{
				targetposition.x = targetC.position.x;
				if (wm.currentRoomType != RoomType.RoomFree)
				{
					targetposition.y=MainVar.instance.ROOM_HIGHT * (wm.CurrentRoomY + .5f);
				}

				targetposition.z =targetC.position.z-10;
				if (num1 > 0 && targetposition.x > num1)
				{
					targetposition.x = num1;
				}

				if (num2 > 0 && targetposition.x < num2)
				{
					targetposition.x = num2;
				}
			}

			float num3 = -1f;
			float num4 = -1f;
			if (wm.currentRoomType == RoomType.RoomOnlyY||wm.currentRoomType==RoomType.RoomFree)
			{
				targetposition.y = targetC.position.y;
				if (wm.GetNextRoom(wm.CurrentRoomX, wm.CurrentRoomY, 0, 1)!=wm.currentRoomType)
				{
					num3 = (wm.CurrentRoomY + .5f) * MainVar.instance.ROOM_HIGHT;
				}

				if(wm.GetNextRoom(wm.CurrentRoomX, wm.CurrentRoomY, 0, -1)!=wm.currentRoomType)
				{
					num4= (wm.CurrentRoomY + .5f) * MainVar.instance.ROOM_HIGHT;
				}

				if (num3 > 0 && targetposition.y > num3)
				{
					targetposition.y = num3;
				}

				if (num4 > 0 && targetposition.y < num4)
				{
					targetposition.y = num4;
				}

				if (wm.currentRoomType != RoomType.RoomFree)
				{
					targetposition.x = MainVar.instance.ROOM_WEIGHT * (wm.CurrentRoomX + .5f);
				}

				targetposition.z =targetC.position.z-10;
			}
		}

		float smooth = GetSmooth(camSpeedx * .6f);
		Vector3 v=Vector3.zero;
		float x = t.position.x;
		x=MoveX(smooth, x, 0.6f);
		float y = t.position.y;
		y=MoveY(smooth, y, 0.6f);
		v.x = x;
		v.y = y;
		v.z = -10f;
		t.position = v;
	}

	private float MoveX(float smooth, float vx, float addsp)
	{
		if (t.position.x != targetposition.x&& Mathf.Abs(targetposition.x-t.position.x)<=MainVar.instance.ROOM_WEIGHT/2.2)
		{
			vx = ((Mathf.Abs(t.position.x - targetposition.x) < 0.018f) ? targetposition.x : ((!(Mathf.Abs(t.position.x - targetposition.x) < 0.09f)) ? Mathf.Lerp(t.position.x, targetposition.x, smooth * addsp) : ((!(t.position.x < targetposition.x)) ? (vx - Time.unscaledDeltaTime * 10f) : (vx + Time.unscaledDeltaTime * 10f))));
		}
		else
		{
			vx = targetposition.x;
		}
		return vx;
	}
	private float MoveY(float smooth, float vy, float addsp)
	{
		if (t.position.y != targetposition.y&&  Mathf.Abs(targetposition.y-t.position.y)<=MainVar.instance.ROOM_HIGHT/2.2)
		{
			vy = ((Mathf.Abs(t.position.y - targetposition.y) < 0.018f) ? targetposition.y : ((!(Mathf.Abs(t.position.y - targetposition.y) < 0.09f)) ? Mathf.Lerp(t.position.y, targetposition.y, smooth * addsp * camSpeedY) : ((!(t.position.y < targetposition.y)) ? (vy - Time.unscaledDeltaTime * 10f) : (vy + Time.unscaledDeltaTime * 10f))));
		}
		else
		{
			vy=targetposition.y;
		}
		return vy;
	}

	public float GetSmooth(float x)
	{
		return 1 - Mathf.Pow(0.6f ,x* Time.unscaledDeltaTime);
	}
}
