using UnityEngine;
	public interface IColliderDetector
	{
		bool OnTriggerEntenEvent(Collider2D collider2D, ColliderDetector detector);

		bool OnTriggerStayEvent(Collider2D collider2D, ColliderDetector detector);
	}

