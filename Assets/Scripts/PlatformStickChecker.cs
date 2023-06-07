using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformStickChecker : MonoBehaviour
{
    public Player player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Constants.TAG_PLATFORM) || other.gameObject.CompareTag(Constants.TAG_FENCE))
        {
            player.stickToPlatform = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Constants.TAG_PLATFORM) || other.gameObject.CompareTag(Constants.TAG_FENCE))
        {
            player.stickToPlatform = false;
        }
    }
}
