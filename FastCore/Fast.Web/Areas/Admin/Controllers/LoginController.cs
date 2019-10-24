using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fast.Framework.Controllers;
using Fast.Framework.Security;
using Fast.Services;
using Fast.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fast.Web.Areas.Admin.Controllers
{

    /// <summary>
    /// 
    /// </summary>
    [Route("admin/login")]
    public class LoginController : WebBaseController
    {
        private SysUserService _sysUserService;
        private SysUserAuthentication _sysUserAuthentication;

        public LoginController(SysUserService sysUserService,
            SysUserAuthentication sysUserAuthentication)
        {
            _sysUserService = sysUserService;
            _sysUserAuthentication = sysUserAuthentication;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("",Name ="adminLogin")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("")]
        public IActionResult DoLogin(ViewLoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return NotValid();
            }
            var res = _sysUserService.ValidateUser(model.Account, model.Password, 0);
            AjaxData.Message = res.Message;
            AjaxData.Code = res.Status ? 0 : 2001;
            if (res.Status)
            {
                _sysUserAuthentication.SignIn(res.Jwt.Jti, res.User, res.Jwt.Expiration);
                AjaxData.Result = new { Url = Url.RouteUrl("mainIndex") };
            }
            return Json(AjaxData);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpGet("salt",Name = "getSalt")]
        public IActionResult GetSalt(string account)
        {
            var item = _sysUserService.GetSalt(account);
            AjaxData.Code = 0;
            AjaxData.Result = new { Salt = item.Salt ?? "", R = item.R ?? "" };
            AjaxData.Message = "获取成功";
            return Json(AjaxData);
        }
    }
}