using AvaloniaUI.DataGrid.Pagination.Interfaces;
using AvaloniaUI.DataGrid.Pagination.Models;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;

namespace AvaloniaUI.DataGrid.Pagination.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IHistoryLoader _historyLoader;
        private ObservableCollection<HistoryPosition> _positionHistory;
        public PaginatedDataGridViewModel PaginatedViewModel { get; }

        public ObservableCollection<HistoryPosition> PositionHistory
        {
            get => _positionHistory;
            set => this.RaiseAndSetIfChanged(ref _positionHistory, value);
        }

        public ReactiveCommand<Unit, Unit> GenerateTradeHistoryCommand { get; }

        public MainWindowViewModel(IHistoryLoader historyLoader)
        {
            _historyLoader = historyLoader;
            PositionHistory = new ObservableCollection<HistoryPosition>();
            PaginatedViewModel = new PaginatedDataGridViewModel();

            // Подписка на изменения отображаемых элементов в PaginatedViewModel
            PaginatedViewModel.WhenAnyValue(x => x.DisplayedItems)
                .Subscribe(_ => RaisePropertyChanged(nameof(DisplayedItems)));

            GenerateTradeHistoryCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var positions = await _historyLoader.LoadHistoryPositionsAsync();
                PositionHistory.Clear();
                foreach (var position in positions)
                {
                    PositionHistory.Add(position);
                }

                // Передаем данные в PaginatedDataGridViewModel
                PaginatedViewModel.SetItems(PositionHistory.ToList());
            });
        }

        // Свойство для отображаемых элементов в DataGrid
        public ObservableCollection<HistoryPosition> DisplayedItems => PaginatedViewModel.DisplayedItems;
    }
}