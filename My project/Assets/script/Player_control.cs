using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_control : MonoBehaviour {

    private Rigidbody2D rb;
    public float speed;
    public float jumpForce;

    private float moveInput;
    private bool isGrounded;
    private bool isJumping;
    private bool isCrouching;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;

    public Animator animator;


    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        moveInput = Input.GetAxisRaw("Horizontal");
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
        isJumping = Input.GetButton("Player_Jump");
        isCrouching = Input.GetButton("Player_crouch");
        Move(isGrounded, isCrouching, isJumping, moveInput);

        /*moveInput = Input.GetAxisRaw("Horizontal");

        animator.SetFloat("Horizontal_Speed", Mathf.Abs(moveInput));

        if (moveInput != 0) {
            transform.localScale = new Vector2(moveInput, 1);
        }
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        */




    }

    void Update() {
        /*
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
        if(isGrounded) {
            animator.SetBool("IsCrouching", Input.GetButton("Player_crouch"));
            
            if(Input.GetButtonDown("Player_Jump") && (Input.GetButton("Player_crouch") == false)) {
                rb.velocity = Vector2.up * jumpForce;
            }
        }
        */
    }
    void Move(bool isGrounded, bool isCrouching, bool isJumping, float moveInput) {

        //控制左右面向以及橫向移動速度(優先級1)，在空中時也能達成此指令
        if (moveInput != 0) {
            transform.localScale = new Vector2(moveInput, 1);
        }
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        //限制在地板上才能完成的動作
        if (isGrounded) {//在地板上時
            animator.SetBool("IsCrouching", isCrouching);
            animator.SetBool("IsJumping", isJumping);
            animator.SetFloat("Horizontal_Speed", Mathf.Abs(moveInput));
            if(isJumping && (isCrouching == false)) {
                rb.velocity = Vector2.up * jumpForce;
                print("jump");
            }
        }
    }
}