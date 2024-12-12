using System.Collections.ObjectModel;
using TheBookNook_WPF.Model;

namespace TheBookNook_WPF.ViewModel
{
    public class StoresVM : VMBase
    {
        public ObservableCollection<Store>? Stores { get; private set; }

        private Customer _selectedStore;

        public Customer SelectedStore
        {
            get { return _selectedStore; }
            set
            {
                _selectedStore = value;
                OnPropertyChanged();
            }
        }


        public StoresVM()
        {
            LoadStoresAsync();
        }


        private async void LoadStoresAsync()
        {
            var data = await Task.Run(() => GetDataFromDatabase());

            Stores = data;
            OnPropertyChanged("Customer");
        }
        private ObservableCollection<Store> GetDataFromDatabase()
        {
            using var db = new TheBookNookDbContext();
            var stores = new ObservableCollection<Store>(db.Stores.ToList());

            return stores;
        }
    }
}
