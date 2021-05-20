using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChartCreator : MonoBehaviour
{
    [Header("Data")]
    public string dataName;
    public string dataUnit;
    private string[] dataLabels;
    private float[] dataValues;

    public string[] tempData1, tempData2;

    [Header("Chart Prefabs")]
    public GameObject pieChartPrefab;
    public GameObject barChartPrefab;
    public GameObject lineChartPrefab;
    public GameObject bubbleChartPrefab;

    public GameObject currentChart;
    public bool isPieChart = false;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void TempToData()
    {
        dataLabels = tempData1;
        dataValues = new float[tempData2.Length];
        for (int i = 0; i < tempData2.Length; i++)
        {
            dataValues[i] = float.Parse(tempData2[i]);
        }
    }

    private void CurrentChartCheck()
    {
        if (currentChart != null)
        {
            Destroy(currentChart);
            currentChart = null;
        }
        isPieChart = false;
    }

    private GameObject InstantiateCurrentChart(GameObject prefab)
    {
        currentChart = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        return currentChart;
    }

    public void PieChartBTN()
    {
        CurrentChartCheck();
        TempToData();
        //AppManager.instance.selectGraphPanel.SetActive(false);
        GameObject pieChartObject = InstantiateCurrentChart(pieChartPrefab);
        PieChart.ViitorCloud.PieChart pieChart;
        pieChart = pieChartObject.GetComponent<PieChart.ViitorCloud.PieChart>();
        pieChart.segments = dataValues.Length;
        float[] newDataValues = new float[dataValues.Length];
        for (int i = 0; i < dataValues.Length; i++)
        {
            newDataValues[i] = (float)dataValues[i];
        }
        pieChart.Data = newDataValues;
        pieChart.dataDescription = new List<string>(dataLabels);
        pieChartObject.SetActive(true);
        isPieChart = true;
        pieChartObject.transform.rotation = Quaternion.Euler(Vector3.right * 45f);
        pieChartObject.transform.position = Vector3.forward * 1f;
        /*pieChart.transform.DORotate(Vector3.right * 45f, 1f).OnComplete(() =>
        {
            AppManager.instance.takeSSPanel.SetActive(true);
        });*/
    }

    public void BarChartBTN()
    {
        CurrentChartCheck();
        TempToData();
        //AppManager.instance.selectGraphPanel.SetActive(false);
        GameObject barChartObject = InstantiateCurrentChart(barChartPrefab);
        BarGraphExample barChartData;
        barChartData = barChartObject.GetComponent<BarGraphExample>();
        barChartData.exampleDataSet = new List<BarGraph.VittorCloud.BarGraphDataSet>();
        BarGraph.VittorCloud.BarGraphDataSet dataSet = new BarGraph.VittorCloud.BarGraphDataSet();
        dataSet.GroupName = dataName;
        dataSet.barColor = Color.blue;
        dataSet.ListOfBars = new List<BarGraph.VittorCloud.XYBarValues>();
        float[] newDataValues = new float[dataValues.Length];
        for (int i = 0; i < dataValues.Length; i++)
        {
            newDataValues[i] = (float)dataValues[i];
        }
        for (int i = 0; i < dataValues.Length; i++)
        {
            BarGraph.VittorCloud.XYBarValues values = new BarGraph.VittorCloud.XYBarValues();
            values.XValue = dataLabels[i];
            values.YValue = newDataValues[i];
            dataSet.ListOfBars.Add(values);
        }
        barChartData.exampleDataSet.Add(dataSet);
        barChartObject.SetActive(true);
        barChartObject.transform.position = new Vector3(-dataValues.Length / 2 - .5f, -1.75f, 3f);
        //barChartObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane));
        /*Vector3 camPos = new Vector3(0, 5, -20);
        if (dataValues.Length % 2 == 1)
        {
            camPos.x = (dataValues.Length + 1) / 2;
        }
        else
        {
            camPos.x = dataValues.Length / 2 + .5f;
        }
        AppManager.instance.mainCamTransform.position = camPos;*/
        //AppManager.instance.takeSSPanel.SetActive(true);
    }

    public void BubbleChartBTN()
    {
        CurrentChartCheck();
        TempToData();
        GameObject bubbleChartObject = InstantiateCurrentChart(bubbleChartPrefab);
        BubbleChartCodes bubbleChart = bubbleChartObject.GetComponent<BubbleChartCodes>();
        bubbleChart.dataLabels = dataLabels;
        float[] newDataValues = new float[dataValues.Length];
        for (int i = 0; i < dataValues.Length; i++)
        {
            newDataValues[i] = (float)dataValues[i];
        }
        bubbleChart.dataValues = newDataValues;
        bubbleChart.transform.position = Vector3.forward * 1f;
        bubbleChartObject.SetActive(true);
    }

    public void LineChartBTN()
    {
        CurrentChartCheck();
        TempToData();
        //AppManager.instance.selectGraphPanel.SetActive(false);
        GameObject lineChartObject = InstantiateCurrentChart(lineChartPrefab);
        BarGraphExample lineChartData;
        lineChartData = lineChartObject.GetComponent<BarGraphExample>();
        lineChartData.exampleDataSet = new List<BarGraph.VittorCloud.BarGraphDataSet>();
        BarGraph.VittorCloud.BarGraphDataSet dataSet = new BarGraph.VittorCloud.BarGraphDataSet();
        dataSet.GroupName = dataName;
        dataSet.barColor = Color.blue;
        dataSet.ListOfBars = new List<BarGraph.VittorCloud.XYBarValues>();
        float[] newDataValues = new float[dataValues.Length];
        for (int i = 0; i < dataValues.Length; i++)
        {
            newDataValues[i] = (float)dataValues[i];
        }
        for (int i = 0; i < dataValues.Length; i++)
        {
            BarGraph.VittorCloud.XYBarValues values = new BarGraph.VittorCloud.XYBarValues();
            values.XValue = dataLabels[i];
            values.YValue = newDataValues[i];
            dataSet.ListOfBars.Add(values);
        }
        lineChartData.exampleDataSet.Add(dataSet);
        lineChartObject.SetActive(true);
        lineChartObject.transform.position = new Vector3(-dataValues.Length / 2 - .5f, -1.75f, 3f);
        //barChartObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane));
        /*Vector3 camPos = new Vector3(0, 5, -20);
        if (dataValues.Length % 2 == 1)
        {
            camPos.x = (dataValues.Length + 1) / 2;
        }
        else
        {
            camPos.x = dataValues.Length / 2 + .5f;
        }
        AppManager.instance.mainCamTransform.position = camPos;*/
        //AppManager.instance.takeSSPanel.SetActive(true);
    }

    public void BTN2D()
    {
        /*AppManager.instance.mainCam.orthographic = true;
        if (pieChartObject.activeInHierarchy)
        {
            pieChartObject.transform.rotation = Quaternion.identity;
            AppManager.instance.mainCam.orthographicSize = 2.75f;
        }
        else
        {
            AppManager.instance.mainCam.orthographicSize = 12f;
        }*/
    }

    public void BTN3D()
    {
        /*AppManager.instance.mainCam.orthographic = false;
        AppManager.instance.mainCam.fieldOfView = 60f;
        if (pieChartObject.activeInHierarchy)
        {
            pieChartObject.transform.rotation = Quaternion.Euler(Vector3.right * 45f);
        }
        else
        {

        }*/
    }
}
