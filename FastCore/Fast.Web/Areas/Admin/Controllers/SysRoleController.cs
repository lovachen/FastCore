using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fast.Framework;
using Fast.Mapping;
using Fast.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fast.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("role")]
    public class SysRoleController : AdminPrmController
    {
        private SysRoleService _sysRoleService;
        private SysUserService _sysUserService;
        private SysCategoryService _sysCategoryService;
        private IMapper _mapper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysRoleService"></param>
        /// <param name="sysUserService"></param>
        /// <param name="sysCategoryService"></param>
        /// <param name="mapper"></param>
        public SysRoleController(SysRoleService sysRoleService,
            SysUserService sysUserService,
            SysCategoryService sysCategoryService,
            IMapper mapper)
        {
            _sysCategoryService = sysCategoryService;
            _sysUserService = sysUserService;
            _mapper = mapper;
            _sysRoleService = sysRoleService;
        }

        /// <summary>
        /// 角色列表
        /// </summary>
        /// <returns></returns>
        [Route("", Name = "roleIndex")]
        public IActionResult RoleIndex()
        {
            return View(_sysRoleService.GetRoles());
        }

        /// <summary>
        /// 查看角色详情
        /// </summary>
        /// <returns></returns>
        [Route("users", Name = "roleUsers")]
        public IActionResult RoleUsers(Guid id)
        {
            var role = _sysRoleService.GetRoleMapping(id);
            role.SysUsers = _sysUserService.GetRoleUsers(id);
            return PartialView(role);
        }

        /// <summary>
        /// 移除角色关联的用户
        /// </summary>
        /// <returns></returns>
        [Route("del/users", Name = "deleteRoleUsers")]
        public IActionResult DeleteRoleUsers(Guid id, Guid roleId)
        {
            _sysRoleService.DeleteUserRole(id, roleId, UserId);
            AjaxData.Code = 0;
            AjaxData.Message = "删除成功";
            return Json(AjaxData);
        }


        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <returns></returns>
        [Route("edit", Name = "roleEdit")]
        public IActionResult RoleEdit(Guid? id)
        {
            if (id.HasValue)
            {
                var role = _sysRoleService.GetRoleMapping(id.Value);
                return PartialView(role);
            }
            return PartialView();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Role"></param>
        /// <returns></returns>
        [HttpPost("edit")]
        public IActionResult RoleEdit(Sys_RoleMapping Role)
        {
            if (!ModelState.IsValid)
                return NotValid();
            (bool Status, string Message) res;
            if (Role.Id != Guid.Empty)
            {
                res = _sysRoleService.UpdateRole(Role, UserId);
            }
            else
            {
                var item = _mapper.Map<Entities.Sys_Role>(Role);
                item.Id = CombGuid.NewGuid();
                item.CreationTime = DateTime.Now;
                item.Creator = UserId;
                res = _sysRoleService.AddRole(item);
            }
            AjaxData.Message = res.Message;
            AjaxData.Code = res.Status ? 0 : 2001;
            return Json(AjaxData);
        }

        /// <summary>
        /// 设置角色权限
        /// </summary>
        /// <returns></returns>
        [Route("prm", Name = "rolePrm")]
        public IActionResult RolePrm(Guid id)
        {
            var role = _sysRoleService.GetRoleMapping(id);
            var categories = _sysCategoryService.GetAllCache();
            ViewBag.Categories = categories.Where(o => o.Target == "0").ToList();
            role.SysPermissions = _sysRoleService.GetRolePermissons().Where(o => o.RoleId == id).ToList();
            return PartialView(role);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="categoryIds"></param>
        /// <returns></returns>
        [HttpPost("prm")]
        public IActionResult RolePrm(Guid id, List<Guid> categoryIds)
        {
            var res = _sysRoleService.SetRolePermission(id, categoryIds, UserId);
            AjaxData.Message = res.Message;
            AjaxData.Code = res.Status ? 0 : 2001;
            return Json(AjaxData);
        }


        /// <summary>
        /// 删除角色
        /// </summary>
        /// <returns></returns>
        [Route("delete", Name = "roleDelete")]
        public IActionResult Delete(Guid id)
        {
            var res = _sysRoleService.DeleteRole(id, UserId);
            AjaxData.Message = res.Message;
            AjaxData.Code = res.Status ? 0 : 2001;
            return Json(AjaxData);
        }

    }
}