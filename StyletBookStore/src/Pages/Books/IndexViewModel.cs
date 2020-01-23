using System;
using System.Collections.ObjectModel;
using System.Linq;
using Stylet;
using StyletBookStore.Models;
using StyletBookStore.Pages.Books.BookItems;
using StyletBookStore.Services;

namespace StyletBookStore.Pages.Books
{
    public class IndexViewModel : Screen
    {
        private readonly IBookService _bookService;
        public ObservableCollection<BookItemViewModel> BookItems { get; set; } = new ObservableCollection<BookItemViewModel>();

        public IndexViewModel(IBookService bookService)
        {
            _bookService = bookService;
        }

        protected override void OnViewLoaded()
        {
            var viewModels = _bookService.GetAllBooks()
                    .Select(book => new BookItemViewModel(book))
                ;

            BookItems = new ObservableCollection<BookItemViewModel>(viewModels);
        }
    }
}