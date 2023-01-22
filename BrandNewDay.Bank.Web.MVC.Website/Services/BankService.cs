using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace BrandNewDay.Bank.Web.MVC.Website.Services
{
    public class BankService
    {
        private readonly IWebHostEnvironment _host;
        public string _webApiUrl { get; set; }
        protected string _rootPath { get; set; }

        /// <summary>
        /// Contructor initial data
        /// </summary>
        /// <param name="apiUrl">Web API Url</param>
        public BankService(IWebHostEnvironment host)
        {
            _host = host;
            _rootPath = _host.WebRootPath;
        }

        /// <summary>
        /// Get new Iban number
        /// </summary>
        /// <param name="countryCode">ISO Country Code</param>
        /// <returns></returns>
        public async Task<string> GetNewIban(string countryCode)
        {
            var client = new HttpClient();

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_webApiUrl}api/iban?countryCode={countryCode}"),
                Method = HttpMethod.Get
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (var response = await client.SendAsync(request))
            {
                string result = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(result))
                    result = result.Replace(@"""", "");

                return result;
            }
        }

        public async Task<bool> CreateDefaultBankAccount(string id)
        {
            var client = new HttpClient();

            var parameters = new Dictionary<string, string> { { "id", id } };
            var encodedContent = new FormUrlEncodedContent(parameters);


            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_webApiUrl}api/BankAccount"),
                Method = HttpMethod.Post,
                Content = encodedContent
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json") { CharSet = "utf8" });

            using (var response = await client.SendAsync(request))
            {
                string result = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(result))
                    result = result.Replace(@"""", "");

                return result.Equals("OK", StringComparison.InvariantCultureIgnoreCase);
            }
        }

        /// <summary>
        /// call api for deposit money
        /// </summary>
        /// <param name="iban"></param>
        /// <param name="amount"></param>
        /// <returns>Account Balance (update)</returns>
        public async Task<decimal> Deposit(string iban, decimal amount)
        {
            decimal balance;
            var client = new HttpClient();

            var parameters = new Dictionary<string, string> { { "iban", iban }, { "amount", amount.ToString("F3") } };
            var encodedContent = new FormUrlEncodedContent(parameters);


            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_webApiUrl}api/Deposit"),
                Method = HttpMethod.Post,
                Content = encodedContent
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json") { CharSet = "utf8" });

            using (var response = await client.SendAsync(request))
            {
                string result = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(result))
                    result = result.Replace(@"""", "");

                if (decimal.TryParse(result, out balance))
                    return balance;
                else
                    throw new Exception(result);
            }
        }

        /// <summary>
        /// call api for transfer money
        /// </summary>
        /// <param name="sourceIban"></param>
        /// <param name="toIban"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<decimal> Transfer(string sourceIban, string toIban, decimal amount)
        {
            decimal balance;
            var client = new HttpClient();

            var parameters = new Dictionary<string, string> { { "sourceIban", sourceIban }, { "toIban", toIban }, { "amount", amount.ToString("F3") } };
            var encodedContent = new FormUrlEncodedContent(parameters);


            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_webApiUrl}api/Transfer"),
                Method = HttpMethod.Post,
                Content = encodedContent
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json") { CharSet = "utf8" });

            using (var response = await client.SendAsync(request))
            {
                string result = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(result))
                    result = result.Replace(@"""", "");

                if (decimal.TryParse(result, out balance))
                    return balance;
                else
                    throw new Exception(result);
            }
        }


        public Dictionary<string, string> GetAvailableCountries()
        {
            //read posible country code from json file (source: http://randomiban.com/static/mapp4.js)
            using (StreamReader reader = new StreamReader($"{_rootPath}/dist/data/iso_abbreviations.json"))
            {
                string json = reader.ReadToEnd();
                var isoDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                return isoDict;
            }
        }
    }
}
