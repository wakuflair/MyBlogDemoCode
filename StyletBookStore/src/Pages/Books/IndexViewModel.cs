using System;
using System.Collections.ObjectModel;
using Stylet;
using StyletBookStore.Models;
using StyletBookStore.Services;

namespace StyletBookStore.Pages.Books
{
    public class IndexViewModel : Screen
    {
        private readonly IBookService _bookService;
        public ObservableCollection<Book> Books { get; set; } = new ObservableCollection<Book>();
        public Book? SelectedBook { get; set; }

        public IndexViewModel(IBookService bookService)
        {
            _bookService = bookService;
        }

        protected override void OnViewLoaded()
        {
            Books = new ObservableCollection<Book>(_bookService.GetAllBooks());
        }
    }
}