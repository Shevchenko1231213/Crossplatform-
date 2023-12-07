using Lab6.Core.Auth;
using Lab6.Core.Dal;
using Lab6.Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Lab6.Core;

public static class Extensions
{
    internal static T BindOptions<T>(this IConfiguration configuration, string sectionName) where T : new()
      => BindOptions<T>(configuration.GetSection(sectionName));

    internal static T BindOptions<T>(this IConfigurationSection section) where T : new()
    {
        var options = new T();
        section.Bind(options);
        return options;
    }

    internal static long ToTimestamp(this DateTime date)
    {
        var centuryBegin = DateTime.UnixEpoch;

        var expectedDate = date.Subtract(new TimeSpan(centuryBegin.Ticks));

        return expectedDate.Ticks / 10000;
    }

    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase<LabContext>(configuration);

        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new HeaderApiVersionReader("x-version")
            );
        });

        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        services.AddAuth(configuration);

        return services;
    }

    internal static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration.BindOptions<JwtOptions>("Security");

        services.AddSingleton(jwtOptions);

        services.AddScoped<JwtTokenProvider>();
        services.AddSingleton<JwtTokenGenerator>();

        services
          .AddAuthentication(options =>
          {
              options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
              options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
              options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
          })
          .AddJwtBearer(jwt =>
          {
              var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey));

              jwt.SaveToken = true;

              jwt.TokenValidationParameters = new TokenValidationParameters()
              {
                  IssuerSigningKey = issuerSigningKey,
                  ValidIssuer = jwtOptions.Issuer,
                  ValidAudience = jwtOptions.Audience,
                  ValidateIssuer = true,
                  ValidateAudience = true
              };
          });

        services.AddAuthorization();

        services.AddServices();

        return services;
    }
}
