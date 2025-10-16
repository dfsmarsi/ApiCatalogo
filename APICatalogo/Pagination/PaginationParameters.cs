namespace APICatalogo.Pagination
{
    public abstract class PaginationParameters
    {
        const int maxPageSize = 30;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = maxPageSize;

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value > maxPageSize) ? maxPageSize : value; }
        }
    }
}
