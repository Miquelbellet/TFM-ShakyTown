using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthScript : MonoBehaviour
{
    public int playerLifes;
    public float intervalBetweenDamage;

    [HideInInspector] public bool canRecieveDamage = true;

    private bool immunePlayer;

    void Start()
    {
        
    }

    void Update()
    {
        if (!canRecieveDamage && !immunePlayer)
        {
            StartCoroutine(PlayerImmune());
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
        Debug.Log("Player hitted, damage: "+damage);
        canRecieveDamage = false;
        Invoke("CanRecieveDamage", intervalBetweenDamage);
    }

    void CanRecieveDamage()
    {
        canRecieveDamage = true;
        immunePlayer = false;
        StopAllCoroutines();
        GetComponent<SpriteRenderer>().enabled = true;
    }
}
