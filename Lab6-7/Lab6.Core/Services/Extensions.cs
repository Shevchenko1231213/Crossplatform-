using Microsoft.Extensions.DependencyInjection;

namespace Lab6.Core.Services;

internal static class Extensions
{
    internal static IServiceCollection AddServices(this IServiceCollection services)
        => services.AddScoped<CoreService>();
}
