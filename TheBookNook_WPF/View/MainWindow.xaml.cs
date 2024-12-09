using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TheBookNook_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isMaximized = false;
        public MainWindow()
        {
            InitializeComponent();

            // Example data
            ObservableCollection<BookExample> books = new();
            books.Add(new BookExample { ISBN = 4624960485, Title = "Creepy Creeps", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 3647920394, Title = "Creepy Bugs", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 9783567396, Title = "Creepy Birds", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 9735870452, Title = "Creepy Fish", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 9364869476, Title = "Creepy Snakes", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 9712412384, Title = "Creepy Butterflies", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 9590375832, Title = "Creepy Squirrels", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 9743563408, Title = "Creepy Lemmings", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 9735127493, Title = "Creepy Mice", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 9746387299, Title = "Nasty People", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 4624960485, Title = "Creepy Creeps", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 3647920394, Title = "Creepy Bugs", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 9783567396, Title = "Creepy Birds", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 9735870452, Title = "Creepy Fish", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 9364869476, Title = "Creepy Snakes", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 9712412384, Title = "Creepy Butterflies", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 9590375832, Title = "Creepy Squirrels", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 9743563408, Title = "Creepy Lemmings", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 9735127493, Title = "Creepy Mice", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 9746387299, Title = "Nasty People", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 4624960485, Title = "Creepy Creeps", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 3647920394, Title = "Creepy Bugs", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 9783567396, Title = "Creepy Birds", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 9735870452, Title = "Creepy Fish", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 9364869476, Title = "Creepy Snakes", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 9712412384, Title = "Creepy Butterflies", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 9590375832, Title = "Creepy Squirrels", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 9743563408, Title = "Creepy Lemmings", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 9735127493, Title = "Creepy Mice", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 9746387299, Title = "Nasty People", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 4624960485, Title = "Creepy Creeps", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 3647920394, Title = "Creepy Bugs", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 9783567396, Title = "Creepy Birds", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 9735870452, Title = "Creepy Fish", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 9364869476, Title = "Creepy Snakes", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 9712412384, Title = "Creepy Butterflies", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 9590375832, Title = "Creepy Squirrels", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 9743563408, Title = "Creepy Lemmings", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 9735127493, Title = "Creepy Mice", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });
            books.Add(new BookExample { ISBN = 9746387299, Title = "Nasty People", Author = "John McNuggin", Genre = "Horror", Type = "Pocket" });


            MainDataGrid.ItemsSource = books;
        }

        private void MoveMainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void MinimizeWindow_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeWindow_Click(object sender, RoutedEventArgs e)
        {
            if (isMaximized)
            {
                this.WindowState = WindowState.Normal;
                this.Width = 1080;
                this.Height = 720;

                isMaximized = false;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                isMaximized = true;
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }

    // Example data class
    public class BookExample
    {
        public long ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public string Type { get; set; }
    }
}