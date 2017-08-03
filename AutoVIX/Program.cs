using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using HtmlAgilityPack;
using MongoDB.Driver;
using MongoDB.Bson;

namespace AutoVIX
{
    class Program
    {
        static void Main(string[] args)
        {
            getVIX();
            //ins();
            //qry_cnt();
            //qry_one();

        }
        
        static void getVIX()
        {
            // 下載 Yahoo 奇摩股市資料 (範例為 2317 鴻海)  
            WebClient client = new WebClient();
            MemoryStream ms = new MemoryStream(client.DownloadData("http://tw.stock.yahoo.com/q/q?s=2317"));  // 使用預設編碼讀入 HTML  

            HtmlDocument doc = new HtmlDocument();
            doc.Load(ms, Encoding.GetEncoding(950));  // 裝載第一層查詢結果  

            HtmlDocument docStockContext = new HtmlDocument();
            docStockContext.LoadHtml(doc.DocumentNode.SelectSingleNode(
                "/html[1]/body[1]/center[1]/table[2]/tr[1]/td[1]/table[1]").InnerHtml);  // 取得個股標頭  
            HtmlNodeCollection nodeHeaders = docStockContext.DocumentNode.SelectNodes("./tr[1]/th");

            // 取得個股數值  
            string[] values = docStockContext.DocumentNode.SelectSingleNode("./tr[2]").InnerText.Trim().Split('\n');

            int i = 0;  // 輸出資料              
            foreach (HtmlNode nodeHeader in nodeHeaders)
            {
                Console.WriteLine("Header: {0}, Value: {1}", nodeHeader.InnerText, values[i].Trim());
                i++;
            } doc = null;

            docStockContext = null;
            client = null;
            ms.Close();

            Console.WriteLine("Completed.");
            Console.ReadLine();        
        }

        static void ins()
        {
            var client = new MongoClient();
            var database = client.GetDatabase("foo");
            var collection = database.GetCollection<BsonDocument>("bar");
            
            //INSERT
            var document = new BsonDocument
            {
                { "name", "MongoDB" },
                { "type", "Database" },
                { "count", 1 },
                { "info", new BsonDocument
                    {
                        { "x", 203 },
                        { "y", 102 }
                    }}
            };

            collection.InsertOne(document);

            Console.WriteLine("ins OK!!");
        }

        static void qry_cnt()
        {
            var client = new MongoClient();
            var database = client.GetDatabase("foo");
            var collection = database.GetCollection<BsonDocument>("bar");
            
            var count = collection.Count(new BsonDocument());

            Console.WriteLine(count.ToString());
        }

        static void qry_one()
        {
            var client = new MongoClient();
            var database = client.GetDatabase("foo");
            var collection = database.GetCollection<BsonDocument>("bar");

            var document = collection.Find(new BsonDocument()).FirstOrDefault();
            Console.WriteLine(document.ToString());
        }

    }
}
