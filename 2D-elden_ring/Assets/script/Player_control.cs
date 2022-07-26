using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_control : MonoBehaviour {

    public BoxCollider2D m_col;
    private Rigidbody2D rb;
    public float speed;
    public float jumpForce;

    private float moveInput;
    private bool isGrounded;
    private bool isJumping;
    private bool isCrouching;
    private bool isdashing;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;
    public Animator animator;

    public float jumpdelay;
    public float jumptime;
    public float jump_cooldown;
    public bool canjump = true;

    public float first_dashspeed;
    public float second_dashspeed;
    public float final_dashspeed;
    public bool candash = true;
    public float dash_cooldown;

    private bool isAttacking;
    public Transform attackpoint;
    public float attack_range = 0.5f;
    public LayerMask enemy_layer;


    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        moveInput = Input.GetAxisRaw("Horizontal");//偵測玩家鍵盤輸入
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
        isJumping = Input.GetButton("Player_Jump");
        isCrouching = Input.GetButton("Player_crouch");
        isdashing = Input.GetButton("Player_dash");
        isAttacking = Input.GetButtonDown("Player_attack");

        Move();
    }

    void Move() {
        //控制左右面向以及橫向移動速度，在空中時也能達成此指令
        if (moveInput != 0) {
            transform.localScale = new Vector2(moveInput, 1);//根據左右移動的方向來改變面向的方向
        }


        run();

        //限制在地板上才能完成的動作
        if (isGrounded) {//在地板上時
            StartCoroutine(crouch());
            if(isJumping && (isCrouching == false)) {
                StartCoroutine(Jump());//出現跳躍動作，且將動畫鎖定(在空中時不可變更動畫)
            } else if (isdashing && (isCrouching == false)) {
                StartCoroutine(dash());
            } else if (isAttacking) {
                StartCoroutine(attack());
            }
        }
    }

    private IEnumerator Jump () {
        if (canjump) {
            canjump = false;
            animator.SetBool("IsJumping", true);
            yield return new WaitForSeconds(0.001f);
            animator.SetBool("animationlock", true);
            yield return new WaitForSeconds(jumpdelay);//在跳躍前的起跳動畫所需的時間
            rb.velocity = Vector2.up * jumpForce;//實際角色跳躍
            yield return new WaitForSeconds(jumptime);//在跳躍期間的剩餘動畫時間
            animator.SetBool("animationlock", false);
            animator.SetBool("IsJumping", false);
            yield return new WaitForSeconds(jump_cooldown);
            canjump = true;
        }
    }

    private IEnumerator dash() {
        if (candash) {
            candash = false;
            animator.SetBool("IsDashing", true);
            yield return new WaitForSeconds(0.001f);
            animator.SetBool("animationlock", true);
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(new Vector2(moveInput * first_dashspeed, 0f));
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(new Vector2(moveInput * second_dashspeed, 0f));
            yield return new WaitForSeconds(0.1f);
            rb.AddForce(new Vector2(moveInput * final_dashspeed, 0f));
            yield return new WaitForSeconds(0.08f);
            animator.SetBool("animationlock", false);
            animator.SetBool("IsDashing", false);
            yield return new WaitForSeconds(dash_cooldown);
            candash = true;
        }
    }

    private IEnumerator crouch() {
        animator.SetBool("IsCrouching", isCrouching);
        yield return new WaitForSeconds(0.001f);
        animator.SetBool("animationlock", isCrouching);
    }

    private IEnumerator attack (){
        animator.SetBool("IsAttacking", true);
        yield return new WaitForSeconds(0.001f);
        animator.SetBool("animationlock", true);
        yield return new WaitForSeconds(0.2f);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackpoint.position, attack_range, enemy_layer);
        foreach(Collider2D enemy in hitEnemies) {
            enemy.GetComponent<Enemy>().take_damage(10);
        }
        animator.SetBool("animationlock", false);
        animator.SetBool("IsAttacking", false);
    }


    void run() {
        animator.SetFloat("Horizontal_Speed", Mathf.Abs(moveInput));
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);//左右移動速度
    }

    void OnDrawGizmosSelected() {
        if(attackpoint == null) {
            return;
        }

        Gizmos.DrawWireSphere(attackpoint.position, attack_range);
    }
}