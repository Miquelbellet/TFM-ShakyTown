using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShotScript : MonoBehaviour
{
    public float arrowSpeed;
    public float timeFlying;

    [HideInInspector] public Tool arrow;
    [HideInInspector] public Tool bowObject;

    private float time;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position += (transform.up - transform.right) * arrowSpeed * Time.deltaTime;
        time += Time.deltaTime;
        if(time > timeFlying)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            int dmg = arrow.damage + bowObject.damage;
            collision.gameObject.GetComponent<EnemyControllerScript>().Hitted(dmg);
            Destroy(gameObject);
        }
    }
}
