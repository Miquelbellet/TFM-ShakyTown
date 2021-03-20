using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BlacksmithControllerScript : MonoBehaviour
{

    ResourcesManagmentScript resourcesManagmentScript;
    GameObject gameController;

    private bool shopOpened;

    void Start()
    {
        resourcesManagmentScript = new ResourcesManagmentScript();
        gameController = GameObject.FindGameObjectWithTag("GameController");
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
}
