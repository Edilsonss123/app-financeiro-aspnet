using Fina.Api.Common.Api;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Transactions;
using Fina.Core.Responses;

namespace Fina.Api.Endpoints.Transactions
{
    public class DeleteTransactionEnpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) 
            => app.MapDelete("/{id}", HandleAsync)
                .WithName("Transaction: Update")
                .WithSummary("Exlui uma nova transação")
                .WithDescription("Exlui uma nova transação")
                .WithOrder(3)
                .Produces<Response<Transaction?>>();

        private static async Task<IResult> HandleAsync(ITransactionHandler handler, long id)
        {
             var request =  new DeleteTransactionRequest {
                Id = id,
                UserId = ApiConfiguration.UserId
            };

            var result = await handler.DeleteAsync(request);
            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }
}