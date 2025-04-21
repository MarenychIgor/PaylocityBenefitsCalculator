using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ApiTests.IntegrationTests;
public class IntegrationTestBase : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
    private HttpClient? _httpClient;

    private readonly WebApplicationFactory<Program> _factory;

    public IntegrationTestBase(WebApplicationFactory<Program> factory)
        => _factory = factory;

    protected HttpClient HttpClient
    {
        get
        {
            if (_httpClient == default)
            {
                _httpClient = _factory.CreateClient(new WebApplicationFactoryClientOptions
                {
                    BaseAddress = new Uri("https://localhost:7124")
                });
                _httpClient.DefaultRequestHeaders.Add("accept", "text/plain");
            }

            return _httpClient;
        }
    }

    public void Dispose()
        => HttpClient.Dispose();
}

