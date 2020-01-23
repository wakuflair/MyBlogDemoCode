using Stylet;
using StyletBookStore.Models;

namespace StyletBookStore.Pages.Books.BookItems
{
    public class BookItemViewModel : Screen
    {
        public Book Book { get; set; }

        public BookItemViewModel(Book book)
        {
            Book = book;
        }
    }
}