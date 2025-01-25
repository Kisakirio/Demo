
using UnityEngine;


	public class LocalizeManager : MonoBehaviour
	{
		public static LocalizeManager instance;


		public TextAsset SystemTextAsset;

		public void Awake()
		{
			if (instance == null)
			{
				instance = this;
			}
		}

		private void Update()
		{

		}
	}

