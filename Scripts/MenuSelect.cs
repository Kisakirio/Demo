using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuSelect : MonoBehaviour
{
    [SerializeField]
	private Image background;

	[SerializeField]
	private  TextMeshPro text;

	[SerializeField]
	private bool isNewGameSelect;

	[SerializeField]
	private bool isCustomGameSelect;

	[SerializeField]
	private bool isAbout;

	[SerializeField]
	private bool isLast;

	private int unlockReq;


	public void SetText(string txt)
	{
		text.text = txt;
	}

	public int GetUnlockReq()
	{
		return unlockReq;
	}

	public bool isLock()
	{
		if (text.text.Contains("?"))
		{
			return true;
		}
		return false;
	}

	/*public void UpdateFont(int ID)
	{
		if (isCustomGameSelect)
		{
			bool flag = false;
			if ((byte)ID == 7 && SettingManager.Instance.GetAchievementUnlockedCount() < 80)
			{
				flag = true;
				unlockReq = 80;
			}
			if ((byte)ID == 2 && SettingManager.Instance.GetAchievementUnlockedCount() < 60)
			{
				flag = true;
				unlockReq = 60;
			}
			if ((byte)ID == 3 && SettingManager.Instance.GetAchievementUnlockedCount() < 50)
			{
				flag = true;
				unlockReq = 50;
			}
			if ((byte)ID == 6 && SettingManager.Instance.GetAchievementUnlockedCount() < 70)
			{
				flag = true;
				unlockReq = 70;
			}
			if (flag)
			{
				text.text = "? ? ?";
			}
			else
			{
				text.text = Localize.GetLocalizeTextWithKeyword("CustomGame." + (CustomGame)ID, contains: false);
			}
		}
		else if (!isNewGameSelect)
		{
			text.text = Localize.GetLocalizeTextWithKeyword("MainMenu.Selections" + ID, contains: false);
		}
		else
		{
			switch (ID)
			{
			case 2:
				ID = 3;
				break;
			case 3:
				ID = 5;
				break;
			case 4:
				ID = 7;
				break;
			case 5:
				ID = 10;
				break;
			}
			text.text = Localize.GetLocalizeTextWithKeyword("Difficulty." + ID, contains: false);
		}
	}*/

	public string GetText()
	{
		return text.text;
	}

	private void Start()
	{
		if (!isNewGameSelect)
		{
			background.transform.localPosition = new Vector3(-800f, 0f, 0f);
		}
		background.color = new Color(1f, 1f, 1f, 0f);
		if (isCustomGameSelect)
		{
			text.color = new Color(1f, 1f, 1f, 0.5f);
			background.transform.localScale = new Vector3(1f, 1f, 1f);
			background.transform.localPosition = new Vector3(isLast ? (-495f) : (-215f), 0f, 0f);
		}

		text.sortingOrder = 1000;
	}

	public void UpdateMe(bool isSelected)
	{
		//Debug.Log(isSelected);
		if (!isNewGameSelect)
		{
			background.transform.localPosition = Vector3.Lerp(background.transform.localPosition, new Vector3(isSelected ? 0f : (-900f), 0f, 0f),1-Mathf.Pow(0.6f,Time.deltaTime*15f));
		}
		if (isSelected)
		{
			text.color = new Color(1f, 1f, 1f, 1f);
		}
		else
		{
			text.color = new Color(1f, 1f, 1f, 0.5f);
		}
		if (isLock())
		{
			text.color /= 1.25f;
		}
		background.color = Color.Lerp(background.color, new Color(1f, 1f, 1f, isSelected ? (GetAlpha(isNewGameSelect ? 0.275f : 0.09f, Time.unscaledTime, isNewGameSelect ? 395f : 100f) + 0.5f) : 0f), 1-Mathf.Pow(0.6f,Time.deltaTime*3f));
	}
	public static float GetAlpha(float range, float rate, float power)
	{
		return range * Mathf.Sin(power * rate *Mathf.PI / 180f);
	}


}
