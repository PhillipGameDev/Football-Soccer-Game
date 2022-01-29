using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, IDuality
{
    [SerializeField] private float speed = 7;
    [SerializeField] private float crouchSpeed = 5;
    [SerializeField] private float pushSpeed = 4;
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private int extraJumpNumber = 1;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 groundCheckSize;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform roofCheck;
    [SerializeField] private Vector2 roofCheckSize;
    [SerializeField] private LayerMask roofLayer;
    
    [Header("Colors")]
    [SerializeField] private Color colorDualityOne = Color.black;
    [SerializeField] private Color colorDualityTwo = Color.white;

    [Header("Colliders")]
    [SerializeField] private Collider2D collBody;
    [SerializeField] private Collider2D collCrouching;
    [SerializeField] private Collider2D attack;
    [SerializeField] float attackTime = 0.2f;
    private bool isAttacking = false;

    private Rigidbody2D rb2d;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private float axisHorizontal;
    private bool jumpInput;
    private bool crouchInput;
    private bool jumping;
    private int jumpCount;
    private bool grounded;
    private bool crouching;
    private bool roofed;
    private Rigidbody2D movableObject;

    public bool IsGrounded 
    { 
        get => grounded; 
        private set
        {
            if (value != grounded)
            {
                grounded = value;

                if (grounded)
                    jumpCount = 0;
            }
        } 
    }

    public bool Crouching 
    { 
        get => crouching;
        private set 
        {
            if (value != crouching)
            {
                crouching = value;

                collBody.gameObject.SetActive(!crouching);
                collCrouching.gameObject.SetActive(crouching && grounded);
            }
        }
    }

    public bool IsRoofed 
    { 
        get => roofed; 
        private set
        {
            if (value != roofed)
            {
                roofed = value;
            }
        } 
    }

    public bool CanCrounch { get => CurrentDualityState == DualityState.DualityTwo; }
    public bool CanDoubleJump { get => CurrentDualityState == DualityState.DualityTwo; }
    public bool CanPush { get => CurrentDualityState == DualityState.DualityOne; }
    public bool CanCrush { get => CurrentDualityState == DualityState.DualityOne; }

    private DualityState currentDualityState;
    public DualityState CurrentDualityState 
    { 
        get => currentDualityState; 
        set
        {
            if (currentDualityState != value)
            {
                currentDualityState = value;
                anim.SetTrigger("dualityChanged");
                spriteRenderer.color = currentDualityState == DualityState.DualityOne ? colorDualityOne : colorDualityTwo; 
            }
        } 
    }

    public static event UnityAction<int> OnKeyDelivered;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        Move(new Vector2(axisHorizontal, 0));
        Jump();
        anim.SetBool("moving", rb2d.velocity.x != 0);
    }

    // Update is called once per frame
    void Update()
    {
        IsGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, groundLayer);
        IsRoofed = Physics2D.OverlapBox(roofCheck.position, roofCheckSize, 0, roofLayer);

        axisHorizontal = Input.GetAxisRaw("Horizontal");
        
        anim.SetBool("isGrounded", IsGrounded);
        Flip();
        

        if (Input.GetButtonDown("Jump") && ((IsGrounded || (CanDoubleJump && jumpCount <= extraJumpNumber)) && !Crouching))
        {
            jumpInput = true;
            jumpCount++;
        }

        if (Input.GetButtonUp("Jump") && jumping)
        {
            jumping = false;

            if (rb2d.velocity.y > 0)
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        }

        if (Input.GetButtonDown("Attack") && CanCrush && !isAttacking && IsGrounded)
        {
            StartCoroutine(Attack());
        }

        crouchInput = Input.GetButton("Crouch");

        Crouch();
    }

    private void Flip()
    {
        if(axisHorizontal != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(axisHorizontal);
            this.transform.localScale = scale;
        }
    }

    private void Crouch()
    { 
        if (crouchInput)
        {
            if (IsGrounded && CanCrounch)
                Crouching = true;
        } 
        else 
        {
            Crouching = IsRoofed;
        }

        anim.SetBool("crouching", Crouching);
    }

    private void Jump()
    {
        if (jumpInput)
        {   
            jumping = true;
            jumpInput = false;
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            SoundManager.Instance.Play(SoundManager.Instance.audioJump);
            anim.SetTrigger("jump");
        }
    }

    private void Move(Vector2 direction) 
    {
        float finalSpeed = speed;

        if (movableObject != null && CanPush)
        {
            finalSpeed = pushSpeed;
        } 
        else if (Crouching)
        {
            finalSpeed = crouchSpeed;
        }

        Vector2 velocity = rb2d.velocity;
        velocity.x = direction.x * finalSpeed /* Time.deltaTime*/;
        rb2d.velocity = velocity;
        
        // transform.Translate(movement);

        if (movableObject != null && CanPush && IsGrounded) 
        {
            velocity.y = movableObject.velocity.y;
            movableObject.velocity = velocity;
        }
    }

    private IEnumerator Attack(){
        anim.SetTrigger("attack");
        isAttacking = true;
        attack.gameObject.SetActive(true);
        yield return new WaitForSeconds(attackTime);
        isAttacking = false;
        attack.gameObject.SetActive(false);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(roofCheck.position, roofCheckSize);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        
        if (other.CompareTag("Totem"))
        {
            OnKeyDelivered?.Invoke(1);
            Debug.Log("Totem");
        }
        if(other.CompareTag("Destructable")){
            other.gameObject.SetActive(false);
        }

        if (other.CompareTag("Movable"))
        {
            movableObject = other.gameObject.GetComponent<Rigidbody2D>();
            if(!CanPush)
            {
                movableObject.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
            }
        } 
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Movable"))
        {
            movableObject.velocity = Vector2.zero;
            if(!CanPush)
            {
                movableObject.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            movableObject = null;
        }
    }
}
