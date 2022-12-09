using Newtonsoft.Json;
namespace Test_Projekat_Web.Models
{
    public class ExchangeRate_API
    {
        class Rates
        {
            public static bool Import()
            {
                try
                {
                    String URLString = "https://v6.exchangerate-api.com/v6/cfd41856c3411698f03a4ece/latest/USD";
                    using (var webClient = new System.Net.WebClient())
                    {
                        var json = webClient.DownloadString(URLString);
                        API_Obj Test = JsonConvert.DeserializeObject<API_Obj>(json);
                        return true;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public class API_Obj
        {
            public string ? result { get; set; }
            public string ? documentation { get; set; }
            public string ? terms_of_use { get; set; }
            public string ? time_last_update_unix { get; set; }
            public string ? time_last_update_utc { get; set; }
            public string ? time_next_update_unix { get; set; }
            public string ? time_next_update_utc { get; set; }
            public string ? base_code { get; set; }
            public ConversionRate ? conversion_rates { get; set; }
        }

        public class ConversionRate
        {
            public double RSD { get; set; }
            public double EUR { get; set; }
            public double USD { get; set; }
        }

 
    }
}
