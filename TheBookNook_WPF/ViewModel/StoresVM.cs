using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using TheBookNook_WPF.Core;
using TheBookNook_WPF.Model;

namespace TheBookNook_WPF.ViewModel
{
    public class StoresVM : VMBase
    {
        #region Fields
        private MainWindowVM _mainWindowVM;
        private ObservableCollection<Store>? _stores;
        private ObservableCollection<Book>? _books;
        private Visibility _addButtonVisibility = Visibility.Hidden;
        private Visibility _addToStockPaneVisibility = Visibility.Hidden;
        private ObservableCollection<Stock> _selectedStoreStock;
        private Store? _selectedStore;
        private Stock _selectedRow;
        private Book _selectedBook;
        #endregion

        #region Properties
        public int NumberOfBooksToAdd { get; set; }
        public RelayCommand AddToStockCMD { get; }
        public RelayCommand DecreaseAmountCMD { get; }
        public RelayCommand IncreaseAmountCMD { get; }
        public RelayCommand SaveToStockCMD { get; }
        public RelayCommand CloseAddToStockButtonCMD { get; }

        public ObservableCollection<Store>? Stores { get => _stores; private set { _stores = value; OnPropertyChanged(); } }
        public ObservableCollection<Book>? Books { get => _books; private set { _books = value; OnPropertyChanged(); } }
        public Visibility AddButtonVisibility { get => _addButtonVisibility; set { _addButtonVisibility = value; OnPropertyChanged(); } }
        public Visibility AddToStockPaneVisibility { get => _addToStockPaneVisibility; set { _addToStockPaneVisibility = value; OnPropertyChanged(); } }
        public Book SelectedBook { get => _selectedBook; set => _selectedBook = value; }
        public Stock SelectedRow
        {
            get => _selectedRow;
            set
            {
                _selectedRow = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Stock> SelectedStoreStock
        {
            get => _selectedStoreStock;
            set
            {
                _selectedStoreStock = value;
                OnPropertyChanged();
            }
        }
        public Store? SelectedStore
        {
            get { return _selectedStore; }
            set
            {
                _selectedStore = value;
                OnPropertyChanged();
                if (SelectedStore != null)
                {
                    SelectedStoreStock = SelectedStore.Stocks;
                    AddButtonVisibility = Visibility.Visible;
                }
                else
                    AddButtonVisibility = Visibility.Hidden;
            }
        }
        #endregion


        public StoresVM(MainWindowVM mainWindowVM)
        {
            _mainWindowVM = mainWindowVM;

            AddToStockCMD = new RelayCommand(AddToStock);
            DecreaseAmountCMD = new RelayCommand(DecreaseAmount);
            IncreaseAmountCMD = new RelayCommand(IncreaseAmount);
            SaveToStockCMD = new RelayCommand(SaveToStock);
            CloseAddToStockButtonCMD = new RelayCommand(CloseAddToStockPane);

            LoadStoresAsync();
        }


        #region Methods
        private async void LoadStoresAsync()
        {
            var stores = await Task.Run(() => GetStoresFromDatabase());

            Stores = stores;
            OnPropertyChanged(nameof(SelectedStore));
        }


        private ObservableCollection<Store> GetStoresFromDatabase()
        {
            using var db = new TheBookNookDbContext();
            var stores = new ObservableCollection<Store>(
                db.Stores.Include(s => s.Stocks)
                         .ThenInclude(i => i.IsbnNavigation)
                         .ThenInclude(a => a.Authors)
                         .ToList()
                );

            return stores;
        }


        private ObservableCollection<Book> GetBooksFromDatabase()
        {
            using var db = new TheBookNookDbContext();
            var books = new ObservableCollection<Book>(
                db.Books.Include(b => b.Authors).ToList());

            return books;
        }


        private async void AddToStock(object obj)
        {
            OpenAddToStockPane(true);
            var books = await Task.Run(() => GetBooksFromDatabase());
            Books = books;
        }


        private void SaveToStock(object obj)
        {
            using var db = new TheBookNookDbContext();
            
            var bookExists = db.Stocks.Any(s => s.Isbn == SelectedBook.Isbn && s.StoreId == SelectedStore.Id);

            if (bookExists)
            {
                var stock = db.Stocks.FirstOrDefault(s => s.StoreId == SelectedStore.Id && s.Isbn == SelectedBook.Isbn);
                stock.Amount += NumberOfBooksToAdd;
                db.SaveChanges();
            }
            else
            {
                Stock newStock = new Stock();

                newStock.Isbn = SelectedBook.Isbn;
                newStock.StoreId = SelectedStore.Id;
                newStock.Amount = NumberOfBooksToAdd;
                db.Stocks.Add(newStock);
                db.SaveChanges();
                
                newStock.IsbnNavigation = SelectedBook;
                SelectedStoreStock.Add(newStock);
            }

            OpenAddToStockPane(false);

            NumberOfBooksToAdd = 0;
        }


        private void CloseAddToStockPane(object obj)
        {
            OpenAddToStockPane(false);
        }


        private void IncreaseAmount(object obj)
        {
            var stockItem = obj as Stock;
            stockItem.Amount++;

            using var db = new TheBookNookDbContext();

            var newStockAmount = db.Stocks.Where(s => s.StoreId == stockItem.StoreId && s.Isbn == stockItem.Isbn).SingleOrDefault();
            newStockAmount.Amount = stockItem.Amount;
            db.SaveChanges();

            OnPropertyChanged(nameof(SelectedRow.Amount));
        }


        private void DecreaseAmount(object obj)
        {
            var stockItem = obj as Stock;
            stockItem.Amount--;
            if (stockItem.Amount < 0)
                stockItem.Amount = 0;

            using var db = new TheBookNookDbContext();

            var newStockAmount = db.Stocks.Where(s => s.StoreId == stockItem.StoreId && s.Isbn == stockItem.Isbn).SingleOrDefault();
            newStockAmount.Amount = stockItem.Amount;
            db.SaveChanges();

            OnPropertyChanged(nameof(SelectedRow.Amount));
        }


        private void OpenAddToStockPane(bool visible)
        {
            if (visible)
            {
                AddToStockPaneVisibility = Visibility.Visible;
                _mainWindowVM.IsBackGroundEnabled = false;
                _mainWindowVM.IsSideMenuEnabled = false;
            }
            else
            {
                AddToStockPaneVisibility = Visibility.Hidden;
                _mainWindowVM.IsBackGroundEnabled = true;
                _mainWindowVM.IsSideMenuEnabled = true;
            }
        }
        #endregion
    }
}
