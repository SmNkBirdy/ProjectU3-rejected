using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBase : MonoBehaviour
{
    public GameObject killBox;
    public float burstLenght;
    public float burstCD;
    private float burstTime = 0;
    public GameObject ourPlanet;
    public GameObject landingZone;
    public GameObject fuelMenu;
    public float landingSpeed = 2;
    public GameObject mainCamera;
    public ShipNavigation sn;
    public ShipMovement sm;
    public Transform pt;
    public GameObject sc;
    public GameManager gm;
    public float scaleMult = 1;
    public bool landing = false;
    public bool launch = false;
    public bool onPlanet = false;
    public bool landingApart = false;

    public void refil()
    {
        if (sm.fuel != sm.maxFuel)
        {
            sm.fuel = sm.maxFuel;
            gm.currencu -= 100;
        }
    }

    public void leftBase()
    {
        landing = false;
        launch = true;
        fuelMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && landingApart)
        {
            onPlanet = true;
            landing = true;
            sc.SetActive(false);
            sm.lowerSpeed();
            landingApart = false;
            Debug.Log("Мы садимся");
        }

        if (landing)
        {
            mainCamera.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z*-1);
            Vector3 direction = (landingZone.transform.position - pt.position).normalized;
            Vector3.Distance(pt.position, pt.position + direction * landingSpeed * Time.deltaTime);
            Vector3.Distance(pt.position, landingZone.transform.position);
            if (Vector3.Distance(pt.position, landingZone.transform.position) > Vector3.Distance(pt.position, pt.position + direction * landingSpeed * Time.deltaTime))
            {
                pt.position += direction * landingSpeed * Time.deltaTime;
            }
            else
            {
                pt.position = landingZone.transform.position;
            }
            if (Vector3.Distance(pt.position, landingZone.transform.position) == 0)
            {
                fuelMenu.SetActive(true);
            }

            if (scaleMult > 0.1f)
            {
                scaleMult -= 0.5f * Time.deltaTime;
                Debug.Log(scaleMult);
            }
            else
            {
                scaleMult = 0.1f;
            }
            pt.localScale = new Vector3(40,40,40) * scaleMult;
        }
        else
        {
            if (launch)
            {
                pt.position += pt.up * landingSpeed * Time.deltaTime;
                if (scaleMult < 1f)
                {
                    scaleMult += 0.5f * Time.deltaTime;
                }
                else
                {
                    scaleMult = 1f;
                }
                pt.localScale = new Vector3(40, 40, 40) * scaleMult;
                if (Vector3.Distance(pt.position,ourPlanet.transform.position)>= sn.orbitSize - sn.orbitRideZone)
                {
                    onPlanet = false;
                    launch = false;
                    burstTime = Time.time;
                    mainCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
                    pt.localScale = new Vector3(40, 40, 40);
                }
            }
        }

        if (sn.currentPlanet == ourPlanet && sm.rideOrbit)
        {
            float angle = Vector3.Angle(transform.position - ourPlanet.transform.position, pt.position - ourPlanet.transform.position);
            if (angle < 35 && !onPlanet)
            {
                sc.SetActive(true);
                landingApart = true;
            }
            else
            {
                sc.SetActive(false);
                landingApart = false;
            }
        }
        else
        {
            landingApart = false;
            sc.SetActive(false);
        }

        if (burstTime + burstLenght + burstCD < Time.time && !launch)
        {
            burstTime = Time.time;
            killBox.SetActive(true);
        }
        if (burstTime + burstLenght < Time.time)
        {
            killBox.SetActive(false);
        }
        sm.landing = onPlanet;
    }
}
