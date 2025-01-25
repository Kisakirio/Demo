using System;
using DefaultNamespace;
using UnityEngine;

	public class UICamera :MonoBehaviour
	{
		public Transform transform;

		public Vector3 targetpos;


		public void Awake()
		{

		}

		private void Update()
		{
			targetpos.x = MiniMapManager.instance.curX *= 2;
			targetpos.y = MiniMapManager.instance.curY *= 2;

			transform.position = targetpos;	
		}
	}

