
	using System;
	using DefaultNamespace;
	using UnityEngine;
	using Object = UnityEngine.Object;

	public class EffectManager:MonoBehaviour
	{
		public static EffectManager instance;
		public int MAXEFFECT=10;

		public	effectScript[] array =new effectScript[10] ;

		public effectScript effect_prefab;

		[ContextMenu("Cache Effects")]
		private void StartCache()
		{
			for (short num = 0; num < MAXEFFECT; num++)
			{
				effectScript effectScript2 = Object.Instantiate(effect_prefab);
				effectScript2.DisableMe(GameRunning: false);
				array[num] = effectScript2;
			}
		}

		public void Awake()
		{
			if (instance == null)
			{
				instance = this;
			}
		}

		public effectScript CreateEffect(CharacterBase owner, Vector3 position, SpriteRenderer sprite)
		{
			for (int i = 0; i < MAXEFFECT; i++)
			{
				if (!array[i] || i == MAXEFFECT - 1)
				{
					array[i].spr = sprite;
					array[i].transform.position = position;
					array[i].EnableMe();
					return array[i];
				}
			}
			return null;
		}

		public void _Update()
		{
			for (int i = 0; i < MAXEFFECT; i++)
			{
				if (array[i])
				{
					array[i]._Update();
				}
			}
		}
	}

