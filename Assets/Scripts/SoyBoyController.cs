using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D), typeof(Animator))]
public class SoyBoyController : MonoBehaviour
{
    public float jump = 14f;
    public float airAccel = 3f;
    public float jumpDurationThreshold = 0.25f;
    private float jumpDuration;
    public float speed = 14f;
    public float accel = 6f;
    private Vector2 input;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Animator animator;

    public bool isJumping;
    public float jumpSpeed = 8f;
    private float rayCastLengthCheck = 0.005f;
    private float width;
    private float height;

    public AudioClip runClip;
    public AudioClip jumpClip;
    public AudioClip slideClip;
    private AudioSource audioSource;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        width = GetComponent<Collider2D>().bounds.extents.x + 0.1f;
        height = GetComponent<Collider2D>().bounds.extents.y + 0.2f;
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    public bool PlayerIsOnGround()
    {
        //The following checks directly below, slightly to the right, and slightly to the left of the center of the character using a length equal to rayCastLengthCheck
        bool groundCheck1 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - height), -Vector2.up, rayCastLengthCheck);
        bool groundCheck2 = Physics2D.Raycast(new Vector2(transform.position.x + (width - 0.2f), transform.position.y - height), -Vector2.up, rayCastLengthCheck);
        bool groundCheck3 = Physics2D.Raycast(new Vector2(transform.position.x - (width - 0.2f), transform.position.y - height), -Vector2.up, rayCastLengthCheck);
        if (groundCheck1 || groundCheck2 || groundCheck3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsWallToLeftOrRight()
    {
        bool wallOnLeft = Physics2D.Raycast(new Vector2(transform.position.x - width, transform.position.y), -Vector2.right, rayCastLengthCheck);
        bool wallOnRight = Physics2D.Raycast(new Vector2(transform.position.x + width, transform.position.y), Vector2.right, rayCastLengthCheck);

        if (wallOnLeft || wallOnRight)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool PlayerIsTouchingGroundOrWall()
    {
        if (PlayerIsOnGround() || IsWallToLeftOrRight())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetWallDirection()
    {
        bool isWallLeft = Physics2D.Raycast(new Vector2(transform.position.x - width, transform.position.y), -Vector2.right, rayCastLengthCheck);
        bool isWallRight = Physics2D.Raycast(new Vector2(transform.position.x + width, transform.position.y), Vector2.right, rayCastLengthCheck);
        if (isWallLeft)
        {
            return -1;
        }
        else if (isWallRight)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    void PlayAudioClip(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            if (!audioSource.isPlaying) audioSource.PlayOneShot(clip);
        }
    }

    // Update is called once per frame
    void Update()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Jump");

        animator.SetFloat("Speed", Mathf.Abs(input.x));

        if (input.x > 0f)
        {
            sr.flipX = false;
        }
        else if (input.x < 0f)
        {
            sr.flipX=true;
        }

        if (input.y >= 1f) //When up is pressed
        {
            jumpDuration += Time.deltaTime; //Times the duration of the jump
            animator.SetBool("IsJumping", true);
        }
        else //As soon as the user let goes of jump key
        {
            isJumping = false; //Set isJumping to false 
            animator.SetBool("IsJumping", false);
            jumpDuration = 0f;  
        }

        if (PlayerIsOnGround() && !isJumping)
        {
            if (input.y > 0f)
            {
                isJumping = true;
                PlayAudioClip(jumpClip);
            }

            animator.SetBool("IsOnWall", false);
            if (input.x < 0f || input.x > 0f)
            {
                PlayAudioClip(runClip);
            }
        }

        if (jumpDuration > jumpDurationThreshold) input.y = 0f;
    }

    void FixedUpdate()
    {
        var acceleration = 0f;
        if(PlayerIsOnGround())
        {
            acceleration = accel;
        }
        else
        {
            acceleration = airAccel;
        }
        var xVelocity = 0f;
        if (PlayerIsOnGround() && input.x == 0)
        {
            xVelocity = 0f;
        }
        else
        {
            xVelocity = rb.velocity.x;
        }

        var yVelocity = 0f;
        if (PlayerIsTouchingGroundOrWall() && input.y == 1)
        {
            yVelocity = jump;
        }
        else
        {
            yVelocity = rb.velocity.y;
        }

        rb.AddForce(new Vector2(((input.x * speed) - rb.velocity.x) * acceleration, 0)); //-rb.velocity.x controls speed of character
        rb.velocity = new Vector2(xVelocity, yVelocity); //Gives a harder stop when you stop moving

        if (IsWallToLeftOrRight() && !PlayerIsOnGround() && input.y == 1)
        {
            rb.velocity = new Vector2(-GetWallDirection() * speed * 0.75f, rb.velocity.y);
            animator.SetBool("IsOnWall", false);
            animator.SetBool("IsJumping", true);
            PlayAudioClip(jumpClip);
        }
        else if (!IsWallToLeftOrRight())
        {
            animator.SetBool("IsOnWall", false);
            animator.SetBool("IsJumping", true);
        }
        if (IsWallToLeftOrRight() && !PlayerIsOnGround())
        {
            animator.SetBool("IsOnWall", true);
            PlayAudioClip(slideClip);
        }

        if (isJumping && jumpDuration < jumpDurationThreshold)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }
}
