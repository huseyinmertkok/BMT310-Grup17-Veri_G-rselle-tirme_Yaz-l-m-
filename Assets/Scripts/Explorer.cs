using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using SmartDLL;

public class Explorer : MonoBehaviour
{
    public Text eText;
    public SmartFileExplorer fileExplorer = new SmartFileExplorer();
    private bool readText = false;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (fileExplorer.resultOK && readText)
        {
            ReadText(fileExplorer.fileName);
            readText = false;
        }
    }

    public void ShowExplorer()
    {
        string initialDir = @"C:\";
        bool restoreDir = true;
        string title = "Open a Text File";
        string defExt = "txt";
        string filter = "txt files (*.txt)|*.txt";
        //defExt = "xls";
        //filter = "txt files (*.xls)|*.xls";

        fileExplorer.OpenExplorer(initialDir, restoreDir, title, defExt, filter);
        readText = true;
    }

    private void ReadText(string path)
    {
        eText.text = File.ReadAllText(path);
    }
}
