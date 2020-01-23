using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using StyletBookStore.Models;

namespace StyletBookStore.Services
{
    public class BookService : IBookService
    {
        private readonly List<Book> _bookStore;

        public BookService()
        {
           _bookStore = new List<Book>
           {
               new Book("阿米尔·汗：我行我素", BookType.Biography, DateTime.Parse("2017-6"), 52.8f, "https://img1.doubanio.com/view/subject/l/public/s29467958.jpg"),
               new Book("三体：“地球往事”三部曲之一", BookType.ScienceFiction, DateTime.Parse("2008-1"), 23f, "https://img1.doubanio.com/view/subject/l/public/s2768378.jpg"),
               new Book("三体Ⅱ：黑暗森林", BookType.ScienceFiction, DateTime.Parse("2008-5"), 32f, "https://img3.doubanio.com/view/subject/l/public/s3078482.jpg"),
               new Book("三体Ⅲ：死神永生", BookType.ScienceFiction, DateTime.Parse("2010-11"), 32f, "https://img9.doubanio.com/view/subject/l/public/s26012674.jpg"),
               new Book("肖申克的救赎", BookType.Mystery, DateTime.Parse("2006-7"), 26.9f, "https://img9.doubanio.com/view/subject/l/public/s4007145.jpg"),
           }; 
        }
    
        public IEnumerable<Book> GetAllBooks()
        {
            return _bookStore;
        }
    }
}