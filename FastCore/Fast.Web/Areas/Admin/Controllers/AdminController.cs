using Fast.Framework;
using Fast.Framework.Controllers;
using Fast.Framework.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;


namespace Fast.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Area("Admin")]
    [Route("admin/[controller]")]
    [SysUserAuth]
    public abstract class AdminController:WebBaseController
    {
        /// <summary>
        /// 
        /// </summary>
        protected Guid UserId { get; private set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var workContext = context.HttpContext.RequestServices.GetService<WorkContext>();
            this.UserId = workContext.GetUserId();
        }
         
        /// <summary>
        /// 
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        protected IActionResult Json((bool Status, string Message) res)
        {
            AjaxData.Code = res.Status ? 0 : 2001;
            AjaxData.Message = res.Message ?? "操作结束";
            return Json(AjaxData);
        } 



    }
}
