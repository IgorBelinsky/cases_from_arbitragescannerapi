using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.JSInterop;

public class SolanaAddressValidator
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime; // Для отображения модального окна

    public SolanaAddressValidator(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
    }

    public async Task<bool> ValidateAddressAsync(string address)
    {
        var url = $"https://b2b.api.arbitragescanner.io/api/onchain/v1/solana/address/general_info?address={address}";
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Add("X-API-Key", "JYnC6aZete6f6edaksMh5x");

        using var response = await _httpClient.SendAsync(request);
        var body = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode || body.Contains("\"error\""))
        {
            await ShowInvalidAddressModal();
            return false; // Не сохраняем в таблицу
        }

        return true; // Можно сохранять
    }

    private async Task ShowInvalidAddressModal()
    {
        // Замените на свою реализацию показа модального окна в вашем UI-фреймворке/Blazor
        await _jsRuntime.InvokeVoidAsync("showModal", "Invalid Solana address", 2000);
    }
}
