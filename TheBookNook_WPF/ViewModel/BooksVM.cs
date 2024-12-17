using System.Collections.ObjectModel;
using TheBookNook_WPF.Model;
using TheBookNook_WPF.View;

namespace TheBookNook_WPF.ViewModel
{
    public class BooksVM : VMBase
    {
        private Book _selectedBook;
        

        public ObservableCollection<Book>? Books { get; private set; }
        public RelayCommand AddRecordCMD { get; }


        public BooksVM()
        {
            AddRecordCMD = new RelayCommand(OpenAddRecordWindow);

            LoadBooksAsync();
        }


        private void OpenAddRecordWindow(object obj)
        {
            var AddWindow = new AddBookView();
            AddWindow.ShowDialog();
        }


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
            //OnPropertyChanged(nameof(Books));
        }


        private ObservableCollection<Book> GetDataFromDatabase()
        {
            using var db = new TheBookNookDbContext();
            var books = new ObservableCollection<Book>(db.Books.ToList());
            
            return books;
        }
    }
}
