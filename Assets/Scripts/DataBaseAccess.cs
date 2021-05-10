using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class DataBaseAccess : MonoBehaviour
{
    MongoClient client = new MongoClient("mongodb+srv://testUser:testPassword@cluster0.uo7yj.mongodb.net/UserDataDB?retryWrites=true&w=majority");
    IMongoDatabase database;
    IMongoCollection<BsonDocument> collection;
    // Start is called before the first frame update
    void Start()
    {
        //Mongo database ile bağlantı kodu.
        database = client.GetDatabase("UserDataDB");
    }

    public void SaveToDatabase(string[,] data, string name)
    {
        //Mevcut tarih ve saati alıp onu stringe dönüştüren kod.
        string date = DateTime.Now.ToString("MM\\/dd\\/yyyy");
        string time = DateTime.Now.ToString("HH\\:mm:ss");

        //Dosya ismi, tarih ve saat ile collection ismi oluşturan kod. 
        //Bu sayede database'de aynı isimli collection oluşturulmaya çalışırken verilen hatanın önüne geçildi.
        string collectionName = name + date + time;

        //Dosya ismi, tarihi ve saati ile collection oluşturma ve bağlantı yapma kodu.
        database.CreateCollection(collectionName);
        collection = database.GetCollection<BsonDocument>(collectionName);

        // Labelları bulmak için olan kod
        string[] labels = new string[data.GetLength(1)];
        for (int i = 0; i < labels.Length; i++)
        {
            labels[i] = data[0, i];
        }

        //MongoDB veritabanına BulkWrite yapmak için BsonDocumentlerden oluşan WriteModel listesi oluşturan kod.
        var listWrites = new List<WriteModel<BsonDocument>>();

        //BsonDocumentlere sırayla her bir satırın bilgileri labellarla birlikte yazdırılıyor.
        /*Örnek olarak bir document = 
         {
         {Row Number: 1}
         {CustomerId: 15634602}
         {Surname: Hargrave}
         {CreditScore: 619}
         {Geography: France}
         {Gender: Female}
         {Age: 42}
         }
         şeklinde oluyor. Ne kadar satır varsa o kadar document eklenerek en son 
         collection.BulkWrite(listWrites);
         komutu ile MongoDB veritabanına tüm tablo eklenmiş oluyor.               
         */
        var document = new BsonDocument { };
        for (int i = 1; i < data.GetLength(0); i++)
        {
            for (int j = 0; j < data.GetLength(1); j++)
            {
                document.Add(labels[j], data[i, j]);
            }
            listWrites.Add(new InsertOneModel<BsonDocument>(document.Clone().AsBsonDocument));
            document.Clear();
        }

        collection.BulkWrite(listWrites);

    }
}
