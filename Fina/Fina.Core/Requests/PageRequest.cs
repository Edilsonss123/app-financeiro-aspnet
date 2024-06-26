namespace Fina.Core.Requests
{
    public class PageRequest : Request
    {
        public int PageSize { get; set; } = Configuration.DefaultPageSize;
        public int PageNumber { get; set; } = Configuration.DefaultPageNumber;
    }
}