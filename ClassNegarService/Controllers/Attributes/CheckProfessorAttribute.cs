using System;
using ClassNegarService.Db;
using ClassNegarService.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ClassNegarService.Controllers.Attributes
{
    public class CheckProfessorAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            var claim = filterContext.HttpContext.User.Claims.Where(x => x.Type == "role_id").FirstOrDefault();
            if (claim != null && claim.Value != ((int)RoleEnum.professor).ToString())
                filterContext.Result = new UnauthorizedResult();


        }

    }
}

