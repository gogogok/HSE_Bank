using Microsoft.Extensions.DependencyInjection;
using HseBank.Domain.Factories;
using HseBank.Infrastructure.Persistence;
using HseBank.Application.Facades;
using HseBank.Application.Strategies;

namespace HseBank.ConsoleApp.DI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddHseBank(this IServiceCollection services)
        {
            // Infrastructure
            services.AddSingleton<InMemoryDb>();
            services.AddSingleton<IRepository>(sp =>
                new CachedRepositoryProxy(sp.GetRequiredService<InMemoryDb>()));

            // Domain
            services.AddSingleton<IDomainFactory, ValidatingDomainFactory>();

            // Strategies
            services.AddSingleton<AutomaticRecalcStrategy>();
            services.AddSingleton<ManualRecalcStrategy>();
            services.AddSingleton<RecalcStrategyContext>();

            // Application (facades)
            services.AddSingleton<AccountFacade>();
            services.AddSingleton<CategoryFacade>();
            services.AddSingleton<OperationFacade>();
            services.AddSingleton<AnalyticsFacade>();

            return services;
        }
    }
}