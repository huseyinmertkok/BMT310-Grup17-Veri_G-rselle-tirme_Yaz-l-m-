using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarChartNamer : MonoBehaviour
{
    public bool isLine = false;
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        FindParentAndBarParts();
    }

    private void FindParentAndBarParts()
    {
        Transform barChartParent = transform.GetChild(0).Find("BarParent");
        for (int i = 0; i < barChartParent.GetChild(0).childCount; i++)
        {
            barChartParent.GetChild(0).GetChild(i).name = "" + i;
            barChartParent.GetChild(0).GetChild(i).GetChild(0).name = "" + i;
            barChartParent.GetChild(0).GetChild(i).GetChild(1).GetChild(0).transform.position -= Vector3.forward * .5f;
            if (isLine)
            {
                //barChartParent.GetChild(0).GetChild(i).gameObject.SetActive(true);
                barChartParent.GetChild(0).GetChild(i).GetChild(0).gameObject.SetActive(false);
            }
        }

        
    }
}
