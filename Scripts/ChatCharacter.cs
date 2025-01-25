using System;
using DefaultNamespace;
using UnityEngine;
	public class ChatCharacter : MonoBehaviour
	{
		[SerializeField]
		private Sprite[] standimage;
		[SerializeField]
		private SpriteRenderer spr_pose;

		[SerializeField]
		private SpriteRenderer spr_emotion;

		public string _character = "";

		public string _pose = "";

		public string _action = "";

		public string _position = "";

		public string _emotion = "";

		public string _emotionkeyword = "";

		private float delayedMove;

		private bool justEnter;

		private Color targetColor;



		public void SetBlack()
		{
			Color color = spr_pose.color;
			color.r = 0f;
			color.g = 0f;
			color.b = 0f;
			color.a = 1f;
			targetColor = color;
			spr_pose.color = color;
			//_black = true;
		}

		public string GetCharacter()
		{
			return _character;
		}

		public void SetPosition(string position)
		{
			if (!(position == _position))
			{
				_position = position;
			}
		}

		public void DisableMe()
		{
			base.gameObject.SetActive(value: false);
		}

		public void EnableMe()
		{
			base.gameObject.SetActive(value: true);
			justEnter = true;
			delayedMove = 0f;
		}


		public void SetBrightness(float bri)
		{
			if (bri == 1f)
			{
				Color color = spr_pose.color;
				color.r = bri;
				color.g = bri;
				color.b = bri;
				color.a = 1f;
				targetColor = color;
			}
		}

		public void SetEmotion(string emotion)
		{
			if (_emotion == emotion)
			{
				return;
			}

			if (emotion == "null")
			{
				spr_emotion.sprite = null;
			}
			for (int i = 0; i < standimage.Length; i++)
			{
				if (standimage[i].name.Contains(emotion))
				{
					spr_emotion.sprite = standimage[i];
					break;
				}
			}
			_emotion = emotion;
		}

		public void SetCharacter(string character, string pose, bool isOld, bool right)
		{
			if (!(_character == character) || !(_pose == pose))
			{
				/*if (!layerInited)
				{
					layerInited = true;
				}*/
				if (!isOld)
				{
					_emotionkeyword = "";
				}
				SetPose(pose);
				_character = character;
				_pose = pose;
				/*if (!isOld)
				{
					_seedid = id;
				}
				else
				{
					SetBrightness(0.2f);
				}*/
				base.transform.localScale = new Vector3(0.75f, 0.75f, 5f);
			}
		}

		public void SetDirection()
		{
			if (_position.Contains("left"))
			{
				spr_emotion.flipX = true;
				spr_pose.flipX = true;
			}
			else
			{
				spr_emotion.flipX = false;
				spr_pose.flipX = false;
			}
		}


		public void SetPose(string pose)
		{
			if (_pose == pose)
			{
				return;
			}
			for (int i = 0; i < standimage.Length; i++)
			{
				if (standimage[i].name.Equals(pose))
				{
					spr_pose.sprite = standimage[i];
					break;
				}
			}
			_pose = pose;
		}

		private void Update()
		{
			Debug.Log(_pose);
			spr_pose.color = Color.Lerp(spr_pose.color, targetColor, 1-Mathf.Pow(0.6F,Time.deltaTime*10));
			spr_emotion.color = spr_pose.color;
			float num = 0f;
			float num2 = 1280f / 10f;
			num = num2 * 2f;
			if (_position.Contains("exit"))
			{
				num = num2 * -5f;
			}
			num = ((!_position.Contains("left")) ? (1280 / 2f - num) : (1280 / 2f * -1f + num));
			Vector3 localPosition = base.transform.localPosition;
			localPosition.x = Mathf.Lerp(localPosition.x, num, 1-Mathf.Pow(0.6F,Time.deltaTime*10));
			if (Mathf.Abs(localPosition.x- num) >= 45f)
			{
				delayedMove +=Time.deltaTime;
			}
			if (delayedMove >= 0.036f || justEnter || _position.Contains("exit"))
			{
				base.transform.localPosition = localPosition;
			}
			if (justEnter && Mathf.Abs(base.transform.localPosition.x - num) < 8f)
			{
				justEnter = false;
			}
			else if (Mathf.Abs(base.transform.localPosition.x - num) < 8f)
			{
				delayedMove = 0f;
			}
			if (justEnter)
			{
				return;
			}

		}
	}

