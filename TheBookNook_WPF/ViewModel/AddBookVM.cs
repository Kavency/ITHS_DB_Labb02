using System.Collections.ObjectModel;
using TheBookNook_WPF.Model;

namespace TheBookNook_WPF.ViewModel
{
    class AddBookVM : VMBase
    {
        private Book? _newRecord;
        private ObservableCollection<Author>? authors;
        private ObservableCollection<Genre>? genres;
        private ObservableCollection<Format>? formats;
        private ObservableCollection<Language>? languages;

        public Book? NewRecord { get => _newRecord; set { _newRecord = value; OnPropertyChanged(); } }
        public ObservableCollection<Author>? Authors { get => authors; set { authors = value; OnPropertyChanged(); } }
        public ObservableCollection<Genre>? Genres { get => genres; set { genres = value; OnPropertyChanged(); } }
        public ObservableCollection<Format>? Formats { get => formats; set { formats = value; OnPropertyChanged(); } }
        public ObservableCollection<Language>? Languages { get => languages; set { languages = value; OnPropertyChanged(); } }

        public AddBookVM()
        {
            NewRecord = new Book();

            LoadDataAsync();
        }


        private async void LoadDataAsync()
        {
            await Task.Run(() => GetDataFromDatabase());
        }


        private void GetDataFromDatabase()
        {
            using var db = new TheBookNookDbContext();
            Authors = new ObservableCollection<Author>(db.Authors.ToList());
            Genres = new ObservableCollection<Genre>(db.Genres.ToList());
            Formats = new ObservableCollection<Format>(db.Formats.ToList());
            Languages = new ObservableCollection<Language>(db.Languages.ToList());
        }

    }
}
