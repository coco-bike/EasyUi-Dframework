using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using IService;
using Model;

namespace UI.Areas.Admin.Controllers
{
    public class RoleAuthorityController : Controller
    {
        //
        // GET: /Admin/RoleAuthority/
        #region 页面
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult addRole()
        {
            return View();
        }
        public ActionResult addAuthority()
        {
            return View();
        }
        public ActionResult addRoleAuthority()
        {
            return View();
        }
        #endregion

        #region 初始化
        readonly IRoleAdminService _roleService;
        readonly IAuthorityAdminService _authorityService;

        #region 角色
        public RoleAuthorityController(IRoleAdminService roleService, IAuthorityAdminService authorityService)
        {
            this._roleService = roleService;
            this._authorityService = authorityService;
        }
        #endregion

        /// <summary>
        /// 分页查询和单个查询
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="rolename"></param>
        /// <returns></returns>
        public JsonResult GetRoleListData(string page,string rows,string rolename)
        {
            int totalCount;
            var pageIndex=Convert.ToInt32(page);
            var pageSize = Convert.ToInt32(rows);
            if(rolename==null||rolename==""){
                var rolelist = this._roleService.GetPagingList(pageIndex, pageSize, out totalCount, true, s => s.State == 1, s => s.Id).Select(t => new { t.Id, t.RoleName, t.Description, t.UpdateTime, t.BuildTime }).ToList();
                return Json(new { total = totalCount, rows = rolelist });
            }
            else
            {
                totalCount = 1;
                var rolelist = this._roleService.GetList(s => s.RoleName == rolename&&s.State==1).ToList();
                return Json(new { total = totalCount, rows = rolelist });
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="rolemodel"></param>
        /// <returns></returns>
        public JsonResult UpdateRoleData(RoleModel rolemodel)
        {
            var role = this._roleService.GetList(s => s.RoleName == rolemodel.RoleName&&s.State==1).FirstOrDefault();
            if (role == null)
            {
                return Json(ResultStatus.Fail);
            }
            role.Description = rolemodel.Description;
            role.UpdateTime = DateTime.Now.ToString();
            var res = this._roleService.Update(role);
            if (res > 0)
            {
                return Json(ResultStatus.Success);
            }
            return Json(ResultStatus.Fail);
        }

        /// <summary>
        /// 保存单个信息
        /// </summary>
        /// <param name="rolemodel"></param>
        /// <returns></returns>
        public JsonResult SaveRoleData(RoleModel rolemodel)
        {
            var rolelist = this._roleService.GetList(s => s.RoleName == rolemodel.RoleName&&s.State==1).ToList();
            if (rolelist.Count > 0)
            {
                return Json(ResultStatus.Fail);
            }
            var role1 = new RoleModel();
            role1 = rolemodel;
            role1.UpdateTime = DateTime.Now.ToString();
            role1.BuildTime = DateTime.Now.ToString();
            this._roleService.Add(role1);
            return Json(ResultStatus.Success);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DestroyRole(int id)
        {
            var role = this._roleService.GetList(s => s.Id == id).FirstOrDefault();
            if (role == null)
            {
                return Json(ResultStatus.Fail);
            }
            int result = this._roleService.DeleteFake(t => t.Id == id, t => new RoleModel() { State = 0 });
            if (result > 0)
            {
                return Json(ResultStatus.Success);
            }
            return Json(ResultStatus.Fail);
        }
        #endregion

        #region 权限
        public JsonResult GetAuthorityListData(string page, string rows, string name)
        {
            int totalCount;
            var pageIndex = Convert.ToInt32(page);
            var pageSize = Convert.ToInt32(rows);
            if (name == null||name=="")
            {
                var authoritylist = this._authorityService.GetPagingList(pageIndex, pageSize, out totalCount, true, s => s.State == 1, s => s.Id).Select(s => new { s.Id, s.Name, s.State, s.Type, s.UpdateTime, s.Description ,s.BuildTime}).ToList();
                return Json(new { total = totalCount, rows = authoritylist });
            }
            else
            {
                totalCount = 1;
                var authoritylist = this._authorityService.GetList(s => s.Name == name && s.State == 1).ToList();
                return Json(new { total = totalCount, rows = authoritylist });

            }
        }
        public JsonResult UpdateAuthorityData(AuthorityModel authoritydata)
        {
            var authority = this._authorityService.GetList(s => s.Name == authoritydata.Name).FirstOrDefault();
            if (authority == null)
            {
                return Json(ResultStatus.Fail);
            }
            authority.Description = authoritydata.Description;
            authority.Type = authoritydata.Type;
            authority.UpdateTime = DateTime.Now.ToString();
            var res = this._authorityService.Update(authority);
            if (res > 0)
            {
                return Json(ResultStatus.Success);
            }
            return Json(ResultStatus.Fail);
        }

        public JsonResult SaveAuthorityData(AuthorityModel authoritydata)
        {
            var authority = this._authorityService.GetList(t => t.Name == authoritydata.Name && t.State == 1).ToList();
            if (authority.Count > 0)
            {
                return Json(ResultStatus.Fail);
            }
            var authority1 = new AuthorityModel();
            authority1 = authoritydata;
            authority1.BuildTime = DateTime.Now.ToString();
            authority1.UpdateTime = DateTime.Now.ToString();
            this._authorityService.Add(authority1);
            return Json(ResultStatus.Success);
        }

        public JsonResult DestroyAuthority(int id)
        {
            var authority = this._authorityService.GetList(s => s.Id == id).FirstOrDefault();
            if (authority == null)
            {
                return Json(ResultStatus.Fail);
            }
            int result = this._authorityService.DeleteFake(t => t.Id == id, t => new AuthorityModel() { State = 0 });
            if (result > 0)
            {
                return Json(ResultStatus.Success);
            }
            return Json(ResultStatus.Fail);
        }
        #endregion
    }
}
