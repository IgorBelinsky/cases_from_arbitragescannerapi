using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DemoBlazor
{
    public class SolanaAddressValidator
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://b2b.api.arbitragescanner.io/api/onchain/v1/solana/address/general_info";
        private const string ApiKey = "JYnC6aZete6f6edaksMh5x";

        public SolanaAddressValidator(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> ValidateAddressAsync(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                return false;
            }

            try
            {
                // Формируем полный URL к API
                var fullApiUrl = $"{ApiUrl}?address={address}";
                
                // Используем AllOrigins прокси (он проще работает)
                var proxyUrl = $"https://api.allorigins.win/raw?url={Uri.EscapeDataString(fullApiUrl)}";
                
                Console.WriteLine($"Calling API via proxy: {proxyUrl}");

                // Делаем простой GET запрос без дополнительных заголовков
                // (прокси сам добавит нужные заголовки к внешнему API)
                var response = await _httpClient.GetAsync(proxyUrl);
                var body = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Response Status: {response.StatusCode}");
                Console.WriteLine($"Response Body: {body}");

                if ((int)response.StatusCode == 400 || body.Contains("\"error\""))
                {
                    Console.WriteLine("Validation FAILED");
                    return false;
                }

                Console.WriteLine("Validation SUCCESS");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Validation ERROR: {ex.Message}");
                return false;
            }
        }
    }
}
