using System.Windows;
using TheBookNook_WPF.Model;

namespace TheBookNook_WPF.ViewModel;

public class AuthorsVM : VMBase
{
    #region Fields
    private MainWindowVM _mainWindowVM;
    private Author? _currentAuthor;
    private Visibility _addAuthorVisibility = Visibility.Hidden;
    private Visibility _editAuthorVisibility;
    #endregion

    #region Properties
    public MainWindowVM MainWindowVM { get => _mainWindowVM; set => _mainWindowVM = value; }
    public Visibility AddAuthorVisibility { get => _addAuthorVisibility; set { _addAuthorVisibility = value; OnPropertyChanged(); } }
    public Visibility EditAuthorVisibility { get => _editAuthorVisibility; set { _editAuthorVisibility = value; OnPropertyChanged(); } }
    public Author? CurrentAuthor { get => _currentAuthor; set { _currentAuthor = value; OnPropertyChanged(); } }
    public RelayCommand AddAuthorButtonCMD { get; }
    public RelayCommand SaveAuthorButtonCMD { get; }
    public RelayCommand AddCancelButtonCMD { get; }
    #endregion


    public AuthorsVM(MainWindowVM mainWindowVM)
    {
        _mainWindowVM = mainWindowVM;
        //_currentAuthor = new Author();

        AddAuthorButtonCMD = new RelayCommand(AddAuthor);
        SaveAuthorButtonCMD = new RelayCommand(SaveAuhorToDB);
        AddCancelButtonCMD = new RelayCommand(CancelAdd);
    }


    private void AddAuthor(object obj)
    {
        MainWindowVM.DimBackgroundVisibility = Visibility.Visible;
        AddAuthorVisibility = Visibility.Visible;
        MainWindowVM.SideMenuIsEnabled = false;

        CurrentAuthor = new Author();
    }
    private void SaveAuhorToDB(object obj)
    {
        Author author = new();
        
        if(CurrentAuthor != null)
        {
            author.FirstName = CurrentAuthor.FirstName;
            author.LastName = CurrentAuthor.LastName;
            author.BirthDate = CurrentAuthor.BirthDate;

            // sparar ändring
            // return
        }
        // else
        // Skapa ny ny författare
        // return



        using var db = new TheBookNookDbContext();


        MainWindowVM.Authors.Add(author);
        db.Authors.Add(author);
        db.SaveChanges();

        CancelAdd(obj);
    }
    private void CancelAdd(object obj)
    {
        MainWindowVM.DimBackgroundVisibility = Visibility.Hidden;
        AddAuthorVisibility = Visibility.Hidden;
        MainWindowVM.SideMenuIsEnabled = true;
        CurrentAuthor = null;
    }
}

