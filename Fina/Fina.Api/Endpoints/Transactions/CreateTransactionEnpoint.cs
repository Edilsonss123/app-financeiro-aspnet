using Fina.Api.Common.Api;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Transactions;
using Fina.Core.Responses;

namespace Fina.Api.Endpoints.Transactions
{
    public class CreateTransactionEnpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) 
            => app.MapPost("/", HandleAsync)
                .WithName("Transaction: Create")
                .WithSummary("Cria uma nova transação")
                .WithDescription("Criar uma nova transação")
                .WithOrder(1)
                .Produces<Response<Transaction?>>();

        private static async Task<IResult> HandleAsync(ITransactionHandler handler, CreateTransactionRequest request)
        {
            request.UserId = ApiConfiguration.UserId;
            var result = await handler.CreateAsync(request);
            return result.IsSuccess
                ? TypedResults.Created($"v1/transaction/{result.Data?.Id }", result)
                : TypedResults.BadRequest(result);
        }
    }
}