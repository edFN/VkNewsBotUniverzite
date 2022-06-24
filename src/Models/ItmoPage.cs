using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using HtmlAgilityPack;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.IO;

namespace VkBot.Models
{
    public class Month
    {
        public static int getMonthByName(string name)
        {
            switch (name.ToLower())
            {
                case "января":
                    return 1;
                case "февраля":
                    return 2;
                case "марта":
                    return 3;
                case "апреля":
                    return 4;
                case "мая":
                    return 5;
                case "июня":
                    return 6;
                case "июля":
                    return 7;
                case "августа":
                    return 8;
                case "сентября":
                    return 9;
                case "октября":
                    return 10;
                case "ноября":
                    return 11;
                case "декабря":
                    return 12;
                default:
                    
                    return -1;
            }
        }
    }

    public class ItmoPage : WebPage
    {
        //const string PATTERN_ANNOUNCE = "/ru/announce/";
        public static string URL = "https://news.itmo.ru/ru/announce/";
        public ItmoPage()
        {
            this.url = "https://news.itmo.ru/ru/events/"; //страница анонсов итмо
            this.name = "ИТМО";
            this.Image = Directory.GetCurrentDirectory() + @"/images/ITMO.jpg";
        }
        public override string getUrl()
        {
            return url;
        }
        public override string getName()
        {
            return name;
        }

        public override string getImage()
        {
            return Image;
        }
    }



    public class ItmoPageParser : PageParser
    {
        public ItmoPage page;

        private const string xcode = "//div[@class='weeklyevents sand']//ul/li";


        public ItmoPageParser(ItmoPage page)
        {
            this.page = page;
        }

        public override NewsPaperContext.NewsPaper Parse(string html)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);
            StringBuilder br = new StringBuilder();
            HtmlNodeCollection node = document.DocumentNode.SelectNodes(xcode + "/ul/li[@class='information']/span");
            HtmlNodeCollection node1 = document.DocumentNode.SelectNodes(xcode + "/h4/a");

            HtmlNode last1, last2;
            last1 = node1[node1.Count - 1];
            last2 = node[node.Count - 1];

            var parseHref = last1.Attributes["href"].Value.Split("/");

            var idNews = long.Parse(parseHref[parseHref.Length - 2]);
            var inner = last1.InnerHtml;
            var date = last2.InnerHtml.Split(' ');

            DateTime time = new DateTime(Int32.Parse(date[date.Length - 1]),
                Month.getMonthByName(date[date.Length - 2]), Int32.Parse(date[date.Length - 3]));


            return new NewsPaperContext.NewsPaper((ulong)idNews, inner, ItmoPage.URL+idNews, page.getName(), time, false);
        }
    }

    public class ItmoPageManager : PageManager
    {

        public ItmoPageManager()
        {
            this.page = new ItmoPage();
            this.client = new HttpClient();
            this.parser = new ItmoPageParser((ItmoPage)page);

        }


    }
}
