using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundImageScript : MonoBehaviour
{

    private GameObject gameController;

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
    }
    public void FadeInFinished()
    {
        gameController.GetComponent<LevelControllerScript>().FadeInFinished();
    }

    public void FadeOutFinished()
    {
        gameController.GetComponent<LevelControllerScript>().FadeOutFinished();
    }
}
