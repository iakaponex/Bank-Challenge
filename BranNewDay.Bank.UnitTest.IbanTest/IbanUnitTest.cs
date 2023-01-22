using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BranNewDay.Bank.UnitTest.IbanTest
{
    [TestClass]
    public partial class IbanUnitTest
    {
        Dictionary<string, string> isoDict = null;
        public IbanUnitTest()
        {
            //read posible country code from json file (source: http://randomiban.com/static/mapp4.js)
            using (StreamReader reader = new StreamReader("iso_abbreviations.json"))
            {
                string json = reader.ReadToEnd();
                isoDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }
        }

        [TestMethod]
        public void TestGenerateIban()
        {
            var isValid = true;
            string message = string.Empty, testCountryCode = string.Empty;

            try
            {
                foreach (var countryCode in isoDict.Values)
                {
                    testCountryCode = countryCode;

                    //test call web api
                    var ibanAccNo = GetNewIbanAccountFromAPI(testCountryCode);
                    isValid &= !string.IsNullOrEmpty(ibanAccNo.Result);
                }
            }
            catch(Exception ex)
            {
                message = $"[{testCountryCode}] - {ex.Message}";
                isValid = false;
            }

            Assert.IsTrue(isValid, message);
        }

        protected async Task<string> GetNewIbanAccountFromAPI(string code)
        {
            var client = new HttpClient();

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://localhost:7127/api/iban?countryCode={code}"),
                Method = HttpMethod.Get
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (var response = await client.SendAsync(request))
            {
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
