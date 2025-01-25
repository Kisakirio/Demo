using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Combat : MonoBehaviour, IColliderDetector
{
	[SerializeField] private ColliderDetector[] _detectors;

	private Collider2D _hurtCollider;

	public Collider2D HurtCollider => _hurtCollider;

	private Dictionary<ColliderDetector, AttackedInfo> _battleInfo;

	public List<CombatInfo> _combatInfos = new List<CombatInfo>();

	public List<int> a = new List<int>();


	public void Init()
	{
		_battleInfo = new Dictionary<ColliderDetector, AttackedInfo>();
		for (int i = 0; i < _detectors.Length; i++)
		{
			if (_detectors[i] != null)
			{
				_detectors[i].Init(this);
				_battleInfo.Add(_detectors[i], new AttackedInfo());
			}
		}

		_hurtCollider = GetComponent<Collider2D>();
	}

	public void SetDetector(int detectorIndex, AttackType attackType, float damage)
	{
		ColliderDetector detector = _detectors[detectorIndex];
		if (detector != null)
		{
			detector.enableDetect = false;
			_battleInfo[detector].damage = damage;
			_battleInfo[detector]._attackType = attackType;
		}

	}

	public void EnableDetector(int detectorIndex)
	{
		ColliderDetector detector = _detectors[detectorIndex];
		if (detector != null)
		{
			detector.enableDetect = true;
			detector._collider.enabled = true;
		}

	}

	public void EnableDetector(int detectorIndex, AttackType attackType, float damage)
	{
		ColliderDetector detector = _detectors[detectorIndex];
		if (detector != null)
		{
			detector.enableDetect = true;
			detector._collider.enabled = true;
			_battleInfo[detector].damage = damage;
			_battleInfo[detector]._attackType = attackType;
		}
	}

	public void DisableDetector(int detectorIndex)
	{
		ColliderDetector detector = _detectors[detectorIndex];
		if (detector != null)
		{
			detector.enableDetect = false;
			detector.ResetHit();
		}
	}

	public bool OnTriggerEntenEvent(Collider2D collider, ColliderDetector detector)
	{
		return AttackedByCollider(collider, _battleInfo[detector]._attackType, _battleInfo[detector].damage,
			detector.GetColliderPositon());
	}


	public bool OnTriggerStayEvent(Collider2D collider, ColliderDetector detector)
	{
		return AttackedByCollider(collider, _battleInfo[detector]._attackType, _battleInfo[detector].damage,
			detector.GetColliderPositon());
	}

	public bool AttackedByCollider(Collider2D collider2D, AttackType attackType, float damage, Vector2 _hitPosition)
	{
		if (collider2D == null)
		{
			return false;
		}

		string _tag = collider2D.gameObject.tag;
		CharacterBase compent = collider2D.gameObject.GetComponentInParent<CharacterBase>();
		if (compent != null)
		{
			CombatInfo combatInfo = new CombatInfo()
			{
				attackType = attackType,
				damage = damage,
				attackReceive = compent,
				attackSender = this,
				hitPosition = _hitPosition,
				tag = _tag,
			};
			AddCombatInfo(combatInfo);
			return true;
		}

		return false;

	}

	public void AddCombatInfo(CombatInfo combatInfo)
	{

		if (_combatInfos.Count > 0)
		{
			for (int i = 0; i < _combatInfos.Count; i++)
			{
				if (_combatInfos[i].attackReceive == combatInfo.attackReceive)
				{
					_combatInfos[i] = combatInfo;
					return;
				}
			}
		}

		_combatInfos.Add(combatInfo);

	}

	public void SendAllInfos()
	{
		for (int i = 0; i < _combatInfos.Count; i++)
		{
			_combatInfos[i].attackReceive.GetInfos(_combatInfos[i]);
			Debug.Log(_combatInfos[i]);
		}

		ClearCombatInfos();
	}

	public void ClearCombatInfos()
	{
		_combatInfos.Clear();
	}

	public bool IsHit(int index)
	{
		return _detectors[index].IsHit;
	}

	public void ResetHit()
	{
		for (int i = 0; i < _detectors.Length; i++)
		{
			_detectors[i].ResetHit();
		}
	}

	public bool IsHitBoxStart(int index)
	{
		return _detectors[index].enableDetect;
	}

	public ColliderDetector GetDetector(int index)
	{
		return _detectors[index];
	}

	public List<CombatInfo> GetCombatInfo()
	{
		return _combatInfos;
	}
}
