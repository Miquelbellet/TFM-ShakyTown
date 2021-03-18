using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItemScript : MonoBehaviour
{
    public float throwItemForce;
    public float floatingTimer;
    public float floatingAcceleration;
    public Tool droppedTool;

    GameObject player;
    GameObject gameManager;

    private float time;
    private Sprite[] toolsSprites;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = GameObject.FindGameObjectWithTag("GameController");
        toolsSprites = Resources.LoadAll<Sprite>("Tools");
    }

    private void Start()
    {
        if (!droppedTool.empty && droppedTool.spriteName != "")
        {
            SetItem(droppedTool);
        }
    }

    void Update()
    {
        time += Time.deltaTime;
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

    public void ThrowItemToFloor()
    {
        Vector2 playerOrientation = player.GetComponent<PlayerMovementScript>().playerOrientation;
        Vector3 itemPos = playerOrientation * throwItemForce;
        transform.position = player.transform.position + itemPos;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if (!droppedTool.empty)
            {
                bool pickedItem = gameManager.GetComponent<ToolBarScript>().SetNewItemTool(droppedTool);
                if (pickedItem) Destroy(gameObject);
            }
        }
    }
}
