using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    public float playerSpeed;
    public float rollMovementForce;
    public float rollForceTimer;
    public float rollCooldownTimer;
    [HideInInspector] public Vector2 playerOrientation;
    [HideInInspector] public bool stopRollMove;

    private Animator playerAnimator;
    private GameObject gameController;

    private Vector2 playerMovement;
    private bool cooldownRoll;

    void Start()
    {
        playerAnimator = transform.GetComponent<Animator>();
        gameController = GameObject.FindGameObjectWithTag("GameController");
        playerOrientation = new Vector2(0, -1);
    }

    void Update()
    {
        PlayerMove();
    }

    void PlayerMove()
    {
        if (playerMovement != Vector2.zero)
        {
            SetPlayerAnimations(playerMovement);
            playerOrientation = playerMovement;
            transform.position = new Vector2(
                transform.position.x + (playerMovement.x * Time.deltaTime * playerSpeed),
                transform.position.y + (playerMovement.y * Time.deltaTime * playerSpeed)
            );
        }
        else
        {
            playerAnimator.SetBool("idle", true);
            playerAnimator.SetBool("walk", false);
        }
    }

    private void SetPlayerAnimations(Vector2 playerMovement)
    {
        playerAnimator.SetBool("left", false);
        playerAnimator.SetBool("right", false);
        playerAnimator.SetBool("up", false);
        playerAnimator.SetBool("down", false);

        playerAnimator.SetBool("walk", true);
        playerAnimator.SetBool("idle", false);
        if (playerMovement.x > 0)
        {
            playerAnimator.SetBool("right", true); 
        }
        else if (playerMovement.x < 0)
        {
            playerAnimator.SetBool("left", true);
        }
        else if (playerMovement.y > 0)
        {
            playerAnimator.SetBool("up", true);
        }
        else if (playerMovement.y < 0)
        {
            playerAnimator.SetBool("down", true);
        }
    }

    public void InputMovement(InputAction.CallbackContext context)
    {
        playerMovement = context.ReadValue<Vector2>();
    }

    public void InputRoll(InputAction.CallbackContext context)
    {
        if (context.performed && !cooldownRoll)
        {
            gameController.GetComponent<SoundsControllerScript>().PlayDash();
            cooldownRoll = true;
            stopRollMove = false;
            Vector3 startPos = transform.position;
            Vector3 rollPos = playerOrientation * rollMovementForce;
            Vector3 finalPos = startPos + rollPos;
            StartCoroutine(RollMovement(startPos, finalPos, rollForceTimer));
            Invoke("RestartCooldownRoll", rollCooldownTimer);
        }
    }

    IEnumerator RollMovement(Vector3 StartPos, Vector3 EndPos, float LerpTime)
    {
        float StartTime = Time.time;
        float EndTime = StartTime + LerpTime;
        while (Time.time < EndTime)
        {
            float timeProgressed = (Time.time - StartTime) / LerpTime;
            if(!stopRollMove) transform.position = Vector3.Lerp(StartPos, EndPos, timeProgressed);
            yield return new WaitForFixedUpdate();
        }
    }

    void RestartCooldownRoll()
    {
        cooldownRoll = false;
    }
}
