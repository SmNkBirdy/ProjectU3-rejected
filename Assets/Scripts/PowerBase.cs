using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBase : MonoBehaviour
{
    public GameObject killBox;
    public float burstLenght;
    public float burstCD;
    private float burstTime = 0;

    void Update()
    {
        if (burstTime + burstLenght + burstCD < Time.time)
        {
            burstTime = Time.time;
            killBox.SetActive(true);
        }
        if (burstTime + burstLenght < Time.time)
        {
            killBox.SetActive(false);
        }
    }
}
