using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;

public class BlacksmithControllerScript : MonoBehaviour
{
    public GameObject droppedItemPrefab;
    public GameObject sellingImagesItems;
    public TextMeshProUGUI sellingValueObject;

    ResourcesManagmentScript resourcesManagmentScript;
    GameObject gameController;

    private bool shopOpened;
    private Tool[] currentItemCost;
    private Tool buyedToolItem;
    private Tool[] sellingItems = new Tool[3];

    void Start()
    {
        resourcesManagmentScript = new ResourcesManagmentScript();
        gameController = GameObject.FindGameObjectWithTag("GameController");
        SetArraySellingItems();
    }

    void SetArraySellingItems()
    {
        for (int i = 0; i < sellingItems.Length; i++)
        {
            sellingItems[i] = new Tool();
        }
    }

    public void ShowDialog()
    {
        if (gameController.GetComponent<UIControllerScript>().dialogActivated)
        {
            gameController.GetComponent<UIControllerScript>().EndDialog();
            if(!gameController.GetComponent<UIControllerScript>().dialogActivated) OpenStore();
        }
        else
        {
            if (!shopOpened)
            {
                StreamReader linesDocument = resourcesManagmentScript.ReadDataFromResource("Assets/Resources/npcs/blacksmith_lines.txt");
                string numberLines = linesDocument.ReadLine();
                int.TryParse(numberLines, out int numLines);
                var randomLine = Random.Range(0, numLines);
                for (int i = 0; i < numLines; i++)
                {
                    string noteStr = linesDocument.ReadLine();
                    Note note = Note.CreateFromJSON(noteStr);
                    if (note.index == randomLine) gameController.GetComponent<UIControllerScript>().ShowDialogText(note.text);
                }
                linesDocument.Close();
            }
            else
            {
                CloseStore();
            }
        }
    }

    public void OpenStore()
    {
        shopOpened = true;
        gameController.GetComponent<UIControllerScript>().OpenBlacksmithShop();
    }

    public void CloseStore()
    {
        gameController.GetComponent<UIControllerScript>().CloseBlacksmithShop();
        shopOpened = false;
    }

    public void CheckIfPlayerHaveItems(string itemName)
    {
        Tool[] itemCost = new Tool[3];
        StreamReader costItemDoc = resourcesManagmentScript.ReadDataFromResource("Assets/Resources/npcs/blacksmith_shop.txt");
        string numberLines = costItemDoc.ReadLine();
        int.TryParse(numberLines, out int numLines);
        for (int i = 0; i < numLines; i++)
        {
            string itemNameStr = costItemDoc.ReadLine();
            if (itemNameStr == itemName)
            {
                string buyedItem = costItemDoc.ReadLine();
                buyedToolItem = Tool.CreateFromJSON(buyedItem);
                for (int n = 0; n < 3; n++)
                {
                    string item = costItemDoc.ReadLine();
                    Tool costTool = Tool.CreateFromJSON(item);
                    itemCost[n] = costTool;
                }
                break;
            }
        }
        costItemDoc.Close();

        Tool[] toolbar = gameController.GetComponent<ToolBarScript>().toolsList;
        bool haveAllItemsCost = true;
        for (int i = 0; i < itemCost.Length; i++)
        {
            bool haveItem = false;
            foreach (Tool tool in toolbar)
            {
                if(itemCost[i].name == tool.name || itemCost[i].name == "empty")
                {
                    if (itemCost[i].isCountable)
                    {
                        if (itemCost[i].countItems <= tool.countItems)
                        {
                            haveItem = true;
                        }
                    }
                    else
                    {
                        haveItem = true;
                    }
                }
            }

            if (!haveItem)
            {
                haveAllItemsCost = false;
                break;
            }
        }
        currentItemCost = itemCost;
        gameController.GetComponent<UIControllerScript>().SetConfirmBuy(itemCost, haveAllItemsCost);
    }

    public void ConfirmBuyingItem()
    {
        RefreshPlayerToolbar(currentItemCost);
        SetNewToolbarItem(buyedToolItem);
        gameController.GetComponent<UIControllerScript>().CloseConfirmBlacksmithBuy();
        currentItemCost = null;
        buyedToolItem = null;
    }

    void RefreshPlayerToolbar(Tool[] itemsCost)
    {
        Tool[] toolbar = gameController.GetComponent<ToolBarScript>().toolsList;
        for (int i = 0; i < itemsCost.Length; i++)
        {
            foreach (Tool tool in toolbar)
            {
                if (itemsCost[i].name == tool.name)
                {
                    if (itemsCost[i].isCountable)
                    {
                        if ((tool.countItems - itemsCost[i].countItems) > 0)
                        {
                            toolbar[tool.toolbarIndex].countItems -= itemsCost[i].countItems;
                        }
                        else
                        {
                            Tool newTool = new Tool();
                            newTool.toolbarIndex = tool.toolbarIndex;
                            toolbar[tool.toolbarIndex] = newTool;
                        }

                    }
                    else
                    {
                        Tool newTool = new Tool();
                        newTool.toolbarIndex = tool.toolbarIndex;
                        toolbar[tool.toolbarIndex] = newTool;
                    }
                }
            }
        }
        gameController.GetComponent<ToolBarScript>().toolsList = toolbar;
        gameController.GetComponent<ToolBarScript>().RefreshToolbarItems();
    }

    public void CancelBuyingItem()
    {
        currentItemCost = null;
        buyedToolItem = null;
        gameController.GetComponent<UIControllerScript>().CloseConfirmBlacksmithBuy();
    }

    void SetNewToolbarItem(Tool newItem)
    {
        bool catchedItem = gameController.GetComponent<ToolBarScript>().SetNewItemTool(newItem);
        if (!catchedItem)
        {
            GameObject dropedItem = Instantiate(droppedItemPrefab);
            dropedItem.GetComponent<DroppedItemScript>().SetItem(newItem);
            dropedItem.GetComponent<DroppedItemScript>().ThrowItemToFloor();
        }
    }

    public void SetSellingItem(int itemImageIndex, Tool sellingItem)
    {
        if (sellingItems[itemImageIndex].empty)
        {
            sellingItems[itemImageIndex] = sellingItem;
            gameController.GetComponent<UIControllerScript>().SetSellingImageItem(sellingItem, sellingImagesItems.transform.GetChild(itemImageIndex));
            int sellingValue = 0;
            foreach (Tool item in sellingItems)
            {
                if (!item.empty)
                {
                    if (item.isCountable)
                    {
                        sellingValue += item.goldValue * item.countItems;
                    }
                    else{
                        sellingValue += item.goldValue;
                    }
                }
            }
            sellingValueObject.text = sellingValue.ToString();
        }
        else
        {
            SetNewToolbarItem(sellingItem);
        }
    }

    public void SellItems()
    {
        int sellingValue = 0;
        foreach (Tool item in sellingItems)
        {
            if (!item.empty)
            {
                if (item.isCountable)
                {
                    sellingValue += item.goldValue * item.countItems;
                }
                else
                {
                    sellingValue += item.goldValue;
                }
            }
        }

        Tool goldItem = new Tool();
        goldItem.empty = false;
        goldItem.name = "gold";
        goldItem.spriteName = "spritesheet_107";
        goldItem.isCountable = true;
        goldItem.countItems = sellingValue;
        SetNewToolbarItem(goldItem);
        for (int i = 0; i < sellingItems.Length; i++)
        {
            gameController.GetComponent<UIControllerScript>().SetSellingImageItem(new Tool(), sellingImagesItems.transform.GetChild(i));
        }
        sellingValueObject.text = "0";
        SetArraySellingItems();
    }

    public void ClearSellingItems()
    {
        for (int i = 0; i < sellingItems.Length; i++)
        {
            if (!sellingItems[i].empty)
            {
                SetNewToolbarItem(sellingItems[i]);
            }
            gameController.GetComponent<UIControllerScript>().SetSellingImageItem(new Tool(), sellingImagesItems.transform.GetChild(i));
        }
        sellingValueObject.text = "0";
        SetArraySellingItems();
    }
}
