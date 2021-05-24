using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using System;

public class AppManager : MonoBehaviour
{
    public static AppManager instance;

    [Header("Panels")]
    public GameObject uploadPanel;
    public GameObject selectGraphPanel;

    public TextMeshProUGUI dataNameText;

    public Transform mainCamTransform;
    public Camera mainCam;
    public Camera ssCam;

    public FileBrowserTest fileBrowser;
    public ScreenshotHandler screenshotHandler;
    public ChartCreator chartCreator;
    public TooltipCodes toolTip;

    public GameObject nextBTN;

    public Transform variablePartParent;
    public GameObject variablePartPrefab;

    public int rowIndex1, rowIndex2;
    public TextMeshProUGUI variable1Text, variable2Text;

    public Slider colorSlider;
    public PostProcessVolume volume;
    private ColorGrading colorGrading;

    public Slider sizeSlider;

    public Toggle[] operationToggles;
    public Toggle[] sortingToggles;
    public Toggle[] dimensionalToggles;
    public Toggle[] ssToggles;

    public InputField dataUnitText;

    public RenderTexture originalRenderTexture;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        toolTip.parentRectTransform.gameObject.SetActive(false);

        nextBTN.SetActive(false);
        uploadPanel.SetActive(true);
        selectGraphPanel.SetActive(false);

        volume.profile.TryGetSettings(out colorGrading);

        originalRenderTexture = ssCam.targetTexture;
    }

    private void Update()
    {
        float tint = colorSlider.value;
        colorGrading.tint.value = tint * 100;

        ssCam.fieldOfView = -sizeSlider.value;
        ssCam.orthographicSize = -sizeSlider.value / 30f;

        DimensionalControl();

        chartCreator.dataUnit =" " + dataUnitText.text;
    }

    private void DimensionalControl()
    {
        int index = GetSelectedToggle(dimensionalToggles);

        if (index == 0)
        {
            ssCam.orthographic = true;
            if (chartCreator.isPieChart)
            {
                chartCreator.currentChart.transform.rotation = Quaternion.identity;
            }
        }
        else
        {
            ssCam.orthographic = false;
            if (chartCreator.isPieChart)
            {
                chartCreator.currentChart.transform.rotation = Quaternion.Euler(Vector3.right * 45f);
            }
        }
    }

    public void ResetBTN()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SaveBTN()
    {
        //screenshotHandler.TakeScreenshot(1920, 1080);
        screenshotHandler.TakeAShot();
    }

    public void BrowseBTN()
    {
        StartCoroutine(fileBrowser.DosyaVeKlasorSecmeDiyaloguGosterCoroutine());
    }

    public void VisualizeBTN()
    {
        uploadPanel.SetActive(false);
        selectGraphPanel.SetActive(true);
    }

    public void CleanVariableButton(bool variable1)
    {
        if (variable1)
        {
            variable1Text.text = "Not Chosen";
        }
        else
        {
            variable2Text.text = "Not Chosen";
        }
    }

    public int GetSelectedToggle(Toggle[] toggles)
    {
        int selected = 0;
        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].isOn)
            {
                selected = i;
            }
        }

        return selected;
    }

    public void SelectGraphBTN(int graphIndex)
    {
        int operation = GetSelectedToggle(operationToggles);
        int sorting = GetSelectedToggle(sortingToggles);
        fileBrowser.SetDataToChartCreator(rowIndex1, rowIndex2, operation, sorting);  

        switch (graphIndex)//0 bar, 1 pie, 2 line, 3 bubble
        {
            case 0:
                chartCreator.BarChartBTN();
                break;

            case 1:
                chartCreator.PieChartBTN();
                break;

            case 2:
                chartCreator.LineChartBTN();
                break;

            case 3:
                chartCreator.BubbleChartBTN();
                break;
        }
    }

    public void TakeSSBTN()
    {
        int index = GetSelectedToggle(ssToggles);
        Vector2Int resulution;
        if (index == 0)
        {
            resulution = new Vector2Int(800, 600);
        }
        else if (index == 1)
        {
            resulution = new Vector2Int(1600, 1200);
        }
        else
        {
            resulution = new Vector2Int(2400, 1800);
        }
        TakeSSCodes.TakeSS(ssCam, resulution.x, resulution.y, screenshotHandler.path, "Chart " + System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss"));
        //Debug.Log(resulution);
    }
}
