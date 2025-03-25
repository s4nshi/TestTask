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
using CommunityToolkit.Mvvm.ComponentModel;
using AvaloniaUI.DataGrid.Pagination.Models;
using AvaloniaUI.DataGrid.Pagination.Interfaces;
using DynamicData;

namespace AvaloniaUI.DataGrid.Pagination.ViewModels
{
    public class PaginatedDataGridViewModel : ReactiveObject, IPaginatedDataGrid
    {
        private int _pageSize;
        private int _numPageButtons;
        private int _currentPage;
        private ObservableCollection<HistoryPosition> _allItems;
        public ObservableCollection<HistoryPosition> DisplayedItems { get; } = new ObservableCollection<HistoryPosition>();
        public ObservableCollection<int> PageButtons { get; } = new ObservableCollection<int>();

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

        public ReactiveCommand<Unit, Unit> GoToFirstPageCommand { get; }
        public ReactiveCommand<Unit, Unit> GoToPreviousPageCommand { get; }
        public ReactiveCommand<Unit, Unit> GoToNextPageCommand { get; }
        public ReactiveCommand<Unit, Unit> GoToLastPageCommand { get; }
        public ReactiveCommand<int, Unit> GoToPageCommand { get; }

        public PaginatedDataGridViewModel()
        {
            PageSize = 30;
            NumPageButtons = 2;
            _currentPage = 1;

            GoToFirstPageCommand = ReactiveCommand.Create(GoToFirstPage);
            GoToPreviousPageCommand = ReactiveCommand.Create(GoToPreviousPage);
            GoToNextPageCommand = ReactiveCommand.Create(GoToNextPage);
            GoToLastPageCommand = ReactiveCommand.Create(GoToLastPage);
            GoToPageCommand = ReactiveCommand.Create<int>(GoToPage);

            // Инициализация PageButtons
            UpdatePageButtons();
        }

        // Методы для обработки команд
        public void GoToFirstPage()
        {
            _currentPage = 1;
            UpdateDisplayedItems();
            UpdatePageButtons();
        }

        public void GoToPreviousPage()
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                UpdateDisplayedItems();
                UpdatePageButtons();
            }
        }

        public void GoToNextPage()
        {
            if (_currentPage < TotalPages)
            {
                _currentPage++;
                UpdateDisplayedItems();
                UpdatePageButtons();
            }
        }

        public void GoToLastPage()
        {
            _currentPage = TotalPages;
            UpdateDisplayedItems();
            UpdatePageButtons();
        }
        public void GoToPage(int pageNumber) // Измените на public
        {
            if (pageNumber < 1 || pageNumber > TotalPages) return;

            _currentPage = pageNumber;
            UpdateDisplayedItems();
            UpdatePageButtons();
        }

        private void UpdateDisplayedItems()
        {
            if (_allItems == null) return;

            int startIndex = (_currentPage - 1) * PageSize;
            int endIndex = Math.Min(startIndex + PageSize, _allItems.Count);
            DisplayedItems.Clear();

            for (int i = startIndex; i < endIndex; i++)
            {
                DisplayedItems.Add(_allItems[i]);
            }

            //логирование для отладки
            Console.WriteLine($"Current Page: {_currentPage}, Start Index: {startIndex}, End Index: {endIndex}, Displayed Items Count: {DisplayedItems.Count}");
        }

        public void SetItems(IEnumerable<HistoryPosition> items) // Изменено на HistoryPosition
        {
            _allItems = new ObservableCollection<HistoryPosition>(items);
            UpdateDisplayedItems();
            UpdatePageButtons();
        }

        public int TotalPages => (_allItems?.Count ?? 0) / PageSize + ((_allItems?.Count ?? 0) % PageSize > 0 ? 1 : 0);


        private void UpdatePageButtons()
        {
            PageButtons.Clear();
            int startPage = Math.Max(1, _currentPage - NumPageButtons / 2);
            int endPage = Math.Min(startPage + NumPageButtons, TotalPages);
            for (int i = startPage; i <= endPage; i++)
            {
                PageButtons.Add(i);
            }
        }
    }
}
