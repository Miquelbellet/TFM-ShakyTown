using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControllerScript : MonoBehaviour
{

    public GameObject newGameBtn;
    public GameObject playBtn;
    public GameObject configBtn;
    public GameObject exitBtn;
    public GameObject configPanel;
    public float reduceY;

    ResourcesManagmentScript resourcesManagmentScript;

    void Start()
    {
        resourcesManagmentScript = new ResourcesManagmentScript();
        if (resourcesManagmentScript.GetGameVariable("showNewGameBtn"))
        {
            newGameBtn.SetActive(true);
        }
        else
        {
            newGameBtn.SetActive(false);
            playBtn.transform.position = new Vector2(playBtn.transform.position.x, playBtn.transform.position.y + reduceY);
            configBtn.transform.position = new Vector2(configBtn.transform.position.x, configBtn.transform.position.y + reduceY);
            exitBtn.transform.position = new Vector2(exitBtn.transform.position.x, exitBtn.transform.position.y + reduceY);
        }
    }

    void Update()
    {
        
    }

    public void PlayNewGameBtn()
    {
        GetComponent<SoundsControllerScript>().PlaySoundUI1();
        ClearResourcesTexts();
        SceneManager.LoadScene("Main");
    }

    void ClearResourcesTexts()
    {
        //Restart tools bar items
        string path1 = "Assets/Resources/tools_bar.txt";
        StreamWriter toolsBar = resourcesManagmentScript.WriteDataToResource(path1);
        for (int i = 0; i < 12; i++)
        {
            Tool toolObj = new Tool();
            toolObj.toolbarIndex = i;
            string toolStr = Tool.CreateFromObject(toolObj);
            toolsBar.WriteLine(toolStr);
        }
        toolsBar.Close();

        //Restart Player Settings
        string path2 = "Assets/Resources/player_settings.txt";
        StreamWriter playerSettings = resourcesManagmentScript.WriteDataToResource(path2);
        playerSettings.WriteLine("1");
        playerSettings.WriteLine("1,3");
        playerSettings.WriteLine("-2,6");
        playerSettings.WriteLine("6");
        playerSettings.WriteLine("6");
        playerSettings.Close();

        //Restart Initial Chest
        string path3 = "Assets/Resources/chests/chest_3.txt";
        StreamWriter initialChest = resourcesManagmentScript.WriteDataToResource(path3);
        Tool sword = new Tool();
        sword.toolbarIndex = 0;
        sword.empty = false;
        sword.name = "sword_1";
        sword.spriteName = "spritesheet_334";
        sword.isWeapon = true;
        sword.damage = 6;
        sword.goldValue = 4;
        string swordStr = Tool.CreateFromObject(sword);
        initialChest.WriteLine(swordStr);
        for (int i = 1; i < 24; i++)
        {
            Tool toolObj = new Tool();
            toolObj.toolbarIndex = i;
            string toolStr = Tool.CreateFromObject(toolObj);
            initialChest.WriteLine(toolStr);
        }
        initialChest.Close();

        //Restart Notes
        string path4 = "Assets/Resources/notes.txt";
        StreamReader notesReader = resourcesManagmentScript.ReadDataFromResource(path4);
        string numberLines = notesReader.ReadLine();
        int.TryParse(numberLines, out int numLines);
        Note[] notesList = new Note[numLines];
        for (int i = 0; i < numLines; i++)
        {
            string noteStr = notesReader.ReadLine();
            Note newNote = Note.CreateFromJSON(noteStr);
            notesList[i] = newNote;
        }
        notesReader.Close();

        StreamWriter notesWritter = resourcesManagmentScript.WriteDataToResource("Assets/Resources/notes.txt");
        notesWritter.WriteLine(notesList.Length);
        foreach (Note noteObj in notesList)
        {
            noteObj.shown = false;
            string noteObjStr = Note.CreateFromObject(noteObj);
            notesWritter.WriteLine(noteObjStr);
        }
        notesWritter.Close();

        //Restart Game Variables
        string path5 = "Assets/Resources/game_variables.txt";
        StreamReader varsReader = resourcesManagmentScript.ReadDataFromResource(path5);
        string numberVars = varsReader.ReadLine();
        int.TryParse(numberVars, out int numVars);
        GameVariable[] allVariables = new GameVariable[numVars];
        for (int i = 0; i < numVars; i++)
        {
            string varStr = varsReader.ReadLine();
            GameVariable variable = GameVariable.CreateFromJSON(varStr);
            allVariables[i] = variable;
        }
        varsReader.Close();

        StreamWriter varsWriter = resourcesManagmentScript.WriteDataToResource(path5);
        varsWriter.WriteLine(allVariables.Length);
        foreach (GameVariable vars in allVariables)
        {
            if(vars.name == "showNewGameBtn" || vars.name == "tutorial") vars.value = true;
            else vars.value = false;
            string varsStr = GameVariable.CreateFromObject(vars);
            varsWriter.WriteLine(varsStr);
        }
        varsWriter.Close();
    }

    public void PlayGameBtn()
    {
        GetComponent<SoundsControllerScript>().PlaySoundUI1();
        if (!resourcesManagmentScript.GetGameVariable("tutorial")) SceneManager.LoadScene("InitTutorial");
        else SceneManager.LoadScene("Main");
    }

    public void ConfigPanel()
    {
        if (configPanel.activeSelf)
        {
            GetComponent<SoundsControllerScript>().PlaySoundUI1();
        }
    }

    public void ExitGameBtn()
    {
        GetComponent<SoundsControllerScript>().PlaySoundUI1();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
