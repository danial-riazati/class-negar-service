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

        [CheckProfessor]
        [HttpPost]
        [Route("addclass")]

        public async Task<IActionResult> AddClass([FromBody] AddClassModel model)
        {

            try
            {

                await _classService.AddClass(model);


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

    }
}

