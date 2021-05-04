using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class AppManager : MonoBehaviour
{
    public static AppManager instance;

    [Header("Panels")]
    public GameObject uploadPanel;
    public GameObject selectGraphPanel;
    public GameObject takeSSPanel;

    public TextMeshProUGUI dataNameText;

    public Transform mainCamTransform;
    public Camera mainCam;
    public Camera ssCam;

    public FileBrowserTest fileBrowser;
    public ScreenshotHandler screenshotHandler;

    public GameObject nextBTN;

    public Transform variablePartParent;
    public GameObject variablePartPrefab;

    public int rowIndex1, rowIndex2;
    public TextMeshProUGUI variable1Text, variable2Text;

    public Slider colorSlider;
    public PostProcessVolume volume;
    private ColorGrading colorGrading;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        nextBTN.SetActive(false);
        uploadPanel.SetActive(true);
        selectGraphPanel.SetActive(false);
        takeSSPanel.SetActive(false);

        volume.profile.TryGetSettings(out colorGrading);
    }

    private void Update()
    {
        float tint = colorSlider.value;
        colorGrading.tint.value = tint * 100;
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
            variable1Text.text = "Seçilmedi";
        }
        else
        {
            variable2Text.text = "Seçilmedi";
        }
    }
}
