using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public GameObject Explosion;

    public void destroyObj()
    {
        Instantiate(Explosion, transform.position,transform.rotation);
        gameObject.SetActive(false);
    }
}
