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

    private void OnTriggerEnter(Collider other)
    {
        Destructible d = other.GetComponent<Destructible>();
        if (d != null)
        {
            d.destroyObj();
        }
    }

    void Update()
    {
        transform.Rotate(0,0,3 * Time.deltaTime);

        Collider[] items = Physics.OverlapSphere(gameObject.transform.position, gravitySize, physObjects);
        foreach (var item in items)
        {
            ShipMovement sm = item.GetComponent<ShipMovement>();
            if (sm != null)
            {
                trustActive = sm.trustActive;
            }
            else
            {
                trustActive = false;
            }
            Vector3 direction = (transform.position - item.transform.position).normalized;
            float angleDif = Vector3.Angle(direction,item.transform.forward);
            float onePrc = (gravitySize - orbitSize) / 100;
            float curGrv = Vector3.Distance(transform.position,item.transform.position) / onePrc;
            curGrv = (gravitySize / onePrc) - curGrv;
            if (curGrv < 0)
            {
                curGrv = 0;
            }
            curGrv = curGrv / 100;
            float curAngP = ((Vector3.Angle(direction, item.transform.forward) + 1) / 0.9f) / 100;

            if (curAngP > 0.98f)
            {
                curAngP = 0.98f;
            }

            if (!trustActive)
            {
                item.transform.Rotate(new Vector3(1, 0, 0), curGrv * Time.deltaTime * 5);
                float newAngleDif = Vector3.Angle(direction, item.transform.forward);
                if (newAngleDif > angleDif)
                {
                    item.transform.Rotate(new Vector3(-1, 0, 0), curGrv * 2 * Time.deltaTime * 5);
                }
            }

            item.transform.position += direction * gravity * (curGrv - curGrv * curAngP) * Time.deltaTime;

            /* РБ движение
            float angleDif = Vector3.Angle(direction, item.transform.forward);
            item.transform.Rotate(new Vector3(1, 0, 0), RotationSpeed * Time.deltaTime);
            float newAngleDif = Vector3.Angle(direction, item.transform.forward);
            if (newAngleDif > angleDif)
            {
                item.transform.Rotate(new Vector3(-1, 0, 0), RotationSpeed * 2 * Time.deltaTime);
            }
            item.GetComponent<Rigidbody>().velocity += direction * (gravity - gravity / 100 * Vector3.Distance(transform.position, item.transform.position) / (gravitySize / 100))  * Time.deltaTime / 2;
            */
        }
    }
}
