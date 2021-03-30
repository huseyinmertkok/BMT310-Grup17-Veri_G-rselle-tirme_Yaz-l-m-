using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class PieChart2D : MonoBehaviour
{
    [Header("Data")]
    public string unit = "kg";
    public string[] labels;
    public float[] values;
    public Color[] wedgeColors;

    [Header("Resources")]
    public Image wedgePrefab;
    public GameObject listPartPrefab;
    public Transform wedgeParent;
    public Transform listParent;
    private List<Image> wedges = new List<Image>();
    private List<GameObject> list = new List<GameObject>();

    [Header("Animations")]
    public float duration = 1f;
    public Ease ease;
    public enum WedgeAnimation { noAnimation, allSameTime, oneByOne };
    public enum ListAnimation { noAnimation, allSameTime, oneByOne };
    public WedgeAnimation wedgeAnimation;
    public ListAnimation listAnimation;

    void Start()
    {
        MakeGraph();
    }

    private void MakeGraph()
    {
        float total = 0f;
        float zRotation = 0f;
        for (int i = 0; i < values.Length; i++)
        {
            total += values[i];
        }

        for (int i = 0; i < values.Length; i++)
        {
            Image newWedge = Instantiate(wedgePrefab);
            newWedge.transform.SetParent(wedgeParent, false);
            newWedge.color = wedgeColors[i];
            newWedge.fillAmount = values[i] / total;
            newWedge.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, zRotation));
            zRotation -= newWedge.fillAmount * 360f;

            GameObject listPart = Instantiate(listPartPrefab);
            listPart.transform.SetParent(listParent, false);
            listPart.GetComponentInChildren<Image>().color = wedgeColors[i];
            TextMeshProUGUI text = listPart.GetComponentInChildren<TextMeshProUGUI>();
            string listPartText = labels[i] + "\n" + values[i] + " " + unit + "\n" + "%" + newWedge.fillAmount * 100f;
            text.text = listPartText;
            text.color = wedgeColors[i];

            wedges.Add(newWedge);
            //newWedge.gameObject.SetActive(false);
            list.Add(listPart);
            //listPart.SetActive(false);

        }

        switch (wedgeAnimation)
        {
            case WedgeAnimation.oneByOne:
                foreach (Image wedge in wedges)
                {
                    wedge.gameObject.SetActive(false);
                }
                StartCoroutine(WedgeAnimOneByOne(0));
                break;
            case WedgeAnimation.allSameTime:
                WedgeAnimAllSameTime();
                break;
        }

        switch (listAnimation)
        {
            case ListAnimation.oneByOne:
                foreach (GameObject listPart in list)
                {
                    listPart.SetActive(false);
                }
                StartCoroutine(ListAnimOneByOne(0));
                break;
            case ListAnimation.allSameTime:
                ListAnimAllSameTime();
                break;
        }
    }

    private IEnumerator ListAnimOneByOne(int partIndex)
    {
        list[partIndex].SetActive(true);
        Tween anim = list[partIndex].transform.DOScaleX(0f, duration * wedges[partIndex].fillAmount).From();
        yield return anim.WaitForCompletion();

        if (partIndex + 1 < list.Count)
        {
            StartCoroutine(ListAnimOneByOne(partIndex + 1));
        }
        else
        {
            VerticalLayoutGroup verticalLayoutGroup = listParent.GetComponent<VerticalLayoutGroup>();
            verticalLayoutGroup.enabled = false;
            verticalLayoutGroup.enabled = true;
        }
    }

    private void ListAnimAllSameTime()
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].SetActive(true);
            list[i].transform.DOScaleX(0f, duration).From();
        }
    }

    private IEnumerator WedgeAnimOneByOne(int imageIndex)
    {
        wedges[imageIndex].gameObject.SetActive(true);
        Tween anim = wedges[imageIndex].DOFillAmount(0, duration * wedges[imageIndex].fillAmount).From();

        yield return anim.WaitForCompletion();

        if (imageIndex + 1 < wedges.Count)
        {
            StartCoroutine(WedgeAnimOneByOne(imageIndex + 1));
        }
    }

    private void WedgeAnimAllSameTime()
    {
        
        for (int i = 0; i < wedges.Count; i++)
        {
            wedges[i].gameObject.SetActive(true);
            wedges[i].DOFillAmount(0, duration).From();
        }
    }
}
