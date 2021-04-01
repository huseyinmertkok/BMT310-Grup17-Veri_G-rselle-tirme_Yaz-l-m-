using UnityEngine;
using System.Collections;
using System.IO;
using SimpleFileBrowser;
using UnityEngine.UI;

public class FileBrowserTest : MonoBehaviour
{
    public Text text;
    // Not1: FileBrowser'un döndürdüğü konumların sonunda '\' karakteri yer almaz
    // Not2: FileBrowser tek seferde sadece 1 diyalog gösterebilir

    void Start()
    {
        // Filtreleri belirle (opsiyonel)
        // Eğer filtreler oyun esnasında hep aynı kalacaksa, filtreleri her seferinde
        // tekrar tekrar belirlemek yerine sadece bir kere belirlemek yeterlidir
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Veri Dosyaları", ".txt", ".pdf", ".xls", ".xlsx", ".csv"));
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

    public void BrowseBTN()
    {
        StartCoroutine(DosyaVeKlasorSecmeDiyaloguGosterCoroutine());
    }

    IEnumerator DosyaVeKlasorSecmeDiyaloguGosterCoroutine()
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

            string fileContent = File.ReadAllText(@FileBrowser.Result[0]);
            text.text = fileContent;

            if (extension.Equals(".csv"))
                StringArrayForCSVFile(@FileBrowser.Result[0]);
            else if (extension.Equals(".txt"))
                StringArrayForTXTFile(@FileBrowser.Result[0]);               
        }
    }

    //CSV dosyasındaki verileri iki boyutlu string arrayine dönüştürür.
    public string [,] StringArrayForCSVFile(string locationOfData)
    {
        int numberofLine = lineReader(locationOfData);
        int numberofColumn = columnReader(locationOfData);
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
        printArr(dataArray);
        return dataArray;
    }

    //TXT dosyasındaki verileri iki boyutlu string arrayine dönüştürür.
    public string [,] StringArrayForTXTFile(string locationOfData)
    {
        int numberofLine = lineReader(locationOfData);
        int numberofColumn = columnReader(locationOfData);
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
                a++;
                if (a == data_values.Length)
                    j++;
            }
        }
        //İki boyutlu arrayi yazdıran kod.
        printArr(dataArray);
        return dataArray;
    }
    
    void printArr(string [,] dataArray)
    {
        for (int i = 0; i < dataArray.GetLength(0); i++)
        {
            for (int z = 0; z < dataArray.GetLength(1); z++)
            {
                Debug.Log(dataArray[i, z]);
            }
        }
    }

    int columnReader(string locationOfData)
    {
        StreamReader strReader1 = new StreamReader(locationOfData);
        bool endOfFile1 = false;
        int numberofLine = 0, numberofColumn = 0;
        //Dosyadaki sütun ve satır sayısını belirleyen kod.
        while (!endOfFile1)
        {
            string data_String = strReader1.ReadLine();
            if (data_String == null)
            {
                endOfFile1 = true;
                break;
            }
            var data_values = data_String.Split(';');
            numberofLine++;
            for (int i = 0; i < data_values.Length; i++)
            {
                if (numberofLine == 1)
                    numberofColumn++;
            }
        }
        return numberofColumn;
    }

    int lineReader(string locationOfData)
    {
        StreamReader strReader1 = new StreamReader(locationOfData);
        bool endOfFile1 = false;
        int numberofLine = 0;
        //Dosyadaki sütun ve satır sayısını belirleyen kod.
        while (!endOfFile1)
        {
            string data_String = strReader1.ReadLine();
            if (data_String == null)
            {
                endOfFile1 = true;
                break;
            }
            var data_values = data_String.Split(';');
            numberofLine++;
        }
        return numberofLine;
    }
}
