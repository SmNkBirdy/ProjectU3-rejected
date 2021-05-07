using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipMovement : MonoBehaviour
{
    public float rotateSpeed = 1f; //�������� ��������
    public float shipSpeed = 5; //�������� �������
    public float trustSpeed = 5; //�������� �����
    public float trustTime = 1; //����������������� �����
    public float enginePower = 0; //��������� �������� ������
    public bool trustActive; //��������� ������ �����
    public speedBar sb;
    public GameObject sc1;
    public GameObject sc2;
    public GameObject sc3;

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
                sb.speed = 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

        //����������� �� ������

        if (onOrbit && orbitSize - .75f < distance && angle > 70 && angle < 110 && !rideOrbit && !trustActive)
        {
            rideOrbit = true;
            sc1.SetActive(true);
            sc2.SetActive(true);
            sc3.SetActive(true);
            rb.constraints = RigidbodyConstraints.FreezeAll;
            Debug.Log(orbitSpeed);
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
            if (angle > 90)
            {
                transform.Rotate(new Vector3(1, 0, 0) * 50 * Time.fixedDeltaTime);
                float newAngleDif = Vector3.Angle(direction, transform.forward);
                if (newAngleDif > angle)
                {
                    transform.Rotate(new Vector3(-1, 0, 0) * 50 * Time.fixedDeltaTime * 2);
                }
            }
            else
            {
                transform.Rotate(new Vector3(1, 0, 0) * 50 * Time.fixedDeltaTime);
                float newAngleDif = Vector3.Angle(direction, transform.forward);
                if (newAngleDif < angle)
                {
                    transform.Rotate(new Vector3(-1, 0, 0) * 50 * Time.fixedDeltaTime * 2);
                }
            }
            transform.position += transform.forward * orbitSpeed * orbSpMul * Time.fixedDeltaTime / 2;
            if (distance > orbitSize)
            {
                transform.position += direction * 0.001f;
            }
            else if (distance < orbitSize)
            {
                transform.position -= direction * 0.001f;
            }
        }
    }
}
