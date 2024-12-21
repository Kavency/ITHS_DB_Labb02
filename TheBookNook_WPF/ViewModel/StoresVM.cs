using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using TheBookNook_WPF.Model;

namespace TheBookNook_WPF.ViewModel
{
    public class StoresVM : VMBase
    {
        #region Fields
        private MainWindowVM _mainWindowVM;
        private Store _selectedStore;
        private IEnumerable<Stock> _selectedStoreStock;
        private ObservableCollection<Store>? _stores;
        #endregion

        #region Properties
        public MainWindowVM MainWindowVM { get => _mainWindowVM; set => _mainWindowVM = value; }
        public ObservableCollection<Store>? Stores { get => _stores; private set { _stores = value; OnPropertyChanged(); } }
        public IEnumerable<Stock> SelectedStoreStock { get => _selectedStoreStock; private set { _selectedStoreStock = SelectedStore?.Stocks; OnPropertyChanged(); } }
        public Store SelectedStore
        {
            get { return _selectedStore; }
            set
            {
                _selectedStore = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public StoresVM(MainWindowVM mainWindowVM)
        {
            _mainWindowVM = mainWindowVM;

            LoadStoresAsync();
        }



        #region Methods
        private async void LoadStoresAsync()
        {
            var data = await Task.Run(() => GetDataFromDatabase());

            Stores = data;
            OnPropertyChanged(nameof(Stores));
        }
        private ObservableCollection<Store> GetDataFromDatabase()
        {
            using var db = new TheBookNookDbContext();
            var stores = new ObservableCollection<Store>(
                db.Stores.Include(s => s.Stocks)
                         .ThenInclude(i => i.IsbnNavigation)
                         .ToList()
                );

            return stores;
        }
        #endregion
    }
}
