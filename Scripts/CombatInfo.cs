using System;
using UnityEngine;
     [Serializable]
	public class CombatInfo
	{
		public AttackType attackType;

		public float damage;

		public Vector3 hitPosition;

		public CharacterBase attackReceive;

		public IColliderDetector attackSender;

		public string tag;
	}

