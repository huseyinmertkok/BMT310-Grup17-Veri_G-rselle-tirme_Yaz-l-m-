using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextContainerCodes : MonoBehaviour
{
    public GameObject labelCont;
    void Start()
    {
        labelCont.SetActive(true);
        labelCont.transform.localScale = transform.localScale;
        labelCont.transform.GetChild(0).GetComponent<TextMeshPro>().text = AppManager.instance.chartCreator.tempData1[int.Parse(transform.parent.name)];
        transform.GetChild(0).GetComponent<TextMeshPro>().text += AppManager.instance.chartCreator.dataUnit;
    }
}
