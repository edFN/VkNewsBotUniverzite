using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Linq;
using System.IO;

namespace VkBot.Models
{
    class SpbuPage : WebPage
    {

        public SpbuPage()
        {
            this.name = "СПБГУ";
            this.Image = Directory.GetCurrentDirectory()+ @"/images/Spbu.jpg";
            this.url = "https://spbu.ru/postupayushchim/all_news";
        }

        public override string getImage()
        {
            return Image;
        }

        public override string getName()
        {
            return name;
        }

        public override string getUrl()
        {
            return url;
        }
    }

    class SpbuPageParser : PageParser
    {
        public SpbuPageParser(SpbuPage page)
        {
            this.page = page;
        }

        public override Models.NewsPaperContext.NewsPaper Parse(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNodeCollection hrefCollection = doc.DocumentNode.SelectNodes("//div[@class='card-context  card--with-img card-context--large ']/a[@class='card__media']");
            HtmlNodeCollection collection = doc.DocumentNode.SelectNodes("//div[@class='card-context  card--with-img card-context--large ']/div[@class='card__content']");
           
            HtmlNode titleBox = collection[collection.Count - 1].SelectSingleNode("a/h4[@class='card__title']");
            HtmlNode detailsBox = collection[collection.Count - 1].SelectSingleNode("div[@class='card__details']/span[@class='card__date']");

            var date = detailsBox.InnerText.Split(" ");
            var id = date[0] + Month.getMonthByName(date[1]) + date[2];
            var title = titleBox.InnerText.Replace("&nbsp;", " ");
            var href = hrefCollection[hrefCollection.Count - 1].Attributes["href"].Value;
               
            DateTime dt = new DateTime(Int32.Parse(date[2]), Month.getMonthByName(date[1]), Int32.Parse(date[0]));

            return new Models.NewsPaperContext.NewsPaper(ulong.Parse(id),title,"https://spbu.ru/"+href,"СПБГУ",dt,false);

        }
    }

    class SpbuPageManager : PageManager {
        public SpbuPageManager()
        {
            this.page = new SpbuPage();
            this.client = new HttpClient(); //для работы с http
            this.parser = new SpbuPageParser((SpbuPage)this.page);
        }
    }

}
