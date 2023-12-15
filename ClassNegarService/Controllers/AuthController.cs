
using ClassNegarService.Models;
using ClassNegarService.Models.Auth;
using ClassNegarService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassNegarService.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {

        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;

        public AuthController(
            IConfiguration configuration,
            IAuthService authService
            )
        {

            _configuration = configuration;
            _authService = authService;
        }


        [HttpPost]
        [Route("signin")]

        public async Task<IActionResult> Signin([FromBody] SigninModel model)
        {
            try
            {
                var result = await _authService.Signin(model);

                if (result == null)
                    return Unauthorized(
                        new ResponseModel<string>
                        {
                            Result = "",
                            Message = "username or password is incorrect"
                        });

                return Ok(new ResponseModel<SigninResponseModel>
                {
                    Result = result,
                    Message = "done"
                });

            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseModel<string>
                {
                    Result = "",
                    Message = ex.Message + ex.InnerException
                });
            }

        }


        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            try
            {
                var result = await _authService.RefreshToken(tokenModel);
                if (result == null)
                    return BadRequest(
                        new ResponseModel<string>
                        {
                            Result = "",
                            Message = ""
                        });

                return Ok(new ResponseModel<RefreshTokenModel>
                {
                    Result = result,
                    Message = "done"
                });
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseModel<string>
                {
                    Result = "",
                    Message = ex.Message + ex.InnerException
                });
            }

        }

        [Authorize]
        [HttpPost]
        [Route("revoke")]
        public async Task<IActionResult> Revoke()
        {
            try
            {
                var username = getUserName();
                if (username == null)
                    return Unauthorized();

                await _authService.Revoke(username);

                return Ok(new ResponseModel<string>
                {
                    Result = "",
                    Message = "done"
                });
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseModel<string>
                {
                    Result = "",
                    Message = ex.Message + ex.InnerException
                });
            }

        }


        private string? getUserName()
        {
            var claim = HttpContext.User.Claims.Where(x => x.Type == "username").FirstOrDefault();
            if (claim == null)
                return null;
            return claim.Value;
        }



    }
}

