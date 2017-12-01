using API.Controllers.Base;
using Common;
using IService;
using Model;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace API.Controllers
{
    public class ValuesController : JsonController
    {
        #region 初始化
        private IUserAdminService _userService { get; set; }
        private IAuthorityAdminService _authorityService { get; set; }
        private IRoleAdminService _roleadminService { get; set; }
        public IUserAdminService UserService
        {
            get
            {
                if (_userService == null)
                {
                    return new UserAdminService();
                }
                return _userService;
            }
        }

        public IAuthorityAdminService AuthorityService
        {
            get
            {
                if (_userService == null)
                {
                    return new AuthorityAdminService();
                }
                return _authorityService;
            }
        }


        public IRoleAdminService RoleService
        {
            get
            {
                if (_roleadminService == null)
                {
                    return new RoleAdminService();
                }
                return _roleadminService;
            }
        }
        #endregion

        public static string yunPath = ConfigurationManager.AppSettings["userYunPath"];

        #region 操作
        #region 登录
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="uName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonNetResult LogIn(string nickName, string pwd)
        {
            string pwdMd5 = EncryptionHelper.GetMd5Str(pwd);
            
            var user = UserService.GetList(t => t.NickName == nickName && t.Pwd == pwdMd5 && t.State == 1).Select(t => new { t.Id, t.NickName, t.Count }).FirstOrDefault();
            if (user == null)
            {
                return JsonNetResult(ResultStatus.Fail);
            }
            int res = UserService.GetUpdate(t => t.NickName == nickName, t => new UserModel() { Count = user.Count + 1, LoginTime = DateTime.Now });
            if (res > 0)
                return JsonNetResult(ResultStatus.Success);
            return JsonNetResult(ResultStatus.Fail);
        }
        #endregion

        #region 注册
        /// <summary>
        ///用户注册发送验证码
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonNetResult GetRegCode(string email)
        {
            //正则判断
            if (!RegularHelper.IsEmail(email))//判断邮箱格式是否正确
            {
                return JsonNetResult(ResultStatus.EmailErr);
            }
            //账户状态判断
            var user = this.UserService.GetList(t => t.EMail == email && t.State > 0).FirstOrDefault();//禁用账户的邮箱不能注册
            if (user != null)//判断邮箱是否存在
            {
                return JsonNetResult(ResultStatus.EmailExist);
            }
            Tuple<string, bool> items = SendEmail(email, "创客教育官方", "用户注册验证码");
            string code = items.Item1;
            bool sendRes = items.Item2;
            if (sendRes)
            {
                bool res1 = CacheHelper.Set("RegCode", code, DateTime.Now.AddSeconds(90));
                bool res2 = CacheHelper.Set("EMail", email, DateTime.Now.AddSeconds(90));//判断用户发送验证码和注册用的是同一个邮箱
                if (res1 && res2)
                {
                    return JsonNetResult(ResultStatus.Success);
                }
            }
            return JsonNetResult(ResultStatus.Fail);
        }


        ///// <summary>
        ///// 用户注册并创建文件夹
        ///// </summary>
        ///// <param name="userName"></param>
        ///// <param name="email"></param>
        ///// <param name="code"></param>
        ///// <param name="pwd"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public JsonNetResult RegUser(string nickName, string email, string code, string pwd)
        //{
        //    if (!RegularHelper.IsLetter(nickName))
        //    {
        //        return JsonNetResult(ResultStatus.NameErr);
        //    }
        //    if (!RegularHelper.IsEmail(email))
        //    {
        //        return JsonNetResult(ResultStatus.EmailErr);
        //    }
        //    FolderTree tree = new FolderTree() { Name = "", Children = new List<FolderTree>() { new FolderTree() { Name = "Synchro" }, new FolderTree() { Name = "FuncT", Children = new List<FolderTree>() { new FolderTree() { Name = "Resource" }, new FolderTree() { Name = "Audit", Children = new List<FolderTree>() { new FolderTree() { Name = "Pend" }, new FolderTree() { Name = "Audited" } } } } } } };

        //    if (CacheHelper.Get("EMail") == null || (CacheHelper.Get("EMail").ToString() != email))//判断用户发送验证码和注册用的是同一个邮箱
        //    {
        //        return JsonNetResult(ResultStatus.EmailErr);
        //    }

        //    var user = this.UserService.GetList(t => t.NickName == nickName).FirstOrDefault();
        //    if (user != null)//判断注册的用户名是否存在
        //    {
        //        return JsonNetResult(ResultStatus.NameExist);
        //    }

        //    if (CacheHelper.Get("RegCode") != null)
        //    {
        //        if (string.Compare(CacheHelper.Get("RegCode").ToString(), code, true) == 0)//判断验证码是否正确(忽略大小写)
        //        {
        //            var regUser = this.UserService.Add(new UserModel() { EMail = email, Pwd = EncryptionHelper.GetMd5Str(pwd), UName = "", BuildTime = DateTime.Now, Count = 0, State = 0, UpdateTime = DateTime.Now, RoleId = 1, OrganizeId = 1, LevelId = 1, FolderUrl = "/FileSave/YUN/" + nickName, LoginTime = DateTime.Now, YunUsedSpace = 0, NickName = nickName, Status = 1 });
        //            if (regUser != null)
        //            {
        //                tree.Name = yunPath + nickName;//savePath通过配置文件读取
        //                bool res1 = CreateDirectory(tree);
        //                bool res2 = CacheHelper.Remove("RegCode");
        //                bool res3 = CacheHelper.Remove("EMail");
        //                //string url = HttpContext.Current.Request.Url.AbsoluteUri;//访问的url    
        //                //string webPath = HttpContext.Current.Request.Url.AbsolutePath;//访问的网页文件路径
        //                //string host = url.Substring(0, url.IndexOf(webPath));//获取域名
        //                //string savePath =host+ "/FileSave/Yun/" + userName;
        //                if (res1 && res2 && res3)
        //                {
        //                    regUser.State = 1;
        //                    this.UserService.Update(regUser);
        //                    return JsonNetResult(ResultStatus.Success);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            return JsonNetResult(ResultStatus.ValidateCodeErr);
        //        }
        //    }
        //    return JsonNetResult(ResultStatus.Fail);
        //}
        //#endregion

        #region 找回密码
        /// <summary>
        /// 用户找回密码发送验证码
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonNetResult GetPwdBackCode(string eMail)
        {
            if (!RegularHelper.IsEmail(eMail))//判断邮箱格式是否正确
            {
                return JsonNetResult(ResultStatus.EmailErr);
            }
            var user = this.UserService.GetList(t => t.EMail == eMail && t.State == 1).FirstOrDefault();//判断邮箱是否存在，防止用户恶性发送邮件,禁用的用户不能找回密码
            if (user == null)//判断邮箱是否存在
            {
                return JsonNetResult(ResultStatus.EmailNoExist);
            }
            Tuple<string, bool> items = SendEmail(eMail, "创客教育官方", "创客教育用户找回密码验证码");
            string code = items.Item1;
            bool sendRes = items.Item2;
            if (sendRes)
            {
                bool res1 = CacheHelper.Set("PwdBackCode", code, DateTime.Now.AddSeconds(90));
                bool res2 = CacheHelper.Set("PwdBackEMail", eMail, DateTime.Now.AddSeconds(90));//判断用户发送验证码和注册用的是同一个邮箱
                if (res1 && res2)
                {
                    return JsonNetResult(ResultStatus.Success);
                }
            }
            return JsonNetResult(ResultStatus.Fail);
        }


        /// <summary>
        /// 找回密码验证码校验
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonNetResult ValidatePwdBackCode(string code, string eMail)
        {
            if (!RegularHelper.IsEmail(eMail))//判断邮箱格式是否正确
            {
                return JsonNetResult(ResultStatus.EmailErr);
            }
            if (CacheHelper.Get("PwdBackEMail") == null || (CacheHelper.Get("PwdBackEMail").ToString() != eMail))//判断用户发送验证码和注册用的是同一个邮箱
            {
                return JsonNetResult(ResultStatus.EmailErr);
            }
            var user = this.UserService.GetList(t => t.EMail == eMail && t.State == 1).FirstOrDefault();
            if (user == null)//判断注册的用户名是否存在
            {
                return JsonNetResult(ResultStatus.EmailNoExist);
            }
            if (CacheHelper.Get("PwdBackCode") != null)
            {
                if (string.Compare(CacheHelper.Get("PwdBackCode").ToString(), code, true) == 0)
                {
                    bool res1 = CacheHelper.Remove("PwdBackCode");
                    bool res2 = CacheHelper.Remove("PwdBackCode");
                    if (res1 && res2)
                    {
                        return JsonNetResult(ResultStatus.Success);
                    }
                }
            }
            return JsonNetResult(ResultStatus.ValidateCodeErr);
        }


        /// <summary>
        /// 重新输入密码
        /// </summary>
        /// <param name="newPwd"></param>
        /// <param name="email"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonNetResult UpdataPwd(string newPwd, string eMail)
        {
            if (!RegularHelper.IsEmail(eMail))//判断邮箱格式是否正确
            {
                return JsonNetResult(ResultStatus.EmailErr);
            }
            var user = this.UserService.GetList(t => t.State == 1 && t.EMail == eMail).FirstOrDefault();
            if (user == null)
            {
                return JsonNetResult(ResultStatus.EmailNoExist);
            }
            user.Pwd = EncryptionHelper.GetMd5Str(newPwd);
            int res = this.UserService.Update(user);
            if (res > 0)
                return JsonNetResult(ResultStatus.Success);
            return JsonNetResult(ResultStatus.Fail);
        }
        #endregion

        #region 其他
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonNetResult GetUserInfo(string nickName)
        {
            if (!RegularHelper.IsLetter(nickName))
            {
                return JsonNetResult(ResultStatus.NameErr);
            }
            var userInfo = this.UserService.GetList(t => t.NickName == nickName).Select(t => new { t.UName, t.EMail }).FirstOrDefault();
            if (userInfo != null)
                return JsonNetResult(ResultStatus.Success, userInfo);
            return JsonNetResult(ResultStatus.Fail);
        }
        #endregion
        ///// <summary>
        ///// 用户上传同步文件
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public JsonNetResult UploadSynchroFile()
        //{
        //    HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];//获取传统context
        //    HttpRequestBase request = context.Request;//定义传统request对象
        //    string nickName = request.Params.Get("nickName");
        //    string version = request.Params.Get("version");
        //    if (nickName == null || nickName == "")
        //    {
        //        return JsonNetResult(ResultStatus.Fail);
        //    }
        //    if (request.Files.Count > 0)
        //    {
        //        string suffix = Path.GetExtension(request.Files[0].FileName);//后缀名
        //        string fileName = request.Files[0].FileName;//文件名
        //        int size = request.Files[0].ContentLength;
        //        var user = this.UserService.GetList(t => t.NickName == nickName && t.State == 1).FirstOrDefault();
        //        if (user == null)//判断用户是否存在
        //        {
        //            return JsonNetResult(ResultStatus.Fail);
        //        }
        //        //string userTempFolder = user.FolderUrl + "/Synchro";
        //        var fileOldModel = this.RoleService.GetList(t => t.FileName == fileName && t.State > 0 && t.Type == 1 && t.Status == 1).FirstOrDefault();

        //        string userTempFolder = user.FolderUrl + "/Synchro";
        //        //如果该用户的云盘文件夹以外丢失则创建
        //        string fileAbsolutePath = yunPath + user.NickName + "\\" + "Synchro";
        //        if (!Directory.Exists(fileAbsolutePath))
        //        {
        //            DirectoryInfo d = Directory.CreateDirectory(fileAbsolutePath);
        //        }
        //        string fileRelativePath = userTempFolder + "/" + fileName;
        //        fileAbsolutePath = fileAbsolutePath + "\\" + fileName;
        //        try
        //        {
        //            request.Files[0].SaveAs(fileAbsolutePath);
        //            if (fileOldModel == null)
        //            {
        //                user.UserUpFileModels.Add(new UserUpFileModel() { FileName = fileName, Size = size, State = 1, Status = 1, Type = 2, DownloadUrl = fileRelativePath, OldUrl = fileRelativePath, BuildTime = DateTime.Now, UpdateTime = DateTime.Now, Version = version });
        //                if (this.UserService.Update(user) > 0)
        //                    return JsonNetResult(ResultStatus.Success);
        //                return JsonNetResult(ResultStatus.Fail);
        //            }
        //            else
        //            {
        //                int res = this.RoleService.GetUpdate(t => t.Id == fileOldModel.Id, t => new UserUpFileModel() { Size = size, UpdateTime = DateTime.Now, Version = version });
        //                if (res > 0)
        //                    return JsonNetResult(ResultStatus.Success);
        //                return JsonNetResult(ResultStatus.Fail);
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            return JsonNetResult(ResultStatus.UploadErr);
        //        }
        //    }
        //    return JsonNetResult(ResultStatus.Fail);
        //}


        ///// <summary>
        ///// 获取用户上传的模型
        ///// </summary>
        ///// <param name="nickName">用户昵称</param>
        ///// <returns></returns>
        //[HttpGet]
        //public JsonNetResult GetSynchroFile(string nickName)
        //{
        //    if (!RegularHelper.IsLetter(nickName))
        //    {
        //        return JsonNetResult(ResultStatus.NameErr);
        //    }
        //    ICollection<UserUpFileModel> userUpFiles = this.UserService.GetList(t => t.NickName == nickName && t.State == 1).Select(t => t.UserUpFileModels).FirstOrDefault();
        //    var userUpModel = userUpFiles.Where(t => t.Status == 1 && t.State == 1 && t.Type == 2).Select(t => new { t.FileName, t.OldUrl, t.Version }).FirstOrDefault();

        //    if (userUpModel != null)
        //        return JsonNetResult(ResultStatus.Success, userUpModel);
        //    return JsonNetResult(ResultStatus.Fail);
        //}


        ///// <summary>
        ///// 上传T功能资源文件(图片、视频、文字压缩包)
        ///// </summary>
        ///// <returns></returns>
        //public JsonNetResult UploadTSourceFile()
        //{
        //    HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];//获取传统context
        //    HttpRequestBase request = context.Request;//定义传统request对象
        //    string nickName = request.Params.Get("nickName");
        //    //string version = request.Params.Get("version");
        //    if (nickName == null || nickName == "")
        //    {
        //        return JsonNetResult(ResultStatus.Fail);
        //    }
        //    if (request.Files.Count > 0)
        //    {
        //        string suffix = Path.GetExtension(request.Files[0].FileName);//后缀名
        //        string fileName = request.Files[0].FileName;//文件名
        //        int size = request.Files[0].ContentLength;
        //        var user = this.UserService.GetList(t => t.NickName == nickName && t.State == 1).FirstOrDefault();
        //        if (user == null)//判断用户是否存在
        //        {
        //            return JsonNetResult(ResultStatus.Fail);
        //        }
        //        var fileOldModel = this.RoleService.GetList(t => t.FileName == fileName && t.State > 0 && t.Type == 0 && t.Status == 1).FirstOrDefault();

        //        string userTempFolder = user.FolderUrl + "/FuncT/Resource/";
        //        //如果该用户的云盘文件夹以外丢失则创建
        //        string fileAbsolutePath = yunPath + user.NickName + "\\FuncT\\Resource\\";
        //        if (!Directory.Exists(fileAbsolutePath))
        //        {
        //            DirectoryInfo d = Directory.CreateDirectory(fileAbsolutePath);
        //        }
        //        string fileRelativePath = userTempFolder + fileName;
        //        fileAbsolutePath = fileAbsolutePath + fileName;
        //        try
        //        {
        //            request.Files[0].SaveAs(fileAbsolutePath);
        //            if (fileOldModel == null)
        //            {
        //                user.UserUpFileModels.Add(new UserUpFileModel() { FileName = fileName, Size = size, State = 1, Status = 1, Type = 0, DownloadUrl = fileRelativePath, OldUrl = fileRelativePath, BuildTime = DateTime.Now, UpdateTime = DateTime.Now });
        //                if (this.UserService.Update(user) > 0)
        //                    return JsonNetResult(ResultStatus.Success);
        //                return JsonNetResult(ResultStatus.Fail);
        //            }
        //            else
        //            {
        //                int res = this.RoleService.GetUpdate(t => t.Id == fileOldModel.Id, t => new UserUpFileModel() { Size = size, UpdateTime = DateTime.Now });
        //                if (res > 0)
        //                    return JsonNetResult(ResultStatus.Success);
        //                return JsonNetResult(ResultStatus.Fail);
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            return JsonNetResult(ResultStatus.UploadErr);
        //        }
        //    }
        //    return JsonNetResult(ResultStatus.Fail);
        //}

        ///// <summary>
        ///// 获取T功能资源文件
        ///// </summary>
        ///// <param name="nickName">昵称</param>
        ///// <returns></returns>
        //public JsonNetResult GetTSourceFile(string nickName)
        //{
        //    if (!RegularHelper.IsLetter(nickName))
        //    {
        //        return JsonNetResult(ResultStatus.NameErr);
        //    }
        //    var user = this.UserService.GetList(t => t.NickName == nickName && t.State == 1).FirstOrDefault();
        //    if (user == null)
        //    {
        //        return JsonNetResult(ResultStatus.UserNoExist);
        //    }
        //    if (user.UserUpFileModels == null)
        //    {
        //        return JsonNetResult(ResultStatus.Fail);
        //    }
        //    var userUpfile = user.UserUpFileModels.Where(t => t.State >0 && t.Type == 0 && t.Status == 1).Select(t => new { t.FileName, t.DownloadUrl }).FirstOrDefault();
        //    if (userUpfile != null)
        //        return JsonNetResult(ResultStatus.Success, userUpfile);
        //    return JsonNetResult(ResultStatus.Fail);
        //}



        ///// <summary>
        ///// 上传T功能待审核文件
        ///// </summary>
        ///// <returns></returns>
        //public JsonNetResult UploadTAuditFile()
        //{
        //    HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];//获取传统context
        //    HttpRequestBase request = context.Request;//定义传统request对象
        //    string nickName = request.Params.Get("nickName");
        //    //string version = request.Params.Get("version");
        //    if (nickName == null || nickName == "")
        //    {
        //        return JsonNetResult(ResultStatus.Fail);
        //    }
        //    if (request.Files.Count > 0)
        //    {
        //        string suffix = Path.GetExtension(request.Files[0].FileName);//后缀名
        //        string fileName = request.Files[0].FileName;//文件名
        //        int size = request.Files[0].ContentLength;
        //        var user = this.UserService.GetList(t => t.NickName == nickName && t.State == 1).FirstOrDefault();
        //        if (user == null)//判断用户是否存在
        //        {
        //            return JsonNetResult(ResultStatus.Fail);
        //        }
        //        var fileOldModel = this.RoleService.GetList(t => t.FileName == fileName && t.State > 0 && t.Type == 1 && t.Status == 1).FirstOrDefault();

        //        string userTempFolder = user.FolderUrl + "/FuncT/Audit/Pend/";
        //        //如果该用户的云盘文件夹以外丢失则创建
        //        string fileAbsolutePath = yunPath + user.NickName + "\\FuncT\\Audit\\Pend\\";
        //        if (!Directory.Exists(fileAbsolutePath))
        //        {
        //            DirectoryInfo d = Directory.CreateDirectory(fileAbsolutePath);
        //        }
        //        string fileRelativePath = userTempFolder + fileName;
        //        fileAbsolutePath = fileAbsolutePath + fileName;
        //        string downLodUrl = user.FolderUrl + "/FuncT/Audit/Pend/" + fileName;
        //        try
        //        {
        //            request.Files[0].SaveAs(fileAbsolutePath);
        //            if (fileOldModel == null)
        //            {
        //                user.UserUpFileModels.Add(new UserUpFileModel() { FileName = fileName, Size = size, State = 1, Status = 1, Type = 1, DownloadUrl = downLodUrl, OldUrl = fileRelativePath, BuildTime = DateTime.Now, UpdateTime = DateTime.Now });
        //                if (this.UserService.Update(user) > 0)
        //                    return JsonNetResult(ResultStatus.Success);
        //                return JsonNetResult(ResultStatus.Fail);
        //            }
        //            else
        //            {
        //                int res = this.RoleService.GetUpdate(t => t.Id == fileOldModel.Id, t => new UserUpFileModel() { Size = size, UpdateTime = DateTime.Now });
        //                if (res > 0)
        //                    return JsonNetResult(ResultStatus.Success);
        //                return JsonNetResult(ResultStatus.Fail);
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            return JsonNetResult(ResultStatus.UploadErr);
        //        }
        //    }
        //    return JsonNetResult(ResultStatus.Fail);
        //}

        ///// <summary>
        ///// 获取审核过的文件
        ///// </summary>
        ///// <param name="nickName"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public JsonNetResult GetTAuditFile(string nickName)
        //{
        //    if (!RegularHelper.IsLetter(nickName))
        //    {
        //        return JsonNetResult(ResultStatus.NameErr);
        //    }
        //    var user = this.UserService.GetList(t => t.NickName == nickName && t.State == 1).FirstOrDefault();
        //    if (user == null)
        //    {
        //        return JsonNetResult(ResultStatus.UserNoExist);
        //    }
        //    if (user.UserUpFileModels == null)
        //    {
        //        return JsonNetResult(ResultStatus.Fail);
        //    }
        //    var userUpfile = user.UserUpFileModels.Where(t => t.State == 1 && t.Type == 1 && t.Status == 1).Select(t => new { t.FileName, t.DownloadUrl }).FirstOrDefault();
        //    if (userUpfile != null)
        //        return JsonNetResult(ResultStatus.Success, userUpfile);
        //    return JsonNetResult(ResultStatus.Fail);
        //}
        //#endregion
        //#endregion

        #region 公共方法
        /// <summary>
        /// 递归创建文件夹
        /// </summary>
        /// <param name="tree"></param>
        /// <returns></returns>
        public bool CreateDirectory(FolderTree tree)
        {
            if (DirectoryHelper.CreateDirectory(tree.Name))
            {
                if (tree.Children != null)
                {
                    foreach (var item in tree.Children)
                    {
                        item.Name = tree.Name + "\\" + item.Name;
                        this.CreateDirectory(item);
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="eMail">接收方邮箱</param>
        /// <param name="mailName">发件人名称</param>
        /// <param name="mailTitle">邮件名称</param>
        /// <returns></returns>
        public static Tuple<string, bool> SendEmail(string eMail, string mailName, string mailTitle)
        {
            MailHelper mail = new MailHelper();
            mail.MailServer = "smtp.qq.com";
            mail.MailboxName = "444503829@qq.com";
            mail.MailboxPassword = "xtdphklmrdczcaji";//开启QQ邮箱POP3/SMTP服务时给的授权码
            //操作打开QQ邮箱->在账号下方点击"设置"->账户->POP3/IMAP/SMTP/Exchange/CardDAV/CalDAV服务
            //obxxsfowztbideee为2872845261@qq的授权码
            mail.MailName = mailName;
            string code;
            VerifyCode codeHelper = new VerifyCode();
            codeHelper.GetVerifyCode(out code);
            if (code == "")
                return new Tuple<string, bool>("", false);
            if (mail.Send(eMail, mailTitle, code))
                return new Tuple<string, bool>(code, true);
            return new Tuple<string, bool>("", false);
        }
        #endregion
    }

    public class FolderTree
    {
        public string Name { get; set; }
        public List<FolderTree> Children { get; set; }
    }
}
        #endregion 
        #endregion