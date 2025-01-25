using UnityEngine;

public class SavePoint : MonoBehaviour
{

	[SerializeField]
	private ParticleSystem ps1;

	[SerializeField]
	private ParticleSystem ps2;

	public TipsType eventType;






	private void OnTriggerStay2D(Collider2D other)
	{
		Debug.Log(true);
		EventManager.instance.mainCharacter.HP=EventManager.instance.mainCharacter.maxHP;
		//if (EventManager.instance.mainCharacter.HP >= EventManager.instance.mainCharacter.maxHP)
		//{
			//EventManager.instance.mainCharacter.HP = EventManager.instance.mainCharacter.maxHP;
		//}
	}



	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!SaveManager.instance.GetTipsFlag((byte)eventType))
		{
			if (collision.gameObject.CompareTag("Player"))
			{
				TipsUI.instance.StartTips("Tips."+eventType.ToString(),6);
				SaveManager.instance.SaveTipsFalg((byte)eventType);
			}
		}

	}



}
