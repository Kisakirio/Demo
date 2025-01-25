using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class DamageScript: MonoBehaviour
	{
		[SerializeField]
		private TextMeshPro tmp;

		[SerializeField]
		private Transform t;

		public int num;

		public bool isPlayer;

		private int tatgetSize;

		private Vector3 cachePos;

		private float rotate;

		private float time;

		private float speed=67f;

		private Color32 targetColor=Color.white;

		public void _Disable()
		{
			tmp.enabled = false;
			base.gameObject.SetActive(false);
		}

		public void SetPosition(Vector3 pos)
		{
			t.localPosition = pos;
			cachePos = pos;
		}

		public void _Update()
		{
			float num = Time.unscaledDeltaTime;
			float num2 = num;
			//Debug.Log(speed);
			/*if (true)
			{
				num *= 0.775f;
				num2 *= 2.1275f;
			}*/
			time += num;
			float smooth = GetSmooth(4f);
			if (time > .05f && time<.5f)
			{
				tmp.enabled = true;
			}

			if (isPlayer)
			{
				speed -= 100 * num2;

				if (speed < 0)
				{
					speed = 0;
				}

				cachePos.y += speed * num2;
			}
			else
			{
				if (rotate <= 5f)
				{
					speed -= 30 * num2;

					if (speed < 0)
					{
						speed = 0;
					}

					cachePos.y += speed*num2;
				}
			}

			t.localPosition = cachePos;
			if (!isPlayer)
			{
				tmp.fontSize = Mathf.Lerp(tmp.fontSize, tatgetSize, smooth);
			}
			else if (speed <= 0f && time > 0.55f)
			{
				t.localScale = Vector3.Lerp(t.localScale, new Vector3(80f, 0f, 16f), GetSmooth(72.5f));
				if (t.localScale.y <= 0.125f)
				{
					_Disable();
					return;
				}
			}
			Color color = tmp.color;
			float a = color.a;
			if (!isPlayer)
			{
				color = time > 0.25f ? Color.Lerp(color, new Color(.3f, .3f, 1f, 1f), smooth) : Color.Lerp(color, new Color(1f, 1f, 1f, 1f), smooth);
				color.a = a;
				if (time > .5f)
				{
					color.a -= num * 1.67f;
					if (color.a < 0)
					{
						_Disable();
						return;
					}
				}
			}
			else
			{
				color = Color32.Lerp(color, targetColor, GetSmooth(15f));
			}

			tmp.color = color;

			if (isPlayer)
			{
				if (speed < 12)
				{
					rotate = Mathf.Lerp(rotate, -15, GetSmooth(22.5f));
				}
			}
			else if (time>0.03f)
			{
				rotate = Mathf.Lerp(rotate, 0, GetSmooth(22.5f));
			}



			t.localRotation = Quaternion.Euler(0, 0, rotate);

		}

		public void Enable(int layout,bool _isPlayer)
		{
			targetColor = new Color32(230, 77, 77,byte.MaxValue);
			time = 0;
			tmp.sortingOrder = layout;
			base.gameObject.SetActive(true);
			isPlayer = _isPlayer;
			tmp.text = num.ToString();
			int num1 = 20+num/2;
			if (num1 > 100)
			{
				num1 = 100;
			}
			tmp.fontSize = num1;
			tatgetSize = num1 / 3;
			if (isPlayer)
			{
				tmp.color = new Color(0, 0, 0, 1f);
				rotate = 0;
				speed = 30f;
			}
			else
			{
				tmp.color = new Color32(0, byte.MaxValue, byte.MaxValue, byte.MaxValue);
				rotate = 30f;
				speed = 15f;
			}
			t.localRotation=Quaternion.Euler(0,0,rotate);
			cachePos.x += Random.Range(-.5f, .5f);
			SetPosition(cachePos);
			t.localScale = new Vector3(0.8f, 1, 1);
		}

		private float GetSmooth(float x)
		{
			return 1 - Mathf.Pow(.6f, x*Time.unscaledDeltaTime);
		}
	}

