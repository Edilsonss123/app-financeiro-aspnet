using Fina.Api.Data;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Categories;
using Fina.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Fina.Api.Handler
{
    public class CategoryHandler(AppDbContext context) : ICategoryHandler
    {
        public async Task<PagedResponse<List<Category?>>> GetAllAsync(GetAllCategoriesRequest request)
        {
            try
            {
                var query = context.Categories
                    .AsNoTracking()
                    .Where(c => c.UserId == request.UserId)
                    .OrderBy(c => c.Title);

                var categories = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync<Category?>(); 

                var count = await query.CountAsync();

                return new PagedResponse<List<Category?>>(
                    categories, 
                    count, 
                    request.PageNumber, 
                    request.PageSize
                );
            }
            catch (System.Exception)
            {
                return new PagedResponse<List<Category?>>(null, 500, "Não foi possível recuperar as categorias");
            }
        }

        public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
        {
            try
            {
                var category = await context.Categories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == request.Id && c.UserId == request.UserId);

                if (category  is null) 
                    return new Response<Category?>(null, 404, "Categoria não encontrada");

                return new Response<Category?>(category);
            }
            catch (Exception)
            {
                return new Response<Category?>(null, 500, "Não foi possível recuperar a categoria");
            }
        }
        public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
        {
            try
            {
                var category = new Category {
                    UserId = request.UserId,
                    Title = request.Title,
                    Description = request.Description
                };

                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();

                return new Response<Category?>(category, 201, "Categoria cadastrada com sucesso");
            }
            catch (Exception)
            {
                return new Response<Category?>(null, 500, "Não foi possível criar a categoria");
            }
        }


        public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
        {
            try
            {
                var category = await context.Categories
                    .FirstOrDefaultAsync(c => c.Id == request.Id && c.UserId == request.UserId);

                if (category  is null) 
                    return new Response<Category?>(null, 404, "Categoria não encontrada");

                category.Title = request.Title;
                category.Description = request.Description;

                context.Categories.Update(category);
                await context.SaveChangesAsync();
                return new Response<Category?>(category, message: "Categoria atualizada com sucesso");
            }
            catch (System.Exception)
            {
                return new Response<Category?>(null, 500, "Não foi possível atualizar a categoria");
            }
        }

        public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
        {
            try
            {
                var category = await context.Categories
                    .FirstOrDefaultAsync(c => c.Id == request.Id && c.UserId == request.UserId);

                if (category  is null) 
                    return new Response<Category?>(null, 404, "Categoria não encontrada");
                
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                
                return new Response<Category?>(null, message: "Categoria removida com sucesso");
            }
            catch (System.Exception)
            {
                return new Response<Category?>(null, 500, "Não foi possível remover a categoria");
            }
        }
    }
}