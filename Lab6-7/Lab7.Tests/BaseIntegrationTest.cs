using Lab6.Core.Dal;
using Lab6.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Lab7.Tests;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    private readonly IServiceScope _serviceScope;

    protected readonly CoreService _coreApiService;

    protected readonly LabContext _dbContext;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _serviceScope = factory.Services.CreateScope();

        _dbContext = _serviceScope.ServiceProvider.GetRequiredService<LabContext>();

        _coreApiService = _serviceScope.ServiceProvider.GetRequiredService<CoreService>();
    }
}
