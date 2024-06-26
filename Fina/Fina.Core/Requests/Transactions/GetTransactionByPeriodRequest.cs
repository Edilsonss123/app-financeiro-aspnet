namespace Fina.Core.Requests.Transactions
{
    public class GetTransactionByPeriodRequest : Request
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}