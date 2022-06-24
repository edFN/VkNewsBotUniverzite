using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
namespace VkBot.Models
{
    /*
    public class UpdatedNewsEventArgs:EventArgs {
        public int id;
        public DateTime timeReached;
        public string text;
        public string? image;
    }
    */

    public abstract class WebPage
    {
        protected string url; // url of page news
        protected string name; //name of site
        //protected string currentContent;
        
        protected string Image { get; set; }
        public WebPage() {
            
        }
        public abstract string getUrl();
        public abstract string getName();

        public abstract string getImage();


    }

    public abstract class PageParser {
        public  WebPage page;
        public abstract Models.NewsPaperContext.NewsPaper Parse(string html);
    }
    public abstract class PageManager {
        protected PageParser parser;
        protected WebPage page;
        protected HttpClient client;
       

        public WebPage getPage()
        {
            return page;
        }

        public PageParser getParser()
        {
            return parser;
        }

        public async Task<string> getMsg(string url)
        {
            
            try
            {
                var result = await client.GetStringAsync(page.getUrl());
                return result;
            }
            catch(HttpRequestException e)
            {
                ConsoleLog.ErrorMessage(e.Message);
                ConsoleLog.ErrorMessage(e.Source);
                return string.Empty;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        //protected abstract void Manage();

    }



}
