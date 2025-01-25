using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTile : MonoBehaviour
{
	[SerializeField]
	private ItemType _itemType;
	public void OnTriggerEnter2D(Collider2D other)
	{
		if (_itemType == ItemType.AddHP)
		{
			if (other.gameObject.CompareTag("Player"))
			{
				ObtainedItemUI.Instance.GiveItem(ItemType.AddHP);
				EventManager.instance.mainCharacter.HP += 20;
				SaveManager.instance.savedata.MaxHP += 20;
				DisableMe();
			}
		}

		if (_itemType == ItemType.DashShoe)
		{
			if (other.gameObject.CompareTag("Player"))
			{
				ObtainedItemUI.Instance.GiveItem(ItemType.DashShoe);
				SaveManager.instance.savedata.canDash = true;
				DisableMe();
			}
		}

	}

	public void Start()
	{
		if (SaveManager.instance.savedata.HPTip)
		{
			DisableMe();
		}
	}

	public void DisableMe()
	{
		base.gameObject.SetActive(false);
	}
}
