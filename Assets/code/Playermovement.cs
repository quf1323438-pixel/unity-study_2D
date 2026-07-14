using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerControl))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerControl control;

    //컴포넌트
    private Rigidbody2D rb;
    private BoxCollider2D col;

    //좌우 이동
    public float speed;

    //점프
    private bool isGround;
    private bool isjump;

    public float maxJumpHeight = 3f;   // 끝까지 누르면 정확히 이 높이
    public float minJumpHeight = 1f;   // 톡 치면 최소 이 높이 보장
    public float timeToApex   = 0.35f; // 정점까지 걸리는 시간 (작을수록 빠릿함)
    private float gravity;
    private float jumpVelocity;
    private float minJumpVelocity;

    
    //바닥 감지
    private Vector2 boxsize;
    public LayerMask ground;

    //대쉬
    public float dashSpeed    = 15f;
    private float dashtime; //유지시간
    private float cooldownTimer; //쿨타임 
    private bool  isDashing;
    private float facing = 1f; 
    public float dashDuration = 0.15f;  // 대쉬 지속 시간
    public float dashCooldown = 0.5f;   // 다음 대쉬까지 대기


    //공격

    private void Awake()
    {
        control = GetComponent<PlayerControl>();
        rb      = GetComponent<Rigidbody2D>();
        col     = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        boxsize = col.bounds.size;

        gravity      = 2f * maxJumpHeight / (timeToApex * timeToApex);
        jumpVelocity = gravity * timeToApex;
        minJumpVelocity = Mathf.Sqrt(2f * gravity * minJumpHeight);
        rb.gravityScale = gravity / -Physics2D.gravity.y;
    }

    private void Update()
    {
        isGround = Physics2D.BoxCast(transform.position, boxsize, 0f, Vector2.down, 0.1f, ground);

        if(control.MoveInput.x != 0) facing = control.MoveInput.x;
    }

    private void FixedUpdate()
    {
        //대쉬
        if (cooldownTimer >= 0f) cooldownTimer -= Time.deltaTime; //쿨타임

        if (control.ConsumeDashPressed() && cooldownTimer < 0f && !isDashing)
        {
            isDashing = true;
            dashtime = dashDuration;
            rb.gravityScale = 0f;     
        }

        //대쉬 중: 다른 이동 로직을 전부 건너뛰고 리턴
        if (isDashing)
        {
            rb.linearVelocity = new Vector2(facing * dashSpeed, 0f);
            dashtime -= Time.fixedDeltaTime;
            if (dashtime <= 0f)
            {
                isDashing = false;
                cooldownTimer = dashCooldown;
                rb.gravityScale = gravity / -Physics2D.gravity.y;  // 중력 복구
            }
            return;
        }

        //좌우 이동
        rb.linearVelocity = new Vector2(speed * control.MoveInput.x, rb.linearVelocity.y);

        if (control.ConsumeJumpPressed() && isGround)
        {
            isjump = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVelocity);
        }

        if (isjump)
        {
            if (!control.JumpHeld && rb.linearVelocity.y > minJumpVelocity)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, minJumpVelocity);
                isjump = false;
            }
                
        }
    }
}