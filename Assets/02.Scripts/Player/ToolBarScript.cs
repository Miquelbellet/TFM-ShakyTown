using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ToolBarScript : MonoBehaviour
{
    public AttackItemScript attackItemScript;
    public GameObject bordersObjectParent;
    public GameObject droppedItemPrefab;
    [HideInInspector] public bool grabbingItem;
    [HideInInspector] public Tool itemGrabbed;

    ResourcesManagmentScript resourcesManagmentScript;
    GameObject playerHandItem;
    GameObject canvasObject;
    GameObject[] chestsInScene;

    private Tool[] toolsList;
    private Sprite[] toolsSprites;
    private int numberOfToolsTotal;
    private int toolNumberSelected = 0;
    private GameObject grabbedItemObject;

    void Start()
    {
        numberOfToolsTotal = bordersObjectParent.transform.childCount-1;
        resourcesManagmentScript = new ResourcesManagmentScript();
        playerHandItem = GameObject.FindGameObjectWithTag("HandItem");
        canvasObject = GameObject.FindGameObjectWithTag("Canvas");
        chestsInScene = GameObject.FindGameObjectsWithTag("Chest");
        toolsSprites = Resources.LoadAll<Sprite>("Tools");
        SetObjectBorders();
        SetToolsForBar();
    }

    private void Update()
    {
        if (grabbingItem)
        {
            try {
                Vector2 mousePoistion = playerHandItem.GetComponent<AttackItemScript>().mousePosition;
                grabbedItemObject.transform.position = mousePoistion;
            }
            catch { }
        }
    }

    void ChangeToolUpwards()
    {
        if (toolNumberSelected == numberOfToolsTotal) toolNumberSelected = 0;
        else toolNumberSelected++;
        SetObjectBorders();
    }

    void ChangeToolBackwards()
    {
        if (toolNumberSelected == 0) toolNumberSelected = numberOfToolsTotal;
        else toolNumberSelected--;
        SetObjectBorders();
    }

    void SetObjectBorders()
    {
        for (int i = 0; i <= numberOfToolsTotal; i++)
        {
            if(toolNumberSelected == i) bordersObjectParent.transform.GetChild(i).transform.gameObject.SetActive(true);
            else bordersObjectParent.transform.GetChild(i).transform.gameObject.SetActive(false);
        }
        try { attackItemScript.SetPlayerHandItem(JsonUtility.ToJson(toolsList[toolNumberSelected]), toolsSprites); }
        catch { }
    }

    void SetToolbarItemUI(Tool tool)
    {
        if (!tool.empty)
        {
            foreach (Sprite toolSp in toolsSprites)
            {
                if (toolSp.name == tool.spriteName)
                {
                    GetComponent<UIControllerScript>().SetToolBarItem(toolSp, tool);
                }
            }
            attackItemScript.SetPlayerHandItem(JsonUtility.ToJson(toolsList[toolNumberSelected]), toolsSprites);
        }
        else GetComponent<UIControllerScript>().SetToolBarItem(null, tool);
    }

    void RemoveItemFromToolbar(int indexTool)
    {
        Tool tool = new Tool();
        tool.toolbarIndex = indexTool;
        toolsList[indexTool] = tool;
        GetComponent<UIControllerScript>().SetToolBarItem(null, tool);
        attackItemScript.SetPlayerHandItem(JsonUtility.ToJson(toolsList[toolNumberSelected]), toolsSprites);
    }

    void DestroyGrabbingItem()
    {
        grabbingItem = false;
        itemGrabbed = null;
        Destroy(grabbedItemObject);
        grabbedItemObject = null;
    }

    void SetToolsForBar()
    {
        toolsList = new Tool[numberOfToolsTotal + 1];
        StreamReader toolsBar = resourcesManagmentScript.ReadDataFromResource("Assets/Resources/tools_bar.txt");
        for (int i = 0; i <= numberOfToolsTotal; i++)
        {
            string toolStr = toolsBar.ReadLine();
            Tool tool = Tool.CreateFromJSON(toolStr);
            toolsList[i] = tool;
            SetToolbarItemUI(tool);
        }
        toolsBar.Close();
    }

    public void SelectItemInToolbar(int itemIndex)
    {
        toolNumberSelected = itemIndex;
        SetObjectBorders();
    }

    public void DragItemInToolbar(int itemIndex)
    {
        if (!grabbingItem)
        {
            itemGrabbed = toolsList[itemIndex];
            RemoveItemFromToolbar(itemIndex);
            if (!itemGrabbed.empty)
            {
                grabbingItem = true;
                Sprite itemSprite = null;
                foreach (Sprite toolSp in toolsSprites)
                {
                    if (toolSp.name == itemGrabbed.spriteName)
                    {
                        itemSprite = toolSp;
                    }
                }
                GameObject itemSpriteObject = new GameObject();
                itemSpriteObject.name = "GrabbedObject";
                RectTransform trans = itemSpriteObject.AddComponent<RectTransform>();
                trans.transform.SetParent(canvasObject.transform);
                itemSpriteObject.AddComponent<CanvasRenderer>();
                itemSpriteObject.AddComponent<Image>().sprite = itemSprite;

                EventTrigger trigger = itemSpriteObject.AddComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.Drop;
                entry.callback.AddListener((eventData) => { DropItem(); });
                trigger.triggers.Add(entry);
                
                grabbedItemObject = itemSpriteObject;
            }
        }
    }

    public void DropItem()
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current) { pointerId = -1 };
        pointer.position = playerHandItem.GetComponent<AttackItemScript>().mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, results);
        bool itemDropped = false;
        if (results.Count > 0)
        {
            foreach (var res in results)
            {
                if (res.gameObject.tag == "ToolbarItem")
                {
                    itemDropped = true;
                    DropItemToToolbar(res.gameObject.transform.GetSiblingIndex(), false);
                }
                else if (res.gameObject.tag == "ToolbarItemChest")
                {
                    foreach (GameObject chest in chestsInScene)
                    {
                        if (chest.GetComponent<ChestScript>().chestOpened)
                        {
                            chest.GetComponent<ChestScript>().itemGrabbed = itemGrabbed;
                            chest.GetComponent<ChestScript>().DropItemToToolbarChest(res.gameObject.transform.GetSiblingIndex(), true);
                        }
                    }
                    itemDropped = true;
                    DestroyGrabbingItem();
                }
            }
        }
        if (!itemDropped) DropItemToFloor(false);
    }

    public void DropItemToFloor(bool droppedByKeyPress)
    {
        if (droppedByKeyPress)
        {
            if (!toolsList[toolNumberSelected].empty)
            {
                GameObject dropedItem = Instantiate(droppedItemPrefab);
                dropedItem.GetComponent<DroppedItemScript>().SetItem(toolsList[toolNumberSelected], toolsSprites);
                dropedItem.GetComponent<DroppedItemScript>().ThrowItemToFloor();
                RemoveItemFromToolbar(toolNumberSelected);
            }
        }
        else
        {
            if (!itemGrabbed.empty)
            {
                GameObject dropedItem = Instantiate(droppedItemPrefab);
                dropedItem.GetComponent<DroppedItemScript>().SetItem(itemGrabbed, toolsSprites);
                dropedItem.GetComponent<DroppedItemScript>().ThrowItemToFloor();
                RemoveItemFromToolbar(itemGrabbed.toolbarIndex);
            }
            DestroyGrabbingItem();
        }
    }

    public void DropItemToToolbar(int itemIndex, bool itemFromChest)
    {
        if (toolsList[itemIndex].empty)
        {
            toolsList[itemIndex] = itemGrabbed;
            itemGrabbed.toolbarIndex = itemIndex;
            SetToolbarItemUI(itemGrabbed);
        }
        else
        {
            if (toolsList[itemIndex].name == itemGrabbed.name && toolsList[itemIndex].isCountable)
            {
                itemGrabbed.toolbarIndex = itemIndex;
                itemGrabbed.countItems += toolsList[itemIndex].countItems;
                toolsList[itemIndex] = itemGrabbed;
                SetToolbarItemUI(itemGrabbed);
            }
            else
            {
                if (itemFromChest)
                {
                    foreach (GameObject chest in chestsInScene)
                    {
                        if (chest.GetComponent<ChestScript>().chestOpened)
                        {
                            chest.GetComponent<ChestScript>().itemGrabbed = itemGrabbed;
                            chest.GetComponent<ChestScript>().DropItemToToolbarChest(itemGrabbed.toolbarIndex, false);
                        }
                    }
                }
                else
                {
                    toolsList[itemGrabbed.toolbarIndex] = itemGrabbed;
                    SetToolbarItemUI(itemGrabbed);
                }
            }
        }
        DestroyGrabbingItem();
    }

    public bool SetNewItemTool(Tool newTool)
    {
        for (int i = 0; i <= toolsList.Length; i++)
        {
            if (toolsList[i].empty)
            {
                newTool.toolbarIndex = i;
                toolsList[i] = newTool;
                SetToolbarItemUI(newTool);
                return true;
            }
        }
        return false;
    }

    public void InputMouseScroll(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            var value = context.ReadValue<Vector2>();
            if (value.y < 0) ChangeToolUpwards();
            if (value.y > 0) ChangeToolBackwards();
        }
    }

    public void InputUseTool(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

        }
    }

    public void InputDropItem(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DropItemToFloor(true);
        }
    }
}
