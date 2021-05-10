using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChartMouseReader : MonoBehaviour
{
    public Camera mouseCam;
    public RectTransform canvasTransform;
    public LayerMask chartObject;
    public RectTransform chartSide;
    void Start()
    {

    }

    void Update()
    {
        if (AppManager.instance.selectGraphPanel.activeSelf)
        {
            RayCast();
        }
    }

    private void RayCast()
    {
        Vector2 newPos = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
        newPos.x -= 0.5f;
        newPos.y -= 0.5f;
        Vector2 canvasPos = new Vector2(newPos.x * canvasTransform.rect.width, newPos.y * canvasTransform.rect.height);

        Vector2 targetPos = canvasPos - chartSide.anchoredPosition;

        Vector3 rayPos = mouseCam.ScreenToWorldPoint(new Vector3(targetPos.x + 400, targetPos.y + 300, 3));
        Ray ray = new Ray(rayPos - Vector3.forward * +3, Vector3.forward);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, chartObject))
        {

        }
    }
}
