using System;
using System.Collections.ObjectModel;
using Stylet;
using StyletBookStore.Models;

namespace StyletBookStore.Pages.Books
{
    public class IndexViewModel : Screen
    {
        public ObservableCollection<Book> Books { get; set; }

        public IndexViewModel()
        {
            Books = new ObservableCollection<Book>(new []
            {
                new Book("阿米尔·汗：我行我素", BookType.Biography, DateTime.Parse("2017-6"), 52.8f, "9787559605115"), 
                new Book("三体：“地球往事”三部曲之一", BookType.ScienceFiction, DateTime.Parse("2008-1"), 23f, "9787536692930"), 
                new Book("三体Ⅱ：黑暗森林", BookType.ScienceFiction, DateTime.Parse("2008-5"), 32f, "9787536693968"), 
                new Book("三体Ⅲ：死神永生", BookType.ScienceFiction, DateTime.Parse("2010-11"), 32f, "9787229030933"), 
                new Book("肖申克的救赎", BookType.Mystery, DateTime.Parse("2006-7"), 26.9f, "9787020054985"), 
            });
        }
    }
}