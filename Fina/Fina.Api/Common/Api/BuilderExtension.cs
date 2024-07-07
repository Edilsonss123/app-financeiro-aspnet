using Fina.Api.Data;
using Fina.Api.Handler;
using Fina.Core;
using Fina.Core.Handlers;
using Microsoft.EntityFrameworkCore;

namespace Fina.Api.Common.Api
{
    public static class BuilderExtension
    {
        public static void AddConfiguration(this WebApplicationBuilder builder)
        {
            Configuration.BackendUrl = builder.Configuration.GetValue<string>("BackendUrl") ?? string.Empty;
            Configuration.FrontendUrl = builder.Configuration.GetValue<string>("FrontendUrl") ?? string.Empty;
            ApiConfiguration.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
            ApiConfiguration.DataBaseVerion = builder.Configuration.GetValue<string>("DataBaseVerion") ?? string.Empty;
        }

        public static void AddDocumentation(this WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }

        public static void AddDataContext(this WebApplicationBuilder builder)
        {
            var serverVersion = new MySqlServerVersion(ApiConfiguration.DataBaseVerion);
            builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(ApiConfiguration.ConnectionString, serverVersion));
        }

        public static void AddCrossOrigin(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(
                options => options.AddPolicy(
                    ApiConfiguration.CorsPolicyName,
                        policy => policy.WithOrigins([
                            Configuration.BackendUrl,
                            Configuration.FrontendUrl
                        ])
                        .AllowAnyHeader()                
                        .AllowAnyMethod()                
                        .AllowCredentials()                
                    ));
        }

        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<ITransactionHandler, TransactionHandler>();
            builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
        }

    }
}