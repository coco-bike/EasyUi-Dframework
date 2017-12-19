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
        public JsonResult GetRoleListData(string page, string rows, string rolename)
        {
            int totalCount;
            var pageIndex = Convert.ToInt32(page);
            var pageSize = Convert.ToInt32(rows);
            if (rolename == null || rolename == "")
            {
                var rolelist = this._roleService.GetPagingList(pageIndex, pageSize, out totalCount, true, s => s.Status == 1, s => s.Id).Select(t => new { t.Id, t.RoleName, t.Description, t.UpdateTime, t.BuildTime, t.AuthorityModels }).ToList();
                List<RoleData> roledatalist = new List<RoleData>();
                for (int i = 0; i < rolelist.Count; i++)
                {
                    int count = 0;
                    RoleData roledata = new RoleData();
                    roledata.Id = rolelist[i].Id;
                    roledata.RoleName = rolelist[i].RoleName;
                    roledata.UpdateTime = rolelist[i].UpdateTime;
                    roledata.BuildTime = rolelist[i].BuildTime;
                    roledata.Description = rolelist[i].Description;
                    var authoritylist = rolelist[i].AuthorityModels.ToList();
                    if (authoritylist.Count == 0)
                    {
                        roledata.AuthorityModels = "无权限";
                    }
                    else
                    {
                        foreach (AuthorityModel j in authoritylist)
                        {
                            if (count == 0)
                            {
                                roledata.AuthorityModels = j.Name;
                            }
                            else
                            {
                                roledata.AuthorityModels = roledata.AuthorityModels + "," + j.Name;
                            }
                            count++;
                        }
                    }

                    roledatalist.Add(roledata);
                }
                return Json(new { total = totalCount, rows = roledatalist });
            }
            else
            {
                totalCount = 1;
                var rolelist = this._roleService.GetList(s => s.RoleName == rolename && s.Status == 1).ToList();
                if (rolelist.Count > 0)
                {
                    List<RoleData> roledatalist = new List<RoleData>();
                    RoleData roledata = new RoleData();
                    roledata.Id = rolelist[0].Id;
                    roledata.RoleName = rolelist[0].RoleName;
                    roledata.UpdateTime = rolelist[0].UpdateTime;
                    roledata.BuildTime = rolelist[0].BuildTime;
                    roledata.Description = rolelist[0].Description;
                    var authoritylist = rolelist[0].AuthorityModels.ToList();
                    if (authoritylist.Count == 0)
                    {
                        roledata.AuthorityModels = "无权限";
                    }
                    else
                    {
                        foreach (AuthorityModel j in authoritylist)
                        {
                            roledata.AuthorityModels = roledata.AuthorityModels + "," + j.Name;
                        }
                    }
                    roledatalist.Add(roledata);
                    return Json(new { total = totalCount, rows = roledatalist });
                }
                return Json(ResultStatus.Fail);
            }
        }
        public class RoleData
        {
            public int Id { get; set; }
            public string RoleName { get; set; }
            public string Description { get; set; }
            public string UpdateTime { get; set; }
            public string BuildTime { get; set; }
            public string AuthorityModels { get; set; }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="rolemodel"></param>
        /// <returns></returns>
        public JsonResult UpdateRoleData(RoleModel rolemodel)
        {
            var role = this._roleService.GetList(s => s.RoleName == rolemodel.RoleName && s.Status == 1).FirstOrDefault();
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
            var rolelist = this._roleService.GetList(s => s.RoleName == rolemodel.RoleName && s.Status == 1).ToList();
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
            int result = this._roleService.DeleteFake(t => t.Id == id, t => new RoleModel() { Status = 0 });
            if (result > 0)
            {
                return Json(ResultStatus.Success);
            }
            return Json(ResultStatus.Fail);
        }
        #endregion

        #region 权限
        /// <summary>
        /// 获取权限数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public JsonResult GetAuthorityListData(string page, string rows, string name)
        {
            int totalCount;
            var pageIndex = Convert.ToInt32(page);
            var pageSize = Convert.ToInt32(rows);
            if (name == null || name == "")
            {
                var authoritylist = this._authorityService.GetPagingList(pageIndex, pageSize, out totalCount, true, s => s.Status == 1, s => s.Id).Select(s => new { s.Id, s.Name, State = s.Status, s.Type, s.UpdateTime, s.Description, s.BuildTime }).ToList();
                return Json(new { total = totalCount, rows = authoritylist });
            }
            else
            {
                totalCount = 1;
                var authoritylist = this._authorityService.GetList(s => s.Name == name && s.Status == 1).ToList();
                return Json(new { total = totalCount, rows = authoritylist });

            }
        }
        /// <summary>
        /// 更新权限数据
        /// </summary>
        /// <param name="authoritydata"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 保存权限数据
        /// </summary>
        /// <param name="authoritydata"></param>
        /// <returns></returns>
        public JsonResult SaveAuthorityData(AuthorityModel authoritydata)
        {
            var authority = this._authorityService.GetList(t => t.Name == authoritydata.Name && t.Status == 1).ToList();
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
        /// <summary>
        /// 删除权限数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DestroyAuthority(int id)
        {
            var authority = this._authorityService.GetList(s => s.Id == id).FirstOrDefault();
            if (authority == null)
            {
                return Json(ResultStatus.Fail);
            }
            int result = this._authorityService.DeleteFake(t => t.Id == id, t => new AuthorityModel() { Status = 0 });
            if (result > 0)
            {
                return Json(ResultStatus.Success);
            }
            return Json(ResultStatus.Fail);
        }
        #endregion

        #region 绑定权限和解除权限
        /// <summary>
        /// 获取角色名称列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetRoleNameList()
        {
            var rolelist = this._roleService.GetList(s => s.Status == 1).Select(t => new { t.RoleName, t.Id }).ToList();
            if (rolelist == null)
            {
                return Json(ResultStatus.Fail);
            }
            List<object> a = new List<object>();
            for (int i = 0; i < rolelist.Count; i++)
            {
                if (i == 0)
                {
                    Default roledefault = new Default();
                    roledefault.id = rolelist[i].Id;
                    roledefault.name = rolelist[i].RoleName;
                    roledefault.desc = " ";
                    roledefault.selected = true;
                    a.Add(roledefault);
                }
                else
                {
                    SelectList list = new SelectList();
                    list.id = rolelist[i].Id;
                    list.name = rolelist[i].RoleName;
                    list.desc = " ";
                    a.Add(list);
                }

            }
            return Json(a, JsonRequestBehavior.AllowGet);
        }
        public class SelectList
        {
            public int id { get; set; }
            public string name { get; set; }
            public string desc { get; set; }
        }
        public class Default
        {
            public int id { get; set; }
            public string name { get; set; }
            public string desc { get; set; }
            public bool selected { get; set; }
        }
        /// <summary>
        /// 获取权限名称列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAuthorityNameList()
        {
            var authoritylist = this._authorityService.GetList(s => s.Status == 1).Select(t => new { t.Name, t.Id }).ToList();
            if (authoritylist == null)
            {
                return Json(ResultStatus.Fail);
            }
            List<object> a = new List<object>();
            for (int i = 0; i < authoritylist.Count; i++)
            {
                if (i == 0)
                {
                    Default authoritydefault = new Default();
                    authoritydefault.id = authoritylist[i].Id;
                    authoritydefault.name = authoritylist[i].Name;
                    authoritydefault.desc = " ";
                    authoritydefault.selected = true;
                    a.Add(authoritydefault);
                }
                else
                {
                    SelectList list = new SelectList();
                    list.id = authoritylist[i].Id;
                    list.name = authoritylist[i].Name;
                    list.desc = " ";
                    a.Add(list);
                }

            }
            return Json(a, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 绑定关系
        /// </summary>
        /// <param name="rolename"></param>
        /// <param name="authorityname"></param>
        /// <returns></returns>
        public JsonResult BindContact(string rolename, string authorityname)
        {
            var roleid = Convert.ToInt32(rolename);
            var authorityid = Convert.ToInt32(authorityname);
            var role = this._roleService.GetList(s => s.Id == roleid).FirstOrDefault();
            var authority = this._authorityService.GetList(s => s.Id == authorityid).FirstOrDefault();
            if (role != null && authority != null)
            {
                List<AuthorityModel> aList = new List<AuthorityModel>();
                aList = role.AuthorityModels.ToList();
                var authorityModel = aList.Find(s => s.Name == authority.Name);
                if (authorityModel == null)
                {
                    role.AuthorityModels.Add(authority);
                    var res = this._roleService.Update(role);
                    if (res > 0)
                    {
                        return Json("绑定成功", JsonRequestBehavior.AllowGet);
                    }
                }
                return Json("已存在相同的权限", JsonRequestBehavior.AllowGet);

            }
            return Json("绑定失败，请检查表单数据或者网络环境", JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveContact(string rolename, string authorityname)
        {
            var roleid = Convert.ToInt32(rolename);
            var authorityid = Convert.ToInt32(authorityname);
            var role = this._roleService.GetList(s => s.Id == roleid).FirstOrDefault();
            var authority = this._authorityService.GetList(s => s.Id == authorityid).FirstOrDefault();
            if (role != null && authority != null)
            {
                List<AuthorityModel> aList = new List<AuthorityModel>();
                aList = role.AuthorityModels.ToList();
                var authorityModel = aList.Find(s => s.Name == authority.Name);
                if (authorityModel != null)
                {
                    role.AuthorityModels.Remove(authority);
                    var res = this._roleService.Update(role);
                    if (res > 0)
                    {
                        return Json("解除成功", JsonRequestBehavior.AllowGet);
                    }
                }
                return Json("不存在相同的权限", JsonRequestBehavior.AllowGet);
            }
            return Json("绑定失败，请检查表单数据或者网络环境", JsonRequestBehavior.AllowGet);
        }
        #endregion


    }
}
