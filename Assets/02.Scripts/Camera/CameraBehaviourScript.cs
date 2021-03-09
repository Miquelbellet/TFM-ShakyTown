using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviourScript : MonoBehaviour
{
    public float lerpPositionsTimer;
    [HideInInspector] public bool followPlayer = true;

    private GameObject player;
    private Vector3 playerPosition;
    private Vector3 velocity;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void LateUpdate()
    {
        if (followPlayer)
        {
            playerPosition = new Vector3(player.transform.position.x, player.transform.position.y, -20);
            transform.position = Vector3.SmoothDamp(transform.position, playerPosition, ref velocity, lerpPositionsTimer);
        }
    }
}
