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

    [Header("Colliders")]
    [SerializeField] private Collider2D collBody;
    [SerializeField] private Collider2D collCrouching;
    [SerializeField] private Collider2D attack;
    [SerializeField] float attackTime = 0.2f;
    private bool isAttacking = false;

    private Rigidbody2D rb2d;
    private Animator anim;
    private float axisHorizontal;
    private bool jumpInput;
    private bool crouchInput;
    private bool jumping;
    private int jumpCount;
    private bool grounded;
    private bool crouching;
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

    public bool CanCrounch { get => CurrentDualityState == DualityState.DualityTwo; }
    public bool CanDoubleJump { get => CurrentDualityState == DualityState.DualityTwo; }
    public bool CanPush { get => CurrentDualityState == DualityState.DualityOne; }
    public bool CanCrush { get => CurrentDualityState == DualityState.DualityOne; }

    public DualityState CurrentDualityState { get; set; }

    public static event UnityAction<int> OnKeyCollected;
    public static event UnityAction<int> OnKeyDelivered;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move(new Vector2(axisHorizontal, 0));
        Jump();
    }

    // Update is called once per frame
    void Update()
    {
        IsGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, groundLayer);

        axisHorizontal = Input.GetAxisRaw("Horizontal");
        
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

        if (Input.GetButtonDown("Fire2") && CanCrush && !isAttacking)
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
            scale.x = axisHorizontal;
            this.transform.localScale = scale;
        }
    }

    private void Crouch()
    { 
        if (crouchInput && IsGrounded && CanCrounch)
        {
            Crouching = true;
        } 
        else 
        {
            Crouching = false;
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
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Key"))
        {
            // TODO: Destroy key
            OnKeyCollected?.Invoke(1);
            Debug.Log("Key");
            Destroy(other.gameObject);
        }
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
        } 
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Movable"))
        {
            movableObject.velocity = Vector2.zero;
            movableObject = null;
        }
    }
}
