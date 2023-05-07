using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public BoxCollider2D b2d;
    public GameObject objCheckPointAnim;
    private bool checkIn;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag(Constants.TAG_PLAYER)) return;// collide with non-player object
        if (checkIn) return;// player already collided
        checkIn = true;
        Destroy(spriteRenderer);
        Destroy(b2d);
        objCheckPointAnim.SetActive(true);
        StartCoroutine(CheckPointAnim());
    }

    private IEnumerator CheckPointAnim()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }

}
