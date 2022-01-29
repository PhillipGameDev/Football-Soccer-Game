using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 7;
    [SerializeField] private float crouchSpeed = 5;
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
    private bool jumpInput;
    private bool crouchInput;
    private bool jumping;
    private int jumpCount;
    private bool grounded;
    private bool crouching;

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
        Jump();
    }

    // Update is called once per frame
    void Update()
    {
        IsGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, groundLayer);

        float axisHorizontal = Input.GetAxisRaw("Horizontal");
        if(axisHorizontal != 0){
            this.transform.localScale = new Vector3(axisHorizontal,1,1);
        }
        Move(new Vector2(axisHorizontal, 0));

        if (Input.GetButtonDown("Jump") && ((grounded  || jumpCount <= extraJumpNumber) && !Crouching))
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

        if (Input.GetButtonDown("Fire2") && !isAttacking){
            StartCoroutine(Attack());
        }

        crouchInput = Input.GetButton("Crouch");

        Crouch();
    }

    private void Crouch()
    {
        if (crouchInput && IsGrounded)
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
        transform.Translate(direction * (Crouching ? crouchSpeed : speed) * Time.deltaTime);
    }

    private IEnumerator Attack(){
        isAttacking = true;
        attack.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
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
    }
}
