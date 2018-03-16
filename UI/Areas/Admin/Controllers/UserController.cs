using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using IService;
using Model;
using UI.Controllers.Base;

namespace UI.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        #region 页面
        /// <summary>
        /// 用户和角色关系的绑定
        /// </summary>
        /// <returns></returns>
        public ActionResult UserRoleBinding()
        {
            return View();
        }
        //
        // GET: /Admin/User/

        public ActionResult Index()
        {
            return View();
        }
        #endregion
        #region 初始化
        readonly IUserAdminService _userService;
        public UserController(IUserAdminService userService)
        {
            this._userService = userService;
        }
        #endregion
        #region 增删改查
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="rows">页面信息数量</param>
        /// <param name="nickname">用户名</param>
        /// <param name="userid">用户ID</param>
        /// <returns>返回一个Json对象包括分页总个数以及用户列表</returns>
        public JsonResult GetUserDataList(string page,string rows,string nickname,string userid)
        {
            int totalCount;
            var pageIndex = Convert.ToInt32(page);
            var pageSize = Convert.ToInt32(rows);
            if (nickname == null && userid == null || (nickname == "" && userid == ""))
            {
                var userList = this._userService.GetPagingList(pageIndex, pageSize, out totalCount, true, s => s.Status == 1, s => s.Id).Select(t => new { t.Id, t.BuildTime, t.Count, t.EMail, t.LoginTime, t.NickName, t.TelNumber, t.UName, t.UpdateTime }).ToList();
                return Json(new { total = totalCount, rows = userList });
            }
            else
            {
                totalCount = 1;
                var userList = this._userService.GetList(s => s.NickName == nickname && s.Id == Convert.ToInt32(userid) && s.Status == 1).ToList();
                return Json(new { tota = totalCount, rows = userList });
            }
        }
        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="userdata">用户对象</param>
        /// <returns>返回json对象</returns>
        public JsonResult UpdateUserData(UserModel userdata)
        {
            var user = this._userService.GetList(s => s.NickName == userdata.NickName && s.Status == 1).FirstOrDefault();
            if (user == null)
            {
                return Json(ResultStatus.Fail);
            }
            user.Count++;
            user.EMail = userdata.EMail;
            user.LoginTime = DateTime.Now.ToString();
            user.TelNumber = userdata.TelNumber;
            user.UName = userdata.UName;
            var res = this._userService.Update(user);
            if (res > 0)
            {
                return Json(ResultStatus.Success);
            }
            return Json(ResultStatus.Fail);
        }
        /// <summary>
        /// 删除用户（假删）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DestroyUser(int id)
        {
            var user = this._userService.GetList(s => s.Id == id).FirstOrDefault();
            if (user == null)
            {
                return Json(ResultStatus.Fail);
            }
            int result = this._userService.DeleteFake(t => t.Id == id, t => new UserModel() { Status = 0 });
            if (result > 0)
            {
                return Json(ResultStatus.Success);
            }
            return Json(ResultStatus.Fail);
        }
        #endregion
    }
}
