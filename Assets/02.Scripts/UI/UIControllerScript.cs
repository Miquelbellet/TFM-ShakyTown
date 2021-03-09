using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIControllerScript : MonoBehaviour
{
    [Header("Menu Objects")]
    public GameObject menuObject;
    public GameObject closeMenuBtn;
    public GameObject mapObject;
    public GameObject closeMapBtn;
    public GameObject mapLevelObject;

    [Header("Tool Bar Objects")]
    public GameObject imagesObjectsParent;

    private bool menuActivated;
    private bool mapActivated;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetToolBarItem(Sprite toolSprite, Tool tool)
    {
        imagesObjectsParent.transform.GetChild(tool.toolbarIndex).GetComponent<Image>().sprite = toolSprite;
        if (tool.isCountable)
        {
            imagesObjectsParent.transform.GetChild(tool.toolbarIndex).transform.GetChild(0).transform.gameObject.SetActive(true);
            imagesObjectsParent.transform.GetChild(tool.toolbarIndex).transform.GetChild(0).transform.GetComponent<TextMeshProUGUI>().text = tool.countItems.ToString();
        }
        else
        {
            imagesObjectsParent.transform.GetChild(tool.toolbarIndex).transform.GetChild(0).transform.gameObject.SetActive(false);
        }
    }

    public void InputMap(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!mapActivated)
            {
                OpenMapBtn();
            }
            else
            {
                CloseMapBtn();
            }
        }
    }

    public void InputMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!menuActivated)
            {
                OpenMenuBtn();
            }
            else CloseMenuBtn();
        }
    }

    public void OpenLevelSelected(int levelNumber)
    {
        mapObject.SetActive(false);
        mapLevelObject.SetActive(true);
    }

    public void OpenMapBtn()
    {
        CloseMenuBtn();
        Time.timeScale = 0;
        mapObject.SetActive(true);
        mapLevelObject.SetActive(false);
        closeMapBtn.SetActive(true);
        mapActivated = true;
    }

    public void CloseMapBtn()
    {
        Time.timeScale = 1;
        mapObject.SetActive(false);
        mapLevelObject.SetActive(false);
        closeMapBtn.SetActive(false);
        mapActivated = false;
    }

    public void OpenMenuBtn()
    {
        CloseMapBtn();
        Time.timeScale = 0;
        menuObject.SetActive(true);
        closeMenuBtn.SetActive(true);
        menuActivated = true;
    }

    public void CloseMenuBtn()
    {
        menuObject.SetActive(false);
        closeMenuBtn.SetActive(false);
        menuActivated = false;
        Time.timeScale = 1;
    }

    public void ConfigurationBtn()
    {

    }

    public void SaveAndReturnBtn()
    {
        SceneManager.LoadScene("Menu");
    }
}
