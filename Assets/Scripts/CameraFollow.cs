using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Rigidbody2D objToFollow;
    public Transform transfEndPoint;
    private Transform mainCamTransf;
    private float rightEndX;

    private void OnEnable()
    {
        Player.PlayerDead += OnPlayerDead;
        mainCamTransf = Camera.main.transform;
        rightEndX = transfEndPoint.position.x - 15f;
    }

    private void LateUpdate()
    {
        if (objToFollow.position.x < 0f)// object exceed left end
            mainCamTransf.position = new Vector3(0f, 0f, mainCamTransf.position.z);
        else if (objToFollow.position.x > rightEndX)// object exceed right end
            mainCamTransf.position = new Vector3(rightEndX, mainCamTransf.position.y, mainCamTransf.position.z);
        else// follow
            mainCamTransf.position = new Vector3(objToFollow.position.x, mainCamTransf.position.y, mainCamTransf.position.z);
    }

    private void OnDisable()
    {
        Player.PlayerDead -= OnPlayerDead;
    }

    private void OnPlayerDead()
    {
        enabled = false;
    }
}
