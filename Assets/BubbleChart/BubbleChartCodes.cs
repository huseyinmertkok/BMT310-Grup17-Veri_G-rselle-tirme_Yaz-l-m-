using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class BubbleChartCodes : MonoBehaviour
{
    public GameObject[] bubbles;
    public string unit;
    public string[] dataLabels;
    public float[] dataValues;
    void Start()
    {
        unit = AppManager.instance.chartCreator.dataUnit;

        for (int i = 0; i < dataValues.Length; i++)
        {
            bubbles[i].SetActive(true);
        }

        for (int i = dataValues.Length; i < bubbles.Length; i++)
        {
            Destroy(bubbles[i]);
        }

        float maxValue = dataValues.Max();
        for (int i = 0; i < dataValues.Length; i++)
        {
            bubbles[i].transform.localScale = new Vector3(dataValues[i] / maxValue, bubbles[i].transform.localScale.y, dataValues[i] / maxValue);
            bubbles[i].GetComponentInChildren<TextMeshPro>().text = dataLabels[i] + "\n" + dataValues[i] + unit;
        }

        float x = 0, y = 0, z = 0;
        for (int i = 0; i < dataValues.Length; i++)
        {
            x += transform.GetChild(i).localPosition.x;
            y += transform.GetChild(i).localPosition.y;
        }
        x /= dataValues.Length;
        y /= dataValues.Length;

        transform.position = new Vector3(-x, -y, transform.position.z);
    }
}
