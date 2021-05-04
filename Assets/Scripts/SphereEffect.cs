using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereEffect : MonoBehaviour
{
    private float startTime;
    private float animTime;
    void Start()
    {
        startTime = Time.time;
        animTime = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > startTime + animTime)
        {
            Destroy(gameObject);
        }
    }
}
