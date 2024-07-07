using System.Net.Http.Json;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Categories;
using Fina.Core.Responses;

namespace Fina.Web.Handlers
{
    public class CategoryHandler(IHttpClientFactory httpClientFactory) : ICategoryHandler
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient(WebConfiguration.HttpClientName);

        public async Task<PagedResponse<List<Category?>>> GetAllAsync(GetAllCategoriesRequest request)
        {
            var result = await _httpClient.GetAsync($"v1/categories");
            var context =  await result.Content.ReadFromJsonAsync<PagedResponse<List<Category?>>>();
            return context ?? new PagedResponse<List<Category?>>(null, 400, "Não foi possível consultar as categorias");
        }

        public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
        {
            var result = await _httpClient.GetAsync($"v1/categories/{request.Id}");
            var context =  await result.Content.ReadFromJsonAsync<Response<Category?>>();
            return context ?? new Response<Category?>(null, 400, "Não foi possível recuperar a categoria");
        }
        public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync("v1/categories", request);
            var context =  await result.Content.ReadFromJsonAsync<Response<Category?>>();
            return context ?? new Response<Category?>(null, 400, "Não foi possível criar a categoria");
        }

        public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
        {
            var result = await _httpClient.PutAsJsonAsync($"v1/categories/{request.Id}", request);
            var context =  await result.Content.ReadFromJsonAsync<Response<Category?>>();
            return context ?? new Response<Category?>(null, 400, "Não foi possível atualizar a categoria");
        }

        public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
        {
            var result = await _httpClient.DeleteAsync($"v1/categories/{request.Id}");
            var context =  await result.Content.ReadFromJsonAsync<Response<Category?>>();
            return context ?? new Response<Category?>(null, 400, "Não foi possível excluir a categoria");
        }
    }
}