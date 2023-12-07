using FluentAssertions;
using Lab6.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Lab7.Tests;

public sealed class CustomerTests : BaseIntegrationTest
{
    public CustomerTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task Login_Should_ReturnEmptyString_When_FirstNameIsNullOrEmpty(string firstName)
    {
        var request = new AuthRequest(firstName, "Shevchenko", "380978673644");

        var result = await _coreApiService.LoginAsync(request);

        result.Should().BeEquivalentTo(string.Empty);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task Login_Should_ReturnEmptyString_When_LastNameIsNullOrEmpty(string lastName)
    {
        var request = new AuthRequest("Andrey", lastName, "380978673644");

        var result = await _coreApiService.LoginAsync(request);

        result.Should().BeEquivalentTo(string.Empty);
    }

    [Fact]
    public async Task Login_Should_Be_Succesessfull()
    {
        var request = new AuthRequest("Andrey", "Shevchenko", "380978673644");

        var result = await _coreApiService.LoginAsync(request);

        result.Should().NotBeEquivalentTo(string.Empty);
    }

    [Fact]
    public async Task Register_Should_Be_Succesessfull()
    {
        var request = new RegisterRequest("Andreyy", "Shevchenkoo", "380978673644", DateTime.UtcNow, "4149...", DateTime.UtcNow.AddYears(2), Guid.Parse("bbff9bbd-63f0-4c82-bbf7-73b5a7b8c0b3"));

        var result = await _coreApiService.RegisterAsync(request);

        var customer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.FirstName == request.FirstName && x.LastName == request.LastName);

        result.Should().NotBeEquivalentTo(string.Empty);
        customer.Should().NotBeNull();
    }

    [Fact]
    public async Task Should_Succesessfully_RetrieveCustomerList()
    {
        var action = await _coreApiService.GetListAsync();

        action.Should().NotBeNull();
        action.Any().Should().BeTrue();
    }
}