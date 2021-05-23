using UnityEngine;
using UnityEngine.UI;

public class ShipMovement : MonoBehaviour
{
    public float fuel = 100;

    public float rotateSpeed = 1f; //скорость поворота
    public float shipSpeed = 5; //скорость коробля
    public float trustSpeed = 5; //мощность буста
    public float trustTime = 1; //продолжительность полёта
    public float enginePower = 0; //индикация мощности движка
    public bool trustActive; //индикация работы буста
    public float maxFuel = 100;
    public speedBar sb;
    public GameObject sc1;
    public GameObject sc2;
    public GameObject sc3;
    public Slider fuelBar;

    private float rotateMult;
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
    private float orbSpMul = 1;
    private float rideZone;

    private void Awake()
    {
        sn = gameObject.GetComponent<ShipNavigation>();
        rb = transform.GetComponent<Rigidbody>();
        trustStart = Time.time;
        onOrbit = false;
        strbc = rb.constraints;
        orbitSpeed = shipSpeed;
    }

    private void getComputedData()
    {
        curPlanet = sn.currentPlanet;
        orbitSize = sn.orbitSize;
        onOrbit = sn.onOrbit;
        distance = sn.distanceFromPlanet;
        angle = sn.angle;
        rideZone = sn.orbitRideZone;
    }

    public void lowerSpeed()
    {
        sb.speed = 0.4f;
    }

    private void Update()
    {
        fuelBar.value = fuel / maxFuel;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (rideOrbit && fuel > 10)
            {
                trustStart = Time.time;
                rb.constraints = strbc;
                sb.speed = 1;
                fuel -= 10;
                fuelBar.value = fuel / maxFuel;
            }
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (sb.speed < 1 && rideOrbit)
            {
                sb.speed += 0.1f * Time.deltaTime;
            }
        }
        if (sb.speed > 1)
        {
            sb.speed = 1;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (sb.speed > 0 && rideOrbit)
            {
                sb.speed -= 0.1f * Time.deltaTime;
            }
        }
        if (sb.speed < 0)
        {
            sb.speed = 0;
        }
    }

    void FixedUpdate()
    {

        orbSpMul = sb.speed;

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

        if (onOrbit && orbitSize - rideZone < distance && angle > 70 && angle < 110 && !rideOrbit && !trustActive)
        {
            rideOrbit = true;
            sc1.SetActive(true);
            sc2.SetActive(true);
            sc3.SetActive(true);
            rb.constraints = RigidbodyConstraints.FreezeAll;

        }

        if (rideOrbit)
        {
            if (trustActive)
            {
                rideOrbit = false;
                sc1.SetActive(false);
                sc2.SetActive(false);
                sc3.SetActive(false);
            }
            Vector3 direction = (curPlanet.transform.position - transform.position).normalized;

            if (distance > orbitSize)
            {
                rotateMult = 3  /distance + 1;
                transform.position += direction * (distance - orbitSize + 1) * Time.fixedDeltaTime;
            }
            else if (distance < orbitSize)
            {
                rotateMult = 1;
                transform.position -= direction * Time.fixedDeltaTime;
            }
            else
            {
                rotateMult = 1;
            }

            if (angle > 90)
            {
                transform.Rotate(new Vector3(1, 0, 0) * 50 * rotateMult * Time.fixedDeltaTime);
                float newAngleDif = Vector3.Angle(direction, transform.forward);
                if (newAngleDif > angle)
                {
                    transform.Rotate(new Vector3(-1, 0, 0) * 50 * rotateMult * Time.fixedDeltaTime * 2);
                }
            }
            else
            {
                transform.Rotate(new Vector3(1, 0, 0) * 50 * rotateMult * Time.fixedDeltaTime);
                float newAngleDif = Vector3.Angle(direction, transform.forward);
                if (newAngleDif < angle)
                {
                    transform.Rotate(new Vector3(-1, 0, 0) * 50 * rotateMult * Time.fixedDeltaTime * 2);
                }
            }
            transform.position += transform.forward * orbitSpeed * orbSpMul * Time.fixedDeltaTime / 2;
                
            sc1.SetActive(true);
            sc2.SetActive(true);
            sc3.SetActive(true);
        }
        else
        {
            sc1.SetActive(false);
            sc2.SetActive(false);
            sc3.SetActive(false);
        }
    }
}
