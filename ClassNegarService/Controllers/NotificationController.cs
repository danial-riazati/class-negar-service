using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassNegarService.Controllers.Attributes;
using ClassNegarService.Models;
using ClassNegarService.Models.Notification;
using ClassNegarService.Services;
using ClassNegarService.Services.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ClassNegarService.Controllers
{
    [Authorize]
    [Route("api/notification")]
    public class NotificationController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly INotificationService _notificationService;

        public NotificationController(
            IConfiguration configuration,
            INotificationService notificationService
            )
        {

            _configuration = configuration;
            _notificationService = notificationService;
        }

        [HttpPost]
        [Route("addnotification")]
        [CheckProfessor]
        public async Task<IActionResult> AddNotification([FromBody] AddNotificationModel model)
        {

            try
            {
                var professorId = getUserId() ?? throw new UnauthorizedAccessException();
                await _notificationService.AddNotification(model, professorId);

                return Ok(new ResponseModel<string?>
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

        [HttpPost]
        [Route("addcomment")]
        public async Task<IActionResult> AddComment([FromBody] AddCommentModel model)

        {

            try
            {
                var userId = getUserId() ?? throw new UnauthorizedAccessException();
                var userRole = getUserRole() ?? throw new UnauthorizedAccessException();

                await _notificationService.AddComment(model, userId, userRole);

                return Ok(new ResponseModel<string?>
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

        [HttpGet]
        [Route("notification/{notificationId}")]
        public async Task<IActionResult> Notification(int notificationId)
        {

            try
            {
                var userId = getUserId() ?? throw new UnauthorizedAccessException();
                var userRole = getUserRole() ?? throw new UnauthorizedAccessException();

                var result = await _notificationService.GetNotification(userId, userRole, notificationId);

                return Ok(new ResponseModel<NotificationResultModel>
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

        [HttpGet]
        [Route("notificationlikes/{notificationId}")]
        public async Task<IActionResult> NotificationLikes(int notificationId)
        {

            try
            {
                var userId = getUserId() ?? throw new UnauthorizedAccessException();
                var userRole = getUserRole() ?? throw new UnauthorizedAccessException();

                var result = await _notificationService.GetNotificationLikes(userId, userRole, notificationId);

                return Ok(new ResponseModel<List<string>>
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

        [HttpGet]
        [Route("notificationdislikes/{notificationId}")]
        public async Task<IActionResult> NotificationDislikes(int notificationId)
        {

            try
            {
                var userId = getUserId() ?? throw new UnauthorizedAccessException();
                var userRole = getUserRole() ?? throw new UnauthorizedAccessException();

                var result = await _notificationService.GetNotificationDislikes(userId, userRole, notificationId);

                return Ok(new ResponseModel<List<string>?>
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
        [Route("addlike/{notificationId}")]
        public async Task<IActionResult> NotificationLike(int notificationId)
        {

            try
            {
                var userId = getUserId() ?? throw new UnauthorizedAccessException();
                var userRole = getUserRole() ?? throw new UnauthorizedAccessException();

                await _notificationService.AddLike(userId, userRole, notificationId);

                return Ok(new ResponseModel<string?>
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
        [HttpPost]
        [Route("adddislike/{notificationId}")]
        public async Task<IActionResult> NotificationDislike(int notificationId)
        {

            try
            {
                var userId = getUserId() ?? throw new UnauthorizedAccessException();
                var userRole = getUserRole() ?? throw new UnauthorizedAccessException();

                await _notificationService.AddDislike(userId, userRole, notificationId);

                return Ok(new ResponseModel<string?>
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
        [HttpPost]
        [Route("removelike/{notificationId}")]
        public async Task<IActionResult> RemoveNotificationLike(int notificationId)
        {

            try
            {
                var userId = getUserId() ?? throw new UnauthorizedAccessException();
                var userRole = getUserRole() ?? throw new UnauthorizedAccessException();

                await _notificationService.RemoveLike(userId, userRole, notificationId);

                return Ok(new ResponseModel<string?>
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
        [HttpPost]
        [Route("removedislike/{notificationId}")]
        public async Task<IActionResult> RemoveNotificationDislike(int notificationId)
        {

            try
            {
                var userId = getUserId() ?? throw new UnauthorizedAccessException();
                var userRole = getUserRole() ?? throw new UnauthorizedAccessException();

                await _notificationService.RemoveDislike(userId, userRole, notificationId);

                return Ok(new ResponseModel<string?>
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
        private string? getUserName()
        {
            var claim = HttpContext.User.Claims.Where(x => x.Type == "username").FirstOrDefault();
            if (claim == null)
                return null;
            return claim.Value;
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

