using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItemScript : MonoBehaviour
{
    public bool isBowGift;
    public bool isArrowGift;
    public float throwItemForce;
    public float floatingTimer;
    public float floatingAcceleration;
    public Tool droppedTool;

    GameObject player;
    GameObject gameController;
    ResourcesManagmentScript resourcesManagmentScript;

    private float time;
    private Sprite[] toolsSprites;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameController = GameObject.FindGameObjectWithTag("GameController");
        toolsSprites = Resources.LoadAll<Sprite>("Tools");
    }

    private void Start()
    {
        resourcesManagmentScript = new ResourcesManagmentScript();
        if (!droppedTool.empty && droppedTool.spriteName != "")
        {
            SetItem(droppedTool);
            if (isBowGift && resourcesManagmentScript.GetGameVariable("getBowGift")) Destroy(gameObject);
            else if (isArrowGift && resourcesManagmentScript.GetGameVariable("getArrowGift")) Destroy(gameObject);
        }
    }

    void Update()
    {
        time += Time.unscaledDeltaTime;
        if (time < floatingTimer / 2)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - floatingAcceleration);
        }
        else if (time < floatingTimer)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + floatingAcceleration);
        }
        else
        {
            time = 0;
        }
    }

    public void SetItem(Tool tool)
    {
        droppedTool = tool;
        if (!tool.empty)
        {
            foreach (Sprite toolSp in toolsSprites)
            {
                if (toolSp.name == tool.spriteName)
                {
                    GetComponent<SpriteRenderer>().sprite = toolSp;
                }
            }
        }
    }

    public void ThrowItemToFloor(bool allItems = default(bool))
    {
        Vector2 playerOrientation;
        if (!allItems)
        {
            playerOrientation = player.GetComponent<PlayerMovementScript>().playerOrientation;
        }
        else
        {
            int randomX = Random.Range(-1, 2);
            int randomY;
            if (randomX != 0) randomY = 0;
            else randomY = Random.Range(-1, 2);
            playerOrientation = new Vector2(randomX, randomY);
        }
        Vector3 itemPos = playerOrientation * throwItemForce;
        transform.position = player.transform.position + itemPos;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if (!droppedTool.empty)
            {
                bool pickedItem = gameController.GetComponent<ToolBarScript>().SetNewItemTool(droppedTool);
                if (pickedItem)
                {
                    gameController.GetComponent<SoundsControllerScript>().PlayDropItem();
                    if (isBowGift) resourcesManagmentScript.SetGameVariable("getBowGift", true);
                    else if (isArrowGift) resourcesManagmentScript.SetGameVariable("getArrowGift", true);
                    Destroy(gameObject);
                }
            }
        }
    }
}
