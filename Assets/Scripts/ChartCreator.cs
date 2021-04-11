using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChartCreator : MonoBehaviour
{
    [Header("Data")]
    public string dataName;
    public string[] dataLabels;
    public float[] dataValues;

    [Header("Graphs")]
    public GameObject pieChartObject;
    public GameObject barChartObject;

    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void PieChartBTN()
    {
        AppManager.instance.selectGraphPanel.SetActive(false);
        PieChart.ViitorCloud.PieChart pieChart;
        pieChart = pieChartObject.GetComponent<PieChart.ViitorCloud.PieChart>();
        pieChart.segments = dataValues.Length;
        pieChart.Data = dataValues;
        pieChart.dataDescription = new List<string>(dataLabels);
        pieChartObject.SetActive(true);
        pieChart.transform.DORotate(Vector3.right * 45f, 1f).OnComplete(() =>
        {
            AppManager.instance.takeSSPanel.SetActive(true);
        });
    }

    public void BarChartBTN()
    {
        AppManager.instance.selectGraphPanel.SetActive(false);
        BarGraphExample barChartData;
        barChartData = barChartObject.GetComponent<BarGraphExample>();
        barChartData.exampleDataSet = new List<BarGraph.VittorCloud.BarGraphDataSet>();
        BarGraph.VittorCloud.BarGraphDataSet dataSet = new BarGraph.VittorCloud.BarGraphDataSet();
        dataSet.GroupName = dataName;
        dataSet.barColor = Color.blue;
        dataSet.ListOfBars = new List<BarGraph.VittorCloud.XYBarValues>();
        for (int i = 0; i < dataValues.Length; i++)
        {
            BarGraph.VittorCloud.XYBarValues values = new BarGraph.VittorCloud.XYBarValues();
            values.XValue = dataLabels[i];
            values.YValue = dataValues[i];
            dataSet.ListOfBars.Add(values);
        }
        barChartData.exampleDataSet.Add(dataSet);
        barChartObject.SetActive(true);
        //barChartObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane));
        Vector3 camPos = new Vector3(0, 5, -20);
        if (dataValues.Length % 2 == 1)
        {
            camPos.x = (dataValues.Length + 1) / 2;
        }
        else
        {
            camPos.x = dataValues.Length / 2 + .5f;
        }
        AppManager.instance.mainCamTransform.position = camPos;
        AppManager.instance.takeSSPanel.SetActive(true);
    }

    public void BTN2D()
    {
        AppManager.instance.mainCam.orthographic = true;
        if (pieChartObject.activeInHierarchy)
        {
            pieChartObject.transform.rotation = Quaternion.identity;
            AppManager.instance.mainCam.orthographicSize = 2.75f;
        }
        else
        {
            AppManager.instance.mainCam.orthographicSize = 12f;
        }
    }

    public void BTN3D()
    {
        AppManager.instance.mainCam.orthographic = false;
        AppManager.instance.mainCam.fieldOfView = 60f;
        if (pieChartObject.activeInHierarchy)
        {
            pieChartObject.transform.rotation = Quaternion.Euler(Vector3.right * 45f);
        }
        else
        {

        }
    }
}
