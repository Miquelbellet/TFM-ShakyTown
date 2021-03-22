using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackItemScript : MonoBehaviour
{
    public GameObject secundaryItem;
    [HideInInspector] public Vector2 mousePosition;
    [HideInInspector] public bool isSwordInEnemie;

    private Tool usingTool;
    private bool toolIsBow;
    private float swordAngleCorrection = 135;
    private float bowAngleCorrection = 225;
    private GameObject enemieObject;

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

        var swordRotation = Quaternion.Euler(new Vector3(0, 0, angle - swordAngleCorrection));
        secundaryItem.transform.rotation = swordRotation;
        if (toolIsBow)
        {
            var bowRotation = Quaternion.Euler(new Vector3(0, 0, angle - bowAngleCorrection));
            transform.rotation = bowRotation;
        }
        else
        {
            transform.rotation = swordRotation;
        }
    }

    public void SetPlayerHandItem(string toolJSON, Sprite[] toolsSprites)
    {
        Tool tool = Tool.CreateFromJSON(toolJSON);
        if (tool.isWeapon)
        {
            usingTool = tool;
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
            usingTool = null;
        }
    }

    public void AttackEnemie()
    {
        enemieObject.GetComponent<EnemyControllerScript>().Hitted(usingTool.damage);
    }

    public void InputMousePosition(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemie")
        {
            isSwordInEnemie = true;
            enemieObject = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemie")
        {
            isSwordInEnemie = false;
            enemieObject = null;
        }
    }
}
