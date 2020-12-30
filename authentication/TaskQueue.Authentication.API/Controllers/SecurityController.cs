using Microsoft.AspNetCore.Mvc;
using TaskQueue.Authentication.Cryptography;

namespace TaskQueue.Authentication.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class SecurityController : Controller
    {
        [HttpGet("public-key/{algorithm}")]
        public IActionResult PublicKey(string algorithm)
        {
            return algorithm.ToUpper() switch
            {
                "RSA" => Ok(new
                {
                    Algorithm = "RSA",
                    Type = "public",
                    Key = RSAEncryptor.ExportPublicKey()
                }),
                _ => NotFound()
            };
        }
    }
}