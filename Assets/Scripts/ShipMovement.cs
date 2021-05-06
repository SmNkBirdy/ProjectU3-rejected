using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipMovement : MonoBehaviour
{
    public float rotateSpeed = 1f; //скорость поворота
    public float shipSpeed = 5; //скорость коробля
    public float trustSpeed = 5; //мощность буста
    public float trustTime = 1; //продолжительность полёта
    public float enginePower = 0; //индикация мощности движка
    public bool trustActive; //индикация работы буста

    private float trustStart;
    private bool onOrbit;
    public bool rideOrbit;
    private GameObject curPlanet;
    private float orbitSize;
    private float distance;
    private float angle;
    private ShipNavigation sn;
    private Rigidbody rb;
    private RigidbodyConstraints strbc;
    private float orbitSpeed;

    private void Awake()
    {
        sn = gameObject.GetComponent<ShipNavigation>();
        rb = transform.GetComponent<Rigidbody>();
        trustStart = Time.time;
        onOrbit = false;
        strbc = rb.constraints;
    }

    private void getComputedData()
    {
        curPlanet = sn.currentPlanet;
        orbitSize = sn.orbitSize;
        onOrbit = sn.onOrbit;
        distance = sn.distanceFromPlanet;
        angle = sn.angle;
    }

    private void turnShip(float neededAngle,float rotateFactor)
    {
        if (angle < neededAngle)
        {
            transform.Rotate(new Vector3(1, 0, 0), rotateSpeed * rotateFactor * Time.deltaTime);
            if (angle > sn.getAngle())
            {
                transform.Rotate(new Vector3(-1, 0, 0), rotateSpeed * rotateFactor * 2 * Time.deltaTime);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (rideOrbit)
            {
                trustStart = Time.time;
                rb.constraints = strbc;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void FixedUpdate()
    {
        getComputedData();

        trustActive = !(trustStart + trustTime < Time.time);

        if (!trustActive)
        {
            rb.velocity += transform.forward * (shipSpeed) * Time.fixedDeltaTime;
        }
        else
        {
            rb.velocity += transform.forward * (shipSpeed + trustSpeed) * Time.fixedDeltaTime;
        }

        //поддержание на орбите

        if (orbitSize + .75f > distance && orbitSize - .75f < distance && angle > 70 && angle < 110 && !rideOrbit && !trustActive)
        {
            rideOrbit = true;
            orbitSpeed = rb.velocity.magnitude;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            Debug.Log(orbitSpeed);
        }

        if (rideOrbit)
        {
            if (trustActive)
            {
                rideOrbit = false;
            }
            if (angle > 90)
            {
                transform.Rotate(new Vector3(1, 0, 0) * 50 * Time.fixedDeltaTime);
                Vector3 direction = (curPlanet.transform.position - transform.position).normalized;
                float newAngleDif = Vector3.Angle(direction, transform.forward);
                if (newAngleDif > angle)
                {
                    transform.Rotate(new Vector3(-1, 0, 0) * 50 * Time.fixedDeltaTime * 2);
                }
            }
            else
            {
                transform.Rotate(new Vector3(1, 0, 0) * 50 * Time.fixedDeltaTime);
                Vector3 direction = (curPlanet.transform.position - transform.position).normalized;
                float newAngleDif = Vector3.Angle(direction, transform.forward);
                if (newAngleDif < angle)
                {
                    transform.Rotate(new Vector3(-1, 0, 0) * 50 * Time.fixedDeltaTime * 2);
                }
            }
            transform.position += transform.forward * orbitSpeed * Time.fixedDeltaTime / 2;
        }
    }
}
