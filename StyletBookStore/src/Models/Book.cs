using System;

namespace StyletBookStore.Models
{
    /// <summary>
    /// 图书
    /// </summary>
    public class Book
    {
        /// <summary>
        /// 书名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public BookType Type { get; set; }

        /// <summary>
        /// 出版年月
        /// </summary>
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public float Price { get; set; }

        /// <summary>
        /// 封面URL
        /// </summary>
        public string CoverUrl { get; set; }

        public Book(string name, BookType type, DateTime publishDate, float price, string coverUrl)
        {
            Name = name;
            Type = type;
            PublishDate = publishDate;
            Price = price;
            CoverUrl = coverUrl;
        }
    }
}