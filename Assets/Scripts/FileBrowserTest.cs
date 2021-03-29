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
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Veri Dosyaları", ".txt", ".pdf", "xls", "xlsx", "csv"));
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

            //Debug.Log(FileBrowser.Result[0]);

            string fileContent = File.ReadAllText(@FileBrowser.Result[0]);
            text.text = fileContent;
        }
    }
}