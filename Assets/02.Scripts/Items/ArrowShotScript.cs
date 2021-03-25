using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShotScript : MonoBehaviour
{
    public float arrowSpeed;

    [HideInInspector] public Tool arrow;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position += (transform.up - transform.right) * arrowSpeed * Time.deltaTime;
    }
}
