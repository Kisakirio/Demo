using System;
using TMPro;
using UnityEngine;
	public class TipsUI : MonoBehaviour
	{
		public static TipsUI instance;

		[SerializeField]
		private TextMeshPro text;

		[SerializeField]
		private SpriteRenderer spr;

		[SerializeField]
		private SpriteRenderer[] sprUD;

		private string lastkeyword = string.Empty;

		private float targetalpha;

		private float readyTime;

		private string message = "";

		private float fadeinspeed = 7f;

		private float fadeoutspeed = 10f;

		private float time;

		private float readytime;





		private void Awake()
		{
			if (instance == null)
			{
				instance = this;
			}
			spr.enabled = false;
		}

		public void SetFontSize(int size)
		{
			text.fontSize = size;
		}

		public void StopTips()
		{
			targetalpha = 0f;
			CancelInvoke();
		}

		public void StartTips(String keyword,float endtime)
		{
			text.sortingOrder = 1251;
			spr.enabled = true;
			SetTop();
			CancelInvoke();
			base.transform.gameObject.SetActive(true);
			targetalpha = 1f;
			text.text=Localize.GetLocalizeTextByKeywor(keyword);
			text.text = Localize.ReplaceButton(text.text);
			time = 0;
			sprUD[0].color = new Color(1f, 1f, 1f, 0f);
			sprUD[1].color = new Color(1f, 1f, 1f, 0f);
			Invoke("StopTips",endtime);

		}

		public void Update()
		{
			if (readyTime > 0f)
			{
				Debug.Log(readytime);
				readyTime -= Time.deltaTime;
				if (readyTime <= 0f)
				{
					StartTips(message, 6f);
				}
			}
			Color color = text.color;
			color.a = targetalpha;
			text.color = Color.Lerp(text.color, color, 1-Mathf.Pow(0.6f,Time.deltaTime*((text.color.a > color.a) ? fadeoutspeed : fadeinspeed)));
			color = text.color;
			color.r = 1f;
			color.g = 1f;
			color.b = 1f;
			color.a /= 1.25f;
			spr.color = color;
			time += Time.deltaTime;
			if (time <= 1.5f)
			{
				sprUD[0].color = Color.Lerp(sprUD[0].color, new Color(1f, 1f, 1f, 0.5f), 1-Mathf.Pow(0.6f,Time.deltaTime*35));
				sprUD[1].color = sprUD[0].color;
			}
			else
			{
				sprUD[0].color = Color.Lerp(sprUD[0].color, new Color(1f, 1f, 1f, 0f), 1-Mathf.Pow(0.6f,Time.deltaTime*3));
				sprUD[1].color = sprUD[0].color;
			}
			if (time >= 0.1f)
			{
				if (color.a <= 0.01f)
				{
					base.gameObject.SetActive(value: false);
					SetTop();
					spr.transform.localScale = new Vector3(3f, 1f, 2f);
				}
			}
		}

		public void SetTop()
		{
			base.transform.localPosition = new Vector3(0f, 237f, 12f);
		}

		public void ReadyMessage(string key,float _time)
		{
			message = key;
			readyTime = _time;
		}
	}

