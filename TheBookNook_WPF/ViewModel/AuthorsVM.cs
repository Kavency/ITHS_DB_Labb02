using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using TheBookNook_WPF.Core;
using TheBookNook_WPF.Model;

namespace TheBookNook_WPF.ViewModel;

public class AuthorsVM : VMBase
{
    #region Fields
    private MainWindowVM _mainWindowVM;
    private Author? _currentAuthor;
    private ObservableCollection<Author>? _authors;
    private Visibility _AuthorDetailsVisibility = Visibility.Hidden;
    private Visibility _editButtonVisibility = Visibility.Hidden;
    private Visibility _deleteButtonVisibility = Visibility.Hidden;
    private int _authorId;
    private string _authorFirstName;
    private string _authorLastName;
    private DateOnly _authorBirthDate;
    #endregion

    #region Properties
    public MainWindowVM MainWindowVM { get => _mainWindowVM; set => _mainWindowVM = value; }
    public ObservableCollection<Author>? Authors { get => _authors; set { _authors = value; OnPropertyChanged(); } }
    public Visibility AuthorDetailsVisibility { get => _AuthorDetailsVisibility; set { _AuthorDetailsVisibility = value; OnPropertyChanged(); } }
    public Visibility EditButtonVisibility { get => _editButtonVisibility; set { _editButtonVisibility = value; OnPropertyChanged(); } }
    public Visibility DeleteButtonVisibility { get => _deleteButtonVisibility; set { _deleteButtonVisibility = value; OnPropertyChanged(); } }
    public int AuthorId { get => _authorId; set { _authorId = value; OnPropertyChanged(); } }
    public string AuthorFirstName { get => _authorFirstName; set { _authorFirstName = value; OnPropertyChanged(); } }
    public string AuthorLastName { get => _authorLastName; set { _authorLastName = value; OnPropertyChanged(); } }
    public DateOnly AuthorBirthDate { get => _authorBirthDate; set { _authorBirthDate = value; OnPropertyChanged(); } }
    public RelayCommand OpenAuthorDetailsButtonCMD { get; }
    public RelayCommand DeleteAuthorButtonCMD { get; }
    public RelayCommand SaveAuthorButtonCMD { get; }
    public RelayCommand CloseAuthorDetailsButtonCMD { get; }
    public Author? CurrentAuthor
    {
        get => _currentAuthor;
        set
        {
            _currentAuthor = value;
            OnPropertyChanged();

            if (_currentAuthor != null)
                EditButtonsVisibility(true);
            else
                EditButtonsVisibility(false);
        }
    }
    #endregion


    public AuthorsVM(MainWindowVM mainWindowVM)
    {
        _mainWindowVM = mainWindowVM;
        _authors = new ObservableCollection<Author>();

        OpenAuthorDetailsButtonCMD = new RelayCommand(OpenAuthorDetails);
        DeleteAuthorButtonCMD = new RelayCommand(DeleteAuthor);
        SaveAuthorButtonCMD = new RelayCommand(SaveAuhorToDB);
        CloseAuthorDetailsButtonCMD = new RelayCommand(CloseAuthorDetails);

        LoadAuthorsAsync();
    }


    #region Methods
    private async void LoadAuthorsAsync()
    {
        var data = await Task.Run(() => GetDataFromDatabase());

        Authors = data;
        OnPropertyChanged(nameof(Authors));
    }


    private ObservableCollection<Author> GetDataFromDatabase()
    {
        using var db = new TheBookNookDbContext();
        var authors = new ObservableCollection<Author>(db.Authors.ToList());

        return authors;
    }


    private void OpenAuthorDetails(object obj)
    {
        AuthorPaneVisibility(true);

        if (_currentAuthor != null && obj as string == "EditBTN")
        {
            AuthorId = _currentAuthor.Id;
            AuthorFirstName = _currentAuthor.FirstName;
            AuthorLastName = _currentAuthor.LastName;
            AuthorBirthDate = _currentAuthor.BirthDate;
            EditButtonsVisibility(true);
        }
        else
        {
            ResetAuthorProperties();
        }
    }


    private void DeleteAuthor(object obj)
    {
        using var db = new TheBookNookDbContext();
        var authorToDelete = db.Authors.SingleOrDefault(x => x.Id == CurrentAuthor.Id);

        var confirm = MessageBox.Show($"Do you want to delete {authorToDelete.FirstName} {authorToDelete.LastName}?", "Confirm deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (confirm == MessageBoxResult.Yes)
        {
            if (authorToDelete != null)
            {
                // Get all books related to author.
                var books = db.Books
                    .Include(b => b.AuthorBooks)
                    .ThenInclude(ba => ba.Author)
                    .Where(b => b.AuthorBooks.Any(ba => ba.AuthorId == authorToDelete.Id))
                    .ToList();

                // Get and remove all entries in AuthorBook table.
                foreach (var book in books)
                {
                    var authorBooks = db.AuthorBook.Where(ab => ab.AuthorId == authorToDelete.Id && ab.BookIsbn == book.Isbn).ToList();
                    db.RemoveRange(authorBooks);
                }
                db.SaveChanges();

                foreach(var book in books)
                {
                    MainWindowVM.BooksVM.Books.Remove(book);
                }

                db.RemoveRange(books);
                db.SaveChanges();

                db.Authors.Remove(authorToDelete);
                db.SaveChanges();
            }
            else
            {
                MessageBox.Show("Could not find object in the DataBase.", "Could not delete", MessageBoxButton.OK);
            }
        }

        LoadAuthorsAsync();
        OnPropertyChanged(nameof(MainWindowVM.BooksVM.Books));
        CloseAuthorDetails(obj);
    }


    private void SaveAuhorToDB(object obj)
    {
        using var db = new TheBookNookDbContext();

        if (CurrentAuthor != null)
        {
            var authorToUpdate = db.Authors.SingleOrDefault(x => x.Id == AuthorId);

            if (authorToUpdate != null)
            {
                authorToUpdate.Id = AuthorId;
                authorToUpdate.FirstName = AuthorFirstName;
                authorToUpdate.LastName = AuthorLastName;
                authorToUpdate.BirthDate = AuthorBirthDate;
            }
        }
        else
        {
            Author author = new();
            author.FirstName = AuthorFirstName;
            author.LastName = AuthorLastName;
            author.BirthDate = AuthorBirthDate;

            db.Authors.Add(author);
            Authors.Add(author);
        }

        db.SaveChanges();
        OnPropertyChanged(nameof(Authors));
        CloseAuthorDetails(obj);
    }


    private void AuthorPaneVisibility(bool visible)
    {
        if (visible)
        {
            AuthorDetailsVisibility = Visibility.Visible;
            MainWindowVM.SideMenuIsEnabled = false;
        }
        else
        {
            AuthorDetailsVisibility = Visibility.Hidden;
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


    private void ResetAuthorProperties()
    {
        CurrentAuthor = null;
        AuthorId = 0;
        AuthorFirstName = string.Empty;
        AuthorLastName = string.Empty;
        AuthorBirthDate = new DateOnly();
    }


    private void CloseAuthorDetails(object obj)
    {
        AuthorPaneVisibility(false);
        ResetAuthorProperties();
    }
    #endregion
}

