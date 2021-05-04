using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{

    public GameObject noteObjPrefab;
    public GameObject dadFinalObject;
    public GameObject bossTownObject;

    [HideInInspector] public bool blacksmithTalk;
    [HideInInspector] public bool witchTalk;

    ResourcesManagmentScript resourcesManagmentScript;

    private GameObject gameController;
    private GameObject interactableObject;
    private bool openChest;
    private bool momTalk;
    private bool dadTalk;
    private bool bossTalk;
    private bool killAllNote;

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        resourcesManagmentScript = new ResourcesManagmentScript();
        if(SceneManager.GetActiveScene().name != "InitTutorial")
        {
            SetPlayerSettings();
            if (!resourcesManagmentScript.GetGameVariable("welcomeTown"))
            {
                DropNoteItem(5);
                resourcesManagmentScript.SetGameVariable("welcomeTown", true);
            }
            if (resourcesManagmentScript.GetGameVariable("gameEnded"))
            {
                dadFinalObject.SetActive(true);
                bossTownObject.SetActive(false);
            }
        }
    }

    void SetPlayerSettings()
    {
        StreamReader playerSettings = resourcesManagmentScript.ReadDataFromResource("Assets/Resources/player_settings.txt");

        string numLevelStr = playerSettings.ReadLine();
        int.TryParse(numLevelStr, out int numLevel);
        gameController.GetComponent<LevelControllerScript>().SetActiveLevel(numLevel);

        string playerPosXStr = playerSettings.ReadLine();
        float.TryParse(playerPosXStr, out float playerPosX);
        string playerPosYStr = playerSettings.ReadLine();
        float.TryParse(playerPosYStr, out float playerPosY);
        transform.position = new Vector2(playerPosX, playerPosY);

        string playerHealthStr = playerSettings.ReadLine();
        float.TryParse(playerHealthStr, out float playerHealth);
        GetComponent<PlayerHealthScript>().currentPlayerLifes = playerHealth;
        string playerFullHealthStr = playerSettings.ReadLine();
        int.TryParse(playerFullHealthStr, out int playerFullHealth);
        GetComponent<PlayerHealthScript>().actualFullPlayerLifes = playerFullHealth;
        GetComponent<PlayerHealthScript>().SetPlayerHearts();

        playerSettings.Close();
    }

    public void ChangeToMainScene()
    {
        resourcesManagmentScript.SetGameVariable("tutorial", true);
        SceneManager.LoadScene("Main");
    }

    public void SavePlayerSettings()
    {
        string path = "Assets/Resources/player_settings.txt";
        StreamWriter playerSettings = resourcesManagmentScript.WriteDataToResource(path);

        playerSettings.WriteLine(gameController.GetComponent<LevelControllerScript>().currentLevelNumber);
        playerSettings.WriteLine(transform.position.x);
        playerSettings.WriteLine(transform.position.y);
        playerSettings.WriteLine(GetComponent<PlayerHealthScript>().currentPlayerLifes);
        playerSettings.WriteLine(GetComponent<PlayerHealthScript>().actualFullPlayerLifes);

        playerSettings.Close();
    }

    public void InputAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (openChest)
            {
                if (interactableObject.GetComponent<ChestScript>().chestOpened) interactableObject.GetComponent<ChestScript>().CloseChest();
                else interactableObject.GetComponent<ChestScript>().OpenChest();
            }
            else if (momTalk)
            {
                interactableObject.GetComponent<MomControllerScript>().ShowDialog();
            }
            else if (dadTalk)
            {
                interactableObject.GetComponent<DadControllerScript>().ShowDialog();
            }
            else if (bossTalk)
            {
                interactableObject.GetComponent<BossControllerScript>().ShowDialog();
            }
            else if (blacksmithTalk)
            {
                interactableObject.GetComponent<BlacksmithControllerScript>().ShowDialog();
            }
            else if (witchTalk)
            {
                interactableObject.GetComponent<WitchControllerScript>().ShowDialog();
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "EndTutorialTrigger")
        {
            gameController.GetComponent<LevelControllerScript>().FadeInAnimation();
        }

        if (other.tag == "Chest")
        {
            openChest = true;
            interactableObject = other.gameObject;
        }
        else if (other.tag == "Mom")
        {
            momTalk = true;
            interactableObject = other.gameObject;
        }
        else if (other.tag == "Dad")
        {
            dadTalk = true;
            interactableObject = other.gameObject;
        }
        else if (other.tag == "Boss")
        {
            bossTalk = true;
            interactableObject = other.gameObject;
        }
        else if (other.tag == "Blacksmith")
        {
            blacksmithTalk = true;
            interactableObject = other.gameObject;
        }
        else if (other.tag == "Witch")
        {
            witchTalk = true;
            interactableObject = other.gameObject;
        }
        else if (other.tag == "Trigger1")
        {
            gameController.GetComponent<LevelControllerScript>().PlayerChangedLevel(1);
        }
        else if (other.tag == "Trigger2")
        {
            gameController.GetComponent<LevelControllerScript>().PlayerChangedLevel(2);
        }
        else if (other.tag == "Trigger3")
        {
            gameController.GetComponent<LevelControllerScript>().PlayerChangedLevel(3);
        }
        else if (other.tag == "Trigger4")
        {
            if (gameController.GetComponent<LevelControllerScript>().CheckEnemiesInLevel(4)) gameController.GetComponent<LevelControllerScript>().PlayerChangedLevel(4);
            else if(!killAllNote)
            {
                killAllNote = true;
                DropNoteItem(14);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Chest")
        {
            try { interactableObject.GetComponent<ChestScript>().CloseChest(); } catch { }
            openChest = false;
            interactableObject = null;
        }
        else if (other.tag == "Mom")
        {
            momTalk = false;
            interactableObject = null;
        }
        else if (other.tag == "Dad")
        {
            dadTalk = false;
            interactableObject = null;
        }
        else if (other.tag == "Boss")
        {
            bossTalk = false;
            interactableObject = null;
        }
        else if (other.tag == "Blacksmith")
        {
            try{ interactableObject.GetComponent<BlacksmithControllerScript>().CloseStore(); } catch { }
            blacksmithTalk = false;
            interactableObject = null;
        }
        else if (other.tag == "Witch")
        {
            try { interactableObject.GetComponent<WitchControllerScript>().CloseStore(); } catch { }
            witchTalk = false;
            interactableObject = null;
        }
    }

    public void DropNoteItem(int noteIndex)
    {
        GameObject noteObj = Instantiate(noteObjPrefab, transform.position, Quaternion.Euler(0,0,0));
        noteObj.GetComponent<SpriteRenderer>().enabled = false;
        noteObj.GetComponent<NotesScript>().noteIndex = noteIndex;
    }

    public void ReturnToHouse()
    {
        gameController.GetComponent<LevelControllerScript>().PlayerChangedLevel(1, true);
        resourcesManagmentScript.SetGameVariable("gameEnded", true);
        resourcesManagmentScript.SetGameVariable("showNewGameBtn", true);
        dadFinalObject.SetActive(true);
        bossTownObject.SetActive(false);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            GetComponent<PlayerMovementScript>().stopRollMove = true;
        }
    }
}
