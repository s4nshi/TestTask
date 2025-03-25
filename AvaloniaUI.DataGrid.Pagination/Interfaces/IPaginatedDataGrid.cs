using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaUI.DataGrid.Pagination.Interfaces
{
    public interface IPaginatedDataGrid
    {
        int PageSize { get; set; }
        int NumPageButtons { get; set; }
        void GoToFirstPage();
        void GoToPreviousPage();
        void GoToNextPage();
        void GoToLastPage();
        void GoToPage(int pageNumber);
    }
}
