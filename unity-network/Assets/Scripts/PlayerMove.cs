using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;


public class PlayerMove : NetworkBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private int speed;
    private Vector3 moveDirection;

    private bool isDashing = false;
    private bool isDashAvailable = true;
    IEnumerator dashCoroutine;

    [SerializeField] private float jumpVelocity;
    [SerializeField] private float jumpRatio;
    [SerializeField] private float jumpPeakTiming;
    [SerializeField] private bool canJump = true;

    float horizontalDirection = 1;
    float verticalDirection = 1;
    [SerializeField] private float normalGravity;

    float horizontal;
    float vertical;

    private float moveX = 0f;
    private float moveY = 0f;

    #region Server

    [Command]
    private void CmdJump()
    {
        Debug.Log($"jump from {NetworkClient.connection.connectionId}");
        canJump = false;
        jumpVelocity = 30f;
        Invoke(nameof(ResetJumpVelocity), jumpPeakTiming);
        rb.velocity = new Vector2(0f, jumpVelocity);
    }

    [Command]
    private void CmdMove(float horizontal, int speed, bool isDashing, float jumpVelocity)
    {
        rb.velocity = new Vector2(horizontal * speed, isDashing ? 0 : jumpVelocity);
    }

    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        normalGravity = rb.gravityScale;
    }

    private void OnCollisionStay2D(Collision2D other) {
        if (other.collider.CompareTag("Wall")) {
            canJump = true;
            Debug.Log("collision");
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (other.collider.CompareTag("Wall")) {
            canJump = false;
            Debug.Log("collision");
        }
    }

    void Update()
    {
        if (!isLocalPlayer || !hasAuthority)
            return;

        if (horizontal != 0)
            horizontalDirection = horizontal;
        if (vertical != 0)
            verticalDirection = vertical;
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && canJump) {
            CmdJump();
            //canJump = false;
            //jumpVelocity = 30f;
            //Invoke("ResetJumpVelocity", jumpPeakTiming);
            //Debug.Log("jump");
            //rb.velocity = new Vector2(0f, jumpVelocity);
        }

        if (Input.GetMouseButtonDown(1) && isDashAvailable) {
            if (dashCoroutine != null)
                StopCoroutine(dashCoroutine);
            dashCoroutine = Dash(0.1f, 2);
            StartCoroutine(dashCoroutine);
        }

        // vertical = Input.GetAxis("Jump");
        // ProcessInputs();
    }

    // target rpc for slow jump

    void ResetJumpVelocity() {
        StartCoroutine(SlowJump());
    }

    IEnumerator SlowJump() {
        while (jumpVelocity >= 0) {
            jumpVelocity -= jumpRatio * Time.deltaTime;
            yield return null;
        }
        jumpVelocity = 0f;
        yield break;
    }

    IEnumerator Dash(float dashDuration, float dashCooldown) {
        isDashing = true;
        isDashAvailable = false;
        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        rb.gravityScale = normalGravity;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(dashCooldown);
        isDashAvailable = true;
    }

    void FixedUpdate() {
        if (!isLocalPlayer || !hasAuthority)
            return;

        //rb.velocity = new Vector2(horizontal * speed, isDashing ? 0 : jumpVelocity);
        CmdMove(horizontal, speed, isDashing, jumpVelocity);
        //if (isDashing == true) {
        //    rb.AddForce(new Vector2(horizontalDirection * 25, 0), ForceMode2D.Impulse);
        //}
    }

}
