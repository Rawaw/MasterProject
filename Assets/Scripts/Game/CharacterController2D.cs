using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
	[SerializeField] private Transform m_WallCheck;							// A position marking where to check for wall
	[SerializeField] private Collider2D m_CrouchDisableCollider;				// A collider that will be disabled when crouching

	const float k_GroundedRadius = .1f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .1f; // Radius of the overlap circle to determine if the player can stand up
	const float k_WallClingRadius = .1f; // Radius of the overlap circle to determine if touching wall
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	[Header("Ability Settings")]
	public bool canMultiJump = false;
	public bool canWallCling = false;
	public bool canDash = false;
	public bool canSwim = false;
	public int maxEnergy;
	public float clingForce;
	public float clingJumpForce;
	public float dashForce;
	public float dashDragForce;
	public float dashDragTime;
	public float dashCooldown;

	private int currentEnergy = 0;
	private Vector3 prevPosition;
	private bool falling = false;
	private bool jumping = false;
	private bool wallCling = true;
	private bool dashReady = true;
	private bool dashDrag = false;
	private bool inWater = false;
	private float dashDragTimer = 0f;
	private float dashCooldownTimer = 0f;
	private float defaultDrag = 0f;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
	}

	private void FixedUpdate()
	{
		if(m_Grounded || wallCling){
			currentEnergy = maxEnergy;
			dashReady = true;
		}

		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}

		CheckVerticalStatus();

		if(Physics2D.OverlapCircle(m_WallCheck.position, k_WallClingRadius, m_WhatIsGround) && !m_Grounded && canWallCling){
			if(falling)
				m_Rigidbody2D.drag = clingForce + defaultDrag;
			else
				m_Rigidbody2D.drag = defaultDrag;
			wallCling = true;
		}else{
			m_Rigidbody2D.drag = defaultDrag;
			wallCling = false;
		}

		if(dashDrag){
			m_Rigidbody2D.velocity = new Vector3(m_Rigidbody2D.velocity.x,0f);
			dashDragTimer -= Time.deltaTime;
			if(dashDragTimer < 0f){
				dashDrag = false;
			}
		}

		if(dashCooldownTimer >= 0f){
			dashCooldownTimer -= Time.deltaTime;
		}
	}


	public void Move(float move, bool crouch)
	{
		// If crouching, check to see if the character can stand up
		if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				crouch = true;
			}
		}

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

			// If crouching
			if (crouch)
			{
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= m_CrouchSpeed;

				// Disable one of the colliders when crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
			} else
			{
				// Enable the collider when not crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;

				if (m_wasCrouching)
				{
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}

			if(inWater)
				move = move * 0.8f;
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		
	}

	public void Jump(bool jump){
		if(jump){
			if (m_Grounded || inWater)
			{
				m_Grounded = false;
				m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce);
			}else if(wallCling){
				if(m_FacingRight){
					m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x - clingJumpForce, m_JumpForce);
				}else{
					m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x + clingJumpForce, m_JumpForce);
				}
			}else if(currentEnergy > 0 && canMultiJump){
				currentEnergy--;
				m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce);
			}
		}
	}

	public void Dash(bool dash){
		if(dash && canDash && dashReady && dashCooldownTimer <= 0f){
			if(m_FacingRight ^ wallCling){
				m_Rigidbody2D.velocity = new Vector2(dashForce, 4f);
			}else{
				m_Rigidbody2D.velocity = new Vector2(-dashForce, 4f);
			}
			if(wallCling)
				Flip();
			dashReady = false;
			dashDrag = true;
			dashDragTimer = dashDragTime;
			dashCooldownTimer = dashCooldown;
		}
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	private void CheckVerticalStatus(){

		if(m_Rigidbody2D.velocity.y < -0.01f){
            falling = true;
			jumping = false;
        }else if(m_Rigidbody2D.velocity.y > 0.01f){
            jumping = true;
            falling = false;
        }else {
            falling = false;
            jumping = false;
        }
        prevPosition = transform.position;

	}

	public bool DashReady(){
		return (dashReady && canDash && dashCooldownTimer <= 0f);
	}
	public bool IsFalling(){
		return falling;
	}
	public bool IsJumping(){
		return jumping;
	}
	public bool IsClinging(){
		return (wallCling && !m_Grounded);
	}
	public bool CanDash(){
		return canDash;
	}
	public bool CanSwim(){
		return canSwim;
	}
	public bool CanWallCling(){
		return canWallCling;
	}
	public bool CanMultiJump(){
		return canMultiJump;
	}

	public void SetInWater(bool inWater){
		this.inWater = inWater;
		if(inWater){
			defaultDrag = 5f;
			m_Rigidbody2D.gravityScale = 1f;
		}
		else{
			defaultDrag = 0f;
			m_Rigidbody2D.gravityScale = 3f;
		}
	}

	public void SetSwim(bool swim){
		canSwim = swim;
	}
	public void SetCling(bool cling){
		canWallCling = cling;
	}
	public void SetDash(bool dash){
		canDash = dash;
	}
	public void SetMultiJump(bool multi){
		canMultiJump = multi;
	}

}