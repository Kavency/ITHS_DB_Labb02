using System.Collections.ObjectModel;
using TheBookNook_WPF.Model;

namespace TheBookNook_WPF.ViewModel
{
    public class AuthorsVM : VMBase
    {
        public ObservableCollection<Author>? Authors { get; private set; }

        public AuthorsVM()
        {
            LoadAuthorsAsync();
        }

        private async void LoadAuthorsAsync()
        {
            var data = await Task.Run(() => GetDataFromDatabase());

            Authors = data;
            OnPropertyChanged("Authors");
        }

        private ObservableCollection<Author> GetDataFromDatabase()
        {
            using var db = new TheBookNookDbContext();
            var authors = new ObservableCollection<Author>(db.Authors.ToList());

            return authors;
        }

    }
}
