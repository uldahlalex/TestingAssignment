using System.ComponentModel.DataAnnotations;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PgCtx;
using Service;
using service.Types;

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
        {
            builder.Configuration[nameof(AppOptions) + ":" + nameof(AppOptions.Database)] =
                pg._postgres.GetConnectionString();
        }

        builder.Services.AddDbContext<LibraryContext>((serviceProvider, options) =>
        {
            var dbOptions = serviceProvider.GetRequiredService<IOptionsMonitor<AppOptions>>().CurrentValue;
            options.UseNpgsql(dbOptions.Database);
        });
        builder.Services.AddScoped<ILibraryService, LibraryService>();
        builder.Services.AddControllers();
        var app = builder.Build();
        app.UseCors(builder =>
        {
            builder.AllowAnyOrigin();
            builder.AllowAnyMethod();
            builder.AllowAnyHeader();
        });
        app.MapControllers();
        using (var scope = app.Services.CreateScope())
        {
            var ctx = scope.ServiceProvider.GetRequiredService<LibraryContext>();
            ctx.Database.EnsureCreated();
            //get SQL for creating the database
            Console.WriteLine(ctx.Database.GenerateCreateScript());
            ctx.Books.ToList();
        }

        app.Run();
    }
}