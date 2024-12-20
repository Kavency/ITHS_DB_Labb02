using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using TheBookNook_WPF.Model;

namespace TheBookNook_WPF.ViewModel
{
    public class BooksVM : VMBase
    {
        #region Fields
        private Book _currentBook;
        private ObservableCollection<Book>? _books;
        //private ObservableCollection<Author>? _authors;
        private ObservableCollection<Genre>? _genres;
        private ObservableCollection<Format>? _formats;
        private ObservableCollection<Language>? _languages;
        private Visibility _addBookVisibility = Visibility.Hidden;
        private Visibility _editBookVisibility = Visibility.Hidden;
        //private Visibility _dimBackgroundVisibility = Visibility.Hidden;
        private Visibility _editButtonVisibility = Visibility.Hidden;
        private Visibility _deleteButtonVisibility = Visibility.Hidden;
        private Author _author;
        private DateOnly _date;
        private string _authorNameString;
        private string _isbnString;
        private string _priceString;
        private string _titleString;
        private Language _language;
        private Genre _genre;
        private Format _format;
        #endregion


        #region Properties
        public MainWindowVM MainWindowVM { get; }
        public Author Author { get => _author; set { _author = value; OnPropertyChanged(); } }
        public DateOnly Date { get => _date; set { _date = value; OnPropertyChanged(); } }
        public string AuthorNameString { get => _authorNameString; set { _authorNameString = value; OnPropertyChanged(); } }
        public string IsbnString { get => _isbnString; set { _isbnString = value; OnPropertyChanged(); } }
        public string PriceString { get => _priceString; set { _priceString = value; OnPropertyChanged(); } }
        public string TitleString { get => _titleString; set { _titleString = value; OnPropertyChanged(); } }
        public Language Language { get => _language; set { _language = value; OnPropertyChanged(); } }
        public Genre Genre { get => _genre; set { _genre = value; OnPropertyChanged(); } }
        public Format Format { get => _format; set { _format = value; OnPropertyChanged(); } }
        public ObservableCollection<Book>? Books { get => _books; private set { _books = value; OnPropertyChanged(); } }
        //public ObservableCollection<Author>? Authors { get => _authors; set { _authors = value; OnPropertyChanged(); } }
        public ObservableCollection<Genre>? Genres { get => _genres; set { _genres = value; OnPropertyChanged(); } }
        public ObservableCollection<Format>? Formats { get => _formats; set { _formats = value; OnPropertyChanged(); } }
        public ObservableCollection<Language>? Languages { get => _languages; set { _languages = value; OnPropertyChanged(); } }
        public Visibility AddBookVisibility { get => _addBookVisibility; set { _addBookVisibility = value; OnPropertyChanged(); } }
        public Visibility EditBookVisibility { get => _editBookVisibility; set { _editBookVisibility = value; OnPropertyChanged(); } }
        //public Visibility DimBackgroundVisibility { get => _dimBackgroundVisibility; set { _dimBackgroundVisibility = value; OnPropertyChanged(); } }
        public Visibility EditButtonVisibility { get => _editButtonVisibility; set { _editButtonVisibility = value; OnPropertyChanged(); } }
        public Visibility DeleteButtonVisibility { get => _deleteButtonVisibility; set { _deleteButtonVisibility = value; OnPropertyChanged(); } }
        public RelayCommand AddBookButtonCMD { get; }
        public RelayCommand EditBookButtonCMD { get; }
        public RelayCommand CancelButtonCMD { get; }
        public RelayCommand SaveNewBookCMD { get; }
        public RelayCommand UpdateBookCMD { get; }
        public RelayCommand DeleteBookButtonCMD { get; }
        public Book CurrentBook
        {
            get => _currentBook;
            set
            {
                _currentBook = value;

                //TODO: Move from setter
                if (_currentBook is not null)
                {
                    EditButtonVisibility = Visibility.Visible;
                    DeleteButtonVisibility = Visibility.Visible;
                }
                else
                {
                    EditButtonVisibility = Visibility.Hidden;
                    DeleteButtonVisibility = Visibility.Hidden;
                }

                if (_currentBook is not null && _currentBook.Authors.Count > 0)
                {
                    var author = CurrentBook.Authors.FirstOrDefault();
                    //AuthorNameString = author.FullName.ToString();
                }

                OnPropertyChanged();
            }
        }

        #endregion


        public BooksVM(MainWindowVM mainWindowVM)
        {
            MainWindowVM = mainWindowVM;

            _format = new();
            _genre = new();
            _language = new();

            AddBookButtonCMD = new RelayCommand(AddBookButtonClick);
            EditBookButtonCMD = new RelayCommand(EditBookButtonClick);
            CancelButtonCMD = new RelayCommand(CancelButtonClick);
            SaveNewBookCMD = new RelayCommand(SaveNewBookToDB);
            UpdateBookCMD = new RelayCommand(UpdateBook);
            DeleteBookButtonCMD = new RelayCommand(DeleteBook);

            LoadDataAsync();
        }

        private void UpdateBook(object obj)
        {
            Author = GetAuthorFromDB();
            PrepareCurrentBook();
            CurrentBook.Authors.Clear();
            CurrentBook.Authors.Add(Author);

            using var db = new TheBookNookDbContext();

            var book = db.Books.SingleOrDefault(b => b.Isbn == CurrentBook.Isbn);

            // Detach any existing instances of the book
            db.Entry(book).State = EntityState.Detached;

            // Reattach the selected book and mark it as modified
            db.Books.Attach(CurrentBook);
            db.Entry(CurrentBook).State = EntityState.Modified;

            book.Title = CurrentBook.Title;
            book.LanguageId = CurrentBook.Language.Id;
            book.GenreId = CurrentBook.Genre.Id;
            book.ReleaseDate = CurrentBook.ReleaseDate;
            book.FormatId = CurrentBook.Format.Id;
            book.Authors.Clear();
            
            foreach (var author in CurrentBook.Authors)
            {
                if (db.Entry(author).State == EntityState.Detached)
                {
                    db.Authors.Attach(author);
                }

                book.Authors.Add(author);
            }

            Author.BookIsbns.Add(CurrentBook);

            db.SaveChanges();

            //TODO: Change visibility, Join Dim and Edit views
            MainWindowVM.DimBackgroundVisibility = Visibility.Hidden;
            EditBookVisibility = Visibility.Hidden;

            CurrentBook = null;
        }

        private void DeleteBook(object obj)
        {
            using var db = new TheBookNookDbContext();

            //TODO: Check if book exists in DB

            db.Books.Remove(CurrentBook);
            db.SaveChanges();
            Books.Remove(CurrentBook);
        }

        private void EditBookButtonClick(object obj)
        {
            MainWindowVM.DimBackgroundVisibility = Visibility.Visible;
            EditBookVisibility = Visibility.Visible;

            Format = CurrentBook.Format;
            Genre = CurrentBook.Genre;
            Language = CurrentBook.Language;
        }


        private async void LoadDataAsync()
        {
            await Task.Run(() => GetDataFromDatabase());
        }


        private void GetDataFromDatabase()
        {
            using var db = new TheBookNookDbContext();
            Books = new ObservableCollection<Book>(db.Books.Include(b => b.Authors).Include(b => b.Format).Include(b => b.Genre).Include(b => b.Language).ToList());
            //Authors = new ObservableCollection<Author>(db.Authors.Include(a => a.BookIsbns).ToList());
            Genres = new ObservableCollection<Genre>(db.Genres.AsNoTracking().ToList());
            Formats = new ObservableCollection<Format>(db.Formats.AsNoTracking().ToList());
            Languages = new ObservableCollection<Language>(db.Languages.AsNoTracking().ToList());
            db.DisposeAsync();
            //AuthorFullName();
        }


        //private void AuthorFullName()
        //{
        //    foreach (var author in Authors)
        //    {
        //        author.FullName = author.FirstName + " " + author.LastName;
        //    }
        //}


        private void AddBookButtonClick(object obj)
        {
            SwitchVisibility(showForms: true);
            CurrentBook = new Book() { Format = new(), Genre = new(), Language = new() };
        }


        private void CancelButtonClick(object obj)
        {
            if (obj is Button btn)
            {
                if (btn.Name == "CancelEdit")
                {
                    EditBookVisibility = Visibility.Hidden;
                    MainWindowVM.DimBackgroundVisibility = Visibility.Hidden;
                }
            }

            ClearProperties();
            SwitchVisibility(showForms: false);
        }


        private void SaveNewBookToDB(object obj)
        {
            Author author = new();


            using var db = new TheBookNookDbContext();

            //TODO: Validate input and add converters
            PrepareCurrentBook();

            if (!MainWindowVM.Authors.Any(a => a.FullName == AuthorNameString))
            {
                author = AddAuthorToDB();
            }
            else
            {
                author = GetAuthorFromDB();
            }

            if (db.Entry(author).State == EntityState.Detached)
                db.Authors.Attach(author);

            CurrentBook.Authors.Add(author);
            db.Books.Add(CurrentBook);
            db.SaveChanges();

            Books.Add(CurrentBook);

            ClearProperties();

            SwitchVisibility(showForms: false);
        }

        private void PrepareCurrentBook()
        {
            CurrentBook.Isbn = long.Parse(IsbnString);
            CurrentBook.Title = TitleString;
            CurrentBook.Price = decimal.Parse(PriceString);
            CurrentBook.LanguageId = Language.Id;
            CurrentBook.FormatId = Format.Id;
            CurrentBook.GenreId = Genre.Id;
            CurrentBook.Language = null;
            CurrentBook.Format = null;
            CurrentBook.Genre = null;
        }

        private Author GetAuthorFromDB()
        {
            Author author = new();
            int authorId;

            using var db = new TheBookNookDbContext();

            authorId = MainWindowVM.Authors.Where(x => $"{x.FirstName} {x.LastName}"
                              .Equals(AuthorNameString, StringComparison.OrdinalIgnoreCase))
                              .Select(x => x.Id).FirstOrDefault();

            return author = MainWindowVM.Authors.First(x => x.Id == authorId);
        }

        private Author AddAuthorToDB()
        {
            Author author = new();

            using var db = new TheBookNookDbContext();

            string[] names = AuthorNameString.Split(' ');
            string firstName = names[0];
            string lastName = names[1];

            author.FirstName = firstName;
            author.LastName = lastName;

            db.Authors.Add(author);
            db.SaveChanges();

            return author;
        }

        private void ClearProperties()
        {
            CurrentBook = null;
            IsbnString = string.Empty;
            TitleString = string.Empty;
            AuthorNameString = string.Empty;
            PriceString = string.Empty;
        }


        private void SwitchVisibility(bool showForms)
        {
            if (showForms)
            {
                MainWindowVM.DimBackgroundVisibility = Visibility.Visible;
                AddBookVisibility = Visibility.Visible;
            }
            else
            {
                MainWindowVM.DimBackgroundVisibility = Visibility.Hidden;
                AddBookVisibility = Visibility.Hidden;
            }
        }
    }
}
