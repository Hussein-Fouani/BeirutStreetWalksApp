using Azure;
using BeirutWalksDomains.ApiResponse;
using BeirutWalksDomains.Dto;
using BeirutWalksWebApi.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;

namespace BeirutWalksWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> manager;
        private readonly ITokenRepository tokenRepository;
        protected APIResponse apiResponse;
        public AuthController(UserManager<IdentityUser> manager,IConfiguration configuration,ITokenRepository tokenRepository)
        {
            this.manager = manager;
            this.tokenRepository = tokenRepository;
            apiResponse =new APIResponse();
        }
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<APIResponse>> Register([FromBody]RegisterRequestDto register)
        {
            try
            {
                var user = new IdentityUser
                {
                    UserName = register.Username,
                    Email = register.Username,
                  
                };
                var result = await manager.CreateAsync(user, register.Password);
                if (result.Succeeded)
                {
                    if(register.Roles!=null && register.Roles.Any())
                    result=    await manager.AddToRolesAsync(user, register.Roles);

                    if (result.Succeeded)
                    {
                        apiResponse.IsSuccess=true;
                        apiResponse.StatusCode=HttpStatusCode.OK;
                        return Ok(apiResponse);
                    } 
                }
                var message = string.Join(", ", result.Errors.Select(x => "Code " + x.Code + " Description" + x.Description));

                apiResponse.IsSuccess = false;
                apiResponse.ErrorMessages = new List<string>{ message };

                apiResponse.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(apiResponse);
            }
            catch (Exception )
            {
                apiResponse.IsSuccess = false;
                apiResponse.ErrorMessages = new List<string> {  };

                apiResponse.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(apiResponse);
            }

        }

        [Route("login")]
        [HttpPost]
        public async Task<ActionResult<APIResponse>> login([FromBody] LoginRequestDto login)
        {
            var usr = await manager.FindByNameAsync(login.Username);
            
            if (usr != null)
            {
                var user =await manager.CheckPasswordAsync(usr, login.Password);
                if (user)
                {
                    var userroles = await manager.GetRolesAsync(usr);
                    if (userroles != null)
                    {
                      var token=  tokenRepository.CreateToken(usr,userroles.ToList());
                        return Ok( token );
                    }
                    

                }
            }
            apiResponse.IsSuccess = false;
            apiResponse.ErrorMessages = new List<string> { "Invalid Username or Password" };
            apiResponse.StatusCode = HttpStatusCode.BadRequest;

            return BadRequest(apiResponse);
        }
    }
}
