using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_control : MonoBehaviour {

    private Rigidbody2D rb;
    public float speed;
    public float jumpForce;
    private float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping;

    private float moveInput;
    private bool isGrounded;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;


    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate() {
        moveInput = Input.GetAxisRaw("Horizontal");
        if (moveInput != 0) {
            transform.localScale = new Vector2(moveInput, 1);
        }
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }

    void Update() {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        if(isGrounded == true && Input.GetButtonDown("Jump")) {
            isJumping = true;
            jumpTimeCounter = jumpTime; 
            rb.velocity = Vector2.up * jumpForce;
        }
        if(Input.GetButton("Jump") && isJumping == true) {
            if(jumpTimeCounter > 0) {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            } else {
                isJumping = false;
            }
        }
        if(Input.GetButtonUp("Jump")) {
            isJumping = false;
        }
    }
}