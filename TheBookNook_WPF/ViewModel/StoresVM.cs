using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TheBookNook_WPF.Core;
using TheBookNook_WPF.Model;

namespace TheBookNook_WPF.ViewModel
{
    public class StoresVM : VMBase
    {
        #region Fields
        private MainWindowVM _mainWindowVM;
        private ObservableCollection<Store>? _stores;
        private IEnumerable<Stock> _selectedStoreStock;
        private Visibility _addButtonVisibility = Visibility.Hidden;
        private Visibility _addToStockPaneVisibility = Visibility.Hidden;
        private Store? _selectedStore;
        private Stock _selectedRow;
        #endregion

        #region Properties
        public RelayCommand DecreaseAmountCMD { get; }
        public RelayCommand IncreaseAmountCMD { get; }
        public ObservableCollection<Store>? Stores { get => _stores; private set { _stores = value; OnPropertyChanged(); } }
        public Visibility AddButtonVisibility { get => _addButtonVisibility; set { _addButtonVisibility = value; OnPropertyChanged(); } }
        public Visibility AddToStockPaneVisibility { get => _addToStockPaneVisibility; set { _addToStockPaneVisibility = value; OnPropertyChanged(); } }
        public Stock SelectedRow 
        { 
            get => _selectedRow; 
            set 
            { 
                _selectedRow = value;
                OnPropertyChanged();
            } 
        }
        public IEnumerable<Stock> SelectedStoreStock 
        { 
            get => _selectedStoreStock;
            private set 
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
                if(SelectedStore != null)
                    SelectedStoreStock = SelectedStore.Stocks;
            }
        }
        #endregion


        public StoresVM(MainWindowVM mainWindowVM)
        {
            _mainWindowVM = mainWindowVM;

            DecreaseAmountCMD = new RelayCommand(DecreaseAmount);
            IncreaseAmountCMD = new RelayCommand(IncreaseAmount);

            LoadStoresAsync();
        }


        #region Methods
        private async void LoadStoresAsync()
        {
            var data = await Task.Run(() => GetDataFromDatabase());

            Stores = data;
            //OnPropertyChanged(nameof(Stores));
        }


        private ObservableCollection<Store> GetDataFromDatabase()
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


        private void IncreaseAmount(object obj)
        {
            var stockItem = obj as Stock;
            stockItem.Amount++;

            using var db = new TheBookNookDbContext();

            var newStockAmount = db.Stocks.Where(s => s.StoreId == stockItem.StoreId && s.Isbn == stockItem.Isbn).SingleOrDefault();
            newStockAmount.Amount = stockItem.Amount;
            db.SaveChanges();
            
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
        }


        private void OpenAddToStockPane(bool visible)
        {
            if (visible)
            {
                AddToStockPaneVisibility = Visibility.Visible;
                _mainWindowVM.SideMenuIsEnabled = false;
            }
            else
            {
                AddToStockPaneVisibility = Visibility.Hidden;
                _mainWindowVM.SideMenuIsEnabled = true;
            }
        }
        #endregion
    }
}
