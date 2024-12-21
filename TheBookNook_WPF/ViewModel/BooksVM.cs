using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
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
        private Visibility _addBookVisibility = Visibility.Hidden;
        private Visibility _editBookVisibility = Visibility.Hidden;
        private Visibility _editButtonVisibility = Visibility.Hidden;
        private Visibility _deleteButtonVisibility = Visibility.Hidden;
        private Author _author;
        private DateOnly _releasedate;
        private string _bookDetailsHeader = string.Empty;
        private string _authorName;
        private string _isbn;
        private string _price;
        private string _title;
        private string _language;
        private string _genre;
        private string _format;
        #endregion


        #region Properties
        public MainWindowVM MainWindowVM { get; }
        public Author Author { get => _author; set { _author = value; OnPropertyChanged(); } }
        public DateOnly ReleaseDate { get => _releasedate; set { _releasedate = value; OnPropertyChanged(); } }
        public string BookDetailsHeader { get => _bookDetailsHeader; set { _bookDetailsHeader = value; OnPropertyChanged(); } }
        public string AuthorName { get => _authorName; set { _authorName = value; OnPropertyChanged(); } }
        public string Isbn { get => _isbn; set { _isbn = value; OnPropertyChanged(); } }
        public string Price { get => _price; set { _price = value; OnPropertyChanged(); } }
        public string Title { get => _title; set { _title = value; OnPropertyChanged(); } }
        public string Language { get => _language; set { _language = value; OnPropertyChanged(); } }
        public string Genre { get => _genre; set { _genre = value; OnPropertyChanged(); } }
        public string Format { get => _format; set { _format = value; OnPropertyChanged(); } }
        public ObservableCollection<Book>? Books { get => _books; private set { _books = value; OnPropertyChanged(); } }
        public ObservableCollection<Genre>? Genres { get => _genres; set { _genres = value; OnPropertyChanged(); } }
        public ObservableCollection<Format>? Formats { get => _formats; set { _formats = value; OnPropertyChanged(); } }
        public ObservableCollection<Language>? Languages { get => _languages; set { _languages = value; OnPropertyChanged(); } }
        public Visibility AddBookVisibility { get => _addBookVisibility; set { _addBookVisibility = value; OnPropertyChanged(); } }
        public Visibility EditBookVisibility { get => _editBookVisibility; set { _editBookVisibility = value; OnPropertyChanged(); } }
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
            UpdateBookCMD = new RelayCommand(UpdateBook);
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
            BookDetailsVisibility(true);

            if(CurrentBook != null && obj as string == "EditBTN")
            {
                BookDetailsHeader = "EDIT BOOK";
                Isbn = CurrentBook.Isbn.ToString();
                Title = CurrentBook.Title;
                Author = CurrentBook.Authors.FirstOrDefault();
                AuthorName = CurrentBook.Authors.FirstOrDefault().FullName;
                ReleaseDate = CurrentBook.ReleaseDate;
                Language = CurrentBook.Language.Language1;
                Format = CurrentBook.Format.Format1;
                Genre = CurrentBook.Genre.Genre1;
                Price = CurrentBook.Price.ToString();
            }
            else
            {
                BookDetailsHeader = "ADD BOOK";
            }

            //OnPropertyChanged(nameof(BookDetailsHeader));
        }


        private void EditBookButtonClick(object obj)
        {
            BookDetailsVisibility(true);
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

            EditBookVisibility = Visibility.Hidden;

            CurrentBook = null;
        }


        private void DeleteBook(object obj)
        {
            using var db = new TheBookNookDbContext();
            var bookToDelete = db.Books.SingleOrDefault(x => x.Isbn == CurrentBook.Isbn);

            var confirm = MessageBox.Show($"Du you want to delete {CurrentBook.Title}", "Confirm deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirm == MessageBoxResult.Yes)
            {
                if (bookToDelete != null)
                {
                    db.Books.Remove(CurrentBook);
                    db.SaveChanges();
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
            BookDetailsVisibility(false);
            ResetBookProperties();
        }


        private void ResetBookProperties()
        {
            CurrentBook = null;
            Isbn = string.Empty;
            AuthorName = string.Empty;
            Title = string.Empty;
            ReleaseDate = new DateOnly();
            //Language = new Language();
            //Format = new Format();
            //Genre = new Genre();
        }


        private void BookDetailsVisibility(bool visible)
        {
            if (visible)
            {
                AddBookVisibility = Visibility.Visible;
                MainWindowVM.SideMenuIsEnabled = false;
            }
            else
            {
                AddBookVisibility = Visibility.Hidden;
                MainWindowVM.SideMenuIsEnabled = true;
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
