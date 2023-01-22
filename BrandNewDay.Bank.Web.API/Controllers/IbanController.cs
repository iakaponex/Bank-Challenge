using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IbanNet;
using IbanNet.Registry;

namespace BrandNewDay.Bank.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IbanController : ControllerBase
    {
        public async Task<IActionResult> Get(string countryCode)
        {
            Iban iban;
            string result;

            var generator = new IbanGenerator();

            try
            {
                iban = generator.Generate(countryCode);
                result = iban.ToString();
            }
            catch (ArgumentException)
            {
                return BadRequest("Invalid country code.");
            }

            return Ok(result);
        }
    }
}
