using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DadControllerScript : MonoBehaviour
{
    ResourcesManagmentScript resourcesManagmentScript;
    GameObject gameController;

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
        }
        else
        {
            StreamReader linesDocument = resourcesManagmentScript.ReadDataFromResource("Assets/Resources/npcs/dad_lines.txt");
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
    }
}
