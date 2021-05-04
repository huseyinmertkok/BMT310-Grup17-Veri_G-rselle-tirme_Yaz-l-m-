using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariablePartCodes : MonoBehaviour
{
    public int rowIndex;
    public Text variableNameText;

    public void ChooseBTN()
    {
        if (AppManager.instance.variable1Text.text.Equals("Seçilmedi"))
        {
            AppManager.instance.variable1Text.text = variableNameText.text;
            AppManager.instance.rowIndex1 = rowIndex;
        }
        else if (AppManager.instance.variable2Text.text.Equals("Seçilmedi"))
        {
            AppManager.instance.variable2Text.text = variableNameText.text;
            AppManager.instance.rowIndex2 = rowIndex;

            AppManager.instance.fileBrowser.SetDataToChartCreator(AppManager.instance.rowIndex1, AppManager.instance.rowIndex2);
        }
    }
}
