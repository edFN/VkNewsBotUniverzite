using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using HtmlAgilityPack;
using System.Linq;
using System.IO;

namespace VkBot.Models
{
    class MsuPage : WebPage
    {
        public MsuPage()
        {
            this.url = "https://www.msu.ru/entrance/ad/index.php?tmpl=clear";
            
            this.Image = Directory.GetCurrentDirectory()+ @"/images/msu.jpg";
            this.name = "МГУ";
        }

        public override string getImage()
        {
            return Image;
        }

        public override string getName()
        {
            return this.name;
        }

        public override string getUrl()
        {
            return this.url;
        }
    }

    class MsuPageParser : PageParser
    {
        public MsuPageParser(MsuPage page)
        {
            this.page = page;
        }
        public override Models.NewsPaperContext.NewsPaper Parse(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNodeCollection idNode = doc.DocumentNode.SelectNodes("//div[@class='news-list']/div[@class='news-list-item news-list-item-nomargin']");
            HtmlNodeCollection collection = doc.DocumentNode.SelectNodes("//div[@class='news-list']/div[@class='news-list-item news-list-item-nomargin']/div[@class='news-list-tag-pane']/div[@class='news-list-item-date']");
            HtmlNodeCollection collection1 = doc.DocumentNode.SelectNodes("//div[@class='news-list']/div[@class='news-list-item news-list-item-nomargin']/div[@class='news-list-item-head']/a/b");
            HtmlNodeCollection hrefCollection = doc.DocumentNode.SelectNodes("//div[@class='news-list']/div[@class='news-list-item news-list-item-nomargin']/div[@class='news-list-item-head']/a");

            HtmlNode time, info,idInfo;

            const int FIRST_ELEMENT = 0;
            
            time = collection[FIRST_ELEMENT];
            info = collection1[FIRST_ELEMENT];
            idInfo = idNode[FIRST_ELEMENT];

            var id = idInfo.Attributes["id"].Value.Split("_");
            

            StringBuilder bd = new StringBuilder();

            var dataArr = time.InnerText.Split("/");
            var title = info.InnerHtml;
            var ids = id[id.Length - 1];
           var href = hrefCollection[FIRST_ELEMENT].Attributes["href"].Value;

            DateTime date = new DateTime(DateTime.Today.Year, Int32.Parse(dataArr[1]), Int32.Parse(dataArr[0]));

            return new Models.NewsPaperContext.NewsPaper(ulong.Parse(ids), title, "https://www.msu.ru/"+href, page.getName(), date, false);


        }
    }

    class MsuPageManager: PageManager
    {
       
        public MsuPageManager()
        {
            this.page = new MsuPage();
            this.client = new HttpClient(); //для работы с http
            this.parser = new MsuPageParser((MsuPage)this.page);
        }

    }




}
