using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{

    private GameObject interactableObject;
    private bool openChest;
    private bool momTalk;
    private bool bossTalk;
    private bool blacksmithTalk;
    private bool witchTalk;


    public void InputAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (openChest)
            {
                if (interactableObject.GetComponent<ChestScript>().chestOpened) interactableObject.GetComponent<ChestScript>().CloseChest();
                else interactableObject.GetComponent<ChestScript>().OpenChest();
            }
            else if (momTalk)
            {
                interactableObject.GetComponent<MomControllerScript>().ShowDialog();
            }
            else if (bossTalk)
            {
                interactableObject.GetComponent<BossControllerScript>().ShowDialog();
            }
            else if (blacksmithTalk)
            {
                interactableObject.GetComponent<BlacksmithControllerScript>().ShowDialog();
            }
            else if (witchTalk)
            {
                interactableObject.GetComponent<WitchControllerScript>().ShowDialog();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Chest")
        {
            openChest = true;
            interactableObject = other.gameObject;
        }
        else if (other.tag == "Mom")
        {
            momTalk = true;
            interactableObject = other.gameObject;
        }
        else if (other.tag == "Boss")
        {
            bossTalk = true;
            interactableObject = other.gameObject;
        }
        else if (other.tag == "Blacksmith")
        {
            blacksmithTalk = true;
            interactableObject = other.gameObject;
        }
        else if (other.tag == "Witch")
        {
            witchTalk = true;
            interactableObject = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Chest")
        {
            try { interactableObject.GetComponent<ChestScript>().CloseChest(); } catch { }
            openChest = false;
            interactableObject = null;
        }
        else if (other.tag == "Mom")
        {
            momTalk = false;
            interactableObject = null;
        }
        else if (other.tag == "Boss")
        {
            bossTalk = false;
            interactableObject = null;
        }
        else if (other.tag == "Blacksmith")
        {
            blacksmithTalk = false;
            interactableObject = null;
        }
        else if (other.tag == "Witch")
        {
            witchTalk = false;
            interactableObject = null;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            GetComponent<PlayerMovementScript>().stopRollMove = true;
        }

        if (collision.gameObject.tag == "Enemie")
        {
            if (GetComponent<PlayerHealthScript>().canRecieveDamage)
            {
                float damage = collision.gameObject.GetComponent<EnemyControllerScript>().damage;
                GetComponent<PlayerHealthScript>().PlayerHitted(damage);
            }
        }
    }
}
