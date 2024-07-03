using Discord;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TrtlBotSharp
{
    public partial class Commands : ModuleBase<SocketCommandContext>
    {
        private readonly HttpClient _httpClient;

        public Commands(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        [Command("price")]
        public async Task PriceAsync([Remainder]string Remainder = "")
        {
            try
            {
                // CoinGecko API endpoint for traaitt
                string apiUrl = "https://api.coingecko.com/api/v3/simple/price";
                string coinId = "traaitt"; // Replace with actual CoinGecko ID for traaitt
                string vsCurrency = "usd"; // USD as an example, you can change to other currencies

                // Parameters for the request
                string url = $"{apiUrl}?ids={coinId}&vs_currencies={vsCurrency}";

                // Send GET request to CoinGecko
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    // Parse response JSON
                    string responseBody = await response.Content.ReadAsStringAsync();
                    JObject coinData = JObject.Parse(responseBody);

                    // Extract necessary data
                    decimal price = (decimal)coinData[coinId][vsCurrency];

                    // Build embed response
                    var responseEmbed = new EmbedBuilder()
                        .WithTitle($"Current Price of {TrtlBotSharp.coinSymbol}")
                        .WithUrl(url)
                        .AddField("Current Price", $"{price} {vsCurrency.ToUpper()}");

                    // Send reply
                    await ReplyAsync("", false, responseEmbed.Build());
                }
                else
                {
                    await ReplyAsync($"Failed to retrieve data from CoinGecko API. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                await ReplyAsync($"An error occurred: {ex.Message}");
            }
        }

        [Command("mcap")]
        public async Task MarketCapAsync([Remainder]string Remainder = "")
        {
            try
            {
                // CoinGecko API endpoint for traaitt
                string apiUrl = "https://api.coingecko.com/api/v3/simple/price";
                string coinId = "traaitt"; // Replace with actual CoinGecko ID for traaitt
                string vsCurrency = "usd"; // USD as an example, you can change to other currencies

                // Parameters for the request
                string url = $"{apiUrl}?ids={coinId}&vs_currencies={vsCurrency}";

                // Send GET request to CoinGecko
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    // Parse response JSON
                    string responseBody = await response.Content.ReadAsStringAsync();
                    JObject coinData = JObject.Parse(responseBody);

                    // Extract necessary data
                    decimal price = (decimal)coinData[coinId][vsCurrency];
                    decimal supply = TrtlBotSharp.GetSupply(); // Assuming GetSupply() method exists

                    // Calculate market cap
                    decimal marketCap = price * supply;

                    // Build response
                    string responseMessage = $"{TrtlBotSharp.coinName}'s market cap is **{marketCap:C}** {vsCurrency.ToUpper()}";

                    // Send reply
                    await ReplyAsync(responseMessage);
                }
                else
                {
                    await ReplyAsync($"Failed to retrieve data from CoinGecko API. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                await ReplyAsync($"An error occurred: {ex.Message}");
            }
        }
    }
}
