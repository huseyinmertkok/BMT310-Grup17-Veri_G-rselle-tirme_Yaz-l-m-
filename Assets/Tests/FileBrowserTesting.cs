using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    //Bu kod dosyasında FileBrowserTest içindeki fonksiyonlara Unit Test uygulanacaktır.
    public class FileBrowserTesting
    {
        
        //FileBrowserTest içindeki getFirstLine() komutunun incelenmesi:
        [Test]
        public void getFirstLineTest()
        {
            FileBrowser fileBrowser = new FileBrowser();
            /*
             Kendi elimizle data adlı bir çift boyutlu string dizisi oluşturduk
             Bunun dizinin ilk satırını almak amacımız.
             expectedFirstLine dizisinde beklenilen ilk satır yazıldı ve fonksiyorun çağırılarak teste tabii tutuldu.
             */

            string[,] data = { {"00","01","02","03" },
                               {"10","11","12","13" },
                               {"20","21","22","23" },
                               {"30","31","32","33" },
                               {"40","41","42","43" },
                               {"50","51","52","53" }};
            string[] expectedFirstLine = { "00", "01", "02", "03" };
            Assert.AreEqual(expectedFirstLine, fileBrowser.getFirstLine(data));
        }

        [Test]
        public void chooseColumnTest()
        {
            /*
             Kendi elimizle data adlı bir çift boyutlu string dizisi oluşturduk
             Bunun dizinin seçilen sütununu almak amacımız. Burada görüldüğü gibi 3 sayısı gönderilerek (sayılmaya 0'dan başlanıldığı için) 4. sütun seçildi.
             expectedChosenLine dizisinde beklenilen 4. sütun yazıldı ve fonksiyorun çağırılarak teste tabii tutuldu.
             */

            FileBrowser fileBrowser = new FileBrowser();
            string[,] data = { {"00","01","02","03" },
                               {"10","11","12","13" },
                               {"20","21","22","23" },
                               {"30","31","32","33" },
                               {"40","41","42","43" },
                               {"50","51","52","53" }};
            string[] expectedChosenLine = { "13", "23", "33", "43", "53" };
            Assert.AreEqual(expectedChosenLine, fileBrowser.chooseColumn(data, 3));
        }

        [Test]
        public void chooseColumnTest2()
        {
            /*
             Kendi elimizle data adlı bir çift boyutlu string dizisi oluşturduk
             Bunun dizinin seçilen sütununu almak amacımız. Burada görüldüğü gibi 2 sayısı gönderilerek (sayılmaya 0'dan başlanıldığı için) 3. sütun seçildi.
             expectedChosenLine dizisinde beklenilen 3. sütun yazıldı ve fonksiyorun çağırılarak teste tabii tutuldu.
             */

            FileBrowser fileBrowser = new FileBrowser();
            string[,] data = { {"00","01","02","03" },
                               {"10","11","12","13" },
                               {"20","21","22","23" },
                               {"30","31","32","33" },
                               {"40","41","42","43" },
                               {"50","51","52","53" }};
            string[] expectedChosenLine = { "12", "22", "32", "42", "52" };
            Assert.AreEqual(expectedChosenLine, fileBrowser.chooseColumn(data, 2));
        }

        [Test]
        public void printArrTest()
        {
            /*
             printArr komutu aslında çift boyutlu diziyi yazdıran bir kod.
             Biz bunu eleman sayısını alarak test ediyoruz.
             Görüldüğü gibi 24 eleman var ve fonksiyon çağırılıp teste tabii tutuluyor.
             */

            FileBrowser fileBrowser = new FileBrowser();
            string[,] data = { {"00","01","02","03" },
                               {"10","11","12","13" },
                               {"20","21","22","23" },
                               {"30","31","32","33" },
                               {"40","41","42","43" },
                               {"50","51","52","53" }};
            Assert.AreEqual(24, fileBrowser.printArr(data));
        }
        
        [Test]
        public void columnReaderTestWithTXT()
        {
            /*
             Seçilen hedefteki dosya içindeki verinin sütun sayısını bulan columnReader() fonksiyonunu TXT için test edeceğiz.
             Test dosyası içindeki veri.txt dosyasında bakacak olursak 6 adet sütunu var. 
             Bu bilgiyle birlikte fonksiyonu çağırırak test ediyoruz.
             */

            FileBrowser fileBrowser = new FileBrowser();
            Assert.AreEqual(6, fileBrowser.columnReader("Assets\\Tests\\veri.txt", ".txt"));
        }

        [Test]
        public void columnReaderTestWithCSV()
        {
            /*
             Seçilen hedefteki dosya içindeki verinin sütun sayısını bulan columnReader() fonksiyonunu CSV için test edeceğiz.
             Test dosyası içindeki data.csv dosyasında bakacak olursak 6 adet sütunu var. 
             Bu bilgiyle birlikte fonksiyonu çağırırak test ediyoruz.
             */

            FileBrowser fileBrowser = new FileBrowser();
            Assert.AreEqual(5, fileBrowser.columnReader("Assets\\Tests\\data.csv", ".csv"));
        }


        [Test]
        public void lineReaderTestWithTXT()
        {
            /*
             Seçilen hedefteki dosya içindeki verinin satır sayısını bulan lineReader() fonksiyonunu TXT için test edeceğiz.
             Test dosyası içindeki veri.txt dosyasında bakacak olursak 4 adet satır var. 
             Bu bilgiyle birlikte fonksiyonu çağırırak test ediyoruz.
             */

            FileBrowser fileBrowser = new FileBrowser();
            Assert.AreEqual(4, fileBrowser.lineReader("Assets\\Tests\\veri.txt"));
        }


        [Test]
        public void lineReaderTestWithCSV()
        {
            /*
             Seçilen hedefteki dosya içindeki verinin satır sayısını bulan lineReader() fonksiyonunu CSV için test edeceğiz.
             Test dosyası içindeki veri.csv dosyasında bakacak olursak 6 adet satır var. 
             Bu bilgiyle birlikte fonksiyonu çağırırak test ediyoruz.
             */

            FileBrowser fileBrowser = new FileBrowser();
            Assert.AreEqual(6, fileBrowser.lineReader("Assets\\Tests\\data.csv"));
        }

        [Test]
        public void stringArrayForTXT_Test()
        {
            /*
             Seçilen hedefteki TXT dosyası içindeki veriyi çift boyutlu string dizisine dönüştüren StringArrayForTXTFile() fonksiyonunu test edeceğiz.
             Aşağıda data adlı çift boyutlu string dizisi oluşturduk ve bunun içinde veri.txt değerini koyduk.
             Oluşturduğumuz bu dizi ile fonksiyondan çıkacak dizisinin eşitliğini test edeceğiz.
             */

            FileBrowser fileBrowser = new FileBrowser();
            string[,] data = { {"Ali","Veli","Orhan","Amca","Salih","Kerim" },
                               {"3","4","5","6","7","3125" },
                               {"8","9","10","11","12","8546"},
                               {"13","14","15","16","17","3356" } };
            Assert.AreEqual(data, fileBrowser.StringArrayForTXTFile("Assets\\Tests\\veri.txt", ".txt"));
        }

        [Test]
        public void stringArrayForCSV_Test()
        {
            /*
             Seçilen hedefteki CSV dosyası içindeki veriyi çift boyutlu string dizisine dönüştüren StringArrayForCSVFile() fonksiyonunu test edeceğiz.
             Aşağıda data adlı çift boyutlu string dizisi oluşturduk ve bunun içinde data.csv değerini koyduk.
             Oluşturduğumuz bu dizi ile fonksiyondan çıkacak dizisinin eşitliğini test edeceğiz.
             */

            FileBrowser fileBrowser = new FileBrowser();
            string[,] data = { {"RowNumber","CustomerId","Surname","CreditScore","Geography" },
                               {"1","15634602","Hargrave","619","France" },
                               {"2","15647311","Hill","608","Spain" },
                               {"3","15619304","Onio","502","France" },
                               {"4","15701354","Boni","699","France" },
                               {"5","15737888","Mitchell","850","Spain" }};
            Assert.AreEqual(data, fileBrowser.StringArrayForCSVFile("Assets\\Tests\\data.csv", ".csv"));
        }
    }
}
