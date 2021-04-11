using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public ScreenshotHandler screenshotHandler;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        uploadPanel.SetActive(true);
        selectGraphPanel.SetActive(false);
        takeSSPanel.SetActive(false);
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
}
