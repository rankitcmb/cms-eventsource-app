using System;
using CMS.Core.Server.WebApi.Persistence.MSSQL.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CMS.Core.Server.WebApi.Persistence.MSSQL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMSSQL(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(ctx =>
            {
                var connectionString = configuration.GetConnectionString("mssql");
                return new MemberRepository(connectionString);
            });


            return services;
        }

        
    }
}