
	using UnityEngine;

	public class MovementPlatform : MonoBehaviour
	{
		[SerializeField]
		private Vector3 start;

		[SerializeField]
		private Vector3 end;

		[SerializeField]
		public bool isup;

		public Transform t;

		public float Maxspeed;

		public float speed;

		[SerializeField]
		public bool Vertical;

		[SerializeField]
		public bool Horizontal;

		private Vector3 targetpos;

		public bool isright;
		private void OnTriggerStay2D(Collider2D c)
		{
			if (!c.CompareTag("Player"))
			{
				return;
			}

			CharacterBase cb = c.GetComponent<CharacterBase>();
			if (cb && cb.onGrounded())
			{

				if (Vertical)
				{
					if (isup)
					{
						cb._transform.position += new Vector3(0, speed, 0) * Time.deltaTime;
					}
					else
					{
						cb._transform.position -= new Vector3(0, speed, 0) * Time.deltaTime;
					}
				}

				if (Horizontal)
				{
					if (isright)
					{
						cb._transform.position += new Vector3(speed, 0, 0) * Time.deltaTime;
					}
					else
					{
						cb._transform.position -= new Vector3(speed, 0, 0) * Time.deltaTime;
					}
				}
			}
		}

		private void Start()
		{
			t.position = start;
		}

		public void FixedUpdate()
		{
			Move();
		}

		private void Move()
		{
			targetpos = t.position;
			if ((Vertical&&speed < Maxspeed && Mathf.Abs(t.position.y - end.y) >0.2f)||(Horizontal&&speed < Maxspeed && Mathf.Abs(t.position.x - end.x)>0.2f))
			{
				speed = Mathf.Lerp(speed, Maxspeed, 1 - Mathf.Pow(0.6f, Time.deltaTime * 25f));
			}

			if ((Vertical&&Mathf.Abs(t.position.y - end.y) <0.2f)||(Horizontal&&Mathf.Abs(t.position.x - end.x) <0.2f))
			{

				speed = Mathf.Lerp(speed, 0, 1 - Mathf.Pow(0.6f, Time.deltaTime * 25f));
				if (speed <= 0.1f)
				{
					speed = 0;
				}
			}

			if (speed == 0)
			{
				(end, start) = (start, end);
				isup = !isup;
				isright = !isright;
			}

			if (Vertical)
			{
				if (isup)
				{
					targetpos.y += speed * Time.deltaTime;
				}
				else
				{
					targetpos.y -= speed * Time.deltaTime;
				}
			}

			if (Horizontal)
			{
				if (isright)
				{
					targetpos.x += speed * Time.deltaTime;
				}
				else
				{
					targetpos.x -= speed * Time.deltaTime;
				}
			}

			t.position = targetpos;
		}
	}

