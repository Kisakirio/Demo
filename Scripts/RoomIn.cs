using UnityEngine;
using UnityEngine.UI;

public class RoomIn : MonoBehaviour
	{

		[SerializeField]
		private CanvasGroup cg;

		[SerializeField]
		private Sprite[] sprs;

		[SerializeField]
		private Image sr;

		private float timer;

		private void Awake()
		{
			base.gameObject.SetActive(value: true);
		}

		public void StartMe(float _time = 0f)
		{
			timer = _time;
			cg.alpha = 1f;
			base.gameObject.SetActive(value: true);
			if (_time == 0f)
			{
				sr.sprite = sprs[0];
				base.transform.localScale = Vector3.one;
			}
			else
			{
				sr.sprite = sprs[1];
				base.transform.localScale = Vector3.one / 2.5f;
			}
		}

		private void Update()
		{

			timer += Time.deltaTime;
			if (timer <=1f)
			{
				cg.alpha = Mathf.Lerp(cg.alpha, 0f, 1-Mathf.Pow(0.6f,10*Time.deltaTime));
				if (cg.alpha <= 0.01f)
				{
					base.gameObject.SetActive(value: false);
				}
			}
			else
			{
				cg.alpha = Mathf.Lerp(cg.alpha, 1f, 1-Mathf.Pow(0.6f,40*Time.deltaTime));
			}
		}

	}

