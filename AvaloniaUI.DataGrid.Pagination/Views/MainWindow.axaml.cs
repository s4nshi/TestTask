using Avalonia.Controls;
using AvaloniaUI.DataGrid.Pagination.ViewModels;

namespace AvaloniaUI.DataGrid.Pagination.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new PaginatedDataGridViewModel();
    }
}