using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileBrowser : MonoBehaviour
{
    /*
    FileBrowserTest class'ının içindeki fonksiyonları test edebilmek için bu kod dosyası oluşturuldu.
    Bunun sebebi unity'de Test dosyalarının ayrı bir Assembly içinde bulunması.
    FileBrowserTest class'ının içinde bulunduğu Assembly-CSharp, Unity'nin içinden ulaşılamadığı için;
    Test dosyasının bulunduğu Assembly'e yeni bir kod dosyası oluşturup, içine aynı fonksiyonları yazarak sorunu herhangi bir problem olmadan çözdük
    */

    public string[] getFirstLine(string[,] data)
    {
        string[] fLineData = new string[data.GetLength(1)];
        for (int i = 0; i < fLineData.GetLength(0); i++)
        {
            fLineData[i] = data[0, i];
        }
        return fLineData;
    }

    public string[] chooseColumn(string[,] data, int columnNumber)
    {
        string[] chosenColumn = new string[(data.GetLength(0)) - 1];
        //string[] chosenColumn = new string[(data.GetLength(0))];
        for (int i = 0; i < chosenColumn.GetLength(0); i++)
        {
            chosenColumn[i] = data[i + 1, columnNumber];
        }
        return chosenColumn;
    }

    public int printArr(string[,] dataArray)
    {
        int count = 0;
        for (int i = 0; i < dataArray.GetLength(0); i++)
        {
            for (int z = 0; z < dataArray.GetLength(1); z++)
            {
                Debug.Log("Satır = " + (i + 1).ToString() + " Sütun = " + (z + 1).ToString() + "\n" + dataArray[i, z]);
                count++;
            }
        }
        return count;
    }

    public int columnReader(string locationOfData, string extension)
    {
        StreamReader strReader1 = new StreamReader(locationOfData);
        bool endOfFile1 = false;
        int numberofLine = 0, numberofColumn = 0;
        //Dosyadaki sütun sayısını belirleyen kod.
        while (!endOfFile1)
        {
            string data_String = strReader1.ReadLine();
            if (data_String == null)
            {
                endOfFile1 = true;
                break;
            }

            if (extension.Equals(".csv"))
            {
                var data_values = data_String.Split(';');
                numberofLine++;
                for (int i = 0; i < data_values.Length; i++)
                {
                    if (numberofLine == 1)
                        numberofColumn++;
                }
            }
            else if (extension.Equals(".txt"))
            {
                var data_values = data_String.Split(' ');
                numberofLine++;
                for (int i = 0; i < data_values.Length; i++)
                {
                    if (numberofLine == 1)
                        numberofColumn++;
                }
            }

        }
        return numberofColumn;
    }

    public int lineReader(string locationOfData)
    {
        StreamReader strReader1 = new StreamReader(locationOfData);
        bool endOfFile1 = false;
        int numberofLine = 0;
        //Dosyadaki satır sayısını belirleyen kod.
        while (!endOfFile1)
        {
            string data_String = strReader1.ReadLine();
            if (data_String == null)
            {
                endOfFile1 = true;
                break;
            }
            numberofLine++;
        }
        return numberofLine;
    }

}
