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
                new Book("阿米尔·汗：我行我素", BookType.Biography, DateTime.Parse("2017-6"), 52.8f), 
                new Book("三体：“地球往事”三部曲之一", BookType.ScienceFiction, DateTime.Parse("2008-1"), 23f), 
                new Book("斯蒂芬·金恐怖小说", BookType.Horror, DateTime.Parse("2000-1"), 20f), 
            });
        }
    }
}