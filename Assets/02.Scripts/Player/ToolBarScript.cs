using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ToolBarScript : MonoBehaviour
{
    public AttackItemScript attackItemScript;
    public GameObject bordersObjectParent;
    public GameObject droppedItemPrefab;

    ResourcesManagmentScript resourcesManagmentScript;
    GameObject player;

    private Tool[] toolsList;
    private Sprite[] toolsSprites;
    private int numberOfToolsTotal;
    private int toolNumberSelected = 0;

    void Start()
    {
        numberOfToolsTotal = bordersObjectParent.transform.childCount-1;
        resourcesManagmentScript = new ResourcesManagmentScript();
        toolsSprites = Resources.LoadAll<Sprite>("Tools");
        player = GameObject.FindGameObjectWithTag("Player");
        SetObjectBorders();
        SetToolsForBar();
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

    void SetToolsForBar()
    {
        toolsList = new Tool[numberOfToolsTotal + 1];
        StreamReader toolsBar = resourcesManagmentScript.ReadDataFromResource("Assets/Resources/tools_bar.txt");
        for (int i = 0; i <= numberOfToolsTotal; i++)
        {
            string toolStr = toolsBar.ReadLine();
            Tool tool = Tool.CreateFromJSON(toolStr);
            toolsList[i] = tool;
            if(!tool.empty)
            {
                foreach (Sprite toolSp in toolsSprites)
                {
                    if (toolSp.name == tool.spriteName)
                    {
                        GetComponent<UIControllerScript>().SetToolBarItem(toolSp, tool);
                    }
                }
            }
        }
        attackItemScript.SetPlayerHandItem(JsonUtility.ToJson(toolsList[toolNumberSelected]), toolsSprites);
        toolsBar.Close();
    }

    public void SelectItemInToolbar(int itemIndex)
    {
        toolNumberSelected = itemIndex;
        SetObjectBorders();
    }

    public bool SetNewItemTool(Tool newTool)
    {
        for (int i = 0; i <= toolsList.Length; i++)
        {
            if (toolsList[i].empty)
            {
                newTool.toolbarIndex = i;
                toolsList[i] = newTool;
                if (!newTool.empty)
                {
                    foreach (Sprite toolSp in toolsSprites)
                    {
                        if (toolSp.name == newTool.spriteName)
                        {
                            GetComponent<UIControllerScript>().SetToolBarItem(toolSp, newTool);
                            if(toolNumberSelected == i) attackItemScript.SetPlayerHandItem(JsonUtility.ToJson(toolsList[i]), toolsSprites);
                            return true;
                        }
                    }
                }
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
            Debug.Log("Tool: "+ toolsList[toolNumberSelected].name);
        }
    }

    public void InputDropItem(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!toolsList[toolNumberSelected].empty)
            {
                GameObject dropedItem = Instantiate(droppedItemPrefab);
                dropedItem.GetComponent<DroppedItemScript>().SetItem(toolsList[toolNumberSelected], toolsSprites);
                dropedItem.GetComponent<DroppedItemScript>().ThrowItemToFloor();

                Tool tool = new Tool();
                tool.toolbarIndex = toolNumberSelected;
                toolsList[toolNumberSelected] = tool;
                GetComponent<UIControllerScript>().SetToolBarItem(null, tool);
                attackItemScript.SetPlayerHandItem(JsonUtility.ToJson(toolsList[toolNumberSelected]), toolsSprites);
            }
        }
    }
}
