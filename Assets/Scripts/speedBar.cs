using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedBar : MonoBehaviour
{
    public GameObject segment1;
    public GameObject segment2;
    public GameObject segment3;
    public GameObject segment4;
    public GameObject segment5;
    public GameObject segment6;
    public GameObject segment7;
    public GameObject segment8;
    [Range(0, 1)]
    public float speed;

    private void LateUpdate()
    {
        if (speed == 0)
        {
            segment1.SetActive(false);
            segment2.SetActive(false);
            segment3.SetActive(false);
            segment4.SetActive(false);
            segment5.SetActive(false);
            segment6.SetActive(false);
            segment7.SetActive(false);
            segment8.SetActive(false);
        }
        else if (speed <= 1f/8f)
        {
            segment1.SetActive(true);
            segment2.SetActive(false);
            segment3.SetActive(false);
            segment4.SetActive(false);
            segment5.SetActive(false);
            segment6.SetActive(false);
            segment7.SetActive(false);
            segment8.SetActive(false);
        }
        else if (speed < 1f / 8f * 2f)
        {
            segment1.SetActive(true);
            segment2.SetActive(true);
            segment3.SetActive(false);
            segment4.SetActive(false);
            segment5.SetActive(false);
            segment6.SetActive(false);
            segment7.SetActive(false);
            segment8.SetActive(false);
        }
        else if (speed < 1f / 8f * 3f)
        {
            segment1.SetActive(true);
            segment2.SetActive(true);
            segment3.SetActive(true);
            segment4.SetActive(false);
            segment5.SetActive(false);
            segment6.SetActive(false);
            segment7.SetActive(false);
            segment8.SetActive(false);
        }
        else if (speed < 1f / 8f * 4f)
        {
            segment1.SetActive(true);
            segment2.SetActive(true);
            segment3.SetActive(true);
            segment4.SetActive(true);
            segment5.SetActive(false);
            segment6.SetActive(false);
            segment7.SetActive(false);
            segment8.SetActive(false);
        }
        else if (speed < 1f / 8f * 5f)
        {
            segment1.SetActive(true);
            segment2.SetActive(true);
            segment3.SetActive(true);
            segment4.SetActive(true);
            segment5.SetActive(true);
            segment6.SetActive(false);
            segment7.SetActive(false);
            segment8.SetActive(false);
        }
        else if (speed < 1f / 8f * 6f)
        {
            segment1.SetActive(true);
            segment2.SetActive(true);
            segment3.SetActive(true);
            segment4.SetActive(true);
            segment5.SetActive(true);
            segment6.SetActive(true);
            segment7.SetActive(false);
            segment8.SetActive(false);
        }
        else if (speed < 1f / 8f * 7f)
        {
            segment1.SetActive(true);
            segment2.SetActive(true);
            segment3.SetActive(true);
            segment4.SetActive(true);
            segment5.SetActive(true);
            segment6.SetActive(true);
            segment7.SetActive(true);
            segment8.SetActive(false);
        }
        else
        {
            segment1.SetActive(true);
            segment2.SetActive(true);
            segment3.SetActive(true);
            segment4.SetActive(true);
            segment5.SetActive(true);
            segment6.SetActive(true);
            segment7.SetActive(true);
            segment8.SetActive(true);
        }
    }
}
