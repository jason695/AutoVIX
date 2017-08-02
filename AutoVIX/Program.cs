﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using HtmlAgilityPack;

namespace AutoVIX
{
    class Program
    {
        static void Main(string[] args)
        {
            //教學 https://getpocket.com/a/read/1498241086
            
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
                Console.WriteLine("Header: {0}, Value: {1}",nodeHeader.InnerText, values[i].Trim());
                i++;
            } doc = null;

            docStockContext = null;
            client = null;
            ms.Close(); Console.WriteLine("Completed.");
            Console.ReadLine();

        }
    }
}
