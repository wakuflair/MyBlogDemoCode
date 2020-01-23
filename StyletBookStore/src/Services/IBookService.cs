using System.Collections.Generic;
using StyletBookStore.Models;

namespace StyletBookStore.Services
{
    public interface IBookService
    {
        IEnumerable<Book> GetAllBooks();
    }
}