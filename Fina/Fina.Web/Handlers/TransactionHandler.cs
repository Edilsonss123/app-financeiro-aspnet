using System.Net.Http.Json;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Transactions;
using Fina.Core.Responses;

namespace Fina.Web.Handlers
{
    public class TransactionHandler(IHttpClientFactory httpClientFactory) : ITransactionHandler
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient(WebConfiguration.HttpClientName);

        public async Task<PagedResponse<List<Transaction?>>> GetPeriodAsync(GetTransactionByPeriodRequest request)
        {
            var result = await _httpClient.GetAsync($"v1/Transactions");
            var context =  await result.Content.ReadFromJsonAsync<PagedResponse<List<Transaction?>>>();
            return context ?? new PagedResponse<List<Transaction?>>(null, 400, "Não foi possível consultar as transações");
        }

        public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
        {
            var result = await _httpClient.GetAsync($"v1/Transactions/{request.Id}");
            var context =  await result.Content.ReadFromJsonAsync<Response<Transaction?>>();
            return context ?? new Response<Transaction?>(null, 400, "Não foi possível recuperar a transação");
        }
        public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync("v1/Transactions", request);
            var context =  await result.Content.ReadFromJsonAsync<Response<Transaction?>>();
            return context ?? new Response<Transaction?>(null, 400, "Não foi possível criar a transação");
        }

        public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
        {
            var result = await _httpClient.PutAsJsonAsync($"v1/Transactions/{request.Id}", request);
            var context =  await result.Content.ReadFromJsonAsync<Response<Transaction?>>();
            return context ?? new Response<Transaction?>(null, 400, "Não foi possível atualizar a transação");
        }

        public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
        {
            var result = await _httpClient.DeleteAsync($"v1/Transactions/{request.Id}");
            var context =  await result.Content.ReadFromJsonAsync<Response<Transaction?>>();
            return context ?? new Response<Transaction?>(null, 400, "Não foi possível excluir a transação");
        }
    }
}