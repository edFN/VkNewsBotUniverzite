using System;

using System.Text;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

namespace VkBot.Models
{


    public class NewsPaperContext : DbContext
    {

        public DbSet<NewsPaper> set { get; set; }

        public class NewsPaper { 
            public ulong id { get; set; }
            public string Title { get; set; }
            public string urlPage { get; set; }
            public string university { get; set; }
            public bool main { get; set; }

            public DateTime date { get; set; }
            public NewsPaper(ulong id, string Title, string urlPage, string university,DateTime date ,bool main = false)
            {
                this.id = id;
                if (string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(urlPage) || string.IsNullOrEmpty(university))
                    throw new Exception("Not enough arguments");
                this.Title = Title;
                this.urlPage = urlPage;
                this.university = university;
                this.main = main;
                this.date = date;

            }
            public override string ToString()
            {
                StringBuilder bd = new StringBuilder();
                bd.AppendLine("Университет: " + university);
                bd.AppendLine(Title);
                bd.AppendLine("Дата: "+ date.ToString("dd.MM.yyyy"));
                bd.Append($"Подробнее: {urlPage}\n");
                return bd.ToString();
            }

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseMySql($"server=127.0.0.1;user=root;database=newspaper;port=3306;Connect Timeout=2");
        
           
        
        
    }
}
