using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineChartCodes : MonoBehaviour
{
    private Transform barChartParent;
    public GameObject spherePrefab;
    private Transform[] barParts;
    private Vector3[] spherePoses;
    private Transform[] sphereObjects;
    public GameObject linePrefab;
    private LineRenderer line;
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        FindParentAndBarParts();
    }

    void Update()
    {
        UpdateSpherePos();
    }

    private void FindParentAndBarParts()
    {
        barChartParent = transform.GetChild(0).Find("BarParent");
        //barChartParent.gameObject.SetActive(false);
        line = Instantiate(linePrefab, Vector3.zero, Quaternion.identity).GetComponent<LineRenderer>();
        line.transform.parent = transform;
        //line.transform.localPosition = Vector3.zero;

        int partCount = barChartParent.GetChild(0).childCount;
        barParts = new Transform[partCount];
        spherePoses = new Vector3[partCount];
        sphereObjects = new Transform[partCount];
        line.positionCount = partCount;
        for (int i = 0; i < partCount; i++)
        {
            barParts[i] = barChartParent.GetChild(0).GetChild(i);
            sphereObjects[i] = Instantiate(spherePrefab, Vector3.zero, Quaternion.identity).transform;
            sphereObjects[i].name = "" + i;
            sphereObjects[i].parent = transform;
        }
    }

    private void UpdateSpherePos()
    {
        for (int i = 0; i < spherePoses.Length; i++)
        {
            spherePoses[i] = barParts[i].GetChild(1).position;
            sphereObjects[i].position = spherePoses[i];
            line.SetPosition(i, spherePoses[i]);
        }
    }
}
