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
    [SerializeField] private float normalGravity;

    float horizontal;
    float vertical;

    private float moveX = 0f;
    private float moveY = 0f;

    #region Server

    [Command]
    private void CmdMove(float horizontal, int speed, bool isDashing, float jumpVelocity)
    {
        rb.velocity = new Vector2(horizontal * speed, isDashing ? 0 : jumpVelocity);
    }

    [Command]
    private void CmdStartDash(float direction)
    {
        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(direction * 25, 0), ForceMode2D.Impulse);
    }

    [Command]
    private void CmdStopDash()
    {
        rb.gravityScale = normalGravity;
        rb.velocity = Vector2.zero;
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
        Debug.Log("Horizontal Direction: " + horizontalDirection);
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && canJump) {
            StartCoroutine(Jump());
        }

        if (Input.GetMouseButtonDown(1) && isDashAvailable) {
            // if (dashCoroutine != null)
            //     StopCoroutine(dashCoroutine);
            // dashCoroutine = Dash(0.1f, 2);
            StartCoroutine(Dash(0.2f, 2));
            // CmdDashCoroutine();
        }
    }

    IEnumerator SlowJump() {
        while (jumpVelocity >= 0) {
            jumpVelocity -= jumpRatio * Time.deltaTime;
            yield return null;
        }
        jumpVelocity = 0f;
        yield break;
    }

    IEnumerator Jump() {
        canJump = false;
        jumpVelocity = 30f;
        rb.velocity = new Vector2(0f, jumpVelocity);
        yield return new WaitForSeconds(jumpPeakTiming);
        Debug.Log("Jump start fall down");
        StartCoroutine(SlowJump());
    }

    IEnumerator Dash(float dashDuration, float dashCooldown) {
        isDashing = true;
        isDashAvailable = false;
        Debug.Log("Start dashing");
        yield return new WaitForSeconds(dashDuration);
        CmdStopDash();
        isDashing = false;
        Debug.Log("Stop Dashing");
        yield return new WaitForSeconds(dashCooldown);
        isDashAvailable = true;
        Debug.Log("Reset Dash");
    }

    void FixedUpdate() {
        if (!isLocalPlayer || !hasAuthority)
            return;

        CmdMove(horizontal, speed, isDashing, jumpVelocity);
        if (isDashing == true) {
            CmdStartDash(horizontalDirection);
        }
    }

}
