using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    public float playerSpeed;
    public float rollMovementForce;
    public float cooldownRollTimer;
    [HideInInspector] public Vector2 playerOrientation;


    private Animator playerAnimator;

    private Vector2 playerMovement;
    private bool cooldownRoll = true;

    void Start()
    {
        playerAnimator = transform.GetComponent<Animator>();
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
        if (context.performed && cooldownRoll)
        {
            playerAnimator.SetTrigger("roll");
            Vector3 rollPos = playerOrientation * rollMovementForce;
            transform.position += rollPos;
            cooldownRoll = false;
            Invoke("RestartRoll", cooldownRollTimer);
        }
    }

    private void RestartRoll()
    {
        cooldownRoll = true;
    }
}
