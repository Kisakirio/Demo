using System;
using UnityEngine;

	public class BulletScripts:MonoBehaviour
	{
		public Combat _combat;

		public Transform t;

		private float speed;

		private Direction dir;

		public AudioSource audio;




		private void Awake()
		{
			_combat.Init();
			_combat.SetDetector(0,AttackType.bulletshoot1,15);
			audio.Play();
		}


		public void Update()
		{
			Vector3 num = t.position;
			float num2 = dir == Direction.LEFT ? -1 : 1;
			num.x += num2 * speed * Time.deltaTime;
			t.position = num;
			_combat.EnableDetector(0);
			_combat.SendAllInfos();
		}

		public void OnTriggerEnter2D(Collider2D other)
		{
			if (other.gameObject.layer == 6)
			{
				DisableMe();
			}
		}
		public void BulletMoveX(float _speed)
		{
			speed = _speed;
		}

		public void Init(Vector3 start, Direction _dir, float _speed)
		{
			t.position = start;
			dir = _dir;
			speed = _speed;
		}

		public void DisableMe()
		{
			base.gameObject.SetActive(false);
		}

	}

