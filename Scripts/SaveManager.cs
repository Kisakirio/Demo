using System;
using UnityEngine;
	public class SaveManager : MonoBehaviour
	{

		[Serializable]
		public struct Savedata
		{
			public float x;

			public float y;

			public byte dir;

			public int HP;

			public int MaxHP;

			public bool HPTip;

			public bool firststartstory;

			public bool canDash;

			public bool[] tipsflag;

		}
		public static SaveManager instance;

		public Savedata savedata;

		private bool AutoSaved;

		[SerializeField]
		private CharacterBase mainCharacter;
		public void Awake()
		{
			if (instance == null)
			{
				instance = this;
			}
		}

		private string GetSaveFileName(byte saveslot)
		{
			string text = "tevisave" + saveslot + ".sav";
			return text;
		}
		[ContextMenu("save")]
		public void SaveGame()
		{

			string saveFileName = GetSaveFileName(1);
			ES3File eS3File = new ES3File(saveFileName);
			CopyVarBeforeSaving();
			eS3File.Save("HP", savedata.HP);
			eS3File.Save("posX", savedata.x);
			eS3File.Save("posY", savedata.y);
			eS3File.Save("Dir", savedata.dir);
			eS3File.Save("HPTips",savedata.HPTip);
			eS3File.Save("firststartstory",savedata.firststartstory);
			eS3File.Save("CANDASH",savedata.canDash);
			eS3File.Save("TIPSFLAG",savedata.tipsflag);
			eS3File.Sync();
		}



		public bool HasAdd()
		{
			return savedata.HPTip;
		}

		public bool HasAddTIPS()
		{
			return savedata.HPTip = true;
		}
		public void LoadGame()
		{
			AutoSaved = true;
			string saveFileName = GetSaveFileName(1);
			bool flag = false;
			if (ES3.FileExists(saveFileName))
			{
				ES3File eS3File = new ES3File(saveFileName);
				Invoke("showLoaded", 0.05f);
				Vector3 position = mainCharacter.transform.position;
				if (eS3File.KeyExists("posX"))
				{
					position.x = eS3File.Load<float>("posX");
				}
				else
				{
					flag = true;
				}
				if (eS3File.KeyExists("posY"))
				{
					position.y = eS3File.Load<float>("posY");
				}
				else
				{
					flag = true;
				}

				savedata.firststartstory = eS3File.Load<bool>("firststartstory");
				savedata.HPTip = eS3File.Load<bool>("HPTips");
				savedata.tipsflag = eS3File.Load<bool[]>("TIPSFLAG");
				savedata.canDash = eS3File.Load<bool>("CANDASH");
				savedata.x = position.x;
				savedata.y = position.y;
				EventManager.instance.tempPos = position;
				if (eS3File.KeyExists("Dir"))
				{
					savedata.dir = eS3File.Load<byte>("Dir");
				}
				else
				{
					flag = true;
				}

				if (!flag)
				{
					mainCharacter.ChangeDirection((Direction)savedata.dir);
					mainCharacter.HP = eS3File.Load<int>("HP");

				}

			}

		}

		private void CopyVarBeforeSaving()
		{

			savedata.HP = mainCharacter.HP;
			float Direction = mainCharacter.transform.rotation.y == 0 ? -1 : 2;
			savedata.x = mainCharacter.transform.position.x+Direction;
			savedata.y = mainCharacter.transform.position.y;
			savedata.dir = (byte)mainCharacter.direction;
		}

		public void SaveTipsFalg(byte i)
		{
			savedata.tipsflag[i] = true;
		}

		public bool GetTipsFlag(byte i)
		{
			return savedata.tipsflag[i];
		}

		public int GetMaxHP()
		{
			return savedata.HPTip?220:200;
		}
	}

