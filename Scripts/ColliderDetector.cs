using UnityEngine;
using System;


     [RequireComponent(typeof(Collider2D))]
     public class ColliderDetector:MonoBehaviour
     {
	     private IColliderDetector _detector;

	     public bool enableDetect;

	     public string[] checkTag;

	     private bool isHit;

	     public bool IsHit => isHit;

	     public Collider2D _collider;

	     private event Action<Collider2D> _OnTriggerEnterEvent;
	     private event Action<Collider2D> _OnTriggerStayEvent;


	     public void Init(IColliderDetector detector)
	     {
		     _detector = detector;
		     _collider = GetComponent<Collider2D>();
	     }


	     public void AddOnTriggerEnter2DEvent(Action<Collider2D> action)
	     {
		     _OnTriggerEnterEvent += action;
	     }

	     public void AddOnTriggerStay2DEvent(Action<Collider2D> action)
	     {
		     _OnTriggerStayEvent += action;
	     }

	     public void RemoveOnTriggerEnter2DEvent(Action<Collider2D> action)
	     {
		     _OnTriggerEnterEvent -= action;
	     }

	     public void RemoveOnTriggerStay2DEvent(Action<Collider2D> action)
	     {
		     _OnTriggerStayEvent -= action;
	     }


	     private void OnTriggerStay2D(Collider2D collider)
	     {
		     if (enableDetect && CheckTags(collider.gameObject.tag, checkTag))
		     {
			     _OnTriggerEnter2D(collider);
		     }
	     }

	     private void OnTriggerEnter2D(Collider2D collider)
	     {
		     if (enableDetect && CheckTags(collider.gameObject.tag, checkTag))
		     {
			     _OnTriggerEnter2D(collider);
		     }
	     }



	     private void _OnTriggerEnter2D(Collider2D collider)
	     {

		     isHit |= _detector.OnTriggerEntenEvent(collider, this);
		     _OnTriggerEnterEvent?.Invoke(collider);
	     }

	     private void _OnTriggerStay2D(Collider2D collider)
	     {
		     isHit |= _detector.OnTriggerStayEvent(collider, this);
		     _OnTriggerStayEvent?.Invoke(collider);
	     }


	     private bool CheckTags(string target, params string[] tags)
	     {
		     foreach (string text in tags)
		     {
			     if (target == text)
				     return true;
		     }

		     return false;
	     }

	     public Vector3 GetColliderPositon()
	     {
		     return new Vector3(_collider.transform.position.x + _collider.offset.x, _collider.transform.position.y + _collider.offset.y, _collider.transform.position.z);
	     }

	     public void ResetHit()
	     {
		     isHit = false;
	     }

     }

