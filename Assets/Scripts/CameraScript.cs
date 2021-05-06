using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject player;
    public GameObject planet;
    public float cameraSpeed = 1f;
    public Vector3 cameraPosition = new Vector3(0,0,-25);
    void Update()
    {
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
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            cameraPosition += new Vector3(0, 0, -10) * Time.deltaTime;
        }
    }
}
