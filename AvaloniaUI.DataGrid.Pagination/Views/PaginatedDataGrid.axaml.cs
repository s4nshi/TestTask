using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AvaloniaUI.DataGrid.Pagination.ViewModels;
using System;

namespace AvaloniaUI.DataGrid.Pagination;

public partial class PaginatedDataGrid : UserControl
{
    public static readonly StyledProperty<int> PageSizeProperty =
 AvaloniaProperty.Register<PaginatedDataGrid, int>(nameof(PageSize), 20);

    public int PageSize
    {
        get => GetValue(PageSizeProperty);
        set => SetValue(PageSizeProperty, value);
    }

    public static readonly StyledProperty<int> NumPageButtonsProperty =
    AvaloniaProperty.Register<PaginatedDataGrid, int>(nameof(NumPageButtons), 5);

    public int NumPageButtons
    {
        get => GetValue(NumPageButtonsProperty);
        set => SetValue(NumPageButtonsProperty, value);
    }

    public PaginatedDataGrid()
    {
        InitializeComponent();
        DataContext = new PaginatedDataGridViewModel();
        // Подписка на команду GoToPageCommand
        ((PaginatedDataGridViewModel)DataContext).GoToPageCommand.Subscribe(_ =>
        {
            Console.WriteLine("Command executed");
        });
    }

    private void PageButton_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        if (button != null && DataContext is PaginatedDataGridViewModel viewModel)
        {
            if (button.CommandParameter is int pageNumber)
            {
                viewModel.GoToPageCommand.Execute(pageNumber);
            }
        }
    }

}