using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityServer.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<Context>(optionsBuilder =>
        {
            var dataSource = Path.Join(builder.Environment.ContentRootPath, "db");
            optionsBuilder.UseSqlite($"Data source={dataSource}");
        });
        builder.Services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<Context>();
        builder.Services.AddIdentityServer()
            .AddInMemoryIdentityResources([
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
                {
                    UserClaims = { "name", "phone_number", "email", "full_name" }
                }
            ])
            .AddInMemoryClients([
                new()
                {
                    ClientId = "WebApplication",
                    ClientSecrets = { new("WebApplication".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes = { "openid", "profile" },
                    RedirectUris = { "http://localhost:5100/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:5100/signout-callback-oidc" }
                }
            ])
            .AddAspNetIdentity<User>()
            .AddProfileService<ProfileService>();
        builder.Services.AddRazorPages();

        var app = builder.Build();
        app.UseCookiePolicy(new CookiePolicyOptions
        {
            Secure = CookieSecurePolicy.Always
        });
        app.UseIdentityServer();
        app.UseAuthorization();
        app.MapRazorPages();
        app.Run();
    }
}


internal class ProfileService(UserManager<User> userManager) : IProfileService
{
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        => context.IssuedClaims.AddRange(context.Subject.Claims);


    public async Task IsActiveAsync(IsActiveContext context)
    {
        context.IsActive = (await userManager.GetUserAsync(context.Subject)) is not null;
    }
}