using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthScript : MonoBehaviour
{
    [Header("Lifes configuration")]
    public float currentPlayerLifes;
    [Range(2, 24)] public int actualFullPlayerLifes;
    public float intervalBetweenDamage;
    
    [Header("Hearts Objects")]
    public GameObject heartsParent;
    public Sprite[] heartsSprites;

    [HideInInspector] public bool canRecieveDamage = true;
    [HideInInspector] public bool playerDead;
    [HideInInspector] public float initLifes;

    private GameObject gameController;
    private int maxNumberLifes = 24;
    private bool immunePlayer;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        SetPlayerHearts();
    }

    void Update()
    {
        if (!canRecieveDamage && !immunePlayer)
        {
            StartCoroutine(PlayerImmune());
        }
    }

    public void SetPlayerHearts()
    {
        for (int i = 0; i < heartsParent.transform.childCount; i++)
        {
            if (i+1 <= currentPlayerLifes / 2)
            {
                heartsParent.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = heartsSprites[0];
            }
            else if (i+1 == (currentPlayerLifes / 2) + 0.5f)
            {
                heartsParent.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = heartsSprites[1];
            }
            else if (i+1 <= actualFullPlayerLifes / 2)
            {
                heartsParent.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = heartsSprites[2];
            }
            else
            {
                heartsParent.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    IEnumerator PlayerImmune()
    {
        immunePlayer = true;
        for (int i = 0; i < Mathf.Infinity; i++)
        {
            if (i % 2 == 0) GetComponent<SpriteRenderer>().enabled = true;
            else GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    public void PlayerHitted(float damage)
    {
        currentPlayerLifes -= damage;
        SetPlayerHearts();
        if (currentPlayerLifes <= 0)
        {
            PlayerDied();
        }
        else
        {
            canRecieveDamage = false;
            Invoke("CanRecieveDamage", intervalBetweenDamage);
        }
    }

    void PlayerDied()
    {
        playerDead = true;
        gameController.GetComponent<ToolBarScript>().DropAllItems();
        gameController.GetComponent<LevelControllerScript>().ResetPlayer();
    }

    public void ResetPlayerLife()
    {
        playerDead = false;
        currentPlayerLifes = initLifes;
        SetPlayerHearts();
    }

    void CanRecieveDamage()
    {
        canRecieveDamage = true;
        immunePlayer = false;
        StopAllCoroutines();
        GetComponent<SpriteRenderer>().enabled = true;
    }
}
