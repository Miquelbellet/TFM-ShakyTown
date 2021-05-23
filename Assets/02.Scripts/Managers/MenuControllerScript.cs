using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControllerScript : MonoBehaviour
{

    public GameObject newGameBtn;
    public GameObject playBtn;
    public GameObject configBtn;
    public GameObject exitBtn;
    public GameObject configPanel;
    public float reduceY;
    public Slider musicSlider;
    public AudioSource musicAS;
    public Slider soundsSlider;
    public AudioSource soundAS;
    public Button[] levelsDifficulty;

    ResourcesManagmentScript resourcesManagmentScript;

    private int levelDifficulty;
    private float musicVolume;
    private float soundVolume;

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

        GetConfigSettings();
        SetLevelDiffButtons();
        SetSoundVolume();
    }

    void GetConfigSettings()
    {
        string path = "Assets/Resources/player_settings.txt";
        StreamReader playerSettingsReader = resourcesManagmentScript.ReadDataFromResource(path);
        playerSettingsReader.ReadLine();
        playerSettingsReader.ReadLine();
        playerSettingsReader.ReadLine();
        playerSettingsReader.ReadLine();
        playerSettingsReader.ReadLine();
        int.TryParse(playerSettingsReader.ReadLine(), out int levelDiff);
        levelDifficulty = levelDiff;
        float.TryParse(playerSettingsReader.ReadLine(), out float musicVol);
        musicSlider.value = musicVolume = musicVol;
        float.TryParse(playerSettingsReader.ReadLine(), out float soundVol);
        soundsSlider.value = soundVolume = soundVol;
        playerSettingsReader.Close();
    }

    void SetConfigSettings()
    {
        string path = "Assets/Resources/player_settings.txt";
        StreamReader playerSettingsReader = resourcesManagmentScript.ReadDataFromResource(path);
        var value1 = playerSettingsReader.ReadLine();
        var value2 = playerSettingsReader.ReadLine();
        var value3 = playerSettingsReader.ReadLine();
        var value4 = playerSettingsReader.ReadLine();
        var value5 = playerSettingsReader.ReadLine();
        playerSettingsReader.Close();

        StreamWriter playerSettings = resourcesManagmentScript.WriteDataToResource(path);
        playerSettings.WriteLine(value1);
        playerSettings.WriteLine(value2);
        playerSettings.WriteLine(value3);
        playerSettings.WriteLine(value4);
        playerSettings.WriteLine(value5);
        playerSettings.WriteLine(levelDifficulty);
        playerSettings.WriteLine(musicVolume);
        playerSettings.WriteLine(soundVolume);
        playerSettings.Close();
    }

    private void SetLevelDiffButtons()
    {
        for (int i = 0; i < levelsDifficulty.Length; i++)
        {
            if (levelDifficulty == i) levelsDifficulty[i].interactable = false;
            else levelsDifficulty[i].interactable = true;
        }
    }

    private void SetSoundVolume()
    {
        musicAS.volume = musicVolume;
        soundAS.volume = soundVolume;
    }

    public void PlayNewGameBtn()
    {
        GetComponent<SoundsControllerScript>().PlaySoundUI1();
        SetConfigSettings();
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
        StreamReader playerSettingsReader = resourcesManagmentScript.ReadDataFromResource(path2);
        playerSettingsReader.ReadLine();
        playerSettingsReader.ReadLine();
        playerSettingsReader.ReadLine();
        playerSettingsReader.ReadLine();
        playerSettingsReader.ReadLine();
        int.TryParse(playerSettingsReader.ReadLine(), out int levelDifficulty);
        float.TryParse(playerSettingsReader.ReadLine(), out float musicVolume);
        float.TryParse(playerSettingsReader.ReadLine(), out float soundVolume);
        playerSettingsReader.Close();

        StreamWriter playerSettings = resourcesManagmentScript.WriteDataToResource(path2);
        playerSettings.WriteLine("1");
        playerSettings.WriteLine("1,3");
        playerSettings.WriteLine("-2,6");
        playerSettings.WriteLine("6");
        playerSettings.WriteLine("6");
        playerSettings.WriteLine(levelDifficulty);
        playerSettings.WriteLine(musicVolume);
        playerSettings.WriteLine(soundVolume);
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
        SetConfigSettings();
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

    public void SetLevelDifficulty(int level)
    {
        GetComponent<SoundsControllerScript>().PlaySoundUI1();
        levelDifficulty = level;
        SetLevelDiffButtons();
    }

    public void OnMusicVolumeChange(float value)
    {
        musicVolume = value;
        SetSoundVolume();
    }

    public void OnSoundsVolumeChange(float value)
    {
        soundVolume = value;
        SetSoundVolume();
    }

    public void ExitGameBtn()
    {
        GetComponent<SoundsControllerScript>().PlaySoundUI1();
        SetConfigSettings();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
