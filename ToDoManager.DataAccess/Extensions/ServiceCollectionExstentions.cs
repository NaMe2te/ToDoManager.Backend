using ToDoManager.DataAccess.Repositories;
using ToDoManager.DataAccess.Repositories.Implementations;
using  Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace ToDoManager.DataAccess.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection serviceCollection, string connectionString)
    {
        serviceCollection.AddScoped<ITaskRepository, TaskRepository>(provider => new TaskRepository(connectionString));
        serviceCollection.AddScoped<IGroupRepository, GroupRepository>(provider => new GroupRepository(connectionString));
        serviceCollection.AddScoped<IAccountRepository, AccountRepository>(provider => new AccountRepository(connectionString));

        return serviceCollection;
    }
}