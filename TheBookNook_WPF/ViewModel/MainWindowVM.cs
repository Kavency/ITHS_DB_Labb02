using System.Collections.ObjectModel;
using System.Windows;
using TheBookNook_WPF.Model;

namespace TheBookNook_WPF.ViewModel
{
    public class MainWindowVM : VMBase
    {
        #region Fields
        private object _currentView;
        private bool _sideMenuIsEnabled = true;
        private ObservableCollection<Author>? _authors;
        private Visibility _dimBackgroundVisibility = Visibility.Hidden;
        #endregion


        #region Properties
        public object CurrentView { get { return _currentView; } set { _currentView = value; OnPropertyChanged(); } }
        public bool SideMenuIsEnabled { get => _sideMenuIsEnabled; set { _sideMenuIsEnabled = value; OnPropertyChanged(); } }
        public HomeVM HomeVM { get; set; }
        public BooksVM BooksVM { get; set; }
        public AuthorsVM AuthorsVM { get; set; }
        public StoresVM StoresVM { get; set; }
        public Visibility DimBackgroundVisibility { get => _dimBackgroundVisibility; set { _dimBackgroundVisibility = value; OnPropertyChanged(); } }
        public RelayCommand ShowHomeViewCMD { get; set; }
        public RelayCommand ShowBookViewCMD { get; set; }
        public RelayCommand ShowAuthorsViewCMD { get; set; }
        public RelayCommand ShowCustomersViewCMD { get; set; }
        public RelayCommand ShowStoresViewCMD { get; set; }
        public ObservableCollection<Author>? Authors { get => _authors; set { _authors = value; OnPropertyChanged(); } }

        #endregion


        public MainWindowVM()
        {
            this.HomeVM = new HomeVM();
            this.BooksVM = new BooksVM(this);
            this.AuthorsVM = new AuthorsVM(this);
            this.StoresVM = new StoresVM();

            _authors = new ObservableCollection<Author>();

            CurrentView = HomeVM;

            ShowHomeViewCMD = new RelayCommand(x => { CurrentView = HomeVM; ClearAllSelected(); });
            ShowBookViewCMD = new RelayCommand(x => { CurrentView = BooksVM; ClearAllSelected(); });
            ShowAuthorsViewCMD = new RelayCommand(x => { CurrentView = AuthorsVM; ClearAllSelected(); });
            ShowStoresViewCMD = new RelayCommand(x => { CurrentView = StoresVM; ClearAllSelected(); });

            LoadAuthorsAsync();
            AuthorFullName();
        }

        #region Methods
        private async void LoadAuthorsAsync()
        {
            var data = await Task.Run(() => GetDataFromDatabase());

            Authors = data;
            OnPropertyChanged(nameof(Authors));
        }

        private ObservableCollection<Author> GetDataFromDatabase()
        {
            using var db = new TheBookNookDbContext();
            var authors = new ObservableCollection<Author>(db.Authors.ToList());

            return authors;
        }

        private void AuthorFullName()
        {
            foreach (var author in Authors)
            {
                author.FullName = author.FirstName + " " + author.LastName;
            }
        }

        private void ClearAllSelected()
        {
            BooksVM.CurrentBook = null;
        }
        #endregion
    }
}
