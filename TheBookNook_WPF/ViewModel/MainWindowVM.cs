using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TheBookNook_WPF.Model;
using TheBookNook_WPF.View;

namespace TheBookNook_WPF.ViewModel
{
    public class MainWindowVM : VMBase
    {
        public ObservableCollection<Book> Books { get; set; } = new();

        public Visibility BookViewVisibility { get; set; } = Visibility.Collapsed;
        public ICommand ShowBookViewCMD { get; set; }

        public MainWindowVM()
        {
            GetBooks();

            ShowBookViewCMD = new RelayCommand(ShowBookView, CanShowBookView);
        }

        private bool CanShowBookView(object obj)
        {
            return true;
        }

        private void ShowBookView(object obj)
        {
            BooksView BooksDG = new BooksView();
            BookViewVisibility = Visibility.Visible;
        }

        private void GetBooks() 
        {
            using var db = new TheBookNookDbContext();
            var books = db.Books.ToList();
            
            foreach (var item in books)
            {
                this.Books.Add(item);
            }
        }
    }
}
