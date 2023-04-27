using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPlatform : MonoBehaviour
{
    public BoxCollider2D b2d;
    public Rigidbody2D rb2d;
    public bool disposable;
    private bool playerLanded;

    private void Awake()
    {
        if (!disposable)
        {
            rb2d.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    private void FixedUpdate()
    {
        if (rb2d.position.y < Constants.DEADLINE_Y_POINT)
        {
            Destroy(this);
        }
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
        Destroy(b2d);
    }

}
