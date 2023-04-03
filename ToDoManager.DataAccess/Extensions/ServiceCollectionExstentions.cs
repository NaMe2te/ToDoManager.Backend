using ToDoManager.DataAccess.Repositories;
using ToDoManager.DataAccess.Repositories.Implementations;
using  Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace ToDoManager.DataAccess.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddScoped<ITaskRepository, TaskRepository>(provider => new TaskRepository(configuration.GetConnectionString("ToDoConnectionString")));
        serviceCollection.AddScoped<IGroupRepository, GroupRepository>(provider => new GroupRepository(configuration.GetConnectionString("ToDoConnectionString"),
            new TaskRepository(configuration.GetConnectionString("ToDoConnectionString"))));

        return serviceCollection;
    }
}