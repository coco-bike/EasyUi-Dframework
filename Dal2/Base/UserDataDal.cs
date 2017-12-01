using IDal;
using Model.Base.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public  class UserDataDal : BaseDal<UserDataModel>, IUserDataDal
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">模型主键</param>
        /// <param name="nickName">昵称</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public bool CollectGood(long id, string nickName,bool type)
        {
            if (id < 1 || nickName == null)
            {
                return false;
            }
            var good = this.entity.Set<GoodsModel>().Where(t => t.Id == id).FirstOrDefault();
            var user = this.entity.Set<UserModel>().Where(t => t.NickName == nickName).FirstOrDefault();
            if (!type)//收藏
            {
                if (user.UserDataModels.Where(t => t.Name == good.GoodsName&&t.State==1&&t.Status ==2).FirstOrDefault()!= null)//数据存在并且状态为1
                {
                    return true;
                }
                if (user.UserDataModels.Where(t => t.Name == good.GoodsName && t.State == 0 && t.Status == 2).FirstOrDefault() != null)//数据存在但状态为0
                {
                    var userDataModel = user.UserDataModels.Where(t => t.Name == good.GoodsName && t.State == 0 && t.Status == 2).FirstOrDefault();
                    userDataModel.State = 1;
                    this.entity.Entry<UserDataModel>(userDataModel).State = System.Data.Entity.EntityState.Modified;
                }
                else//数据不存在
                {
                    good.CollectCount += 1;
                    this.entity.Entry<GoodsModel>(good).State = System.Data.Entity.EntityState.Modified;//该模型收藏总次数+1
                    user.UserDataModels.Add(new UserDataModel() { BuildTime = DateTime.Now, Name = good.GoodsName, State = 1, GoodsModel = good, Status = 2, UpdateTime = DateTime.Now });
                    this.entity.Entry<UserModel>(user).State = System.Data.Entity.EntityState.Modified;
                }
            }
            else//取消收藏
            {
              var userDataModel=  user.UserDataModels.Where(t => t.Name == good.GoodsName&&t.State==1&&t.Status==2).FirstOrDefault();
              userDataModel.State = 0;
              this.entity.Entry<UserDataModel>(userDataModel).State = System.Data.Entity.EntityState.Modified;
            }
            int result= this.entity.SaveChanges();
            return result > 0;
        }
    }
}
