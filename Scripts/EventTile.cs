using System;
using UnityEngine;

public class EventTile : MonoBehaviour
{
	public TipsType eventType;

	public void Start()
	{
		if(SaveManager.instance.GetTipsFlag((byte)eventType))
		{
			base.gameObject.SetActive(false);
		}
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		if (!SaveManager.instance.GetTipsFlag((byte)eventType))
		{
			if (other.gameObject.CompareTag("Player"))
			{

				TipsUI.instance.StartTips("Tips."+eventType.ToString(),6);
				base.gameObject.SetActive(false);
				SaveManager.instance.SaveTipsFalg((byte)eventType);
			}
		}

	}
}
