using System.Windows;

namespace TheBookNook_WPF.ViewModel
{
    public class MainWindowVM : VMBase
    {
        #region Fields
        private object _currentView;
        private bool _mainWindowIsEnabled = true;
        #endregion


        #region Auto properties
        public HomeVM HomeVM { get; set; }
        public BooksVM BooksVM { get; set; }
        public AuthorsVM AuthorsVM { get; set; }
        public CustomersVM CustomersVM { get; set; }
        public StoresVM StoresVM { get; set; }
        public bool MainWindowIsEnabled { get => _mainWindowIsEnabled; set { _mainWindowIsEnabled = value; OnPropertyChanged(); } }
        public RelayCommand ShowHomeViewCMD { get; set; }
        public RelayCommand ShowBookViewCMD { get; set; }
        public RelayCommand ShowAuthorsViewCMD { get; set; }
        public RelayCommand ShowCustomersViewCMD { get; set; }
        public RelayCommand ShowStoresViewCMD { get; set; }
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
            ShowAuthorsViewCMD = new RelayCommand(x => CurrentView = AuthorsVM);
            ShowCustomersViewCMD = new RelayCommand(x => CurrentView = CustomersVM);
            ShowStoresViewCMD = new RelayCommand(x => CurrentView = StoresVM);
        }

        
    }
}
