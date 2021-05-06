using UnityEngine;

public class PlanetScript : MonoBehaviour
{
    public GameObject player;
    public float gravitySize;
    public float orbitSize;
    public float gravity = 9.8f;
    public LayerMask physObjects;

    private bool trustActive;

    //отрисовка параметров
    public int segments = 50;
    private float xradius = 5;
    private float yradius = 5;
    private LineRenderer line;
    private int currPos = 0;


    void CreatePoints()
    {
        float x;
        float y;
        int currPosSave = currPos;

        float angle = 20f;
        for (int i = 0 + currPosSave; i < (segments + 1) + currPosSave; i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius / transform.localScale.x;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius / transform.localScale.y;
            line.SetPosition(i, new Vector3(x, y, 0));
            angle += (360f / segments);
            currPos++;
        }
    }

    private void Awake()
    {
        currPos = 0;
        orbitSize = gameObject.GetComponent<MeshRenderer>().bounds.size.x;
        gravitySize = orbitSize * 2;
        if (orbitSize < 4)
        {
            orbitSize = 4;
        }

        //отрисовка
        line = gameObject.GetComponent<LineRenderer>();
        line.positionCount = (segments + 1) * 2;
        line.useWorldSpace = false;
        xradius = orbitSize;
        yradius = orbitSize;
        CreatePoints();
        xradius = gravitySize;
        yradius = gravitySize;
        CreatePoints();
        trustActive = false;
    }

    //разрушение объекта если его можно разрушить.
    private void OnTriggerEnter(Collider other)
    {
        Destructible d = other.GetComponent<Destructible>();
        if (d != null)
        {
            d.destroyObj();
        }
    }

    void FixedUpdate()
    {
        //планета крутится ловешка мутится
        transform.Rotate(0,0,3 * Time.deltaTime);

        //находим всё что в поле гравитации
        Collider[] items = Physics.OverlapSphere(gameObject.transform.position, gravitySize, physObjects);

        foreach (var item in items)
        {
            //определяем работают ли трастеры
            ShipMovement sm = item.GetComponent<ShipMovement>();
            if (sm != null)
            {
                trustActive = sm.trustActive;
            }
            else
            {
                trustActive = false;
            }

            //находим угол меж планетой и кораблём
            Vector3 direction = (transform.position - item.transform.position).normalized;
            float angleDif = Vector3.Angle(direction, item.transform.forward);

            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb != null)
            {

                item.transform.Rotate(new Vector3(1, 0, 0) * 50 * Time.deltaTime);
                float newAngleDif = Vector3.Angle(direction, item.transform.forward);
                item.transform.Rotate(new Vector3(-1, 0, 0) * 50 * Time.deltaTime);
                if (newAngleDif > angleDif)
                {
                    rb.AddTorque(new Vector3(0, 0, 1) * 3 * Time.deltaTime);
                }
                else
                {
                    rb.AddTorque(new Vector3(0, 0, -1) * 3 * Time.deltaTime);
                }
            }
        }
    } 
}
