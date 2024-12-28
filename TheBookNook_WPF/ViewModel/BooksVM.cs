using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using TheBookNook_WPF.Core;
using TheBookNook_WPF.Model;

namespace TheBookNook_WPF.ViewModel
{
    public class BooksVM : VMBase
    {
        #region Fields
        private Book _currentBook;
        private ObservableCollection<Book>? _books;
        private ObservableCollection<Genre>? _genres;
        private ObservableCollection<Format>? _formats;
        private ObservableCollection<Language>? _languages;
        private Visibility _bookDetailsVisibility = Visibility.Hidden;
        private Visibility _editButtonVisibility = Visibility.Hidden;
        private Visibility _deleteButtonVisibility = Visibility.Hidden;
        private Author _author;
        private DateOnly _releasedate;
        private bool _isbnFieldEnabled;
        private string _bookDetailsHeader = string.Empty;
        private string _authorName;
        private string _isbn;
        private string _price;
        private string _title;
        private Language _language;
        private Genre _genre;
        private Format _format;
        #endregion


        #region Properties
        public MainWindowVM MainWindowVM { get; }
        public Author Author { get => _author; set { _author = value; OnPropertyChanged(); } }
        public DateOnly ReleaseDate { get => _releasedate; set { _releasedate = value; OnPropertyChanged(); } }
        public bool IsbnFieldEnabled { get => _isbnFieldEnabled; set { _isbnFieldEnabled = value; OnPropertyChanged(); } }
        public string BookDetailsHeader { get => _bookDetailsHeader; set { _bookDetailsHeader = value; OnPropertyChanged(); } }
        public string AuthorName { get => _authorName; set { _authorName = value; OnPropertyChanged(); } }
        public string Isbn { get => _isbn; set { _isbn = value; OnPropertyChanged(); } }
        public string Price { get => _price; set { _price = value; OnPropertyChanged(); } }
        public string Title { get => _title; set { _title = value; OnPropertyChanged(); } }
        public Language Language { get => _language; set { _language = value; OnPropertyChanged(); } }
        public Genre Genre { get => _genre; set { _genre = value; OnPropertyChanged(); } }
        public Format Format { get => _format; set { _format = value; OnPropertyChanged(); } }
        public ObservableCollection<Book>? Books { get => _books; private set { _books = value; OnPropertyChanged(); } }
        public ObservableCollection<Genre>? Genres { get => _genres; set { _genres = value; OnPropertyChanged(); } }
        public ObservableCollection<Format>? Formats { get => _formats; set { _formats = value; OnPropertyChanged(); } }
        public ObservableCollection<Language>? Languages { get => _languages; set { _languages = value; OnPropertyChanged(); } }
        public Visibility BookDetailsVisibility { get => _bookDetailsVisibility; set { _bookDetailsVisibility = value; OnPropertyChanged(); } }
        public Visibility EditButtonVisibility { get => _editButtonVisibility; set { _editButtonVisibility = value; OnPropertyChanged(); } }
        public Visibility DeleteButtonVisibility { get => _deleteButtonVisibility; set { _deleteButtonVisibility = value; OnPropertyChanged(); } }
        public RelayCommand OpenBookDetailsCMD { get; }
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
                OnPropertyChanged();

                if (_currentBook != null)
                    EditButtonsVisibility(true);
                else
                    EditButtonsVisibility(false);
            }
        }
        #endregion


        public BooksVM(MainWindowVM mainWindowVM)
        {
            MainWindowVM = mainWindowVM;

            OpenBookDetailsCMD = new RelayCommand(OpenBookDetails);
            CancelButtonCMD = new RelayCommand(CloseBookDetails);
            SaveNewBookCMD = new RelayCommand(SaveBookToDB);
            UpdateBookCMD = new RelayCommand(SaveBookToDB);
            DeleteBookButtonCMD = new RelayCommand(DeleteBook);

            LoadDataAsync();
        }


        #region Methods
        private async void LoadDataAsync()
        {
            await Task.Run(() => GetDataFromDatabase());
        }


        private void GetDataFromDatabase()
        {
            using var db = new TheBookNookDbContext();
            Books = new ObservableCollection<Book>(db.Books.Include(b => b.Authors).Include(b => b.Format).Include(b => b.Genre).Include(b => b.Language).ToList());
            Genres = new ObservableCollection<Genre>(db.Genres.AsNoTracking().ToList());
            Formats = new ObservableCollection<Format>(db.Formats.AsNoTracking().ToList());
            Languages = new ObservableCollection<Language>(db.Languages.AsNoTracking().ToList());
            db.DisposeAsync();
        }


        private void OpenBookDetails(object obj)
        {
            ShowBookDetails(true);

            if(CurrentBook != null && obj as string == "EditBTN")
            {
                BookDetailsHeader = "EDIT BOOK";
                IsbnFieldEnabled = false;
                Isbn = CurrentBook.Isbn.ToString();
                Title = CurrentBook.Title;
                Author = CurrentBook.Authors.FirstOrDefault();
                AuthorName = CurrentBook.Authors.FirstOrDefault().FullName;
                ReleaseDate = CurrentBook.ReleaseDate;
                Language = CurrentBook.Language;
                Format = CurrentBook.Format;
                Genre = CurrentBook.Genre;
                Price = CurrentBook.Price.ToString();
            }
            else
            {
                BookDetailsHeader = "ADD BOOK";
                IsbnFieldEnabled = true;
                ResetBookProperties();
            }
        }


        private void SaveBookToDB(object obj)
        {
            using var db = new TheBookNookDbContext();

            #region Update existing book.
            if (CurrentBook != null)
            {
                var bookToUpdate = db.Books
                    .Include(b => b.AuthorBooks)
                    .ThenInclude(ba => ba.Author).AsNoTracking()
                    .SingleOrDefault(b => b.Isbn == CurrentBook.Isbn);

                if (bookToUpdate != null)
                {
                    db.ChangeTracker.Clear();
                    db.Attach(bookToUpdate);
                   
                    bookToUpdate.Title = Title;
                    bookToUpdate.ReleaseDate = ReleaseDate;
                    bookToUpdate.Language = Language;
                    bookToUpdate.Format = Format;
                    bookToUpdate.Genre = Genre;
                    bookToUpdate.Price = decimal.Parse(Price);

                    db.Entry(bookToUpdate).State = EntityState.Modified;

                    var bookAuthors = db.AuthorBook.Where(ab => ab.BookIsbn == CurrentBook.Isbn).ToList();
                    db.AuthorBook.RemoveRange(bookAuthors);

                    var newAuthorBook = new AuthorBook { BookIsbn = CurrentBook.Isbn, AuthorId = Author.Id };
                    db.AuthorBook.Add(newAuthorBook);
                }
            }
            #endregion
            #region Add new book.
            else
            {
                Book newBook = new Book();
                newBook.Isbn = long.Parse(Isbn);
                newBook.Title = Title;
                newBook.ReleaseDate = ReleaseDate;
                newBook.LanguageId = Language.Id;
                newBook.FormatId = Format.Id;
                newBook.GenreId = Genre.Id;
                newBook.Price = decimal.Parse(Price);

                db.Books.Add(newBook);
                
                var bookAuthors = new AuthorBook { AuthorId = Author.Id, BookIsbn = newBook.Isbn };
                db.AuthorBook.Add(bookAuthors);
            }
            #endregion

            db.SaveChanges();
            LoadDataAsync();
            CloseBookDetails(obj);
        }


        private void DeleteBook(object obj)
        {
            using var db = new TheBookNookDbContext();
            var bookToDelete = db.Books.SingleOrDefault(x => x.Isbn == CurrentBook.Isbn);

            var confirm = MessageBox.Show($"Du you want to delete {CurrentBook.Title}?", "Confirm deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (confirm == MessageBoxResult.Yes)
            {
                if (bookToDelete != null)
                {
                    var bookAuthors = db.AuthorBook.Where(ab => ab.BookIsbn == bookToDelete.Isbn).ToList();
                    db.AuthorBook.RemoveRange(bookAuthors);
                    db.SaveChanges();

                    var booksInStock = db.Stocks.Where(sb => sb.Isbn == bookToDelete.Isbn).ToList();
                    db.Stocks.RemoveRange(booksInStock);
                    db.SaveChanges();

                    db.Books.Remove(bookToDelete);
                    db.SaveChanges();

                    OnPropertyChanged(nameof(MainWindowVM.StoresVM.Stores));
                }
                else
                {
                    MessageBox.Show("Could not find object in the DataBase.", "Could not delete", MessageBoxButton.OK);
                }
            }

            LoadDataAsync();
            ResetBookProperties();
        }



        private void CloseBookDetails(object obj)
        {
            ShowBookDetails(false);
            ResetBookProperties();
        }


        private void ResetBookProperties()
        {
            CurrentBook = null;
            Isbn = string.Empty;
            Title = string.Empty;
            Author = new Author();
            AuthorName = string.Empty;
            ReleaseDate = new DateOnly();
            Language = new Language();
            Format = new Format();
            Genre = new Genre();
            Price = string.Empty;
        }


        private void ShowBookDetails(bool visible)
        {
            if (visible)
            {
                BookDetailsVisibility = Visibility.Visible;
                MainWindowVM.IsSideMenuEnabled = false;
                MainWindowVM.IsBackGroundEnabled = false;
            }
            else
            {
                BookDetailsVisibility = Visibility.Hidden;
                MainWindowVM.IsSideMenuEnabled = true;
                MainWindowVM.IsBackGroundEnabled = true;
            }
        }


        private void EditButtonsVisibility(bool visible)
        {
            if (visible)
            {
                EditButtonVisibility = Visibility.Visible;
                DeleteButtonVisibility = Visibility.Visible;
            }
            else
            {
                EditButtonVisibility = Visibility.Hidden;
                DeleteButtonVisibility = Visibility.Hidden;
            }
        }
        #endregion
    }
}
