using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPlatform : MonoBehaviour
{
    public BoxCollider2D b2d;
    public Rigidbody2D rb2d;
    public Vector2 originalPos;
    public Animator animator;
    public bool disposable;
    private bool playerLanded;
    private bool respawnStarted;

    private void Awake()
    {
        if (!disposable)
        {
            rb2d.bodyType = RigidbodyType2D.Kinematic;
        }
        originalPos = rb2d.position;
    }

    private void FixedUpdate()
    {
        if (rb2d.position.y > Constants.DEADLINE_Y_POINT) return;
        if (respawnStarted) return;
        StartCoroutine(WaitRespawn());
    }

    private void OnBecameVisible()
    {
        enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(Constants.TAG_PLAYER) && !playerLanded)
        {
            StartCoroutine(CountToFall());
            playerLanded = true;
        }
    }

    private IEnumerator CountToFall()
    {
        yield return new WaitForSeconds(1f);
        rb2d.mass = 1f;
        rb2d.gravityScale = 10f;
        b2d.enabled = false;
    }

    private IEnumerator WaitRespawn()
    {
        respawnStarted = true;
        rb2d.velocity = Vector2.zero;
        rb2d.mass = 200f;
        rb2d.gravityScale = 0f;
        yield return new WaitForSeconds(4f);
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        rb2d.position = originalPos;
        animator.SetTrigger(Constants.RESPAWN);
        yield return new WaitForSeconds(1.25f);
        b2d.enabled = true;
        playerLanded = false;
        respawnStarted = false;
    }

}
