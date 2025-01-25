using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatArrows : MonoBehaviour
{
	private float starty;

	[SerializeField]
	private SpriteRenderer sr_perfer;

	private Color targetColor;

	private bool isIcon;

	public void StartMe(bool active)
	{
		base.enabled = active;
		sr_perfer.enabled = active;
	}

	private void Start()
	{
		targetColor = sr_perfer.color;
		starty = base.transform.localPosition.y;
		StartMe(active: false);
	}

	public void ShowArrow()
	{
		Color color = sr_perfer.color;
		if (targetColor.a != 1f)
		{
			color.a = 0f;
		}
		sr_perfer.color = color;
		targetColor.a = 1f;
		StartMe(active: true);
	}

	public void HideArrow()
	{
		Color color = sr_perfer.color;
		color.a = 0f;
		sr_perfer.color = color;
		targetColor.a = 0f;
	}

	public void SetIcon(Sprite s, bool _isIcon)
	{
		sr_perfer.sprite = s;
		isIcon = _isIcon;
	}

	private void Update()
	{
		float t = 1f - Mathf.Pow(0.5f, Time.deltaTime * 10f);
		sr_perfer.color = Color.Lerp(sr_perfer.color, targetColor, t);
		Vector3 localPosition = base.transform.localPosition;
		localPosition.y = starty;
		localPosition.y += 5f;
		if (!isIcon)
		{
			localPosition.y +=GetAlphaSin(2.725f, Time.unscaledTime, 280f);
			base.transform.localEulerAngles = Vector3.zero;
			base.transform.localScale = Vector3.one;
		}
		else
		{
			base.transform.Rotate(0f, 0f, Time.deltaTime * -150f);
			base.transform.localScale = Vector3.one * 0.8f;
		}
		base.transform.localPosition = localPosition;
	}

	public static float GetAlphaSin(float range, float rate, float power)
	{
		return range * Mathf.Sin(power * rate * Mathf.PI / 180f);
	}

}
