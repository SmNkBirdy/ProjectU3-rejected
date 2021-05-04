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
    private GameObject curPlanet;
    private float orbitSize;
    private float distance;
    private float angle;
    private ShipNavigation sn;
    private Rigidbody rb;

    private void Awake()
    {
        sn = gameObject.GetComponent<ShipNavigation>();
        rb = transform.GetComponent<Rigidbody>();
        trustStart = Time.time;
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

    private void forceMove()
    {
        if (angle < 100 && angle > 80 && !trustActive)
        {

        }
    }

    private void stabilazeShip()
    {
        if (angle >= 90)
        {
            transform.Rotate(new Vector3(1, 0, 0), rotateSpeed * 1.5f * Time.deltaTime);
            if (angle < sn.getAngle())
            {
                transform.Rotate(new Vector3(-1, 0, 0), rotateSpeed * 1.5f * 2 * Time.deltaTime);
            }
        }
        else
        {
            transform.Rotate(new Vector3(1, 0, 0), rotateSpeed * 1.5f * Time.deltaTime);
            if (angle > sn.getAngle())
            {
                transform.Rotate(new Vector3(-1, 0, 0), rotateSpeed * 1.5f * 2 * Time.deltaTime);
            }
        }
    }


    void Update()
    {
        Debug.Log("ENGINE POWER:" + enginePower);

        getComputedData();
        //логика поворота
        if (curPlanet != null)
        {
            if (distance > orbitSize && distance < orbitSize + 1 && !trustActive)
            {
                if (angle >= 70)
                {
                    turnShip(90,0.8f);
                }
                else
                {
                    turnShip(90,1.2f);
                }
            }
            else if (distance < orbitSize - 1f && !trustActive)
            {
                turnShip(110,1.2f);
            }
            else if (distance > orbitSize - 1f && distance < orbitSize && !trustActive)
            {
                stabilazeShip();
            }
        }

        trustActive = trustStart + trustTime > Time.time;

        if (!trustActive)
        {
            transform.position += transform.forward * shipSpeed * Time.deltaTime;
        }
        else
        {
            transform.position += transform.forward * (shipSpeed + trustSpeed) * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && onOrbit && !trustActive)
        {
            trustStart = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        /*РБ движение
        if (!trustActive)
        {
            rb.velocity += transform.forward * shipSpeed * Time.deltaTime * enginePower;
        }
        else
        {
            rb.velocity += transform.forward * (shipSpeed + trustSpeed) * Time.deltaTime;
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            trustActive = !trustActive;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (enginePower < 1f)
            {
                enginePower += 0.3f * Time.deltaTime;
            }
            else
            {
                enginePower = 1;
            }
        }

        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            float playerRotateAngle = Vector3.Angle(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * -1, transform.forward);
            transform.Rotate(new Vector3(1, 0, 0), rotateSpeed * Time.deltaTime);
            if (playerRotateAngle > Vector3.Angle(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * -1, transform.forward))
            {
                transform.Rotate(new Vector3(-1, 0, 0), rotateSpeed * 2 * Time.deltaTime);
            }
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (enginePower > 0f)
            {
                enginePower -= 0.3f * Time.deltaTime;
            }
            else
            {
                enginePower = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        */
    }
}
