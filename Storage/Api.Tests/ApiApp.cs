using Microsoft.AspNetCore.Mvc.Testing;

namespace Api.Tests;

public class ApiApp : WebApplicationFactory<Program>
{
    public IServiceProvider GetServiceCollection()
    {
        return Services;           
    }
}

