using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassNegarService.Controllers.Attributes;
using ClassNegarService.Models;
using ClassNegarService.Models.Auth;
using ClassNegarService.Models.Class;
using ClassNegarService.Models.Enums;
using ClassNegarService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassNegarService.Controllers
{

    [Authorize]
    [Route("api/class")]
    public class ClassController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IClassService _classService;

        public ClassController(
            IConfiguration configuration,
            IClassService classService
            )
        {

            _configuration = configuration;
            _classService = classService;
        }

        [HttpPost]
        [Route("addclass")]
        [CheckProfessor]
        public async Task<IActionResult> AddClass([FromBody] AddClassModel model)
        {

            try
            {
                var professorId = getUserId() ?? throw new UnauthorizedAccessException();
                await _classService.AddClass(model, professorId);

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
        [Route("allprofessorclasses")]
        [CheckProfessor]
        public async Task<IActionResult> AllProfessorClasses()
        {

            try
            {
                var professorId = getUserId() ?? throw new UnauthorizedAccessException();
                var result = await _classService.GetAllProfessorClasses(professorId);

                return Ok(new ResponseModel<List<ProfessorClassesModel>>
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
        [Route("allstudentclasses")]
        [CheckStudent]
        public async Task<IActionResult> AllStudentClasses()
        {

            try
            {
                var studentId = getUserId() ?? throw new UnauthorizedAccessException();
                var result = await _classService.GetAllStudentClasses(studentId);

                return Ok(new ResponseModel<List<StudentClassesModel>>
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
        [Route("joinclass")]
        [CheckStudent]
        public async Task<IActionResult> JoinClass([FromBody] JoinClassModel model)
        {

            try
            {
                var studentId = getUserId() ?? throw new UnauthorizedAccessException();
                await _classService.JoinClass(model, studentId);

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



    }
}

