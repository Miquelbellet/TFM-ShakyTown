using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControllerScript : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        
    }

    public void PlayGameBtn()
    {
        if (PlayerPrefs.GetString("tutorial", "false") == "false") SceneManager.LoadScene("InitTutorial");
        else SceneManager.LoadScene("Main");
    }

    public void ExitGameBtn()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
