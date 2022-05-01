using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ApiAndIS4.Models;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiAndIS4.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<AccountController> logger;

        public AccountController(UserManager<ApplicationUser> userManager, ILogger<AccountController> logger)
        {
            this.userManager = userManager;
            this.logger = logger;
            logger.LogInformation($"Tet");

        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            logger.LogInformation($"Registering {registerModel.FullName}");
            var theUser = new ApplicationUser
            {
                UserName = registerModel.Email,
                Email = registerModel.Email,
                EmailConfirmed = true,
            };

            var result = await userManager.CreateAsync(theUser, registerModel.Password);
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = await userManager.AddClaimsAsync(theUser, new Claim[]{
                            new Claim(JwtClaimTypes.Name, registerModel.FullName),
                            new Claim(JwtClaimTypes.GivenName, registerModel.FirstName),
                            new Claim(JwtClaimTypes.FamilyName, registerModel.LastName),
                            new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                        });

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            return Ok();
        }
    }
}

