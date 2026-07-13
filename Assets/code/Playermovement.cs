using System.Runtime.InteropServices.WindowsRuntime;
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
    public float jumpforce;
    private float jumptime;
    private bool isjump;
    private float jump_pls = 0.3f;

    //바닥 감지
    private Vector2 boxsize;
    public LayerMask ground;

    //대쉬
    private float dashspeed;

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
    }

    private void Update()
    {
        isGround = Physics2D.BoxCast(transform.position, boxsize, 0f, Vector2.down, 0.1f, ground);
    }

    private void FixedUpdate()
    {
        //좌우 이동
        rb.linearVelocity = new Vector2(speed * control.MoveInput.x, rb.linearVelocity.y);

        if (control.ConsumeJumpPressed() && isGround)
        {
            isjump = true;
            jumptime = 0f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpforce);
        }

        if (isjump)
        {
            if (control.JumpHeld && jumptime < jump_pls)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpforce);
                jumptime += Time.deltaTime;
            }
            else
            {
                isjump = false;
            }
        }
    }
}