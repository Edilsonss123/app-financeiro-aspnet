using System.Text.Json.Serialization;

namespace Fina.Core.Responses
{
    public class PagedResponse<TData> : Response<TData>
    {
        public int CurrentPage { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public int PageSize { get; set; } = Configuration.DefaultPageSize;
        public int TotalCount { get; set; }

        [JsonConstructor]
        public PagedResponse(
            TData? data,
            int totalCount = 1,
            int currentPage = 1,
            int pageSize = Configuration.DefaultPageSize)
            : base(data)
        {
            Data = data;
            TotalCount  = totalCount;
            CurrentPage = currentPage;
            PageSize = pageSize;
        }
        
        public PagedResponse(
            TData? data,
            int code = Configuration.DefaultStatusCode, 
            string? message = null) 
            : base(data, code, message)
        {
        }
    }
}