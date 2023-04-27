using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBeforeContact : MonoBehaviour
{
    public static event UnityAction PlayerDead;
    //public static event Action PlayerDead;
    //public static event Action PlayerScore;
    //public static event Action PlayerGainCoin;
    public Rigidbody2D rb2d;
    public BoxCollider2D b2d;
    public Animator animator;
    public float moveSpeed;
    public float jumpSpeed;
    private string[] groundCheckLayerMasks;
    private bool movePressed;
    private bool jumpPressed;
    private float movDir;

    private void Awake()
    {
        groundCheckLayerMasks = new string[] { Constants.LY_PLATFORM };
        movePressed = false;
        jumpPressed = false;
    }

    private void FixedUpdate()
    {
        DoMove();
        DoJump();
        CheckDead();
    }

    private void Update()
    {
        DoMoveInput();
        DoJumpInput();
        DoMoveAnim();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        CheckEnemyCollision(other);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Constants.TAG_PICKUP))
        {
            //PlayerGainCoin?.Invoke();
            Destroy(other.gameObject);
        }
    }

    private void CheckEnemyCollision(Collision2D other)
    {
        if (!other.gameObject.CompareTag(Constants.TAG_ENEMY)) return;
        Rigidbody2D enemyRb2d = other.gameObject.GetComponent<Rigidbody2D>();
        if ((int)rb2d.position.y > (int)enemyRb2d.position.y)
        {// Player is above enemy (Jump from above)
            rb2d.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
            Destroy(other.gameObject);
            //PlayerScore?.Invoke();
        }
        else
        {// collide with side
            //PlayerDead?.Invoke();
            rb2d.AddForce(Vector2.up * 20f, ForceMode2D.Impulse);
            Destroy(b2d);
        }
    }

    private void DoMoveInput()
    {
        movDir = Input.GetAxisRaw("Horizontal");
    }
    private void DoMove()
    {
        rb2d.velocity = new Vector2(moveSpeed * movDir, rb2d.velocity.y);
    }

    private void DoMoveAnim()
    {
        animator.SetBool(Constants.MOV, movDir != 0);
    }

    private void DoJumpInput()
    {
        if (!jumpPressed) jumpPressed = Input.GetKeyDown(KeyCode.C);
    }

    private void DoJump()
    {
        if (!jumpPressed) return;
        if (GroundCheck())
        {
            rb2d.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            animator.SetTrigger(Constants.JMP);
        }
        jumpPressed = false;
    }

    private void CheckDead()
    {
        if (rb2d.position.y <= Constants.DEADLINE_Y_POINT)
        {
            PlayerDead?.Invoke();
            Destroy(gameObject);
        }
    }

    private bool GroundCheck()
    {
        Vector2 hv1 = rb2d.position + new Vector2(0.12f, 0f);
        RaycastHit2D hit1 = Physics2D.Raycast(hv1, Vector2.down, 1f, LayerMask.GetMask(groundCheckLayerMasks));
        // Debug.DrawRay(hv1, Vector2.down, Color.red, 10f);

        Vector2 hv2 = rb2d.position - new Vector2(0.12f, 0f);
        RaycastHit2D hit2 = Physics2D.Raycast(hv2, Vector2.down, 1f, LayerMask.GetMask(groundCheckLayerMasks));
        // Debug.DrawRay(hv2, Vector2.down, Color.cyan, 10f);

        return hit1.collider != null || hit2.collider != null;
    }
}
