using System.Collections.ObjectModel;
using System.Windows;
using TheBookNook_WPF.Model;

namespace TheBookNook_WPF.ViewModel;

public class AuthorsVM : VMBase
{
    #region Fields
    private MainWindowVM _mainWindowVM;
    private Author? _currentAuthor;
    private ObservableCollection<Author>? _authors;
    private Visibility _AuthorDetailsVisibility = Visibility.Hidden;
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
    public Visibility DeleteButtonVisibility { get => _deleteButtonVisibility; set { _deleteButtonVisibility = value; OnPropertyChanged(); } }
    public int AuthorId { get => _authorId; set { _authorId = value; OnPropertyChanged(); } }
    public string AuthorFirstName { get => _authorFirstName; set { _authorFirstName = value; OnPropertyChanged(); } }
    public string AuthorLastName { get => _authorLastName; set { _authorLastName = value; OnPropertyChanged(); } }
    public DateOnly AuthorBirthDate { get => _authorBirthDate; set { _authorBirthDate = value; OnPropertyChanged(); } }
    public RelayCommand AddAuthorButtonCMD { get; }
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

            if(_currentAuthor != null)
            {
                AuthorId = _currentAuthor.Id;
                AuthorFirstName = _currentAuthor.FirstName;
                AuthorLastName = _currentAuthor.LastName;
                AuthorBirthDate = _currentAuthor.BirthDate;

                DeleteButtonVisibility = Visibility.Visible;
            }
            else
            {
                DeleteButtonVisibility = Visibility.Hidden;
            }
        }
    }
    #endregion


    public AuthorsVM(MainWindowVM mainWindowVM)
    {
        _mainWindowVM = mainWindowVM;
        _authors = new ObservableCollection<Author>();

        AddAuthorButtonCMD = new RelayCommand(AddAuthor);
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
        AuthorFullName();
    }


    private ObservableCollection<Author> GetDataFromDatabase()
    {
        using var db = new TheBookNookDbContext();
        var authors = new ObservableCollection<Author>(db.Authors.ToList());

        return authors;
    }


    private void AuthorFullName()
    {
        foreach (var author in Authors)
        {
            author.FullName = author.FirstName + " " + author.LastName;
        }
    }


    private void AddAuthor(object obj)
    {
        MainWindowVM.DimBackgroundVisibility = Visibility.Visible;
        AuthorDetailsVisibility = Visibility.Visible;
        MainWindowVM.SideMenuIsEnabled = false;
    }


    private void SaveAuhorToDB(object obj)
    {
        using var db = new TheBookNookDbContext();
        
        if(CurrentAuthor != null)
        {
            var authorToUpdate = db.Authors.SingleOrDefault(x => x.Id == AuthorId);

            if(authorToUpdate != null)
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
            
            Authors.Add(author);
        }
        db.SaveChanges();
        LoadAuthorsAsync();
        ResetAuthorProperties();
        CloseAuthorDetails(obj);
    }


    private void DeleteAuthor(object obj)
    {
        using var db = new TheBookNookDbContext();
        var authorToDelete = db.Authors.SingleOrDefault(x => x.Id == CurrentAuthor.Id);

        if (authorToDelete != null)
            db.Authors.Remove(authorToDelete);
        
        db.SaveChanges();
        LoadAuthorsAsync();
        ResetAuthorProperties();
        CloseAuthorDetails(obj);
    }


    private void ResetAuthorProperties()
    {
        CurrentAuthor = null;
        AuthorId = 0;
        AuthorFirstName = null;
        AuthorLastName = null;
        AuthorBirthDate = new DateOnly();
    }


    private void CloseAuthorDetails(object obj)
    {
        MainWindowVM.DimBackgroundVisibility = Visibility.Hidden;
        AuthorDetailsVisibility = Visibility.Hidden;
        MainWindowVM.SideMenuIsEnabled = true;
        CurrentAuthor = null;
    }
    #endregion
}

