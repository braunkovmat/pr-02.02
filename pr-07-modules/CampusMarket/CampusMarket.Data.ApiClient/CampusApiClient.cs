using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using CampusMarket.Data.ApiClient.Models;
using CampusMarket.Security.Auth;

namespace CampusMarket.Data.ApiClient;

// Сетевой слой: единственное место, которое знает про HTTP и JSON
public class CampusApiClient : IApiClient
{
    private readonly HttpClient _http;
    private readonly ITokenProvider _tokens;

    public CampusApiClient(HttpClient http, ITokenProvider tokens)
    {
        _http = http;
        _tokens = tokens;
    }

    // GET /catalog/items
    public async Task<CatalogItemList> GetCatalogItemsAsync(string? category = null)
    {
        var url = category == null ? "catalog/items" : $"catalog/items?category={Uri.EscapeDataString(category)}";
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        AddToken(request);
        var response = await _http.SendAsync(request);
        await EnsureOkAsync(response);
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CatalogItemList>(json) ?? new CatalogItemList();
    }

    // POST /orders/checkout
    public async Task<CheckoutResponse> CheckoutAsync(CheckoutRequest body)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "orders/checkout")
        {
            Content = JsonContent.Create(body)
        };
        AddToken(request);
        var response = await _http.SendAsync(request);
        await EnsureOkAsync(response);
        return (await response.Content.ReadFromJsonAsync<CheckoutResponse>())!;
    }

    // Токен берётся у Security.Auth (интерфейс ITokenProvider)
    private void AddToken(HttpRequestMessage request)
    {
        var token = _tokens.GetToken();
        if (!string.IsNullOrEmpty(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    // Превращаем ответ с ошибкой в понятное исключение
    private static async Task EnsureOkAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode) return;
        var body = await response.Content.ReadAsStringAsync();
        ApiError? err = null;
        try { err = JsonSerializer.Deserialize<ApiError>(body); } catch (JsonException) { }
        throw new CampusApiException((int)response.StatusCode, err?.Code ?? "UNKNOWN", err?.Message ?? body);
    }
}

public class CampusApiException : Exception
{
    public int StatusCode { get; }
    public string Code { get; }
    public CampusApiException(int status, string code, string message) : base(message)
    { StatusCode = status; Code = code; }
}
