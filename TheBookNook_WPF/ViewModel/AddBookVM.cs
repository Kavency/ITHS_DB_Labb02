using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using TheBookNook_WPF.Core;
using TheBookNook_WPF.Model;

namespace TheBookNook_WPF.ViewModel
{
    class AddBookVM : VMBase
    {
        private Book? _newBook;
        private ObservableCollection<Author>? authors;
        private ObservableCollection<Genre>? genres;
        private ObservableCollection<Format>? formats;
        private ObservableCollection<Language>? languages;

        public Book? NewBook { get => _newBook; set { _newBook = value; OnPropertyChanged(); } }
        public Format Format { get; set; }
        public Genre Genre { get; set; }
        public Language Language { get; set; }
        public DateOnly Date { get; set; }
        public string AuthorNameString { get; set; }
        public string IsbnString { get; set; }
        public string PriceString { get; set; }
        public string TitleString { get; set; }

        public RelayCommand SaveRecordCMD { get; }

        public ObservableCollection<Author>? Authors { get => authors; set { authors = value; OnPropertyChanged(); } }
        public ObservableCollection<Genre>? Genres { get => genres; set { genres = value; OnPropertyChanged(); } }
        public ObservableCollection<Format>? Formats { get => formats; set { formats = value; OnPropertyChanged(); } }
        public ObservableCollection<Language>? Languages { get => languages; set { languages = value; OnPropertyChanged(); } }

        public AddBookVM()
        {
            SaveRecordCMD = new RelayCommand(SaveRecord, CanSaveRecord);

            _newBook = new Book();

            LoadDataAsync();
        }

        private void SaveRecord(object obj)
        {
            Author author = new();
            int authorId;

            using var db = new TheBookNookDbContext();

            // TODO: Validate input
            NewBook.Isbn = long.Parse(IsbnString);
            NewBook.Title = TitleString;
            NewBook.Price = decimal.Parse(PriceString);
            NewBook.LanguageId = Language.Id;
            NewBook.FormatId = Format.Id;
            NewBook.GenreId = Genre.Id;

            if (!Authors.Any(a => a.FullName == AuthorNameString))
            {
                string[] names = AuthorNameString.Split(' ');
                string firstName = names[0];
                string lastName = names[1];

                author.FirstName = firstName;
                author.LastName = lastName;

                db.Authors.Add(author);
                db.SaveChanges();
                
            }
            else
            {
                authorId = Authors
                    .Where(x => $"{x.FirstName} {x.LastName}"
                    .Equals(AuthorNameString, StringComparison.OrdinalIgnoreCase))
                    .Select(x => x.Id).FirstOrDefault();
                    
                author = Authors.First(x => x.Id == authorId);
            }

            if (db.Entry(author).State == EntityState.Detached) 
                db.Authors.Attach(author);

            author.BookIsbns.Add(NewBook);
            db.SaveChanges();
            
            if(obj is Window addWindow)
            {
                addWindow.Close();
            }
            
        }


        private bool CanSaveRecord(object arg)
        {
            //if (_newRecord.Authors is not null
            //    && _newRecord.Genre is not null
            //    && _newRecord.Format is not null
            //    && _newRecord.Language is not null)
            //    return true;
            //else
            //    return false;

            return true;
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
            db.DisposeAsync();
        }
    }
}
