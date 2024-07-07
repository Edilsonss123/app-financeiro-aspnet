using Fina.Api.Common.Api;
using Fina.Core;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Transactions;
using Fina.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Fina.Api.Endpoints.Transactions
{
    public class GetAllTransactionsByPeriodEnpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) 
            => app.MapGet("/", HandleAsync)
                .WithName("Trasanctions: Get All")
                .WithSummary("Recuperar todas as transações")
                .WithDescription("Recuperar todas as transações")
                .WithOrder(5)
                .Produces<PagedResponse<List<Transaction?>>>();

        private static async Task<IResult> HandleAsync(
            ITransactionHandler handler,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
            [FromQuery] int pageSize = Configuration.DefaultPageSize)
        {
            var request = new GetTransactionByPeriodRequest {
                StartDate = startDate,
                EndDate = endDate,
                PageNumber = pageNumber,
                PageSize = pageSize,
                UserId = ApiConfiguration.UserId
            };
            var result = await handler.GetPeriodAsync(request);
            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        } 
    }
}