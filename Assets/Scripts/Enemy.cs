using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Enemy : MonoBehaviour
{
    public static event UnityAction<int> GivePlayerScore;
    public Rigidbody2D rb2d;
    public float movSpeed;
    public int maxHealth;
    public int scoreValue;
    private int health;
    private bool isReachedEdge;

    private void Awake()
    {
        health = maxHealth;
    }

    private void OnEnable()
    {
        Player.PlayerDead += OnPlayerDead;
    }

    private void FixedUpdate()
    {
        if (!isReachedEdge)
        {
            rb2d.velocity = Vector2.left * movSpeed;
        }
        CheckDeadOnFall();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        CheckFenceCollision(other);
        CheckPlayerCollision(other);
        CheckPlatformCollision(other);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(Constants.TAG_PLATFORM))
        {
            rb2d.velocity = Vector2.zero;
            isReachedEdge = true;
        }
    }

    private void OnBecameVisible()
    {
        enabled = true;
    }

    private void OnDisable()
    {
        Player.PlayerDead -= OnPlayerDead;
    }

    private void CheckPlayerCollision(Collision2D other)
    {
        if (!other.gameObject.CompareTag(Constants.TAG_PLAYER)) return;

        ContactPoint2D contact = other.GetContact(0);
        if (contact.normal.y < 0f)
        {
            // collide from top, enemy dead
            TakeDamage();
        }
        else
        {// collide from side, player dead
        }
    }

    private void CheckPlatformCollision(Collision2D other)
    {
        if (other.gameObject.CompareTag(Constants.TAG_PLATFORM)) isReachedEdge = false;
    }

    private void CheckFenceCollision(Collision2D other)
    {
        if (other.gameObject.CompareTag(Constants.TAG_FENCE))
        {
            movSpeed *= -1;
            transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
        }
    }

    private void TakeDamage()
    {
        --health;
        if (health < 1)
        {
            GivePlayerScore?.Invoke(scoreValue);
            Destroy(gameObject);
        }
    }

    private void CheckDeadOnFall()
    {
        if (rb2d.position.y <= Constants.DEADLINE_Y_POINT) Destroy(gameObject);
    }

    private void OnPlayerDead()
    {
        Destroy(gameObject);
    }
}
