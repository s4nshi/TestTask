using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

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
    }
}