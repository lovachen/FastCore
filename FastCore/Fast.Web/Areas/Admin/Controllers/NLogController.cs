using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fast.Framework;
using Fast.Mapping;
using Fast.Services;
using cts.web.core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fast.Framework.Filters;

namespace Fast.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("nlog")]
    public class NLogController : AdminPrmController
    {
        private SysNLogService _sysNLogService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysNLogService"></param>
        public NLogController(SysNLogService sysNLogService)
        {
            _sysNLogService = sysNLogService;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("",Name = "nLogIndex")]
        public IActionResult NLogIndex()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("data", Name = "nLogData"),PrmSpare("nLogIndex")]
        public IActionResult NLogData(NLogSearchArg arg)
        {
            var parameters = Request.QueryString.ToTableParms();
            var pageList = _sysNLogService.AdminSearch(arg, parameters);
            return Json(pageList.ToAjax());
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("details", Name = "nLogDetails")]
        public IActionResult NLogDetails(Guid id)
        { 
            return PartialView(_sysNLogService.GetNlog(id));
        }


    }
}