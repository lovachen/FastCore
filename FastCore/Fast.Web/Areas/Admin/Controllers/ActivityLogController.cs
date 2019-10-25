using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fast.Framework;
using Fast.Mapping;
using Fast.Services;
using cts.web.core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Fast.Framework.Filters;

namespace Fast.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("activitylog")]
    public class ActivityLogController : AdminPrmController
    {
        private ActivityLogService _activityLogService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityLogService"></param>
        public ActivityLogController(ActivityLogService activityLogService)
        {
            _activityLogService = activityLogService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("",Name = "activityLogIndex")]
        public IActionResult ActivityLogIndex()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("data",Name = "activityLogData"), PrmSpare("activityLogIndex")]
        public IActionResult ActivityLogData(ActivityLogSearchArg arg)
        { 
            var parameters = Request.QueryString.ToTableParms();
            var pageList = _activityLogService.AdminSearch(arg, parameters);
            var data = pageList.ToAjax();
            return Json(data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("details",Name = "activityLogDetails")]
        public IActionResult ActivityLogDetails(Guid id)
        {
            var item = _activityLogService.GetActivityLogMapping(id);
            NewJsonValu(item);
            return PartialView(item);
        }

        /// <summary>
        /// 序列化对象
        /// </summary>
        private void NewJsonValu(Sys_ActivityLogMapping activityLog)
        {
            try
            {
                if (!String.IsNullOrEmpty(activityLog.OldValue))
                {
                    ViewBag.OldObject = (JObject)JsonConvert.DeserializeObject(activityLog.OldValue);
                }
                if (!String.IsNullOrEmpty(activityLog.NewValue))
                {
                    ViewBag.NewObject = (JObject)JsonConvert.DeserializeObject(activityLog.NewValue);
                }
            }
            catch (Exception)
            {

            }
        }

    }
}