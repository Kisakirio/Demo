using UnityEngine;
	public class Chatbox : MonoBehaviour
	{
		private float starty;

		private float targety;



		[SerializeField]
		private SpriteRenderer render;

		public void EnableMe(bool active)
		{
			base.enabled = active;
			render.enabled = active;
		}

		private void Start()
		{
			starty = -600f;
			targety = -360f;
		}

		private void Update()
		{
			Vector3 localPosition = base.transform.localPosition;
			float y = starty;
			if (DialogueSystem.instance.GetStatus() == Status.OPEN )
			{
				y = targety;
			}
			localPosition.y = y;
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, localPosition, 1-Mathf.Pow(0.6f,Time.deltaTime*27.5f));
			if (DialogueSystem.instance.GetStatus() == Status.EXITING&& Mathf.Abs(base.transform.localPosition.y - starty) < 1f)
			{
				DialogueSystem.instance.TurnOffChatSystem();
			}
		}
	}

