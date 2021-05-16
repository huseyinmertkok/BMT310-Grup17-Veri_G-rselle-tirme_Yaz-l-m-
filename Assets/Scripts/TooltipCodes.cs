using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipCodes : MonoBehaviour
{
    public RectTransform backgroundRectTransform, parentRectTransform, canvasRextTransform;
    public TextMeshProUGUI textMeshPro;
    private void Awake()
    {
        //SetText("Data Label" + "\n" + "Data Value");
    }

    private void Update()
    {
        parentRectTransform.anchoredPosition = Input.mousePosition / canvasRextTransform.localScale.x;
    }

    public void SetText(string label, string value)
    {
        string text = label + "\n" + value + AppManager.instance.chartCreator.dataUnit;
        textMeshPro.SetText(text);
        textMeshPro.ForceMeshUpdate();

        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(16, 16);

        backgroundRectTransform.sizeDelta = textSize + paddingSize;
    }
}
