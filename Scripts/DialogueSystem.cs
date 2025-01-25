using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

public class DialogueSystem : MonoBehaviour
	{
		[SerializeField]
		private TextAsset textAsset;

		public static DialogueSystem instance;

		[SerializeField]
		private Status status;

		[SerializeField]
		private Chatbox cb_perfer;

		[SerializeField]
		private TextMeshPro text;

		[SerializeField]
		private ChatArrows ca_perfer;

		private byte lineLoad;

		private byte INIT = 0;

		private byte SET = 1;

		private byte FINISH = 2;

		private byte ALLFINISH = 3;

		private float linePause;

		private float letterSpeed = 0.025f;

		public Player player;

		public List<ChatCharacter> cclist=new List<ChatCharacter>();




		public class  ChatRow
		{
			public string section;

			public string character;

			public string pose;

			public string emotion;

			public string position;

			public string dialog;

			public string flag;

		}

		private float chatTime;

		private List<ChatRow> chats = new List<ChatRow>();

		private string targetText = "";

		private int currentline;

		private float lineTime;

		private float addCharTimer;

		private int lineChar;

		private float lineEndTime;

		private bool isClick;

		private int MAXCC = 4;

		[SerializeField]
		private GameObject cc_prefab;

		private void Start()
		{
			if (instance == null)
			{
				instance = this;
			}
			text.outlineColor = Color.black;
			text.outlineWidth = 0.3f;
			for (int i = 0; i < MAXCC; i++)
			{
				ChatCharacter component = Object.Instantiate(cc_prefab).GetComponent<ChatCharacter>();
				component.DisableMe();
				component.transform.parent = base.transform;
				Vector3 localPosition = new Vector3(0f, 0f, 2f);
				component.transform.localPosition = localPosition;
				cclist.Add(component);
			}
			base.gameObject.SetActive(value: false);
			player.m_Controls.UI.Click.started += ctx =>
			{
				isClick = true;
			};
			player.m_Controls.UI.Enter.started += ctx =>
			{
				isClick = true;
			};
		}


		public Status GetStatus()
		{
			return status;
		}

		public void CleanStand()
		{
			for (int i = 0; i < cclist.Count; i++)
			{
				Object.Destroy(cclist[i].gameObject);
			}
			cclist.Clear();
		}

		public void StartDialog(string section)
		{
			SetStatus(Status.OPEN);
			text.color = Color.white;
			cb_perfer.EnableMe(true);
			List<ChatRow> _chat=GetChatList();
			chats.Clear();
			foreach (var item in _chat)
			{
				if (item.section.Equals(section))
				{
					chats.Add(item);
				}
			}
			ca_perfer.HideArrow();
			CleanStand();
			isClick = false;
			currentline = 0;
			lineLoad = INIT;
			chatTime = 0f;
			linePause = 0;
			addCharTimer = 0;
			lineChar = 0;
			lineEndTime = 0;

		}
		public void SetStatus(Status s)
		{
			if (s == Status.OPEN )
			{
				bool flag = status == Status.OFF && s == Status.OPEN;
				status = s;
				if (flag)
				{
					base.gameObject.SetActive(value: true);
					//cn_perfer.ToggleActive(active: true);
					cb_perfer.EnableMe(active: true);
				}
			}
		}



		public void Exit()
		{
			AllExit();
			Color color = text.color;
			color.a = 0.1f;
			text.color = color;
			status = Status.EXITING;
		}

		public int GetChatsCount()
		{
			return chats.Count;
		}

		private ChatCharacter CreateCharacter(string character, string pose, string position)
		{

			ChatCharacter chatCharacter = Object.Instantiate(cc_prefab).GetComponent<ChatCharacter>();
			if (chatCharacter!= null)
			{
				//RemoveExiting();
				bool right = false;
				Vector3 localPosition = new Vector3(0f, -365f, 10f);
				if (position.Contains("right"))
				{
					right = true;
					localPosition.x += 1280f * 0.75f;
				}
				else
				{
					localPosition.x -=1280f * 0.75f;
				}
				chatCharacter.DisableMe();
				chatCharacter.transform.parent = base.transform;
				cclist.Add(chatCharacter);
				chatCharacter.EnableMe();
				chatCharacter.SetBrightness(1f);
				chatCharacter.SetCharacter(character, pose, isOld: false, right);
				chatCharacter._character = character;
				chatCharacter.transform.localPosition = localPosition;
				return chatCharacter;
			}
			return null;
		}

		private ChatCharacter FindStand(string character)
		{
			foreach (ChatCharacter stand in cclist)
			{
				if (stand.isActiveAndEnabled && stand.GetCharacter() == character)
				{
					return stand;
				}
			}
			return null;
		}



		public void Update()
		{

			if (status == Status.OPEN)
			{
				chatTime += Time.deltaTime;
				if (lineLoad == INIT || lineLoad == SET)
				{
					targetText = GetText(currentline);
					Debug.Log(targetText);

				}
				string Character = chats[currentline].character;
				string Pose = chats[currentline].pose;
				string text1 = chats[currentline].position;
				string text3 = chats[currentline].flag;
				ChatCharacter chatStand = FindStand(Character);
				if (lineLoad == INIT)
				{
					if (Character.Length > 1 && chatStand == null)
					{
						chatStand = CreateCharacter(Character, Pose, text1);
						chatStand.SetPosition(text1);


					}
					chatStand.SetDirection();
					lineLoad = SET;
					ca_perfer.ShowArrow();
				}

				if (lineLoad == SET)
				{
					if (Character.Length > 1)
					{
						string emotion = chats[currentline].emotion;
						chatStand.SetEmotion(emotion);
						chatStand.SetCharacter(Character, Pose, isOld: true, right: false);
						if (text3.Contains("black"))
						{
							chatStand.SetBlack();
						}

					}
					lineLoad = FINISH;
				}

				if (lineLoad == FINISH)
				{
					string text2 = "";
					int num2 = 1;
					for (int i = 0; i < num2; i++)
					{
						if (lineLoad == ALLFINISH)
						{
							continue;
						}

						if (linePause > 0f || chatTime < 1f)
						{
							linePause -= Time.deltaTime;
						}
						else
						{
							lineTime += Time.unscaledDeltaTime;
						}

						if (!(linePause <= 0f))
						{
							continue;
						}

						int num3 = (int)(lineTime / letterSpeed) - 5;
						if (num3 >= 0)
						{
							num3 = 0;
							addCharTimer += Time.deltaTime;
							if (addCharTimer >= letterSpeed)
							{
								addCharTimer = 0f;
								lineChar++;
							}

							num3 += lineChar;
						}

						if (num3 < 0)
						{
							num3 = 0;
						}
						else if (num3 < targetText.Length)
						{
							if (targetText[num3] == '<')
							{
								byte b = 0;
								for (b = 1; b < 48; b++)
								{
									if (targetText[num3 + b] == '>')
									{
										b++;
										break;
									}
								}

								lineChar += b;
								num3 += b;
							}
							else if (targetText[num3] == '\n')
							{
								linePause = 0.5f;
							}
						}

						if (num3 >= targetText.Length)
						{
							num3 = targetText.Length;
							lineLoad = ALLFINISH;
						}

						//Debug.Log(targetText);
						text2 = targetText.Substring(0, num3);
						text2 = Localize.ReplaceButton(text2);
						text.text = text2;
					}
				}

				if (lineLoad != ALLFINISH)
				{
					return;
				}

				lineEndTime += Time.deltaTime;
				if (isClick)
				{
					MovetoNextLine();
				}
			}

		}

		public void TurnOffChatSystem()
		{
			foreach (ChatCharacter stand in cclist)
			{
				if (stand.isActiveAndEnabled)
				{
					stand.DisableMe();
				}
			}
			status=Status.OFF;
			//cn_perfer.ToggleActive(active: false);
			cb_perfer.EnableMe(active: false);
			//ca_perfer.ToggleActive(active: false);
			base.gameObject.SetActive(value: false);
			text.text = "";
			//SettingManager.Instance.SaveSystemValueBool("isAutoVoiceAdvance", autovoiceadvance, usesaveicon: false);
		}

		private void AllExit()
		{
			foreach (ChatCharacter cc in cclist)
			{
				if (cc.isActiveAndEnabled)
				{
					if (cc._position.Contains("right"))
					{
						cc.SetPosition("rightexit");
					}
					else
					{
						cc.SetPosition("leftexit");
					}

				}
			}
		}


		public void MovetoNextLine()
		{
			if (status != Status.OPEN)
			{
				return;
			}
			lineLoad = INIT;
			currentline++;
			lineTime = 0;
			lineChar = 0;
			addCharTimer = 0;
			isClick = false;
			if (currentline >= GetChatsCount())
			{
				Exit();
			}
		}

		private string GetText(int id)
		{
			if (id < chats.Count)
			{
				return chats[id].dialog;
			}
			return "";
		}
		public List<ChatRow> GetChatList()
		{
			return (List<ChatRow>)JsonConvert.DeserializeObject(textAsset.text,typeof(List<ChatRow>));
		}
	}

