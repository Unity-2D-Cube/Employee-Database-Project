
namespace Test_Projekat_Web.Models
{
    public class ExchangeRateProvider
    {
        public ConversionRate? Rate { get; private set; }

        private const string ApiUrl = "https://v6.exchangerate-api.com/v6/cfd41856c3411698f03a4ece/latest/RSD"; //Currency you want to exchange salary in this example RSD. Otherwise Will return 404 error if you put ".../latest/{0}"

        public ExchangeRateProvider()
        {
        }

        public async Task UpdateRatesAsync(string foreignCurrency = "USD", string foreignCurrency_02 = "EUR") //Update Rates for USD & EUR
        {
            try
            {
                using var httpClient = new HttpClient();
                API_Obj? jsonResponse = await httpClient.GetFromJsonAsync<API_Obj>(string.Format(ApiUrl, foreignCurrency, foreignCurrency_02));
                Rate = jsonResponse?.conversion_rates;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to download Exchange API data.", ex);
            }
        }

        public class API_Obj
        {
            public string? result { get; set; }
            public string? documentation { get; set; }
            public string? terms_of_use { get; set; }
            public long? time_last_update_unix { get; set; }
            public string? time_last_update_utc { get; set; }
            public long? time_next_update_unix { get; set; }
            public string? time_next_update_utc { get; set; }
            public string? base_code { get; set; }
            public ConversionRate? conversion_rates { get; set; }
        }

        public class ConversionRate
        {
            public double EUR { get; set; }
            public double USD { get; set; }
        }
    }
}
