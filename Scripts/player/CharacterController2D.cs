using System;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class CharacterController2D : MonoBehaviour
{

	private struct rayCastOrigins
	{
		public Vector2 topLeft;
		public Vector2 bottomLeft;
		public Vector2 bottomRight;
		public Vector2 center;

	}

	public class CharacterCollisionState2D
	{
		public bool right;

		public bool left;

		public bool above;

		public bool below;

		public bool becameGroundedThisFrame;

		public bool wasGroundedLastFrame;

		public bool movingSlope;

		public float slopeAngle;


		public bool hasCollision()
		{
			if (!below && !right && !left)
			{
				return above;
			}
			return true;
		}

		public void reset()
		{
			right = (left = (above = (below = (becameGroundedThisFrame = (movingSlope = false)))));
			slopeAngle = 0f;
		}
		public override string ToString()
		{
			return $"[CharacterCollisionState2D] r: {right}, l: {left}, a: {above}, b: {below}, movingDownSlope: {movingSlope}, angle: {slopeAngle}, wasGroundedLastFrame: {wasGroundedLastFrame}, becameGroundedThisFrame: {becameGroundedThisFrame}";
		}
	}


	public LayerMask platformMask=0;

	public LayerMask oneWayPlatformMask = 0;

	private Rigidbody2D m_Rigidbody;

	private ContactFilter2D m_ContactFilter;

	private Vector2[] m_RayCastStartPostions=new Vector2[3];

	private RaycastHit2D[] m_HitBuffer=new RaycastHit2D[5];

	private RaycastHit2D[] m_FounHit=new RaycastHit2D[3];

	private Collider2D[] m_OnGroundCollider = new Collider2D[3];

	private List<RaycastHit2D> m_rayCastHitThisFrame=new List<RaycastHit2D>(2);

	public BoxCollider2D m_Boxcollider;

	private rayCastOrigins m_rayCastOrigins;

	private RaycastHit2D m_rayCastHit;

	private float m_KeepWidth=.02f;

	public float groundTime;

	public float justJumped;

	public float airTime;

	public LayerMask test1 = 0;

	private float _slopeLimitTangent = Mathf.Tan(1.3089969f);

	[NonSerialized]
	[HideInInspector]
	public CharacterCollisionState2D m_colliderState = new CharacterCollisionState2D();

	[Range(2,20)]
	public int totalHorizontalRay=8;




	[NonSerialized]
	[HideInInspector]
	public new Transform transform;

	[NonSerialized]
	[HideInInspector]
	public Vector2 velocity;


	public AnimationCurve SlopspeedMultiple = new AnimationCurve(new Keyframe(-90f, 1.5f), new Keyframe(0f, 1f), new Keyframe(90f, 0f));
	public bool IsGrounded => m_colliderState.below;

	public bool isAir => !m_colliderState.below;

	public bool setJustjumped;
	public bool OnGround()
	{
		if (groundTime >=.15f)
		{
			return true;
		}
		return false;
	}

	public Collider2D[] m_GroundCollider { get { return m_OnGroundCollider; } }


    void Awake()
    {
	    platformMask = platformMask | oneWayPlatformMask;
	    transform = GetComponent<Transform>();
	    m_Boxcollider = GetComponent<BoxCollider2D>();
	    m_Rigidbody = GetComponent<Rigidbody2D>();

    }



    public void InitalRayCastOrigin()
    {
	    Bounds bounds = m_Boxcollider.bounds;
	    bounds.Expand(-2f * m_KeepWidth);
		m_rayCastOrigins.bottomLeft = bounds.min;
	    m_rayCastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
	    m_rayCastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
	    m_rayCastOrigins.center = (m_rayCastOrigins.bottomLeft + m_rayCastOrigins.bottomRight)/2;

    }


    private void DrawRay(Vector3 start, Vector3 dir, Color color)
    {
	    UnityEngine.Debug.DrawRay(start, dir, color);
    }

	public void Move(Vector2 deltaMovement, float moveSpeed)
	{
		m_colliderState.wasGroundedLastFrame = m_colliderState.below;

	    m_colliderState.reset();
	    Vector2 position = transform.position;
	    m_Rigidbody.position = position;
		InitalRayCastOrigin();
		if (deltaMovement.y < 0 && m_colliderState.wasGroundedLastFrame)
			MoveHorizontalDownSlop(ref deltaMovement);
		if (deltaMovement.x != 0)
			MoveHorizontal(ref deltaMovement);
		if(deltaMovement.y!=0)
		   MoveVertical(ref deltaMovement);

		position += deltaMovement * moveSpeed;
		Rigidbody2D rigidbody2D =m_Rigidbody;
		Vector3 vector2 = (transform.position = position);
		rigidbody2D.position = vector2;
		if (Time.deltaTime > 0f)
		{
			velocity = deltaMovement / Time.deltaTime;

		}
		if (m_colliderState.below)
		{
			groundTime += Time.deltaTime;
		}
		else
		{
			groundTime = 0;
		}

		if (!m_colliderState.wasGroundedLastFrame && m_colliderState.below)
		{
			m_colliderState.becameGroundedThisFrame = true;
		}

		if (m_colliderState.movingSlope)
		{
			velocity.y = 0;
		}



	}
	public void warpToGrounded()
	{
		do
		{
			Move(new Vector2(0f, -1f), 1f);
		}
		while (!IsGrounded);
	}

	private void MoveHorizontal(ref Vector2 deltaMovement)
    {

	    bool flag = deltaMovement.x > 0;
	    float distance = MathF.Abs(deltaMovement.x)+m_KeepWidth;
	    Vector2 direction = flag ? Vector2.right : (-Vector2.right);
	    Vector2 vector = flag ? m_rayCastOrigins.bottomRight : m_rayCastOrigins.bottomLeft;
	    for (int i = 0; i < totalHorizontalRay; i++)
	    {
		    Vector2 start = new Vector2(vector.x, vector.y + i * (m_rayCastOrigins.topLeft.y-m_rayCastOrigins.bottomLeft.y)/(totalHorizontalRay-1));

		    if (i == 0 && m_colliderState.wasGroundedLastFrame)
		    {
			   m_rayCastHit = Physics2D.Raycast(start, direction,distance, platformMask);
		    }
		    else
		    {
			    m_rayCastHit= Physics2D.Raycast(start, direction, distance, (int)platformMask & ~(int)oneWayPlatformMask);
		    }

		    if ((bool)m_rayCastHit)
		    {
			    DrawRay(m_rayCastHit.point,m_rayCastHit.normal,Color.red);
			    if (i==0&&MoveHorizontalUpSlop(ref deltaMovement, Vector2.Angle(m_rayCastHit.normal, Vector2.up)))
			    {
				    m_colliderState.movingSlope = true;
					m_rayCastHitThisFrame.Add(m_rayCastHit);
					break;
			    }
			    deltaMovement.x = m_rayCastHit.point.x - start.x;
			    float num = MathF.Abs(deltaMovement.x);
			    if (flag)
			    {
				    deltaMovement.x -= m_KeepWidth;
				    m_colliderState.right = true;
			    }
			    else
			    {
				    deltaMovement.x += m_KeepWidth;
				    m_colliderState.left = true;
			    }
				m_rayCastHitThisFrame.Add(m_rayCastHit);
				if(num<m_KeepWidth+.001f)
					break;
		    }

	    }
    }



   private bool MoveHorizontalUpSlop(ref Vector2 deltaMovement, float angle)
    {

	    if (justJumped > 0)
	    {
		    return false;
	    }
	    if (Mathf.RoundToInt(angle) == 90)
	    {
		    return false;
	    }

	    float num = SlopspeedMultiple.Evaluate(angle);

	    deltaMovement.x *= num;
	    deltaMovement.y = Mathf.Abs(Mathf.Tan(angle * (MathF.PI / 180f)) * deltaMovement.x);
	    bool flag = deltaMovement.x > .0f;
	    Vector2 vector = flag ? m_rayCastOrigins.bottomRight : m_rayCastOrigins.bottomLeft;
		m_colliderState.movingSlope = true;
		m_colliderState.below = true;

		return true;

    }

    private void MoveVertical(ref Vector2 deltaMovement)
    {
	    bool flag = deltaMovement.y > 0;
	    Vector2 vector = flag ? m_rayCastOrigins.topLeft : m_rayCastOrigins.bottomLeft;
	    float distance = Mathf.Abs(deltaMovement.y) + m_KeepWidth;
		Vector2 direction=(flag ? Vector2.up : Vector2.down);
		vector.x += deltaMovement.x;
		LayerMask layerMask = platformMask;
		if (flag && !m_colliderState.wasGroundedLastFrame)
		{

			layerMask &= ~oneWayPlatformMask;
	    }
		for (int i = 0; i < totalHorizontalRay; i++)
		{

			Vector2 start = new Vector2(vector.x + i * (m_rayCastOrigins.bottomRight.x - m_rayCastOrigins.bottomLeft.x) / (totalHorizontalRay - 1), vector.y );

			m_rayCastHit = Physics2D.Raycast(start, direction, distance, layerMask);
			if (m_rayCastHit)
			{
				DrawRay(m_rayCastHit.point,Vector2.up,Color.red);
				deltaMovement.y =  m_rayCastHit.point.y-start.y;
				if (flag)
				{
					deltaMovement.y -= m_KeepWidth;
					m_colliderState.above = true;
				}
				else
				{
					deltaMovement.y += m_KeepWidth;
					m_colliderState.below = true;
				}
				distance = Mathf.Abs(deltaMovement.y);
				m_rayCastHitThisFrame.Add(m_rayCastHit);
				if (!flag && deltaMovement.y > 1E-05f)
				{
					m_colliderState.movingSlope = true;

				}
				if (distance < m_KeepWidth + 0.001f)
				{
					deltaMovement.y = 0;
					break;
				}
			}
		}
    }

    private void MoveHorizontalDownSlop(ref Vector2 deltaMovement)
    {
	    Vector2 start = m_rayCastOrigins.center;
		Vector2 direction=Vector2.down;
		float distance = (m_rayCastOrigins.bottomRight.x - start.x)*_slopeLimitTangent;
		m_rayCastHit = Physics2D.Raycast(start, direction, distance, platformMask);
		if (m_rayCastHit)
		{
			float angle = Vector2.Angle(m_rayCastHit.normal, Vector2.up);
			if (angle != 0f && Mathf.Sign(deltaMovement.x) == Mathf.Sign(m_rayCastHit.normal.x))
			{
				//Debug.Log(true);
				float num = SlopspeedMultiple.Evaluate(0 - angle);
				deltaMovement.x *= num;
				deltaMovement.y += m_rayCastHit.point.y - start.y - m_KeepWidth;
				m_colliderState.movingSlope = true;

			}
		}


    }

}
