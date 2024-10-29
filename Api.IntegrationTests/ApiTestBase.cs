using System.Net.Http.Headers;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PgCtx;
using LibraryContext = DataAccess.LibraryContext;

namespace Api.IntegrationTests;

public class ApiTestBase : WebApplicationFactory<Program>
{
    public PgCtxSetup<LibraryContext> PgCtxSetup;
    public HttpClient Client { get; set; }
    
    public string UserJwt { get; set; } =
        "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.0Bk7pFvb2zgnomw3gUNpoCNq9fEhAD-qrzD38eOjo4PN0PZwiZbcssGRuslR0KG9umsY1lB0MFCH54eRSficnQ";

    public IServiceProvider ApplicationServices { get; set; }

    public ApiTestBase()
    {
        PgCtxSetup = new PgCtxSetup<LibraryContext>();
        ApplicationServices = base.Services.CreateScope().ServiceProvider;
        Client = CreateClient();
        //If you have enabled authentication, you can attach a default JWT for the http client
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(UserJwt);
    }

    /// <summary>
    /// Data that will be populated before each test
    /// </summary>
    public async Task Seed()
    {
        var ctx = ApplicationServices.GetRequiredService<LibraryContext>();
        
        var user = new Libraryuser()
        {
            Address = "",
            CreatedAt = DateTime.UtcNow,
            Email = "",
            Phone = "123",
            FirstName = "",
            IsActive = true,
            LastName = ""
        };
        ctx.Libraryusers.Add(user);
        ctx.SaveChanges();
    }



    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<LibraryContext>));

            if (descriptor != null) services.Remove(descriptor);

            services.AddDbContext<LibraryContext>(opt =>
            {
                opt.UseNpgsql(PgCtxSetup._postgres.GetConnectionString());
                opt.EnableSensitiveDataLogging(false);
                opt.LogTo(_ => { });
            });
        });
        return base.CreateHost(builder);
    }
}