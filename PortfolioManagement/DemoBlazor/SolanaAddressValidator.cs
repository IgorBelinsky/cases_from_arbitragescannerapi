using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DemoBlazor // Замените на ваш namespace
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
                var url = $"{ApiUrl}?address={address}";
                
                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Add("X-API-Key", ApiKey);

                using var response = await _httpClient.SendAsync(request);
                var body = await response.Content.ReadAsStringAsync();

                // Проверяем код ответа 400 или наличие "error" в теле ответа
                if ((int)response.StatusCode == 400 || body.Contains("\"error\""))
                {
                    return false;
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
