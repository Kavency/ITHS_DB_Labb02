using System.Collections.ObjectModel;
using TheBookNook_WPF.Model;

namespace TheBookNook_WPF.ViewModel
{
    public class MainWindowVM : VMBase
    {
        #region Auto properties
        public HomeVM HomeVM { get; set; }
        public BooksVM BooksVM { get; set; }
        public AuthorsVM AuthorsVM { get; set; }
        public CustomersVM CustomersVM { get; set; }
        public StoresVM StoresVM { get; set; }
        public ObservableCollection<Book> Books { get; set; } = new();
        public RelayCommand ShowHomeViewCMD { get; set; }
        public RelayCommand ShowBookViewCMD { get; set; }
        public RelayCommand ShowAuthorsViewCMD { get; set; }
        public RelayCommand ShowCustomersViewCMD { get; set; }
        public RelayCommand ShowStoresViewCMD { get; set; }
        #endregion


        #region Fields
        private object _currentView;
        #endregion


        #region Properties
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        #endregion


        public MainWindowVM()
        {
            this.HomeVM = new HomeVM();
            this.BooksVM = new BooksVM();
            this.AuthorsVM = new AuthorsVM();
            this.CustomersVM = new CustomersVM();
            this.StoresVM = new StoresVM();

            CurrentView = HomeVM;


            ShowHomeViewCMD = new RelayCommand(x => CurrentView = HomeVM);
            ShowBookViewCMD = new RelayCommand(x => CurrentView = BooksVM);
            ShowAuthorsViewCMD = new RelayCommand(ShowAuthorsView);
            ShowCustomersViewCMD = new RelayCommand(ShowCustomersView);
            ShowStoresViewCMD = new RelayCommand(ShowStoresView);
            //GetBooks();
        }

        private void ShowHomeView(object obj)
        {
            CurrentView = HomeVM;
        }

        private void ShowBookView(object obj)
        {
            CurrentView = BooksVM;
        }
        private void ShowAuthorsView(object obj)
        {
            CurrentView = AuthorsVM;
        }
        private void ShowCustomersView(object obj)
        {
            CurrentView = CustomersVM;
        }

        private void ShowStoresView(object obj)
        {
            CurrentView = StoresVM;
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
