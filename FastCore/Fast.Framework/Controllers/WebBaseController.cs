using cts.web.core.Model;
using Fast.Framework.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fast.Framework.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [WebException(0)]
    public abstract class WebBaseController:BaseController
    {

        /// <summary>
        /// ajax请求返回结果
        /// </summary> 
        protected AjaxResult AjaxData = new AjaxResult() { Code = -1, Message = "未知信息" };


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected IActionResult NotValid()
        {
            AjaxData.Code = 1005;
            AjaxData.Message = ModelState.GetErrMsg();
            return new JsonResult(AjaxData);
        }
    }
}
