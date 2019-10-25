using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using cts.web.core.Librs;
using Fast.Framework.Filters;
using Fast.Mapping;
using Fast.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fast.Web.Areas.Admin.Controllers
{
    public class SysUserController : AdminPrmController
    {
        private SysUserService _sysUserService;
        private SysRoleService _sysRoleService;
        private SysUserLoginService _sysUserLoginLogService;
        private ActivityLogService _activityLogService;
        private SysUserJwtService _sysUserJwtService;
        private IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        public SysUserController(SysUserService sysUserService,
            SysRoleService sysRoleService,
            SysUserLoginService sysUserLoginLogService,
            ActivityLogService activityLogService,
            SysUserJwtService sysUserJwtService,
            IMapper mapper)
        {
            _sysUserJwtService = sysUserJwtService;
            _sysUserLoginLogService = sysUserLoginLogService;
            _sysRoleService = sysRoleService;
            _sysUserService = sysUserService;
            _activityLogService = activityLogService;
            _mapper = mapper;
        }


        [Route("", Name = "userIndex")]
        public IActionResult UserIndex()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("data", Name = "userData"), PrmSpare("userIndex")]
        public IActionResult UserData(SysUserSearchArg arg)
        {
            var parms = Request.QueryString.ToTableParms();
            var pageList = _sysUserService.AdminSearch(arg, parms);

            if (pageList.Any())
            {
                foreach (var user in pageList)
                {
                    user.SysRoles = _sysRoleService.GetUserRoles(user.Id);
                }
            }
            var data = pageList.ToAjax();
            return Json(data);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("details", Name = "userDetails")]
        public IActionResult UserDetails(Guid id)
        {
            var user = _sysUserService.GetUserMapping(id);
            user.SysRoles = _sysRoleService.GetUserRoles(id);
            return PartialView(user);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="draw"></param>
        /// <returns></returns>
        [Route("dtl/loginlogs", Name = "userLoginLogs"), PrmSpare("userDetails")]
        public IActionResult UserLoginLogs(Guid id, int draw)
        {
            var list = _sysUserLoginLogService.GetLastUserLogins(id);
            DatatableModel<Sys_UserLoginMapping> data = new DatatableModel<Sys_UserLoginMapping>(list);
            data.draw = draw;
            return Json(data);
        }

        /// <summary>
        /// 获取操作的最新记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="draw"></param>
        [Route("dtl/activitylogs", Name = "userActivityLogs"), PrmSpare("userDetails")]
        public IActionResult ActivityLog(Guid id, int draw)
        {
            var list = _activityLogService.GetLastUserActivityLogs(id);
            DatatableModel<Sys_ActivityLogMapping> data = new DatatableModel<Sys_ActivityLogMapping>(list);
            data.draw = draw;
            return Json(data);
        }

        /// <summary>
        /// 获取登陆的JwtToken
        /// </summary>
        /// <param name="draw"></param>
        /// <param name="id"></param>
        [Route("dtl/userjwt", Name = "userJwtToken"), PrmSpare("userDetails")]
        public IActionResult UserJwtToken(Guid id, int draw)
        {
            var list = _sysUserJwtService.GetLastUserJwt(id);
            DatatableModel<Sys_UserJwtMapping> data = new DatatableModel<Sys_UserJwtMapping>(list);
            data.draw = draw;
            return Json(data);
        }

        /// <summary>
        /// 添加人员
        /// </summary>
        /// <returns></returns>
        [Route("edit", Name = "userEdit")]
        public IActionResult UserEdit(Guid? id)
        {
            Sys_UserMapping model = null;
            ViewBag.Roles = _sysRoleService.GetRoles();
            if (id.HasValue)
            {
                model = _sysUserService.GetUserMapping(id.Value);
                if (model != null)
                {
                    var userRoles = _sysRoleService.GetUserRoles();
                    model.UserRoles = userRoles.Where(o => o.UserId == id.Value).Distinct().ToList();
                }
            }
            if (model == null)
                model = new Sys_UserMapping();
            return PartialView(model);
        }

        [HttpPost("edit")]
        public IActionResult UserEdit(Sys_UserMapping SysUser,List<Guid> RoleIds)
        {
            if (!ModelState.IsValid)
                return NotValid();
            (bool Status, string Message) res;
            var item = _mapper.Map<Entities.Sys_User>(SysUser);

            if (SysUser.Id != Guid.Empty)
            {
                res = _sysUserService.UpdateUser(SysUser, UserId);
            }
            else
            {
                item.Account = item.Account.TrimSpace();
                item.Id = CombGuid.NewGuid();
                item.CreationTime = DateTime.Now;
                item.Creator = UserId;
                item.Salt = EncryptorHelper.CreateSaltKey();
                item.Password = (EncryptorHelper.GetMD5(item.Account + item.Salt));
                res = _sysUserService.AddUser(item);
            }
            AjaxData.Message = res.Message;
            AjaxData.Code = res.Status ? 0 : 2001;
            if (res.Status)
            {
                _sysRoleService.SetUserRoles(item.Id, RoleIds, UserId);
            }
            return Json(AjaxData);
        }










        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("roles", Name = "userRoles")]
        public IActionResult UserRoles(Guid id)
        {
            var user = _sysUserService.GetUserMapping(id);
            user.SysRoles = _sysRoleService.GetUserRoles(id);
            ViewBag.Roles = _sysRoleService.GetRoles();

            return PartialView(user);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="RoleIds"></param>
        /// <returns></returns>
        [HttpPost("roles")]
        public IActionResult UserRoles(Guid id, List<Guid> RoleIds)
        {
            _sysRoleService.SetUserRoles(id, RoleIds, UserId);
            AjaxData.Code = 0;
            AjaxData.Message = "保存成功";
            return Json(AjaxData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("delete", Name = "userDelete")]
        public IActionResult Delete(Guid id)
        {
            _sysUserService.Delete(id, UserId);
            try
            {
                _sysUserJwtService.CompelOut(id);
                _sysRoleService.SetUserRoles(id, null, UserId);
            }
            catch (Exception)
            {

            }
            AjaxData.Code = 0;
            AjaxData.Message = "删除成功";
            return Json(AjaxData);
        }


    }
}