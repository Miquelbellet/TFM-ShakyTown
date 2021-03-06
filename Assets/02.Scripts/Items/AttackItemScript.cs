using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackItemScript : MonoBehaviour
{
    public GameObject secondaryItem;
    public GameObject arrowShotPrefab;

    [HideInInspector] public bool isSwordInEnemy;
    [HideInInspector] public Tool usingTool;

    GameObject gameController;

    private bool toolIsBow;
    private float swordAngleCorrection = 135;
    private float bowAngleCorrection = 225;
    private GameObject enemyObject;
    private Sprite[] toolsItemSprites;
    private Tool bowArrow;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
    }

    void Update()
    {
        RotateObjectToMouse();
    }

    void RotateObjectToMouse()
    {
        Vector3 object_pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 mousePosition = gameController.GetComponent<ToolBarScript>().mousePosition;
        Vector2 newObjectPos = new Vector2(mousePosition.x - object_pos.x, mousePosition.y - object_pos.y);
        float angle = Mathf.Atan2(newObjectPos.y, newObjectPos.x) * Mathf.Rad2Deg;

        var swordRotation = Quaternion.Euler(new Vector3(0, 0, angle - swordAngleCorrection));
        secondaryItem.transform.rotation = swordRotation;
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
            gameObject.SetActive(true);
            usingTool = tool;
            toolsItemSprites = toolsSprites;
            if (tool.isBow)
            {
                SetSecondaryItem();
            }
            else
            {
                secondaryItem.SetActive(false);
                bowArrow = null;
            }

            foreach (Sprite toolSp in toolsSprites)
            {
                if (toolSp.name == tool.spriteName) GetComponent<SpriteRenderer>().sprite = toolSp;
            }
            if (tool.isBow) toolIsBow = true;
            else toolIsBow = false;
        }
        else
        {
            gameObject.SetActive(false);
            GetComponent<SpriteRenderer>().sprite = null;
            secondaryItem.SetActive(false);
            usingTool = null;
            bowArrow = null;
        }
    }

    public void AttackEnemySword()
    {
        if(enemyObject) enemyObject.GetComponent<EnemyControllerScript>().Hitted(usingTool.damage);
    }

    public void AttackEnemyBow()
    {
        if(bowArrow != null)
        {
            gameController.GetComponent<SoundsControllerScript>().PlayArrow();
            GameObject arrowPrefab = Instantiate(arrowShotPrefab, transform.position, secondaryItem.transform.rotation);
            arrowPrefab.GetComponent<SpriteRenderer>().sprite = secondaryItem.GetComponent<SpriteRenderer>().sprite;
            arrowPrefab.GetComponent<ArrowShotScript>().arrow = bowArrow;
            arrowPrefab.GetComponent<ArrowShotScript>().bowObject = usingTool;

            Tool[] newToolList = gameController.GetComponent<ToolBarScript>().toolsList;
            bowArrow.countItems--;
            if(bowArrow.countItems > 0)
            {
                newToolList[bowArrow.toolbarIndex] = bowArrow;
                gameController.GetComponent<ToolBarScript>().toolsList = newToolList;
            }
            else
            {
                Tool emptyTool = new Tool();
                emptyTool.toolbarIndex = bowArrow.toolbarIndex;
                newToolList[bowArrow.toolbarIndex] = emptyTool;
                gameController.GetComponent<ToolBarScript>().toolsList = newToolList;
                secondaryItem.SetActive(false);
                bowArrow = null;
            }
            SetSecondaryItem();
            gameController.GetComponent<ToolBarScript>().RefreshToolbarItems();
        }
    }

    void SetSecondaryItem()
    {
        try
        {
            Tool[] toolsList = gameController.GetComponent<ToolBarScript>().toolsList;
            for (int n = 0; n < toolsList.Length; n++)
            {
                if (!toolsList[n].empty)
                {
                    if (toolsList[n].isArrow && toolsList[n].countItems > 0)
                    {
                        secondaryItem.SetActive(true);
                        bowArrow = toolsList[n];
                        foreach (Sprite toolSp in toolsItemSprites)
                        {
                            if (toolSp.name == toolsList[n].spriteName) secondaryItem.GetComponent<SpriteRenderer>().sprite = toolSp;
                        }
                    }
                }
            }
        }
        catch { }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            isSwordInEnemy = true;
            enemyObject = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            isSwordInEnemy = false;
            enemyObject = null;
        }
    }
}
