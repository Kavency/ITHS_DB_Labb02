using System.Collections.ObjectModel;
using TheBookNook_WPF.Model;

namespace TheBookNook_WPF.ViewModel
{
    public class CustomersVM : VMBase
    {
        public ObservableCollection<Customer>? Customers { get; private set; }

        private Customer _selectedCustomer;

        public Customer SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged();
            }
        }


        public CustomersVM()
        {
            LoadCustomersAsync();
        }


        private async void LoadCustomersAsync()
        {
            var data = await Task.Run(() => GetDataFromDatabase());

            Customers = data;
            OnPropertyChanged("Customer");
        }
        private ObservableCollection<Customer> GetDataFromDatabase()
        {
            using var db = new TheBookNookDbContext();
            var customers = new ObservableCollection<Customer>(db.Customers.ToList());

            return customers;
        }

    }
}