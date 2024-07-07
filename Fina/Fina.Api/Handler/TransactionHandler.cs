using Fina.Api.Data;
using Fina.Core.Common;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Transactions;
using Fina.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Fina.Api.Handler
{
    public class TransactionHandler(AppDbContext context) : ITransactionHandler
    {
        public async Task<PagedResponse<List<Transaction?>>> GetPeriodAsync(GetTransactionByPeriodRequest request)
        {
            try
            {
                request.StartDate ??= DateTime.Now.GetFirsDay();
                request.EndDate ??= DateTime.Now.GetLastDay();

                var query = context.Transactions
                    .AsNoTracking()
                    .Where(t => 
                        t.UserId == request.UserId &&
                        t.PaidOrReceivedAt >= request.StartDate &&
                        t.PaidOrReceivedAt <= request.EndDate)
                    .OrderBy(t => t.PaidOrReceivedAt);

                var transaction = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync<Transaction?>(); 

                var count = await query.CountAsync();

                return new PagedResponse<List<Transaction?>>(
                    transaction, 
                    count, 
                    request.PageNumber, 
                    request.PageSize
                );
            }
            catch (System.Exception)
            {
                return new PagedResponse<List<Transaction?>>(null, 500, message: "Não foi possível recuperar as transações");
            }
        }
        public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
        {
            var transaction = await context.Transactions
                .FirstOrDefaultAsync(t => t.Id == request.Id && t.UserId == request.UserId);
            
            if (transaction  is null) 
                return new Response<Transaction?>(null, 404, "Transação não encontrada");

            transaction.Title = request.Title;
            transaction.Amount = request.Amount;
            transaction.CategoryId = request.CategoryId;
            transaction.PaidOrReceivedAt = request.PaidOrReceivedAt;
            transaction.Type = request.Type;

            context.Transactions.Update(transaction);
            await context.SaveChangesAsync();

            return new Response<Transaction?>(transaction, message: "Transação atualizada com sucesso");
        }
        public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
        {
            try
            {
                var transaction = await context.Transactions
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Id == request.Id && t.UserId == request.UserId);

                if (transaction  is null) 
                    return new Response<Transaction?>(null, 404, "Transação não encontrada");
                
                return new Response<Transaction?>(transaction);

            }
            catch (System.Exception)
            {
                return new Response<Transaction?>(null, 500, "Não foi possível recuperar a categoria");
            }
        }
        public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
        {
            try
            {
                if (request is { Type: Core.Enums.ETransactionType.WithDraw, Amount: >= 0 }) request.Amount *= -1; 
                
                var transaction = new Transaction {
                    Amount = request.Amount,
                    CategoryId = request.CategoryId,
                    PaidOrReceivedAt = request.PaidOrReceivedAt,
                    Title = request.Title,
                    Type = request.Type,
                    UserId = request.UserId
                };
                await context.Transactions.AddAsync(transaction);
                await context.SaveChangesAsync();

                return new Response<Transaction?>(transaction, 201, "Transação cadastrada com sucesso");
            }
            catch (System.Exception)
            {
                return new Response<Transaction?>(null, 500, "Não foi possível criar a transação");
            }
        }
        public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
        {
            try
            {
                var transaction = await context.Transactions
                    .FirstOrDefaultAsync(t => t.Id == request.Id && t.UserId == request.UserId);

                if (transaction  is null) 
                    return new Response<Transaction?>(null, 404, "Transação não encontrada");
                
                context.Transactions.Remove(transaction);
                await context.SaveChangesAsync();
                
                return new Response<Transaction?>(null, message: "Transação removida com sucesso");
            }
            catch (System.Exception)
            {
                return new Response<Transaction?>(null, 500, "Não foi possível remover a transação");
            }
        }
    }
}