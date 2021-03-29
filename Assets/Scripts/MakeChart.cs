using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeChart : MonoBehaviour
{
    public GameObject barCube3D;
    public string[] testTitles, testData;
    void Start()
    {
        Create3DBarChart(testTitles, testData);
    }

    void Update()
    {
        
    }

    private void Create3DBarChart(string[] titles,string[] data)
    {
        float distance = 2f;
        Vector3 middlePos = Vector3.zero;
        Vector3 barPos = middlePos;
        if (titles.Length % 2 == 1)
        {
            barPos.x = -(int)(titles.Length / 2) * (float)distance;
        }
        else
        {
            barPos.x = -(int)(titles.Length / 2) * (float)distance + distance / 2;
        }

        for (int i = 0; i < titles.Length; i++)
        {
            Instantiate(barCube3D, barPos, Quaternion.identity);
            barPos.x += distance;
        }
    }
}
