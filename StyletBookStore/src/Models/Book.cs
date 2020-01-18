using System;
using Stylet;

namespace StyletBookStore.Models
{
    /// <summary>
    /// 图书
    /// </summary>
    public class Book : PropertyChangedBase
    {
        public string Name { get; set; }

        public BookType Type { get; set; }

        public DateTime PublishDate { get; set; }

        public float Price { get; set; }

        public Book(string name, BookType type, DateTime publishDate, float price)
        {
            Name = name;
            Type = type;
            PublishDate = publishDate;
            Price = price;
        }
    }
}