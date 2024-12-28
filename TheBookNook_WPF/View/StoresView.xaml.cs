using System.Windows;
using System.Windows.Controls;

namespace TheBookNook_WPF.View;

public partial class StoresView : UserControl
{
    public StoresView()
    {
        InitializeComponent();
        this.DataContext = ((MainWindow)Application.Current.MainWindow).DataContext;
    }
}
