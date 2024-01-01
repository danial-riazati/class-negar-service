using ClassNegarService.Models;
using ClassNegarService.Models.Report;
using ClassNegarService.Services.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassNegarService.Controllers
{
    [Authorize]
    [Route("api/report")]
    public class ReportController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IReportService _reportService;

        public ReportController(
            IConfiguration configuration,
            IReportService reportService
            )
        {

            _configuration = configuration;
            _reportService = reportService;
        }

        [HttpGet]
        [Route("classattendance/{classid}")]
        public async Task<IActionResult> GetClassAttendance(int studentid, int classid)
        {

            try
            {
                var userId = GetUserId() ?? throw new UnauthorizedAccessException();
                var userRole = GetUserRole() ?? throw new UnauthorizedAccessException();

                var result = await _reportService.GetClassAttendance(classid, userId, userRole);

                return Ok(new ResponseModel<ClassAttendanceResultModel?>
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


        private int? GetUserId()
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

        private int? GetUserRole()
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

