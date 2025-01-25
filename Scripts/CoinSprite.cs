using UnityEngine.UI;
using UnityEngine;

	public class CoinSprite: MonoBehaviour
	{
		[SerializeField]
		private Sprite[] sprites;

		[SerializeField]
		private float timePerFrame = 0.075f;

		[SerializeField]
		private float nativeSize = 100f;

		private int index;

		[SerializeField]
		private Image image;

		[SerializeField]
		private RectTransform rectTransform;

		private float frame;

		private void FixedUpdate()
		{
			frame += Time.deltaTime;
			if (!(frame < timePerFrame))
			{
				image.sprite = sprites[index];
				image.SetNativeSize();
				rectTransform.sizeDelta /= nativeSize;
				frame -= timePerFrame;
				index++;
				if (index >= sprites.Length)
				{
					index = 0;
				}
			}
		}

	}

