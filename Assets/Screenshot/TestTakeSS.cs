using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTakeSS : MonoBehaviour
{
    public string path;
    public Camera ssCam;
    public Vector2Int resulution;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeSSCodes.TakeSS(ssCam, resulution.x, resulution.y, path, "deneme");
        }
    }
}
