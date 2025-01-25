using System;
using DefaultNamespace;
using UnityEngine;
using Object = UnityEngine.Object;


public class DamageManager:MonoBehaviour
	{
		[SerializeField]
		private DamageScript damageprefeb;

		private static short MAXDAMAGE=16;
		[SerializeField]
		private DamageScript[] damages = new DamageScript[MAXDAMAGE];

		public static DamageManager Instance;

		[ContextMenu("Cache")]
		public void Cache()
		{
			damages = new DamageScript[MAXDAMAGE];
			for (int i = 0; i < MAXDAMAGE; i++)
			{
				DamageScript damage = Object.Instantiate(damageprefeb);
				damage._Disable();
				damage.transform.SetParent(base.transform);
				damages[i] = damage;

			}
		}

		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
			}
		}

		public void _Update()
		{
			for (int i = 0; i < MAXDAMAGE; i++)
			{
				if (damages[i].isActiveAndEnabled)
				{
					damages[i]._Update();
				}
			}
		}

		public DamageScript CreateDamage(int damage, CharacterBase cb, Vector3 position)
		{
			for (int i=0; i < damages.Length; i++)
			{
				if (damages[i].isActiveAndEnabled)
				{
					continue;
				}

				damages[i].num = damage;
				float num=0;
				num = EventManager.instance.mainCharacter.transform.position.x < position.x ? (num + 2f) : (num - 2f);
				position.x += num;
				position.y += 2.3f;
				damages[i].SetPosition(position);
				Debug.Log(cb);
				damages[i].Enable(3,cb.isPlayer());
				return damages[i];
			}

			return null;

		}
	}


