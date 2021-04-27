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
    public TextMeshProUGUI titleLevelMap;
    public Image levelImageObj;
    public Sprite[] levelImages;
    public GameObject menuNotesObject;
    public GameObject closeNotesBtn;

    [Header("Items Objects")]
    public GameObject toolsImagesObjectsParent;
    public GameObject chestImagesObjectsParent;

    [Header("Blacksmith Shop")]
    public GameObject blacksmithShop;
    public GameObject confirmBlacksmithBuy;
    public GameObject confirmBlacksmithBuyButton;
    public GameObject notEnoughItemsBlacksmithText;
    public GameObject[] costBlacksmithItemImages;

    [Header("Witch Shop")]
    public GameObject witchShop;
    public GameObject confirmWitchBuy;
    public GameObject confirmWitchBuyButton;
    public GameObject notEnoughWitchItemsText;
    public GameObject[] costWitchItemImages;

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
        Time.timeScale = 1;
    }

    public void SetToolBarItem(Sprite toolSprite, Tool tool)
    {
        toolsImagesObjectsParent.transform.GetChild(tool.toolbarIndex).GetChild(0).GetComponent<Image>().sprite = toolSprite;
        if (toolSprite == null) toolsImagesObjectsParent.transform.GetChild(tool.toolbarIndex).GetChild(0).GetComponent<Image>().enabled = false;
        else toolsImagesObjectsParent.transform.GetChild(tool.toolbarIndex).GetChild(0).GetComponent<Image>().enabled = true;
        if (tool.isCountable)
        {
            toolsImagesObjectsParent.transform.GetChild(tool.toolbarIndex).GetChild(0).GetChild(0).gameObject.SetActive(true);
            toolsImagesObjectsParent.transform.GetChild(tool.toolbarIndex).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = tool.countItems.ToString();
        }
        else
        {
            toolsImagesObjectsParent.transform.GetChild(tool.toolbarIndex).GetChild(0).GetChild(0).gameObject.SetActive(false);
        }
    }

    public void SetChestToolBarItem(Sprite toolSprite, Tool tool)
    {
        chestImagesObjectsParent.transform.GetChild(tool.toolbarIndex).GetChild(0).GetComponent<Image>().sprite = toolSprite;
        if (toolSprite == null) chestImagesObjectsParent.transform.GetChild(tool.toolbarIndex).GetChild(0).GetComponent<Image>().enabled = false;
        else chestImagesObjectsParent.transform.GetChild(tool.toolbarIndex).GetChild(0).GetComponent<Image>().enabled = true;
        if (tool.isCountable)
        {
            chestImagesObjectsParent.transform.GetChild(tool.toolbarIndex).GetChild(0).GetChild(0).gameObject.SetActive(true);
            chestImagesObjectsParent.transform.GetChild(tool.toolbarIndex).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = tool.countItems.ToString();
        }
        else
        {
            chestImagesObjectsParent.transform.GetChild(tool.toolbarIndex).GetChild(0).GetChild(0).gameObject.SetActive(false);
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

    public bool ShowDialogText(string text)
    {
        if (!dialogActivated)
        {
            dialogActivated = true;
            showingDialog = true;
            dialogText = text;
            dialogTextParent.SetActive(true);
            Time.timeScale = 0;
            StopAllCoroutines();
            StartCoroutine(WriteSentence(text));
            return true;
        }
        else return false;
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

    public void SetConfirmBlacksmithBuy(Tool[] costItem, bool haveItems)
    {
        for (int i = 0; i < costItem.Length; i++)
        {
            if (!costItem[i].empty)
            {
                foreach (Sprite toolSp in toolsSprites)
                {
                    if (toolSp.name == costItem[i].spriteName)
                    {
                        costBlacksmithItemImages[i].transform.GetChild(0).GetComponent<Image>().sprite = toolSp;
                        costBlacksmithItemImages[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                        if (costItem[i].isCountable)
                        {
                            costBlacksmithItemImages[i].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                            costBlacksmithItemImages[i].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = costItem[i].countItems.ToString();
                        }
                        else costBlacksmithItemImages[i].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                costBlacksmithItemImages[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                costBlacksmithItemImages[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                costBlacksmithItemImages[i].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            }
        }
        confirmBlacksmithBuy.SetActive(true);
        if (haveItems)
        {
            confirmBlacksmithBuyButton.GetComponent<Button>().interactable = true;
            notEnoughItemsBlacksmithText.SetActive(false);
        }
        else
        {
            confirmBlacksmithBuyButton.GetComponent<Button>().interactable = false;
            notEnoughItemsBlacksmithText.SetActive(true);
        }
    }

    public void SetConfirmWitchBuy(Tool[] costItem, bool haveItems)
    {
        for (int i = 0; i < costItem.Length; i++)
        {
            if (!costItem[i].empty)
            {
                foreach (Sprite toolSp in toolsSprites)
                {
                    if (toolSp.name == costItem[i].spriteName)
                    {
                        costWitchItemImages[i].transform.GetChild(0).GetComponent<Image>().sprite = toolSp;
                        costWitchItemImages[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                        if (costItem[i].isCountable)
                        {
                            costWitchItemImages[i].transform.GetChild(0).GetChild(0).transform.gameObject.SetActive(true);
                            costWitchItemImages[i].transform.GetChild(0).GetChild(0).transform.GetComponent<TextMeshProUGUI>().text = costItem[i].countItems.ToString();
                        }
                        else costWitchItemImages[i].transform.GetChild(0).GetChild(0).transform.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                costWitchItemImages[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                costWitchItemImages[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                costWitchItemImages[i].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            }
        }
        confirmWitchBuy.SetActive(true);
        if (haveItems)
        {
            confirmWitchBuyButton.GetComponent<Button>().interactable = true;
            notEnoughWitchItemsText.SetActive(false);
        }
        else
        {
            confirmWitchBuyButton.GetComponent<Button>().interactable = false;
            notEnoughWitchItemsText.SetActive(true);
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
                    imageTransform.GetChild(0).GetComponent<Image>().sprite = toolSp;
                    imageTransform.GetChild(0).GetComponent<Image>().enabled = true;
                    if (tool.isCountable)
                    {
                        imageTransform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                        imageTransform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = tool.countItems.ToString();
                    }
                    else
                    {
                        imageTransform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                    }
                }
            }
        }
        else
        {
            imageTransform.GetChild(0).GetComponent<Image>().sprite = null;
            imageTransform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            imageTransform.GetChild(0).GetComponent<Image>().enabled = false;
        }
    }

    public void CloseConfirmBlacksmithBuy()
    {
        confirmBlacksmithBuy.SetActive(false);
    }

    public void CloseConfirmWitchBuy()
    {
        confirmWitchBuy.SetActive(false);
    }

    public void InputMap(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (SceneManager.GetActiveScene().name != "InitTutorial")
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
    }

    public void InputMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (SceneManager.GetActiveScene().name != "InitTutorial")
            {
                if (!menuActivated)
                {
                    OpenMenuBtn();
                }
                else CloseMenuBtn();
            }
        }
    }

    public void OpenLevelSelected(int levelNumber)
    {
        mapObject.SetActive(false);
        titleLevelMap.text = "Level "+ levelNumber +" Map";
        levelImageObj.sprite = levelImages[levelNumber-1];
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
        GetComponent<NotesMenuScript>().SetEnabledNotes();
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

    public void OpenWitchShop()
    {
        Time.timeScale = 0;
        witchShop.SetActive(true);
    }

    public void CloseWitchShop()
    {
        witchShop.SetActive(false);
        Time.timeScale = 1;
    }

    public void ConfigurationBtn()
    {

    }

    public void SaveAndReturnBtn()
    {
        GetComponent<LevelControllerScript>().SaveGame();
        Invoke("GoToMenu", 1f);
    }

    void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
