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

    [Header("Items Objects")]
    public GameObject toolsImagesObjectsParent;
    public GameObject chestImagesObjectsParent;

    [Header("Blacksmith Shop")]
    public GameObject blacksmithShop;
    public GameObject confirmBlacksmithBuy;
    public GameObject confirmBuyButton;
    public GameObject noteEnoughItemsText;
    public GameObject[] costItemImages;

    [Header("Dialog Text Objects")]
    public GameObject dialogTextParent;
    public GameObject dialogTextObject;
    [HideInInspector] public bool dialogActivated;

    private Sprite[] toolsSprites;
    private bool menuActivated;
    private bool mapActivated;
    private bool showingDialog;
    private string dialogText;

    private void Start()
    {
        toolsSprites = Resources.LoadAll<Sprite>("Tools");
    }

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
        showingDialog = true;
        dialogText = text;
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
        showingDialog = false;
    }

    public void EndDialog()
    {
        if (showingDialog)
        {
            StopAllCoroutines();
            dialogTextObject.GetComponent<TextMeshProUGUI>().text = dialogText;
            showingDialog = false;
        }
        else
        {
            dialogActivated = false;
            dialogTextParent.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void SetConfirmBuy(Tool[] costItem, bool haveItems)
    {
        for (int i = 0; i < costItem.Length; i++)
        {
            if (!costItem[i].empty)
            {
                foreach (Sprite toolSp in toolsSprites)
                {
                    if (toolSp.name == costItem[i].spriteName)
                    {
                        costItemImages[i].GetComponent<Image>().sprite = toolSp;
                        if (costItem[i].isCountable)
                        {
                            costItemImages[i].transform.GetChild(0).transform.gameObject.SetActive(true);
                            costItemImages[i].transform.GetChild(0).transform.GetComponent<TextMeshProUGUI>().text = costItem[i].countItems.ToString();
                        }
                        else costItemImages[i].transform.GetChild(0).transform.gameObject.SetActive(false);
                    }
                }
            }
        }
        confirmBlacksmithBuy.SetActive(true);
        if (haveItems)
        {
            confirmBuyButton.GetComponent<Button>().interactable = true;
            noteEnoughItemsText.SetActive(false);
        }
        else
        {
            confirmBuyButton.GetComponent<Button>().interactable = false;
            noteEnoughItemsText.SetActive(true);
        }
    }

    public void SetSellingImageItem(Tool tool, Transform imageTransform)
    {
        if (!tool.empty)
        {
            foreach (Sprite toolSp in toolsSprites)
            {
                if (toolSp.name == tool.spriteName)
                {
                    imageTransform.GetComponent<Image>().sprite = toolSp;
                    if (tool.isCountable)
                    {
                        imageTransform.GetChild(0).transform.gameObject.SetActive(true);
                        imageTransform.GetChild(0).transform.GetComponent<TextMeshProUGUI>().text = tool.countItems.ToString();
                    }
                    else
                    {
                        imageTransform.GetChild(0).transform.gameObject.SetActive(false);
                    }
                }
            }
        }
        else
        {
            imageTransform.GetComponent<Image>().sprite = null;
            imageTransform.GetChild(0).transform.gameObject.SetActive(false);
        }
    }

    public void CloseConfirmBlacksmithBuy()
    {
        confirmBlacksmithBuy.SetActive(false);
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

    public void OpenBlacksmithShop()
    {
        Time.timeScale = 0;
        blacksmithShop.SetActive(true);
    }

    public void CloseBlacksmithShop()
    {
        blacksmithShop.SetActive(false);
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
