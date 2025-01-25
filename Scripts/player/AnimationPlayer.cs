using System;

using UnityEngine;

using Random = UnityEngine.Random;


public class AnimationPlayer:MonoBehaviour
{
	public Animator _animator;

	private string _curanimtion;

	[SerializeField]
	private CharacterBase cb;

	private Direction dir;

	public event Action AnimationEvent;

	public bool stunshake;


	[SerializeField]
	public SpriteRenderer basesprite;

	[SerializeField]
	public SpriteRenderer outlinesprite;

	[SerializeField]
	public SpriteRenderer effectsprite;

	[SerializeField]
	public SpriteRenderer flashsprite;

	private bool isFlash;

	private float time;

	private float flashing;

	private Player player;

	public void PlayAnimation(string animstate)
	{
		_animator.Play(animstate);
		_curanimtion = animstate;
	}

	public AnimationPlayer(Animator animator)
	{
		_animator = animator;
	}

	public bool IsCurrentAnimationThis(string name)
	{
		return _animator.GetCurrentAnimatorStateInfo(0).IsName(name);
	}

	public bool IsCurrentAnimationAlmostFinshed(float normalizetime)
	{
		if (!IsCurrentAnimationThis(_curanimtion))
		{
			return false;
		}

		if (!(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= normalizetime))
		{
			return false;
		}

		return true;
	}

	public bool IsCurrentAnimationAlmostFinshed()
	{
		return IsCurrentAnimationAlmostFinshed(0.9999f);
	}

	public float CurrentAnimationTime()
	{
		return _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
	}
	public string GetCurrentAnimationName()
	{
		return _curanimtion;
	}

	public void SetAnimationSpeed(float speed)
	{
		_animator.speed = speed;
	}
	public void FreezeThisFrame()
	{
		SetAnimationSpeed(0f);
		Invoke("ContinuePlay",.265f);
	}

	public void ContinuePlay()
	{
		SetAnimationSpeed(1f);
	}

	public void _AnimationEvent()
	{
		AnimationEvent?.Invoke();
	}
	public void SyncFlashSprite()
	{
		flashsprite.sprite = basesprite.sprite;
		flashsprite.flipX = basesprite.flipX;
	}

	private void Awake()
	{
		if (cb.isPlayer())
		{
			player= cb as Player;
		}
	}

	public void Update()
	{
		CheckDir();
		if (isFlash)
		{
			time -= Time.deltaTime;
			if (time >= 0)
			{
				if (cb.isPlayer())
				{
					player.SetHitBox(false);
				}
				else
				{
					cb.combat.DisableDetector(1);
				}
				if (flashing > 0f)
				{
					flashing -= Time.deltaTime;
					if (flashing <= 0f)
					{
						SyncFlashSprite();
						flashsprite.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 155);
						flashing = -0.04f;
					}
				}
				else if (flashing < 0f)
				{
					flashing += Time.deltaTime;
					if (flashing >= 0f)
					{
						flashsprite.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
						flashing = 0.04f;
					}
				}
			}
			else
			{
				if (cb.isPlayer())
				{
					player.SetHitBox(true);
				}
				else
				{
					cb.combat.EnableDetector(1,AttackType.bodyattack,10);
				}
				flashsprite.color = new Color(0, 0, 0, 0);
				isFlash= false;
			}
		}
	}

	public void CheckDir()
	{
		if(!cb.isPlayer()){

			if (dir != cb.direction)
			{
				dir = cb.direction;
				if (cb.direction == Direction.RIGHT)
				{
					basesprite.flipX = true;
					effectsprite.flipX = true;
					outlinesprite.flipX = true;
				}
				else
				{
					Debug.Log("changetoright");
					basesprite.flipX = false;
					effectsprite.flipX = false;
					outlinesprite.flipX = false;
				}
			}
		}
	}

	public void StartHitstun()
	{
		if (cb.shake > 0f)
		{
			if (Random.Range(0, 1) > 0.5f)
			{
				ShakeLeft();
			}
			else
			{
				ShakeRight();
			}
		}
		else
		{
			StopShake();
		}
	}

	private void SetXY(float x, float y)
	{
		Vector3 localPosition = base.transform.localPosition;
		localPosition.x = x;
		localPosition.z = 0f;
		base.transform.localPosition = localPosition;
	}

	private void StopShake()
	{
		SetXY(0f, 0f);
		stunshake = false;
	}

	private void ShakeLeft()
	{
		float num = cb.shake * 1.2f;
		if (num > 0.15f)
		{
			num = 0.15f;
		}
		else if (num < 0.025f)
		{
			num = 0.025f;
		}
		SetXY(0f - num, 0f);
		if (cb.shake > 0f)
		{
			Invoke("ShakeRight", 0.0375f);
		}
		else
		{
			StopShake();
		}
	}

	private void ShakeRight()
	{
		float num = cb.shake * 1.2f;
		if (num > 0.15f)
		{
			num = 0.15f;
		}
		else if (num < 0.025f)
		{
			num = 0.025f;
		}
		SetXY(num, 0f);
		if (cb.shake > 0f)
		{
			Invoke("ShakeLeft", 0.0375f);
		}
		else
		{
			StopShake();
		}
	}

	public void SetFlashMode(float _time)
	{
		isFlash = true;
		flashing = 0.1f;
		time = _time;
	}

	public void StopFlash()
	{
		Color color = flashsprite.color;
		color.r = 0f;
		color.g = 0f;
		color.b = 0f;
		color.a = 0f;
		flashsprite.color = color;
		flashing = 0f;
		isFlash= false;
	}
	public void ReduceAlpha(float a)
	{
		Color color = basesprite.color;
		Color color2 = outlinesprite.color;
		color.a -= a * Time.deltaTime;
		color2.a -= a * Time.deltaTime;
		if (color.a < 0f)
		{
			color.a = 0f;
		}
		if (color2.a < 0f)
		{
			color2.a = 0f;
		}
		basesprite.color = color;
		outlinesprite.color = color2;
	}

}
