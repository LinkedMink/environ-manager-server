namespace LinkedMink.Data.Base
{
    public class PageCriteria
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int Offset => PageSize * PageNumber;
    }
}
