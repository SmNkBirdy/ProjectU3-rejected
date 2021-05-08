using UnityEngine;

public class ShipNavigation : MonoBehaviour
{
    [Header("debug")]
    public GameObject currentPlanet;
    public float orbitSize;
    public float angle;
    public float distanceFromPlanet;
    public bool onOrbit;
    public float orbitRideZone;
    [Header("settings")]
    public LayerMask planetsMask;
    public float detectionRadius = 10f;
    private float speed;

    void Update()
    {
        findClosestPlanet();
        if (currentPlanet != null)
        {
            orbitSize = currentPlanet.GetComponent<PlanetScript>().orbitSize;
            angle = getAngle();
            orbitRideZone = currentPlanet.GetComponent<PlanetScript>().orbitRideZone;
            distanceFromPlanet = Vector3.Distance(transform.position, currentPlanet.transform.position);
            if (distanceFromPlanet < orbitSize)
            {
                onOrbit = true;
            }
            else
            {
                onOrbit = false;
            }
        }
    }


    public float getAngle()
    {
        if (currentPlanet != null)
        {
            return Vector3.Angle((currentPlanet.transform.position - transform.position).normalized, transform.forward);
        }
        else
        {
            return 0;
        }
    }
    private void findClosestPlanet()
    {
        if (currentPlanet != null)
        {
            if (Vector3.Distance(gameObject.transform.position, currentPlanet.transform.position) > detectionRadius)
            {
                currentPlanet = null;
            }
        }
        Collider[] items = Physics.OverlapSphere(gameObject.transform.position, detectionRadius, planetsMask);
        float minDist = detectionRadius + 1;
        foreach (Collider item in items)
        {
            if (Vector3.Distance(gameObject.transform.position, item.transform.position) < minDist)
            {
                currentPlanet = item.gameObject;
                minDist = Vector3.Distance(gameObject.transform.position, item.transform.position);
            }
        }

    }
}
