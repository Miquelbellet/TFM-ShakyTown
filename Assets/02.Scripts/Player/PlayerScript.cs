using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{

    private bool openChest;
    private GameObject chestObject;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void ActionChest()
    {
        if (chestObject.GetComponent<ChestScript>().chestOpened) chestObject.GetComponent<ChestScript>().CloseChest();
        else chestObject.GetComponent<ChestScript>().OpenChest();
    }

    public void InputAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (openChest)
            {
                ActionChest();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Chest")
        {
            openChest = true;
            chestObject = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Chest")
        {
            try { chestObject.GetComponent<ChestScript>().CloseChest(); }
            catch { }
            openChest = false;
            chestObject = null;
        }
    }
}
