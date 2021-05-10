using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject player;
    public GameObject planet;
    public RectTransform arrow;
    public float cameraSpeed = 1f;
    public Vector3 cameraPosition = new Vector3(0,0,-25);
    private float stX;
    public float scMult = 0;

    private void Awake()
    {
        stX = arrow.localPosition.z;
    }

    void Update()
    {
        arrow.transform.localScale = new Vector3(1, 1, 1) * (1 + scMult);
        arrow.localPosition = new Vector3(0, 0, stX * (1 + scMult / 3));
        if (scMult < -0.2f)
        {
            arrow.gameObject.SetActive(false);
        }
        else
        {
            arrow.gameObject.SetActive(true);
        }

        planet = player.GetComponent<ShipNavigation>().currentPlanet;
        if (planet != null)
        {
            transform.position = Vector3.Lerp(transform.position,planet.transform.position + cameraPosition, cameraSpeed * Time.deltaTime);
        }
        else
        {
                transform.position = Vector3.Lerp(transform.position, player.transform.position + cameraPosition, cameraSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            cameraPosition += new Vector3(0,0,10) * Time.deltaTime;
            scMult -= 0.28f * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            scMult += 0.28f * Time.deltaTime;
            cameraPosition += new Vector3(0, 0, -10) * Time.deltaTime;
        }
    }
}
