using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D), typeof(Animator))]
public class SoyBoyController : MonoBehaviour
{
    public float speed = 14f;
    public float accel = 6f;
    private Vector2 input;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        if (input.x > 0f)
        {
            sr.flipX = false;
        }
        else if (input.x < 0f)
        {
            sr.flipX=true;
        }

    }

    void FixedUpdate()
    {
        var acceleration = accel;
        var xVelocity = 0f;
        if (input.x == 0f)
        {
            xVelocity = 0f;
        }
        else
        {
            xVelocity = rb.velocity.x;
        }

        rb.AddForce(new Vector2(((input.x * speed) - rb.velocity.x) * acceleration, 0)); //-rb.velocity.x controls speed of character
        rb.velocity = new Vector2(xVelocity, rb.velocity.y); //Gives a harder stop when you stop moving
    }
}
