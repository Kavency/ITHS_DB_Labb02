using TheBookNook_WPF.Core;

namespace TheBookNook_WPF.ViewModel
{
    public class MainWindowVM : VMBase
    {
        #region Fields
        private object _currentView;
        private bool _sideMenuIsEnabled = true;
        #endregion


        #region Properties
        public object CurrentView { get { return _currentView; } set { _currentView = value; OnPropertyChanged(); } }
        public bool SideMenuIsEnabled { get => _sideMenuIsEnabled; set { _sideMenuIsEnabled = value; OnPropertyChanged(); } }
        public HomeVM HomeVM { get; set; }
        public BooksVM BooksVM { get; set; }
        public AuthorsVM AuthorsVM { get; set; }
        public StoresVM StoresVM { get; set; }
        public RelayCommand ShowHomeViewCMD { get; set; }
        public RelayCommand ShowBookViewCMD { get; set; }
        public RelayCommand ShowAuthorsViewCMD { get; set; }
        public RelayCommand ShowCustomersViewCMD { get; set; }
        public RelayCommand ShowStoresViewCMD { get; set; }
        #endregion


        public MainWindowVM()
        {
            this.HomeVM = new HomeVM();
            this.BooksVM = new BooksVM(this);
            this.AuthorsVM = new AuthorsVM(this);
            this.StoresVM = new StoresVM(this);

            CurrentView = HomeVM;

            ShowHomeViewCMD = new RelayCommand(x => { CurrentView = HomeVM; ClearAllSelected(); });
            ShowBookViewCMD = new RelayCommand(x => { CurrentView = BooksVM; ClearAllSelected(); });
            ShowAuthorsViewCMD = new RelayCommand(x => { CurrentView = AuthorsVM; ClearAllSelected(); });
            ShowStoresViewCMD = new RelayCommand(x => { CurrentView = StoresVM; ClearAllSelected(); });
        }

        #region Methods
        private void ClearAllSelected()
        {
            BooksVM.CurrentBook = null;
            AuthorsVM.CurrentAuthor = null;
            StoresVM.SelectedStore = null;
            StoresVM.SelectedStoreStock = null;
        }
        #endregion
    }
}
