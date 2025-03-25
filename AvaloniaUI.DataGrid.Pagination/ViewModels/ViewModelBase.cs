using ReactiveUI;
using System.ComponentModel;

namespace AvaloniaUI.DataGrid.Pagination.ViewModels;

public class ViewModelBase : ReactiveObject
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected void RaisePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
