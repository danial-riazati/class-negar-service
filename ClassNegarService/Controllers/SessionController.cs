using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassNegarService.Controllers.Attributes;
using ClassNegarService.Models;
using ClassNegarService.Models.Notification;
using ClassNegarService.Models.Session;
using ClassNegarService.Services.Notification;
using ClassNegarService.Services.Session;
using ClassNegarService.Services.WebSocket;
using ClassNegarService.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassNegarService.Controllers
{
    [Authorize]
    [Route("api/session")]
    public class SessionController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ISessionService _sessionService;

        public SessionController(
            IConfiguration configuration,
            ISessionService sessionService
            )
        {

            _configuration = configuration;
            _sessionService = sessionService;
        }

        [HttpGet]
        [Route("sessionqr/{classId}/{isLogin}")]
        public async Task<IActionResult> GetSessionQr(int classId, bool isLogin)
        {

            try
            {
                var userId = getUserId() ?? throw new UnauthorizedAccessException();
                var userRole = getUserRole() ?? throw new UnauthorizedAccessException();

                var result = await _sessionService.GetSessionQr(classId, userId, userRole, isLogin);

                return Ok(new ResponseModel<SessionQrResultModel>
                {
                    Result = result,
                    Message = "done"
                });

            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ResponseModel<string>
                {
                    Result = "",
                    Message = ex.Message + ex.InnerException
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
        [Route("joinsession")]
        public async Task<IActionResult> JoinSession([FromBody] string qrcode)
        {

            try
            {
                await _sessionService.JoinSession(qrcode);

                return Ok(new ResponseModel<string>
                {
                    Result = "",
                    Message = "done"
                });

            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ResponseModel<string>
                {
                    Result = "",
                    Message = ex.Message + ex.InnerException
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

        private int? getUserId()
        {
            int id;
            var claim = HttpContext.User.Claims.Where(x => x.Type == "user_id").FirstOrDefault();
            if (claim == null)
                return null;
            try
            {
                id = int.Parse(claim.Value);
            }
            catch (Exception)
            {
                return null;
            }

            return id;
        }

        private int? getUserRole()
        {
            int id;
            var claim = HttpContext.User.Claims.Where(x => x.Type == "role_id").FirstOrDefault();
            if (claim == null)
                return null;
            try
            {
                id = int.Parse(claim.Value);
            }
            catch (Exception)
            {
                return null;
            }

            return id;
        }
    }
}

