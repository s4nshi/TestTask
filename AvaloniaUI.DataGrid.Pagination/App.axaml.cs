using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaUI.DataGrid.Pagination.Interfaces;
using AvaloniaUI.DataGrid.Pagination.Services;
using AvaloniaUI.DataGrid.Pagination.ViewModels;
using AvaloniaUI.DataGrid.Pagination.Views;
using Autofac;


namespace AvaloniaUI.DataGrid.Pagination;

public partial class App : Application
{
    public MainWindow MainWindow;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        var builder = new ContainerBuilder();
        builder.RegisterType<HistoryService>().As<IHistoryLoader>();
        builder.RegisterType<MainWindowViewModel>(); // Регистрация MainWindowViewModel
        var container = builder.Build();

        MainWindow = new MainWindow();
        // Передаем IHistoryLoader в конструктор MainWindowViewModel
        MainWindow.DataContext = container.Resolve<MainWindowViewModel>();

    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = MainWindow; // Используем уже созданный MainWindow
        }

        base.OnFrameworkInitializationCompleted();
    }

}