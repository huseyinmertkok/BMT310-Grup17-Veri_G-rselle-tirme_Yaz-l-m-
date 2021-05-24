using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeSSCodes : MonoBehaviour
{
    public static TakeSSCodes instance;

    private bool takeSSOnNextFrame = false;
    
    private Camera cam;
    private string path;
    private string fileName;

    private void Awake()
    {
        instance = this;
    }

    private void OnPostRender()
    {
        if (takeSSOnNextFrame)
        {
            takeSSOnNextFrame = false;

            RenderTexture renderTexture = cam.targetTexture;

            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0, 0);

            byte[] byteArray = renderResult.EncodeToPNG();
            System.IO.File.WriteAllBytes(path + "/" + fileName + ".png", byteArray);
            //Debug.Log("Screenshot saved.");

            RenderTexture.ReleaseTemporary(renderTexture);
            cam.targetTexture = AppManager.instance.originalRenderTexture;
        }
    }

    private void TakeScreenshot(Camera cam, int width, int height, string path, string fileName)
    {
        cam.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        this.cam = cam;
        this.path = path;
        this.fileName = fileName;
        takeSSOnNextFrame = true;
    }

    public static void TakeSS(Camera cam, int width, int height, string path, string fileName)
    {
        instance.TakeScreenshot(cam, width, height, path, fileName);
    }
}
