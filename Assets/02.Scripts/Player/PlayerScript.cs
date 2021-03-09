using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void InputAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Action");
        }
    }
}
