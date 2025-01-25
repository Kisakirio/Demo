using System;
using UnityEngine;
using UnityEngine.UI;

public class HPBar:MonoBehaviour
{
	[SerializeField]
	private Image hpBar;

	public CharacterBase cb;

	[SerializeField]
	private bool isFake;

	public float health;
	public void FixedUpdate()
	{
		float num = 1;
		float num1 = cb.HP;
		float num2 = cb.maxHP;

		if (health == num1)
		{
			return;
		}
		if (isFake)
		{
			num = num * health / num2;
			health = Mathf.Lerp(health, num1, 1 - Mathf.Pow(0.6f, Time.unscaledDeltaTime * 7.25f));
		}
		else
		{
			num = num * health / num2;
			health = Mathf.Lerp(health, num1, 1 - Mathf.Pow(0.6f, Time.unscaledDeltaTime * 17.25f));
		}

		hpBar.fillAmount = num;
	}
}

