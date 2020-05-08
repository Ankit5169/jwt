using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using OrgAPI.ViewModel;

namespace jwt.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        UserManager<IdentityUser> _userManager;
        SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        // POST: api/Account
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.UserName,
                    Email = model.Email
                };
                var userResult = await _userManager.CreateAsync(user, model.Password);
                if (userResult.Succeeded)
                { return Ok(user); }
            }
            return BadRequest(ModelState.Values);
        }
        // POST: api/Account
        [HttpPost("signIn")]
        public async Task<IActionResult> signIn(SignInViewModel model)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
                if (signInResult.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.UserName);

                    var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this-is-my-secret-key"));
                    var signingCredentails = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
                    var jwt = new JwtSecurityToken(signingCredentials: signingCredentails, expires: DateTime.Now.AddMinutes(30));

                    var obj = new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(jwt),
                        UserId = user.Id,
                        UserName = user.UserName
                    };
                    return Ok(obj);

                    //return Ok();
                }
            }
            return BadRequest(ModelState);
        }
        // POST: api/Account
        [HttpPost("signOut")]
        public async Task<IActionResult> signOut()
        {
            await _signInManager.SignOutAsync();
            return NoContent();
        }
    }
}
