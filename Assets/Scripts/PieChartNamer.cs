using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieChartNamer : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < transform.childCount; i++)
        {
            yield return new WaitForEndOfFrame();
        }
        FindParentAndBarParts();
    }

    private void FindParentAndBarParts()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).name = "" + i;
            transform.GetChild(i).localRotation = Quaternion.identity;
            transform.GetChild(i).GetComponent<MeshCollider>().convex = false;
        }
    }
}
