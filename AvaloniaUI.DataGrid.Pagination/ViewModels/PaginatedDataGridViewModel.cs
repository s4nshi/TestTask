using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Subjects;
using Avalonia.Controls;
using AvaloniaUI.DataGrid.Pagination.ViewModels;

namespace AvaloniaUI.DataGrid.Pagination.ViewModels
{
    public class PaginatedDataGridViewModel : ReactiveObject
    {

        private int _currentPage;
        private int _pageSize;
        private int _numPageButtons;
        private ObservableCollection<object> _itemsSource;
        private ObservableCollection<object> _displayedItems;
        private IDisposable _throttleSubscription;
        private readonly Subject<Unit> _canExecuteChanged = new Subject<Unit>();

        public int CurrentPage
        {
            get => _currentPage;
            set => this.RaiseAndSetIfChanged(ref _currentPage, value);
        }

        public int PageSize
        {
            get => _pageSize;
            set => this.RaiseAndSetIfChanged(ref _pageSize, value);
        }

        public int NumPageButtons
        {
            get => _numPageButtons;
            set => this.RaiseAndSetIfChanged(ref _numPageButtons, value);
        }

        public ObservableCollection<object> ItemsSource
        {
            get => _itemsSource;
            set => this.RaiseAndSetIfChanged(ref _itemsSource, value);
        }

        public ObservableCollection<object> DisplayedItems
        {
            get => _displayedItems;
            set => this.RaiseAndSetIfChanged(ref _displayedItems, value);
        }

        public ReactiveCommand<Unit, Unit> FirstPageCommand { get; }
        public ReactiveCommand<Unit, Unit> PreviousPageCommand { get; }
        public ReactiveCommand<Unit, Unit> NextPageCommand { get; }
        public ReactiveCommand<Unit, Unit> LastPageCommand { get; }
        public ReactiveCommand<int, Unit> GoToPageCommand { get; }

        public bool CanGoToFirstPage => CurrentPage > 1;
        public bool CanGoToPreviousPage => CurrentPage > 1;
        public bool CanGoToNextPage => CurrentPage < TotalPages;
        public bool CanGoToLastPage => CurrentPage < TotalPages;

        public PaginatedDataGridViewModel()
        {
            FirstPageCommand = ReactiveCommand.Create(() => GoToPage(1));
            PreviousPageCommand = ReactiveCommand.Create(() => GoToPage(CurrentPage - 1));
            NextPageCommand = ReactiveCommand.Create(() => GoToPage(CurrentPage + 1));
            LastPageCommand = ReactiveCommand.Create(() => GoToPage(TotalPages));
            GoToPageCommand = ReactiveCommand.Create<int>(page => GoToPage(page));

            // Подписка на изменения для обновления возможности выполнения команд
            this.WhenAnyValue(x => x.CurrentPage, x => x.TotalPages)
                .Subscribe(_ => _canExecuteChanged.OnNext(Unit.Default));

            // Правильно управляем подпиской на Throttle
            _throttleSubscription = this.WhenAnyValue(x => x.CurrentPage)
                .Throttle(TimeSpan.FromMilliseconds(100))
                .Subscribe(_ => UpdateDisplayedItems());

            // Подписка на изменения ItemsSource
            this.WhenAnyValue(x => x.ItemsSource)
                .Subscribe(_ => UpdateDisplayedItems());
        }

        private void GoToPage(int page)
        {
            if (page >= 1 && page <= TotalPages)
            {
                CurrentPage = page;
            }
        }
        private void UpdateDisplayedItems()
        {
            if (ItemsSource == null) return;

            var startIndex = (CurrentPage - 1) * PageSize;
            var endIndex = Math.Min(startIndex + PageSize, ItemsSource.Count);

            DisplayedItems = new ObservableCollection<object>(
                ItemsSource.Skip(startIndex).Take(endIndex - startIndex));
        }

        public int TotalPages => ItemsSource == null ? 0 : (int)Math.Ceiling((double)ItemsSource.Count / PageSize);

        public IEnumerable<int> PageButtons => GetPageButtons();

        private IEnumerable<int> GetPageButtons()
        {
            var start = Math.Max(1, CurrentPage - (NumPageButtons / 2));
            var end = Math.Min(start + NumPageButtons, TotalPages);

            // Корректировка начала, если оно вышло за границы
            start = Math.Max(1, end - NumPageButtons);

            return Enumerable.Range(start, (int)(end - start + 1));
        }

    }
}
