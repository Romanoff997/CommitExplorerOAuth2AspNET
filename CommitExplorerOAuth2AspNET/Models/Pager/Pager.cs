namespace CommitExplorerOAuth2AspNET.Models.Pager
{
    public class ListPager
    {
        public int TotalItems { get; private set; } 
        public int CurrentPage { get; set; } // номер текущей страницы
        public int PageSize { get; } = 20; // кол-во объектов на странице
        public int TotalPages { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set;}

        public ListPager()
        { 
        }

        public ListPager(int totalItems, int page)
        {
            int totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)PageSize);
            int currentPage = page;

            int startPage = currentPage - 5;
            int endPage = currentPage + 4;

            if (startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }
            if (endPage > totalPages)
            {
                endPage = totalPages;
                if(endPage>10)
                {
                    startPage = endPage - 9;
                }
            }
            TotalItems = totalItems;
            CurrentPage = currentPage;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
        }
    }

}
