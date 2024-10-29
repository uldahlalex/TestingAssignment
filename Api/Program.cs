using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NSwag;
using NSwag.Generation.Processors.Security;
using PgCtx;
using Service;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOptionsWithValidateOnStart<AppOptions>()
            .Bind(builder.Configuration.GetSection(nameof(AppOptions)))
            .ValidateDataAnnotations()
            .Validate(options =>
                {
                    var context = new ValidationContext(options);
                    var validationResults = new List<ValidationResult>();
                    var isValid = Validator.TryValidateObject(options, context, validationResults, true);

                    if (!isValid)
                    {
                        var errors = validationResults.Select(r => r.ErrorMessage).ToList();
                        throw new OptionsValidationException(nameof(AppOptions), typeof(AppOptions), errors);
                    }

                    return isValid;
                }, $"{nameof(AppOptions)} validation failed");

        var appOptions = builder.Configuration.GetSection(nameof(AppOptions)).Get<AppOptions>();
        var pg = new PgCtxSetup<LibraryContext>();
        if (appOptions?.RunInTestContainer == true)
            builder.Configuration[nameof(AppOptions) + ":" + nameof(AppOptions.Database)] =
                pg._postgres.GetConnectionString();

        builder.Services.AddDbContext<LibraryContext>((serviceProvider, options) =>
        {
            var dbOptions = serviceProvider.GetRequiredService<IOptionsMonitor<AppOptions>>().CurrentValue;
            options.UseNpgsql(dbOptions.Database);
        });

        builder.Services.AddScoped<ILibraryService, LibraryService>();
        builder.Services.AddControllers();
        builder.Services.AddOpenApiDocument(configuration =>
        {
            configuration.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into the textbox: Bearer {your JWT token}."
            });

            configuration.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        });
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            var key = Encoding.UTF8.GetBytes(appOptions.JwtKey);
    
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = appOptions.Issuer,
                ValidAudience = appOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero,
                ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha512 }
            };
        });
        Console.WriteLine(JwtHelper.GenerateToken("test", appOptions.JwtKey, appOptions.Issuer, appOptions.Audience));
        builder.Services.AddAuthorization();

        var app = builder.Build();
        app.UseCors(builder =>
        {
            builder.AllowAnyOrigin();
            builder.AllowAnyMethod();
            builder.AllowAnyHeader();
        });
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        using (var scope = app.Services.CreateScope())
        {
            var ctx = scope.ServiceProvider.GetRequiredService<LibraryContext>();
            ctx.Database.EnsureCreated();
            Console.WriteLine(ctx.Database.GenerateCreateScript());
            ctx.Books.ToList();
        }

        app.UseOpenApi();
        app.UseSwaggerUi();

        app.Run();
    }
}