using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_control : MonoBehaviour {

    private Rigidbody2D rb;
    public float speed;
    public float jumpForce;

    private float moveInput;
    private bool isGrounded;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;

    public Animator animator;


    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate() {
        moveInput = Input.GetAxisRaw("Horizontal");

        animator.SetFloat("Horizontal_Speed", Mathf.Abs(moveInput));

        if (moveInput != 0) {
            transform.localScale = new Vector2(moveInput, 1);
        }
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }

    void Update() {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        //if(isGrounded == true && Input.GetButtonDown("Player_Jump")) {
        //    rb.velocity = Vector2.up * jumpForce;
        //}

        if(isGrounded) {
            animator.SetBool("IsCrouching", Input.GetButton("Player_crouch"));
            
            if(Input.GetButtonDown("Player_Jump") && (Input.GetButton("Player_crouch") == false)) {
                rb.velocity = Vector2.up * jumpForce;
            }
        }
    }
}