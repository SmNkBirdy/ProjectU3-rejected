using UnityEngine;

public class PlanetScript : MonoBehaviour
{
    public GameObject player;
    public float gravitySize;
    public float orbitSize;
    public float gravity = 9.8f;
    public float orbitRideZone = 1;
    public float radiuseRenderWidth = 0.25f;
    public LayerMask physObjects;
    public LineRenderer orbitRenderer;
    public LineRenderer orbitRadiusRenderer;
    public LineRenderer gravityRadiusRenderer;

    private bool trustActive;

    //отрисовка параметров
    public int segments = 50;
    private float xradius = 5;
    private float yradius = 5;
    private float x;
    private float y;
    private float angle = 20f;

    private void Awake()
    {

        orbitSize = gameObject.GetComponent<MeshRenderer>().bounds.size.x;
        orbitRideZone = gameObject.GetComponent<MeshRenderer>().bounds.size.x/2;
        gravitySize = orbitSize * 2;
        if (orbitSize < 4)
        {
            orbitSize = 4;
        }

        //отрисовка
        orbitRenderer.positionCount = segments + 1;
        orbitRenderer.generateLightingData = true;
        orbitRenderer.useWorldSpace = false;
        orbitRenderer.startWidth = orbitRideZone;
        orbitRenderer.endWidth = orbitRideZone;
        orbitRenderer.loop = true;
        orbitRadiusRenderer.positionCount = segments + 1;
        orbitRadiusRenderer.generateLightingData = true;
        orbitRadiusRenderer.useWorldSpace = false;
        orbitRadiusRenderer.startWidth = radiuseRenderWidth;
        orbitRadiusRenderer.endWidth = radiuseRenderWidth;
        orbitRadiusRenderer.loop = true;
        gravityRadiusRenderer.positionCount = segments + 1;
        gravityRadiusRenderer.generateLightingData = true;
        gravityRadiusRenderer.useWorldSpace = false;
        gravityRadiusRenderer.startWidth = radiuseRenderWidth;
        gravityRadiusRenderer.endWidth = radiuseRenderWidth;
        gravityRadiusRenderer.loop = true;

        xradius = orbitSize;
        yradius = orbitSize;
        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius / transform.localScale.x;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius / transform.localScale.y;
            orbitRadiusRenderer.SetPosition(i, new Vector3(x, y, 0));
            angle += (360f / segments) - 0.055f;
        }

        xradius = gravitySize;
        yradius = gravitySize;
        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius / transform.localScale.x;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius / transform.localScale.y;
            gravityRadiusRenderer.SetPosition(i, new Vector3(x, y, 0));
            angle += (360f / segments) - 0.055f;
        }

        xradius = orbitSize - (orbitRideZone / 2);
        yradius = orbitSize - (orbitRideZone / 2);
        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius / transform.localScale.x;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius / transform.localScale.y;
            orbitRenderer.SetPosition(i, new Vector3(x, y, 0));
            angle += (360f / segments) - 0.055f;
        }

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
