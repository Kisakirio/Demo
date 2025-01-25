using System;
using UnityEngine;

namespace DefaultNamespace
{
	public class Test:MonoBehaviour
	{
		private CharacterBase a;
		private void Start()
		{
			a = base.gameObject.GetComponentInParent<CharacterBase>();
		}

		private void Update()
		{
			Debug.Log(a.isPlayer());
		}
	}
}
