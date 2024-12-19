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
        private Book? _newBook;
        private Book _selectedBook;
        private ObservableCollection<Book>? _books;
        private ObservableCollection<Author>? _authors;
        private ObservableCollection<Genre>? _genres;
        private ObservableCollection<Format>? _formats;
        private ObservableCollection<Language>? _languages;
        private Visibility _addBookVisibility = Visibility.Hidden;
        private Visibility _editBookVisibility = Visibility.Hidden;
        private Visibility _dimBackgroundVisibility = Visibility.Hidden;
        private Visibility _editButtonVisibility = Visibility.Hidden;
        private Visibility _deleteButtonVisibility = Visibility.Hidden;
        private Author _author;
        private Format _format;
        private Genre _genre;
        private Language _language;
        private DateOnly _date;
        private string _authorNameString;
        private string _isbnString;
        private string _priceString;
        private string _titleString;
        #endregion


        #region Properties
        public Book? NewBook { get => _newBook; set { _newBook = value; OnPropertyChanged(); } }
        public Author Author { get => _author; set { _author = value; OnPropertyChanged(); } }
        public Format Format { get => _format; set { _format = value; OnPropertyChanged(); } }
        public Genre Genre { get => _genre; set { _genre = value; OnPropertyChanged(); } }
        public Language Language { get => _language; set { _language = value; OnPropertyChanged(); } }
        public DateOnly Date { get => _date; set { _date = value; OnPropertyChanged(); } }
        public string AuthorNameString { get => _authorNameString; set { _authorNameString = value; OnPropertyChanged(); } }
        public string IsbnString { get => _isbnString; set { _isbnString = value; OnPropertyChanged(); } }
        public string PriceString { get => _priceString; set { _priceString = value; OnPropertyChanged(); } }
        public string TitleString { get => _titleString; set { _titleString = value; OnPropertyChanged(); } }
        public ObservableCollection<Book>? Books { get => _books; private set { _books = value; OnPropertyChanged(); } }
        public ObservableCollection<Author>? Authors { get => _authors; set { _authors = value; OnPropertyChanged(); } }
        public ObservableCollection<Genre>? Genres { get => _genres; set { _genres = value; OnPropertyChanged(); } }
        public ObservableCollection<Format>? Formats { get => _formats; set { _formats = value; OnPropertyChanged(); } }
        public ObservableCollection<Language>? Languages { get => _languages; set { _languages = value; OnPropertyChanged(); } }
        public Visibility AddBookVisibility { get => _addBookVisibility; set { _addBookVisibility = value; OnPropertyChanged(); } }
        public Visibility EditBookVisibility { get => _editBookVisibility; set { _editBookVisibility = value; OnPropertyChanged(); } }
        public Visibility DimBackgroundVisibility { get => _dimBackgroundVisibility; set { _dimBackgroundVisibility = value; OnPropertyChanged(); } }
        public Visibility EditButtonVisibility { get => _editButtonVisibility; set { _editButtonVisibility = value; OnPropertyChanged(); } }
        public Visibility DeleteButtonVisibility { get => _deleteButtonVisibility; set { _deleteButtonVisibility = value; OnPropertyChanged(); } }
        public RelayCommand AddBookButtonCMD { get; }
        public RelayCommand EditBookButtonCMD { get; }
        public RelayCommand CancelButtonCMD { get; }
        public RelayCommand SaveNewBookCMD { get; }
        public RelayCommand DeleteBookButtonCMD { get; }

        public Book SelectedBook 
        { 
            get => _selectedBook; 
            set 
            { 
                _selectedBook = value;
                
                if(_selectedBook is not null)
                {
                    EditButtonVisibility = Visibility.Visible;
                    DeleteButtonVisibility = Visibility.Visible;
                }
                else
                {
                    EditButtonVisibility = Visibility.Hidden;
                    DeleteButtonVisibility = Visibility.Hidden;
                }

                OnPropertyChanged(); 
                
            } 
        }
        #endregion


        public BooksVM()
        {
            AddBookButtonCMD = new RelayCommand(AddBookButtonClick);
            EditBookButtonCMD = new RelayCommand(EditBookButtonClick);
            CancelButtonCMD = new RelayCommand(CancelButtonClick);
            SaveNewBookCMD = new RelayCommand(SaveNewBookToDB);
            DeleteBookButtonCMD = new RelayCommand(DeleteBook);

            _newBook = new Book();

            LoadDataAsync();
        }

        private void DeleteBook(object obj)
        {
            using var db = new TheBookNookDbContext();

            //var authors = db.Authors
            //                .Include(a => a.BookIsbns)
            //                .Where(a => a.BookIsbns
            //                .Any(b => b.Isbn == SelectedBook.Isbn))
            //                .AsNoTracking()
            //                .ToList();

            //foreach(var author in authors)
            //{
            //    var bookToRemove = author.BookIsbns.SingleOrDefault(b => b.Isbn == SelectedBook.Isbn);
            //    author.BookIsbns.Remove(bookToRemove);
            //}


            db.Books.Remove(SelectedBook);
            db.SaveChanges();
            Books.Remove(SelectedBook);
        }

        private void EditBookButtonClick(object obj)
        {
            DimBackgroundVisibility = Visibility.Visible;
            EditBookVisibility = Visibility.Visible;
        }


        private async void LoadDataAsync()
        {
            await Task.Run(() => GetDataFromDatabase());
        }


        private void GetDataFromDatabase()
        {
            using var db = new TheBookNookDbContext();
            Books = new ObservableCollection<Book>(db.Books.Include(b => b.Authors).ToList());
            Authors = new ObservableCollection<Author>(db.Authors.Include(a => a.BookIsbns).ToList());
            Genres = new ObservableCollection<Genre>(db.Genres.AsNoTracking().ToList());
            Formats = new ObservableCollection<Format>(db.Formats.AsNoTracking().ToList());
            Languages = new ObservableCollection<Language>(db.Languages.AsNoTracking().ToList());
            db.DisposeAsync();
            AuthorFullName();
        }


        private void AuthorFullName()
        {
            foreach (var author in Authors)
            {
                author.FullName = author.FirstName + " " + author.LastName;
            }
        }


        private void AddBookButtonClick(object obj)
        {
            SwitchVisibility(showForms: true);
        }


        private void CancelButtonClick(object obj)
        {
            if(obj is Button btn)
            {
                if(btn.Name == "CancelEdit")
                {
                    EditBookVisibility = Visibility.Hidden;
                    DimBackgroundVisibility = Visibility.Hidden;
                }
            }

            ClearProperties();
            SwitchVisibility(showForms: false);
        }


        private void SaveNewBookToDB(object obj)
        {
            NewBook = new Book();
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

            //author.BookIsbns.Add(NewBook);
            NewBook.Authors.Add(author);
            db.Books.Add(NewBook);
            db.SaveChanges();

            Books.Add(NewBook);

            ClearProperties();

            SwitchVisibility(showForms: false);
        }


        private void ClearProperties()
        {
            NewBook = null;
            IsbnString = string.Empty;
            TitleString = string.Empty;
            AuthorNameString = string.Empty;
            PriceString = string.Empty;
            Language = null;
            Format = null;
            Genre = null;
        }


        private void SwitchVisibility(bool showForms)
        {
            if (showForms)
            {
                DimBackgroundVisibility = Visibility.Visible;
                AddBookVisibility = Visibility.Visible;
            }
            else
            {
                DimBackgroundVisibility = Visibility.Hidden;
                AddBookVisibility = Visibility.Hidden;
            }
        }
    }
}
