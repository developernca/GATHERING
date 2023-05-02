using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
    public static event UnityAction PlayerDead;
    public static event UnityAction<int> PlayerPickup;
    public static event UnityAction PlayerPickupFinish;
    public AudioSource audioCoinPickup;
    public AudioSource audioJumpOnEnemy;
    public AudioSource audioPlayerDead;
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
        groundCheckLayerMasks = new string[] { Constants.LY_PLATFORM, Constants.LY_FENCE, Constants.LY_ENEMY };
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
        CoinPickup(other);
        FinishPickup(other);
    }

    private void CheckEnemyCollision(Collision2D other)
    {
        if (!other.gameObject.CompareTag(Constants.TAG_ENEMY)) return;

        ContactPoint2D contact = other.GetContact(0);
        if (contact.normal.y > 0f)
        {
            // collide from top, enemy dead. Get Score.
            audioJumpOnEnemy.Play();
            rb2d.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
        }
        else
        {
            // collide from side, player dead. Game over.
            audioPlayerDead.Play();
            rb2d.AddForce(Vector2.up * 20f, ForceMode2D.Impulse);
            Destroy(b2d);
        }
    }

    private void CoinPickup(Collider2D other)
    {
        if (!other.gameObject.CompareTag(Constants.TAG_PICKUP)) return;// not pickup
        if (other.gameObject.name.Contains(Constants.NAME_COIN))
        {
            audioCoinPickup.Play();
            Pickup pickup = other.gameObject.GetComponent<Pickup>();
            PlayerPickup?.Invoke(pickup.scoreValue);
        }
    }

    private void FinishPickup(Collider2D other)
    {
        if (!other.gameObject.CompareTag(Constants.TAG_PICKUP)) return;// not pickup
        if (other.gameObject.name == Constants.NAME_FINISH)
        {
            enabled = false;
            PlayerPickupFinish?.Invoke();
        }
    }

    private void DoMoveInput()
    {
        movDir = Input.GetAxisRaw("Horizontal");
    }
    private void DoMove()
    {
        rb2d.velocity = new Vector2(moveSpeed * movDir, rb2d.velocity.y);
        if (movDir != 0) transform.localScale = new Vector2(movDir, transform.localScale.y);
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
