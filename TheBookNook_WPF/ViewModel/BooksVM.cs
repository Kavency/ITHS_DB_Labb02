using System.Collections.ObjectModel;
using System.Windows.Controls;
using TheBookNook_WPF.Model;

namespace TheBookNook_WPF.ViewModel
{
    public class BooksVM : VMBase
    {
        public ObservableCollection<Book>? Books { get; private set; }

        public BooksVM()
        {
            LoadBooksAsync();
        }

        private Book _selectedBook;

        public Book SelectedBook
        {
            get { return _selectedBook; }
            set
            {
                _selectedBook = value;
                OnPropertyChanged();
            }
        }

        private async void LoadBooksAsync()
        {
            var data = await Task.Run(() => GetDataFromDatabase());

            Books = data;
            OnPropertyChanged("Books");
        }

        private ObservableCollection<Book> GetDataFromDatabase()
        {
            using var db = new TheBookNookDbContext();
            var books = new ObservableCollection<Book>(db.Books.ToList());

            //var query = from book in db.Books
            //                //from author in db.Authors

            //            join bookAuthor in db.Books on book.Isbn equals bookAuthor.Isbn
            //            //join author in db.Authors on author.Id equals author2.Id
            //            //join bookAuthor in db.Books on book.Authors equals 
            //            //join bookisbn in db.Authors on b
            //            select new
            //            {
            //                book.Isbn,
            //                book.Title
            //                //author.FirstName,
            //                //author.LastName
            //            };

            //var someList = query.ToList();

            return books;
        }
    }
}
