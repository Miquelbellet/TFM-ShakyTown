using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviourScript : MonoBehaviour
{
    [Header("Camera Config")]
    public float lerpPlayerPosTimer;
    public float earthquakesTimer;
    public float earthquakeDuration;
    public float earthquakesForce;

    [Header("Level 1 limits")]
    public float Lvl1_LeftX;
    public float Lvl1_RightX;
    public float Lvl1_BotY;
    public float Lvl1_TopY;

    [Header("Level 2 limits")]
    public float Lvl2_LeftX;
    public float Lvl2_RightX;
    public float Lvl2_BotY;
    public float Lvl2_TopY;

    [Header("Level 3 limits")]
    public float Lvl3_LeftX;
    public float Lvl3_RightX;
    public float Lvl3_BotY;
    public float Lvl3_TopY;

    [Header("Level 4 limits")]
    public float Lvl4_LeftX;
    public float Lvl4_RightX;
    public float Lvl4_BotY;
    public float Lvl4_TopY;


    private GameObject player;
    private GameObject gameController;
    private Vector3 playerPosition;
    private Vector3 velocity;
    private float earthquakeTimer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameController = GameObject.FindGameObjectWithTag("GameController");
        InvokeRepeating("Earthquake", earthquakesTimer, earthquakesTimer);
    }

    void LateUpdate()
    {
        earthquakeTimer += Time.deltaTime;
        Vector2 targetPos = CheckCameraLimits();
        playerPosition = new Vector3(targetPos.x, targetPos.y, -20);
        transform.position = Vector3.SmoothDamp(transform.position, playerPosition, ref velocity, lerpPlayerPosTimer, Mathf.Infinity, Time.unscaledDeltaTime);
    }

    Vector2 CheckCameraLimits()
    {
        Vector2 playerPos = player.transform.position;
        switch (gameController.GetComponent<LevelControllerScript>().currentLevelNumber)
        {
            case 1:
                if (playerPos.x <= Lvl1_LeftX) playerPos.x = Lvl1_LeftX;
                else if (playerPos.x >= Lvl1_RightX) playerPos.x = Lvl1_RightX;
                if (playerPos.y <= Lvl1_BotY) playerPos.y = Lvl1_BotY;
                else if (playerPos.y >= Lvl1_TopY) playerPos.y = Lvl1_TopY;
                break;
            case 2:
                if (playerPos.x <= Lvl2_LeftX) playerPos.x = Lvl2_LeftX;
                else if (playerPos.x >= Lvl2_RightX) playerPos.x = Lvl2_RightX;
                if (playerPos.y <= Lvl2_BotY) playerPos.y = Lvl2_BotY;
                else if (playerPos.y >= Lvl2_TopY) playerPos.y = Lvl2_TopY;
                break;
            case 3:
                if (playerPos.x <= Lvl3_LeftX) playerPos.x = Lvl3_LeftX;
                else if (playerPos.x >= Lvl3_RightX) playerPos.x = Lvl3_RightX;
                if (playerPos.y <= Lvl3_BotY) playerPos.y = Lvl3_BotY;
                else if (playerPos.y >= Lvl3_TopY) playerPos.y = Lvl3_TopY;
                break;
            case 4:
                if (playerPos.x <= Lvl4_LeftX) playerPos.x = Lvl4_LeftX;
                else if (playerPos.x >= Lvl4_RightX) playerPos.x = Lvl4_RightX;
                if (playerPos.y <= Lvl4_BotY) playerPos.y = Lvl4_BotY;
                else if (playerPos.y >= Lvl4_TopY) playerPos.y = Lvl4_TopY;
                break;
        }
        return playerPos;
    }

    void Earthquake()
    {
        earthquakeTimer = 0;
        StartCoroutine(ShakeCamera());
    }

    IEnumerator ShakeCamera()
    {
        for (float i = 0; i < Mathf.Infinity; i++)
        {
            transform.position = new Vector2(transform.position.x + Random.Range(-earthquakesForce, earthquakesForce), transform.position.y + Random.Range(-earthquakesForce, earthquakesForce));
            yield return new WaitForSeconds(0.1f);
            if (earthquakeTimer >= earthquakeDuration) i = Mathf.Infinity;
        }
    }
}
