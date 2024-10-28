using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Api.IntegrationTests;

public class UnitTestBase : WebApplicationFactory<Program> 
{
    
    
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        
    }

    public UnitTestBase()
    {
        
    }
}