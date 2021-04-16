using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    ResourcesManagmentScript resourcesManagmentScript;

    private GameObject gameController;
    private GameObject interactableObject;
    private bool openChest;
    private bool momTalk;
    private bool bossTalk;
    private bool blacksmithTalk;
    private bool witchTalk;

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        resourcesManagmentScript = new ResourcesManagmentScript();
        if(SceneManager.GetActiveScene().name != "InitTutorial") SetPlayerSettings();
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
        GetComponent<PlayerHealthScript>().initLifes = playerHealth;
        string playerFullHealthStr = playerSettings.ReadLine();
        int.TryParse(playerFullHealthStr, out int playerFullHealth);
        GetComponent<PlayerHealthScript>().actualFullPlayerLifes = playerFullHealth;
        GetComponent<PlayerHealthScript>().SetPlayerHearts();

        playerSettings.Close();
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
            PlayerPrefs.SetString("tutorial", "true");
            SceneManager.LoadScene("Main");
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
        else if (other.tag == "Boss")
        {
            bossTalk = false;
            interactableObject = null;
        }
        else if (other.tag == "Blacksmith")
        {
            interactableObject.GetComponent<BlacksmithControllerScript>().CloseStore();
            blacksmithTalk = false;
            interactableObject = null;
        }
        else if (other.tag == "Witch")
        {
            witchTalk = false;
            interactableObject = null;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            GetComponent<PlayerMovementScript>().stopRollMove = true;
        }
    }
}
