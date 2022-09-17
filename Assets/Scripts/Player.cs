using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Horizontal Movement")]
    public float moveSpeed = 10;
    private Vector2 direction;
    public bool facingRight = true;

    [Header("Physics")]
    public float maxSpeed;
    public float linearDrag = 4f;
    public float gravity = 1;
    public float fallMultiplier = 5;
    public float maxVerticalSpeed;
    public LayerMask darkness;

    [Header("Jump Stuff")]
    public float jumpHeight;
    public float jumpDelay = 0.25f;
    private float jumpTimer;
    private float coyoteTimer;

    [Header("Player Components")]
    public Rigidbody2D rb;
    public SpriteRenderer spri;
    public LayerMask groundLayer;
    public GameObject player;

    [Header("Player Collision")]
    public bool onGround = false;
    public Vector3 colliderOffset;
    public float groundDistance;
    public bool dead;
    
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        bool wasOnGround = onGround;
        //Casts 2 raycasts down to see if the player is on the ground
        onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundDistance, groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundDistance, groundLayer);
        //Starts a coyote time after you leave ground
        if(wasOnGround && !onGround){
            coyoteTimer = Time.time + 0.15f;
        }

        //Gets directional input from the player
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    
        if(Input.GetButtonDown("Jump")){
            //Starts a jump delay so you don't have to time your jumps perfectly to string them
            jumpTimer = Time.time + jumpDelay;
        }

    }

    void FixedUpdate()
    {
        //Moves character and checks to see if it should change the gravity and drag
        if(!dead){
            moveCharacter(direction.x);
        }
        if(!Physics2D.Raycast(transform.position, Vector3.forward, 1, darkness)){
            modifyPhysics();
        }

        if(Mathf.Abs(rb.velocity.y) > maxVerticalSpeed){
            rb.velocity = new Vector2(rb.velocity.x , Mathf.Sign(rb.velocity.y) * maxVerticalSpeed);
        }

        //If the jumptimer is still active since you inputted and you still have your coyote time/on the ground you jump
        if(jumpTimer > Time.time && (onGround || coyoteTimer > Time.time)){
            Jump();
        }
    }

    void moveCharacter(float horizontal)
    {
        //Adds a force the direction inputted to move the player
        rb.AddForce(Vector2.right * horizontal * moveSpeed);

        //Checks to see which the direction the player is moving and which direction they are facing than flips if their not the same
        if((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight)){
            Flip();
        }
        if(Mathf.Abs(rb.velocity.x) > maxSpeed){
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }
    }

    void Jump()
    {
        //Makes the character jump
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
        //Resets coyote and jump timer to prevent exploits
        jumpTimer = 0;
        coyoteTimer = 0;
    }

    void Flip(){
        //Changes the facing right variable to its opposite than flips the player
        facingRight = !facingRight;
        player.transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
    }

    void modifyPhysics(){
        bool changingDirection = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);
        if(onGround){
            //when you turn around or stop inputting your drag will be increased
            if(Mathf.Abs(direction.x) < 0.4f || changingDirection){
                rb.drag = linearDrag * 5;
            }
            else{
                rb.drag = 0;
            }
            rb.gravityScale = 0;
        }
        else{
            //Applies gravity and drag for jump, which starts later if you hold down jump
            rb.gravityScale = gravity;
            rb.drag = linearDrag * 0.15f;
            if(rb.velocity.y < 0){
                rb.gravityScale = gravity * fallMultiplier;
            }
            // else if(rb.velocity.y > 0 && !Input.GetButton("Jump")){
            //     rb.gravityScale = gravity * (fallMultiplier/1.7f);
            // }
            if(rb.velocity.y > 0){
                rb.gravityScale = gravity * (fallMultiplier/1.7f);
            }
            
        }
    }

    private void OnDrawGizmos(){
        //Shows me where my ground check raycasts point
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundDistance);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundDistance);
    }
}
