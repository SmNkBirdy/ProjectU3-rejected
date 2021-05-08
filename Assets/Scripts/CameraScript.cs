using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject player;
    public GameObject planet;
    public bool onPlanet;
    public RectTransform arrow;
    public float cameraSpeed = 1f;
    public Vector3 cameraPosition = new Vector3(0,0,-25);
    private float stX;
    public float scMult = 0;
    public GameObject sc1;
    public GameObject sc2;

    private void Awake()
    {
        stX = arrow.localPosition.z;
    }

    void Update()
    {
        if (onPlanet)
        {
            sc1.SetActive(false);
            sc2.SetActive(false);
        }
        else
        {
            sc1.SetActive(true);
            sc2.SetActive(true);
        }
        arrow.transform.localScale = new Vector3(1, 1, 1) * (1 + scMult);
        arrow.localPosition = new Vector3(0, 0, stX * (1 + scMult / 3));
        if (scMult < -0.2f || onPlanet)
        {
            arrow.gameObject.SetActive(false);
        }
        else
        {
            arrow.gameObject.SetActive(true);
        }

        planet = player.GetComponent<ShipNavigation>().currentPlanet;
        onPlanet = player.GetComponent<ShipMovement>().landing;
        if (planet != null && !onPlanet)
        {
            transform.position = Vector3.Lerp(transform.position,planet.transform.position + cameraPosition, cameraSpeed * Time.deltaTime);
        }
        else
        {
            if (onPlanet)
            {
                transform.position = Vector3.Lerp(transform.position, player.transform.position + new Vector3(0,0,-3), cameraSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, player.transform.position + cameraPosition, cameraSpeed * Time.deltaTime);
            }
        }
        if (Input.GetKey(KeyCode.UpArrow) && !onPlanet)
        {
            cameraPosition += new Vector3(0,0,10) * Time.deltaTime;
            scMult -= 0.28f * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow) && !onPlanet)
        {
            scMult += 0.28f * Time.deltaTime;
            cameraPosition += new Vector3(0, 0, -10) * Time.deltaTime;
        }
    }
}
