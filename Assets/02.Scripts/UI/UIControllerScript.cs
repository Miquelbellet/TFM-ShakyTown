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
    public GameObject menuNotesObject;
    public GameObject closeNotesBtn;

    [Header("Tool Bar Objects")]
    public GameObject toolsImagesObjectsParent;

    [Header("Chest Bar Objects")]
    public GameObject chestImagesObjectsParent;

    [Header("Dialog Text Objects")]
    public GameObject dialogTextParent;
    public GameObject dialogTextObject;
    [HideInInspector] public bool dialogActivated;

    private bool menuActivated;
    private bool mapActivated;

    public void SetToolBarItem(Sprite toolSprite, Tool tool)
    {
        toolsImagesObjectsParent.transform.GetChild(tool.toolbarIndex).GetComponent<Image>().sprite = toolSprite;
        if (tool.isCountable)
        {
            toolsImagesObjectsParent.transform.GetChild(tool.toolbarIndex).transform.GetChild(0).transform.gameObject.SetActive(true);
            toolsImagesObjectsParent.transform.GetChild(tool.toolbarIndex).transform.GetChild(0).transform.GetComponent<TextMeshProUGUI>().text = tool.countItems.ToString();
        }
        else
        {
            toolsImagesObjectsParent.transform.GetChild(tool.toolbarIndex).transform.GetChild(0).transform.gameObject.SetActive(false);
        }
    }

    public void SetChestToolBarItem(Sprite toolSprite, Tool tool)
    {
        chestImagesObjectsParent.transform.GetChild(tool.toolbarIndex).GetComponent<Image>().sprite = toolSprite;
        if (tool.isCountable)
        {
            chestImagesObjectsParent.transform.GetChild(tool.toolbarIndex).transform.GetChild(0).transform.gameObject.SetActive(true);
            chestImagesObjectsParent.transform.GetChild(tool.toolbarIndex).transform.GetChild(0).transform.GetComponent<TextMeshProUGUI>().text = tool.countItems.ToString();
        }
        else
        {
            chestImagesObjectsParent.transform.GetChild(tool.toolbarIndex).transform.GetChild(0).transform.gameObject.SetActive(false);
        }
    }

    public void ActivateChestUI()
    {
        chestImagesObjectsParent.SetActive(true);
    }

    public void DeactivateChestUI()
    {
        chestImagesObjectsParent.SetActive(false);
    }

    public void ShowDialogText(string text)
    {
        dialogActivated = true;
        dialogTextParent.SetActive(true);
        Time.timeScale = 0;
        StopAllCoroutines();
        StartCoroutine(WriteSentence(text));
    }

    IEnumerator WriteSentence(string text)
    {
        dialogTextObject.GetComponent<TextMeshProUGUI>().text = "";
        foreach (char letter in text.ToCharArray())
        {
            dialogTextObject.GetComponent<TextMeshProUGUI>().text += letter;
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }

    public void EndDialog()
    {
        dialogActivated = false;
        dialogTextParent.SetActive(false);
        Time.timeScale = 1;
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
        menuNotesObject.SetActive(false);
        closeNotesBtn.SetActive(false);
        menuActivated = false;
        Time.timeScale = 1;
    }

    public void OpenNotesMenu()
    {
        menuObject.SetActive(false);
        closeMenuBtn.SetActive(false);
        menuNotesObject.SetActive(true);
        closeNotesBtn.SetActive(true);
    }

    public void CloseNotesMenu()
    {
        menuObject.SetActive(true);
        closeMenuBtn.SetActive(true);
        menuNotesObject.SetActive(false);
        closeNotesBtn.SetActive(false);
    }

    public void ConfigurationBtn()
    {

    }

    public void SaveAndReturnBtn()
    {
        SceneManager.LoadScene("Menu");
    }
}
