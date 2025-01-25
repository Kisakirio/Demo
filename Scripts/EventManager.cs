
	using System;
	using UnityEngine;

	public class EventManager:MonoBehaviour
	{
		public static EventManager instance;

		public CharacterBase mainCharacter;

		public Vector3 tempPos;

		public void Awake()
		{
			if (instance == null)
			{
				instance = this;
			}
		}


		public void MovePlayerByTempPos()
		{
			mainCharacter._transform.position = tempPos;
		}
	}

