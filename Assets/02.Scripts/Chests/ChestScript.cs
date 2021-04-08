using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChestScript : MonoBehaviour
{
    public int chestId;
    public GameObject droppedItemPrefab;
    [HideInInspector] public bool chestOpened;
    [HideInInspector] public Tool itemGrabbed;

    ResourcesManagmentScript resourcesManagmentScript;
    GameObject gameController;
    GameObject canvasObject;
    GameObject playerHandItem;
    GameObject chestImagesPartentObject;

    private Tool[] chestObjectsList = new Tool[0];
    private Sprite[] toolsSprites;
    private int numberTotalObjects = 23;
    private Animator chestAnimatorController;
    private bool grabbingItem;
    private GameObject grabbedItemObject;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        canvasObject = GameObject.FindGameObjectWithTag("Canvas");
        playerHandItem = GameObject.FindGameObjectWithTag("HandItem");
        resourcesManagmentScript = new ResourcesManagmentScript();
        chestAnimatorController = GetComponent<Animator>();
        toolsSprites = Resources.LoadAll<Sprite>("Tools");
    }

    void Update()
    {
        if (grabbingItem)
        {
            try
            {
                Vector2 mousePoistion = playerHandItem.GetComponent<AttackItemScript>().mousePosition;
                grabbedItemObject.transform.position = mousePoistion;
            }
            catch { }
        }
    }

    void SetChestToolUI(Tool tool)
    {
        if (!tool.empty)
        {
            foreach (Sprite toolSp in toolsSprites)
            {
                if (toolSp.name == tool.spriteName)
                {
                    gameController.GetComponent<UIControllerScript>().SetChestToolBarItem(toolSp, tool);
                }
            }
        }
        else gameController.GetComponent<UIControllerScript>().SetChestToolBarItem(null, tool);
    }

    void RemoveItemFromToolbar(int toolIndex)
    {
        Tool tool = new Tool();
        tool.toolbarIndex = toolIndex;
        chestObjectsList[toolIndex] = tool;
        gameController.GetComponent<UIControllerScript>().SetChestToolBarItem(null, tool);
    }

    void DestroyGrabbingItem()
    {
        grabbingItem = false;
        itemGrabbed = null;
        Destroy(grabbedItemObject);
        grabbedItemObject = null;
    }

    public void OpenChest()
    {
        if(chestObjectsList.Length == 0)
        {
            StreamReader chestObjects = resourcesManagmentScript.ReadDataFromResource("Assets/Resources/chests/chest_" + chestId + ".txt");
            chestObjectsList = new Tool[numberTotalObjects + 1];
            gameController.GetComponent<UIControllerScript>().ActivateChestUI();
            chestAnimatorController.SetBool("open", true);
            SetButtonsEvents();
            chestOpened = true;
            for (int i = 0; i <= numberTotalObjects; i++)
            {
                string toolStr = chestObjects.ReadLine();
                Tool tool = Tool.CreateFromJSON(toolStr);
                if(tool == null)
                {
                    tool = new Tool();
                    tool.toolbarIndex = i;
                }
                chestObjectsList[i] = tool;
                SetChestToolUI(tool);
            }
            chestObjects.Close();
        }
        else
        {
            gameController.GetComponent<UIControllerScript>().ActivateChestUI();
            chestAnimatorController.SetBool("open", true);
            SetButtonsEvents();
            chestOpened = true;
            foreach (Tool tool in chestObjectsList)
            {
                SetChestToolUI(tool);
            }
        }
    }

    void SetButtonsEvents()
    {
        chestImagesPartentObject = GameObject.FindGameObjectWithTag("ChestImagesParent");
        for (int i = 1; i <= chestImagesPartentObject.transform.childCount; i++)
        {
            int x = i - 1;
            Destroy(chestImagesPartentObject.transform.GetChild(x).gameObject.GetComponent<EventTrigger>());
            EventTrigger trigger = chestImagesPartentObject.transform.GetChild(x).gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.BeginDrag;
            entry.callback.AddListener((eventData) => { DragItemFromToolbar(x); });
            trigger.triggers.Add(entry);
        }
    }

    void DragItemFromToolbar(int indexImage)
    {
        if (!grabbingItem)
        {
            grabbingItem = true;
            itemGrabbed = chestObjectsList[indexImage];
            RemoveItemFromToolbar(indexImage);
            if (!itemGrabbed.empty)
            {
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

    void DropItem()
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
                if (res.gameObject.tag == "ToolbarItemChest")
                {
                    itemDropped = true;
                    DropItemToToolbarChest(res.gameObject.transform.GetSiblingIndex(), false);
                }
                else if (res.gameObject.tag == "ToolbarItem")
                {
                    itemDropped = true;
                    DropItemToToolbar(res.gameObject.transform.GetSiblingIndex());
                }
            }
        }
        if (!itemDropped) DropItemGrabbed();
    }

    public void DropItemToToolbarChest(int indexImage, bool itemFromToolbar)
    {
        if (chestObjectsList[indexImage].empty)
        {
            itemGrabbed.toolbarIndex = indexImage;
            chestObjectsList[indexImage] = itemGrabbed;
            SetChestToolUI(itemGrabbed);
        }
        else
        {
            if (chestObjectsList[indexImage].name == itemGrabbed.name && chestObjectsList[indexImage].isCountable)
            {
                itemGrabbed.toolbarIndex = indexImage;
                itemGrabbed.countItems += chestObjectsList[indexImage].countItems;
                chestObjectsList[indexImage] = itemGrabbed;
                SetChestToolUI(itemGrabbed);
            }
            else
            {
                if (itemFromToolbar)
                {
                    gameController.GetComponent<ToolBarScript>().itemGrabbed = itemGrabbed;
                    gameController.GetComponent<ToolBarScript>().DropItemToToolbar(itemGrabbed.toolbarIndex, false);
                }
                else
                {
                    chestObjectsList[itemGrabbed.toolbarIndex] = itemGrabbed;
                    SetChestToolUI(itemGrabbed);
                }
            }
        }
        DestroyGrabbingItem();
    }

    void DropItemToToolbar(int indexToolbar)
    {
        gameController.GetComponent<ToolBarScript>().itemGrabbed = itemGrabbed;
        gameController.GetComponent<ToolBarScript>().DropItemToToolbar(indexToolbar, true);
        DestroyGrabbingItem();
    }

    void DropItemGrabbed()
    {
        if (!itemGrabbed.empty)
        {
            GameObject dropedItem = Instantiate(droppedItemPrefab);
            dropedItem.GetComponent<DroppedItemScript>().SetItem(itemGrabbed);
            dropedItem.GetComponent<DroppedItemScript>().ThrowItemToFloor();
            RemoveItemFromToolbar(itemGrabbed.toolbarIndex);
        }
        DestroyGrabbingItem();
    }

    public void CloseChest()
    {
        gameController.GetComponent<UIControllerScript>().DeactivateChestUI();
        chestAnimatorController.SetBool("open", false);
        chestOpened = false;
    }

    public void SaveChestItems()
    {
        string path = "Assets/Resources/chests/chest_" + chestId + ".txt";
        StreamWriter chestObjects = resourcesManagmentScript.WriteDataToResource(path);
        foreach (Tool toolObj in chestObjectsList)
        {
            string toolStr = Tool.CreateFromObject(toolObj);
            chestObjects.WriteLine(toolStr);
        }
        chestObjects.Close();
    }
}
