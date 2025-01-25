using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObtainedItemUI : MonoBehaviour
{
   public static ObtainedItemUI Instance;

	[SerializeField]
	private SpriteRenderer background;

	[SerializeField]
	private SpriteRenderer itemicon;

	[SerializeField]
	private List<Sprite> sprites;

	[SerializeField]
	private TextMeshPro itemname;

	[SerializeField]
	private TextMeshPro itemdesc;

	[SerializeField]
	private TextMeshPro promote;

	public Player player;

	private bool isExit;



	private ItemType gotitem;

	private float isDisplay;

	private bool allowEquip;

	public bool isDisplaying()
	{
		if (!(isDisplay > 0f))
		{
			return false;
		}
		return true;
	}

	private void ToStartPosition()
	{
		base.transform.position = Vector3.zero - new Vector3(1500f, 0f, 0f);
	}

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}

		player.m_Controls.UI.Enter.performed += ctx =>
		{
			Debug.Log("enter");
			isExit = true;
		};
		base.enabled = false;
		base.gameObject.SetActive(value: false);
	}


	public void GiveItem(ItemType type)
	{
		isExit = false;
		allowEquip = false;
		gotitem = type;
		promote.color = new Color(1f, 1f, 1f, 0f);
		promote.text = Localize.GetLocalizeTextByKeywor("BottomBarPrompt.YesOnly");
		promote.text = Localize.ReplaceButton(promote.text);
		bool flag = false;
		if (type.ToString().Contains("Add"))
		{
			if (!SaveManager.instance.HasAdd())
			{
				player.m_Controls.GamePlayer.Disable();
				SaveManager.instance.HasAddTIPS();
				flag = true;
			}
			
		}

		if (type.ToString().Contains("Dash"))
		{
			if (!SaveManager.instance.savedata.canDash)
			{
				player.m_Controls.GamePlayer.Disable();
				SaveManager.instance.savedata.canDash=true;
				flag = true;
			}
		}
		if (flag)
		{
			if (gotitem == ItemType.AddHP)
			{
				itemicon.sprite = sprites[0];
				itemname.text = Localize.GetLocalizeTextByKeywor("ITEMNAME.STACKABLE_HP");
				itemdesc.text = Localize.GetLocalizeTextByKeywor("ITEMDESC.STACKABLE_HP");
			}

			if (gotitem == ItemType.DashShoe)
			{
				itemicon.sprite = sprites[1];
				itemname.text = Localize.GetLocalizeTextByKeywor("ITEMNAME.DashShoe");
				itemdesc.text = Localize.GetLocalizeTextByKeywor("ITEMDESC.DashShoe");
				itemdesc.text = Localize.ReplaceButton(itemdesc.text);
			}

			base.enabled = true;
			base.gameObject.SetActive(value: true);
			isDisplay = 0.01f;
			ToStartPosition();
			Debug.Log("[Popup] Item obtained window spawned : " + type.ToString() + " Lv: " );
			//EventManager.Instance.StopFastForward();
			//EventManager.Instance.mainCharacter.playerc_perfer.itemExplorer.DelayStart();
		}
		else
		{
			Debug.LogWarning("[Popup] Item obtained window skipped because player already owned it : " + type.ToString() + " Lv: ");
		}
	}

	public void Close()
	{
		isDisplay = 0f;
		base.enabled = false;
		base.gameObject.SetActive(value: false);
		Debug.Log("[Popup] Item obtained window closed");
	}

	private void Update()
	{
		if (!isDisplaying())
		{
			return;
		}

		float num = 1f;
		isDisplay +=Time.deltaTime;
		if (isDisplay >= 357.5f)
		{
			num = 0.5f;
			if (Vector3.Distance(Vector3.zero + new Vector3(1500f, 0f, 0f), base.transform.localPosition) <= 200f)
			{
				Close();
			}
		}
		/*else if ((isDisplay >= 0.675f || GemaUIPauseMenu.Instance.isPauseMenuEnabled()) &&
		         InputButtonManager.Instance.GetButtonDown(13))
		{
			isDisplay = 357.5f;
			MusicManager.Instance.PlaySound(AllSound.SEList.MENUSELECT);
		}
		else if ((isDisplay >= 0.675f || GemaUIPauseMenu.Instance.isPauseMenuEnabled()) &&
		         InputButtonManager.Instance.GetButtonDown(11) && allowEquip &&
		         (SaveManager.Instance.GetItem(gotitem) == 1 || SaveManager.Instance.GetItem(gotitem) == 2))
		{
			SaveManager.Instance.SetItem(gotitem, 3);
			isDisplay = 357.5f;
			MusicManager.Instance.PlaySound(AllSound.SEList.EQUIP);
		}*/
		if (isExit)
		{
			isDisplay = 357.5f;
			player.m_Controls.GamePlayer.Enable();
		}

		if (isDisplay >= 0.675f)
		{
			promote.color = Color.Lerp(promote.color, new Color(1f, 1f, 1f, 1f), 1-Mathf.Pow(0.6f,Time.deltaTime*12.5f));
		}

		Vector3 zero = Vector3.one;
		if (isDisplay >= 357.5f)
		{
			zero.x = 1500f;
		}

		base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, zero, 1-Mathf.Pow(0.6f,Time.deltaTime*37.5f));
	}
}
