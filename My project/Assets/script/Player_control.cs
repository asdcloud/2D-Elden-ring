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
    public bool animationlock;
    public Animator animator;
    public float jumpdelay;


    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        moveInput = Input.GetAxisRaw("Horizontal");//偵測玩家鍵盤輸入
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
        isJumping = Input.GetButton("Player_Jump");
        isCrouching = Input.GetButton("Player_crouch");
        Move(isGrounded, isCrouching, isJumping, moveInput);
    }

    void Move(bool isGrounded, bool isCrouching, bool isJumping, float moveInput) {

        //控制左右面向以及橫向移動速度，在空中時也能達成此指令
        if (moveInput != 0) {
            transform.localScale = new Vector2(moveInput, 1);//根據左右移動的方向來改變面向的方向
        }
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);//左右移動速度

        //限制在地板上才能完成的動作
        if (isGrounded) {//在地板上時
            animationcontrol (isCrouching, isJumping, moveInput, animationlock);//使用函式控制動畫播放
            if(isJumping && (isCrouching == false)) {
                StartCoroutine(Jump());//出現跳躍動作，且將動畫鎖定(在空中時不可變更動畫)
            }
        }
    }

    private IEnumerator Jump () {
        animationlock = true;//將動畫鎖定
        yield return new WaitForSeconds(jumpdelay);//在跳躍前的起跳動畫所需的時間
        rb.velocity = Vector2.up * jumpForce;//實際角色跳躍
        yield return new WaitForSeconds(0.44f);//在跳躍期間的剩餘動畫時間
        animationlock = false;//解除動畫鎖定
    }

    void animationcontrol (bool isCrouching, bool isJumping, float moveInput, bool animationlock) {
        if (animationlock == false) {//如果沒有將動畫鎖定，則透過改變變數的方式更改動畫
            animator.SetBool("IsCrouching", isCrouching);
            animator.SetBool("IsJumping", isJumping);
            animator.SetFloat("Horizontal_Speed", Mathf.Abs(moveInput));
        }
    }
}