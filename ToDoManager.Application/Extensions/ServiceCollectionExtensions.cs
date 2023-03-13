using ToDoManager.Application.Services;
using ToDoManager.Application.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace ToDoManager.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IGroupService, GroupService>();
        serviceCollection.AddScoped<ITaskService, TaskService>();

        return serviceCollection;
    }
}