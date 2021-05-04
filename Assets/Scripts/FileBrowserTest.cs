using UnityEngine;
using System.Collections;
using System.IO;
using SimpleFileBrowser;
using UnityEngine.UI;
using FlexFramework.Excel;
using UnityEditor;
using System;

public class FileBrowserTest : MonoBehaviour
{
    //public Text text;
    public ChartCreator chartCreator;
    private string[,] data;
    // Not1: FileBrowser'un döndürdüğü konumların sonunda '\' karakteri yer almaz
    // Not2: FileBrowser tek seferde sadece 1 diyalog gösterebilir

    void Start()
    {
        // Filtreleri belirle (opsiyonel)
        // Eğer filtreler oyun esnasında hep aynı kalacaksa, filtreleri her seferinde
        // tekrar tekrar belirlemek yerine sadece bir kere belirlemek yeterlidir
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Veri Dosyaları", ".txt", ".xls", ".xlsx", ".csv"));
        // Varsayılan filtreyi belirle (opsiyonel)
        // Eğer varsayılan filtre başarıyla belirlendiyse fonksiyon true döndürür
        // Bu örnekte, varsayılan filtre olarak .jpg'i tutan "Resimler"i belirle 
        FileBrowser.SetDefaultFilter(".txt");

        // Yoksayılacak dosya uzantılarını belirle (opsiyonel) (varsayılan olarak .lnk ve .tmp uzantılı dosyalar yoksayılır)
        // Bu fonksiyonu çağırırsanız, siz elle eklemediğiniz müddetçe .lnk ve .tmp uzantıları artık yoksayılmaz
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

        // Yeni bir hızlı erişim klasörü ekle (opsiyonel) (eğer işlem başarılı olursa true döndürülür)
        // Bir hızlı erişim klasörünü sadece bir kere eklemek yeterlidir
        // İsim: Kullanıcılar
        // Konum: C:\Users
        // İkon: varsayılan (klasör ikonu)
        FileBrowser.AddQuickLink("Kullanıcılar", "C:\\Users", null);

        // Dosya kaydetme diyaloğu göster
        // onSuccess: null, bir şey yapma (bir başka deyişle, bu amaçsız bir diyalog)
        // onCancel: null, bir şey yapma
        // Kaydetme modu: sadece dosyalar, Birden çok dosya seçebilme: kapalı (false)
        // İlk konum: "C:\", Varsayılan dosya ismi: "Resim.png"
        // Başlık: "Farklı Kaydet", Kaydet butonu yazısı: "Kaydet"
        // FileBrowser.ShowSaveDialog( null, null, FileBrowser.PickMode.Files, false, "C:\\", "Resim.png", "Farklı Kaydet", "Kaydet" );

        // Klasör seçme diyaloğu göster 
        // onSuccess: klasörün konumunu konsola yazdır
        // onCancel: konsola "İptal edildi" yazdır
        // Dosya seçme modu: sadece klasörler, Birden çok klasör seçebilme: kapalı (false)
        // İlk konum: varsayılan (Belgelerim), Varsayılan dosya ismi: boş
        // Başlık: "Klasör Seç", Seç butonu yazısı: "Seç"
        // FileBrowser.ShowLoadDialog( ( konum ) => { Debug.Log( "Seçilen klasör: " + konum[0] ); },
        //                         () => { Debug.Log( "İptal edildi" ); }, 
        //                         FileBrowser.PickMode.Folders, false, null, null, "Klasör Seç", "Seç" );

        // Coroutine örneğini çalıştır
        //StartCoroutine(DosyaVeKlasorSecmeDiyaloguGosterCoroutine());
    }

    public IEnumerator DosyaVeKlasorSecmeDiyaloguGosterCoroutine()
    {
        // Dosya ve klasör seçme diyaloğu göster ve kullanıcının diyaloğu kapatmasını bekle
        // Dosya seçme modu: hem dosyalar hem klasörler, Birden çok dosya/klasör seçebilme: açık (true)
        // İlk konum: varsayılan (Belgelerim), Varsayılan dosya ismi: boş
        // Başlık: "Yüklenecek Dosya ve Klasörleri Seçin", Seç butonu yazısı: "Yükle"
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Yüklenecek Dosya ve Klasörleri Seçin", "Yükle");

        // Diyalog kapatıldı
        // Konsola, kullanıcının en az 1 dosya ve/veya klasör seçip seçmediğini yazdır (FileBrowser.Success)
        //Debug.Log(FileBrowser.Success);

        if (FileBrowser.Success)
        {
            // Seçilen dosya ve/veya klasör(ler)in konumunu da yazır (FileBrowser.Result) (eğer FileBrowser.Success false ise, değeri null'dır)
            /*for (int i = 0; i < FileBrowser.Result.Length; i++)
                Debug.Log(FileBrowser.Result[i]);*/

            // Seçilen ilk dosyanın byte'larını FileBrowserHelpers vasıtasıyla oku
            // File.ReadAllBytes'in aksine, bu fonksiyon Android 10 ve üzerinde de çalışır
            byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);
            string extension = Path.GetExtension(@FileBrowser.Result[0]);
            // Veya, ilk dosyayı persistentDataPath konumuna kopyala
            /*string hedefKonum = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[0]));
            FileBrowserHelpers.CopyFile(FileBrowser.Result[0], hedefKonum);*/

            //string fileContent = File.ReadAllText(@FileBrowser.Result[0]);
            //text.text = fileContent;

            if (extension.Equals(".csv"))
                data = StringArrayForCSVFile(@FileBrowser.Result[0], extension);
            else if (extension.Equals(".txt"))
                data = StringArrayForTXTFile(@FileBrowser.Result[0], extension);
            else if (extension.Equals(".xlsx"))
                data = StringArrayForXLSXFile(bytes);

            chartCreator.dataName = Path.GetFileNameWithoutExtension(@FileBrowser.Result[0]);

            string[] rows = getFirstLine(data);
            for (int i = 0; i < rows.Length; i++)
            {
                //Debug.Log(rows[i]);
                GameObject part = Instantiate(AppManager.instance.variablePartPrefab);
                part.transform.SetParent(AppManager.instance.variablePartParent, false);
                VariablePartCodes partCodes = part.GetComponent<VariablePartCodes>();
                partCodes.rowIndex = i;
                partCodes.variableNameText.text = rows[i];
                partCodes.transform.localScale = Vector3.one;
            }

            //SetDataToChartCreator();

            AppManager.instance.nextBTN.SetActive(true);
            AppManager.instance.dataNameText.text = chartCreator.dataName;
            AppManager.instance.screenshotHandler.path = Path.GetDirectoryName(@FileBrowser.Result[0]) + Path.GetDirectoryName(@FileBrowser.Result[0])[2];
        }
    }

    public void SetDataToChartCreator(int rowIndex1, int rowIndex2)
    {
        //Buradaki temp hangi sütunun Label olarak kullanılacağıdır. '2' sayısını değiştirerek sutünu değiştirebilirsiniz. Sütun string değerler içermelidir.
        //string[] temp = chooseColumn(data, 2);
        string[] temp = chooseColumn(data, rowIndex1);
        bool isDuplicate;
        int elemanSayisi = 0;

        //Aşağıdaki işlem unique olarak kaç değişken olduğunu belirler. 
        for (int i = 0; i < temp.GetLength(0); i++)
        {
            isDuplicate = false;
            for (int j = 0; j < i; j++)
            {
                if (temp[i] == temp[j])
                {
                    isDuplicate = true;
                    break;
                }
            }
            if (!isDuplicate)
            {
                elemanSayisi++;
            }
        }

        //Aşşağıdaki işlem unique elemanları diziye alır.
        string[] uniqueElements = new string[elemanSayisi];
        int a = 0;
        for (int i = 0; i < temp.GetLength(0); i++)
        {
            isDuplicate = false;
            for (int j = 0; j < i; j++)
            {
                if (temp[i] == temp[j])
                {
                    isDuplicate = true;
                    break;
                }
            }
            if (!isDuplicate)
            {
                uniqueElements[a] = temp[i];
                a++;
            }
        }

        /*chartCreator.dataLabels = new string[uniqueElements.GetLength(0)];
        chartCreator.dataValues = new float[uniqueElements.GetLength(0)];
        /*for (int z = 0; z < uniqueElements.GetLength(0); z++)
        {
            Debug.Log(uniqueElements[z]);
        }

        for (int i = 0; i < uniqueElements.GetLength(0); i++)
        {
            chartCreator.dataLabels[i] = uniqueElements[i];
        }

        for (int i = 0; i < uniqueElements.GetLength(0); i++)
        {
            //Buradaki 3 sayısı Value olarak hangi sütundaki değeri aldığını seçer. Sayıyı değiştirerek sütunu da değiştirebilirsiniz. Sütun elemanları sayı olmalıdır yoksa hata verir.
            //chartCreator.dataValues[i] = float.Parse(data[i + 1, 3]);
            chartCreator.dataValues[i] = float.Parse(data[i + 1, rowIndex2]);
        }*/
        chartCreator.tempData1 = uniqueElements;
        chartCreator.tempData2 = new string[uniqueElements.Length];
        for (int i = 0; i < uniqueElements.Length; i++)
        {
            chartCreator.tempData2[i] = data[i + 1, rowIndex2];
        }
    }

    //CSV dosyasındaki verileri iki boyutlu string arrayine dönüştürür.
    public string [,] StringArrayForCSVFile(string locationOfData, string extension)
    {
        int numberofLine = lineReader(locationOfData);
        int numberofColumn = columnReader(locationOfData,extension);
        Debug.Log("CSV Dosyası\nSatır Sayısı: " + numberofLine.ToString() + " " + "Sütun Sayısı: " + numberofColumn);
        string[,] dataArray = new string[numberofLine, numberofColumn];
        int a = 0, j = 0;
        StreamReader strReader = new StreamReader(locationOfData);
        bool endOfFile = false;
        //Dosyadaki sütun ve satırları iki boyutlu dizi yapan kod.
        while (!endOfFile)
        {
            a = 0;
            string data_String = strReader.ReadLine();
            if (data_String == null)
            {
                endOfFile = true;
                break;
            }
            var data_values = data_String.Split(';');
            for (int i = 0; i < data_values.Length; i++)
            {
                dataArray[j, a] = data_values[i].ToString();
                a++;
                if (a == data_values.Length)
                    j++;
            }
        }
        //İki boyutlu arrayi yazdıran kod.
        //printArr(dataArray);
        return dataArray;
    }

    //TXT dosyasındaki verileri iki boyutlu string arrayine dönüştürür.
    public string [,] StringArrayForTXTFile(string locationOfData, string extension)
    {
        int numberofLine = lineReader(locationOfData);
        int numberofColumn = columnReader(locationOfData,extension);
        Debug.Log("TXT Dosyası\nSatır Sayısı: " + numberofLine.ToString() + " " + "Sütun Sayısı: " + numberofColumn);
        string[,] dataArray = new string[numberofLine, numberofColumn];
        int a = 0, j = 0;
        StreamReader strReader = new StreamReader(locationOfData);
        bool endOfFile = false;
        //Dosyadaki sütun ve satırları iki boyutlu dizi yapan kod.
        while (!endOfFile)
        {
            a = 0;
            string data_String = strReader.ReadLine();
            if (data_String == null)
            {
                endOfFile = true;
                break;
            }
            var data_values = data_String.Split(' ');
            for (int i = 0; i < data_values.Length; i++)
            {
                dataArray[j, a] = data_values[i].ToString();
                //Debug.Log(j.ToString() + " " + a.ToString() + dataArray[j, a].ToString());
                a++;              
            }
            j++;
        }
        //İki boyutlu arrayi yazdıran kod.
        //printArr(dataArray);
        return dataArray;
    }
    
    void printArr(string [,] dataArray)
    {
        for (int i = 0; i < dataArray.GetLength(0); i++)
        {
            for (int z = 0; z < dataArray.GetLength(1); z++)
            {
                Debug.Log("Satır = "+(i+1).ToString()+" Sütun = " +(z+1).ToString() + "\n" +dataArray[i, z]);
            }
        }
    }
    
    //XLSX dosyasındaki verileri iki boyutlu string arrayine dönüştürür.
    public string[,] StringArrayForXLSXFile(byte[] bytes)
    {
        string[,] temp;
        var book = new WorkBook(bytes);
        var sheet = book[0];
        int line = 0; int column = 0;
        while (true)
        {
            try
            {
                if (sheet[line][0] != null)
                {
                    line++;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                break;
            }
        }

        while (true)
        {
            try
            {
                if (sheet[0][column] != null)
                {
                    column++;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                break;
            }
        }
        temp = new string[line, column];
        for (int i = 0; i < temp.GetLength(0); i++)
        {
            for (int j = 0; j < temp.GetLength(1); j++)
            {
                temp[i, j] = sheet[i][j].ToString();
                //Debug.Log(temp[i, j]);
            }
        }
        return temp;
    }
    
    int columnReader(string locationOfData, string extension)
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

    int lineReader(string locationOfData)
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
    
    string[] getFirstLine(string[,] data)
    {
        string[] fLineData = new string[data.GetLength(1)];
        for(int i = 0; i<fLineData.GetLength(0); i++)
        {
            fLineData[i] = data[0,i];
        }
        return fLineData;
    }
    
    string[] chooseColumn(string[,] data, int columnNumber)
    {
        string[] chosenColumn = new string[(data.GetLength(0)) - 1];
        //string[] chosenColumn = new string[(data.GetLength(0))];
        for (int i = 0; i < chosenColumn.GetLength(0); i++)
        {
            chosenColumn[i] = data[i+1, columnNumber];
        }
        return chosenColumn;
    }

    /*string[] getDataLabels(string[,] data)
    {
        string[] result = new string[data.GetLength(1)];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = data[1, i];
        }
        return result;
    }*/
}
