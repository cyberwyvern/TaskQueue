using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.RegularExpressions;
using TaskQueue.Authentication.API;
using TaskQueue.Authentication.Cryptography;
using TaskQueue.Authentication.DAO.Entities;
using TaskQueue.Authentication.DAO.Interface;

namespace TaskQueue.Authentication.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IDatabaseUnitOfWork db;
        private readonly JwtIssuerService jwt;
        private readonly IConfiguration config;

        public AuthController(IDatabaseUnitOfWork db, JwtIssuerService jwt, IConfiguration config)
        {
            this.db = db;
            this.jwt = jwt;
            this.config = config;
        }

        [HttpPost("login-or-register")]
        public IActionResult LoginOrRegister(AuthModel model)
        {
            User userEntity = db.Users.GetByUsername(model.Username);
            if (userEntity == null)
            {
                return Register(model);
            }

            string passwordHash = HashUtil.ComputeHash(RSAEncryptor.DecryptString(model.Password), userEntity.Salt);
            if (passwordHash != userEntity.PasswordHash)
            {
                return Unauthorized("Invalid password!");
            }

            return Ok(GenerateAccessToken(userEntity));
        }

        [HttpPost("register")]
        public IActionResult Register(AuthModel model)
        {
            if (db.Users.GetByUsername(model.Username) != null)
            {
                return Conflict("User with the same name already exists");
            }

            User userEntity = new User
            {
                Username = model.Username,
                Salt = HashUtil.GetRandomSalt()
            };

            var decryptedPassword = RSAEncryptor.DecryptString(model.Password);
            var regex = new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])[A-Za-z\d!@#$%^&*()_+=]{8,32}$");
            if (!regex.IsMatch(decryptedPassword))
            {
                ModelState.AddModelError(
                    nameof(model.Password),
                    "Password must contain 8 - 32 alpanumeric characters with both uppercase and lowercase");

                return BadRequest(ModelState);
            }

            userEntity.PasswordHash = HashUtil.ComputeHash(decryptedPassword, userEntity.Salt);

            try
            {
                db.StartTransaction();
                db.Users.Create(userEntity);
                db.Commit();
            }
            catch
            {
                db.Rollback();
                throw;
            }

            return Ok(GenerateAccessToken(userEntity));
        }

        private AccessToken GenerateAccessToken(User user)
        {
            return new AccessToken()
            {
                Access_token = jwt.GenerateToken(new List<Claim>
                {
                    new Claim("UserId", user.Id.ToString()),
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Username),
                }),
                Token_type = "bearer",
                Expires_in = int.Parse(config["JwtSettings:LifetimeSeconds"])
            };
        }
    }
}