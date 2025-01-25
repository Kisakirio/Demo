using UnityEngine;
	public class GearRotaate : MonoBehaviour
	{

		[SerializeField]
		private float speed = 3.333f;

		private void Update()
		{
			base.transform.Rotate(0f, 0f, speed *Time.deltaTime);
		}
	}

