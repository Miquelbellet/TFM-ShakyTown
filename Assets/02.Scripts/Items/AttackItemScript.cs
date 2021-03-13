using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackItemScript : MonoBehaviour
{
    public GameObject playerHandItem;
    [HideInInspector] public Vector2 mousePosition;

    private bool toolIsBow;
    private float swordAngleCorrection = 135;
    private float bowAngleCorrection = 225;

    void Start()
    {

    }

    void Update()
    {
        RotateObjectToMouse();
    }

    void RotateObjectToMouse()
    {
        Vector3 object_pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 newObjectPos = new Vector2(mousePosition.x - object_pos.x, mousePosition.y - object_pos.y);
        float angle = Mathf.Atan2(newObjectPos.y, newObjectPos.x) * Mathf.Rad2Deg;

        if (toolIsBow) transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - bowAngleCorrection));
        else transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - swordAngleCorrection));
    }

    public void SetPlayerHandItem(string toolJSON, Sprite[] toolsSprites)
    {
        Tool tool = Tool.CreateFromJSON(toolJSON);
        if (tool.isWeapon)
        {
            foreach (Sprite toolSp in toolsSprites)
            {
                if (toolSp.name == tool.spriteName) GetComponent<SpriteRenderer>().sprite = toolSp;
            }
            if (tool.isBow) toolIsBow = true;
            else toolIsBow = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = null;
        }
    }

    public void InputMousePosition(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
    }
}
