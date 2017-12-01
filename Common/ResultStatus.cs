using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public enum ResultStatus
    {
        [Description("操作成功")]
        Success = 1,
        [Description("操作失败")]
        Fail = 2,
        [Description("验证码错误")]
        ValidateCodeErr = 3,
        [Description("文件上传失败")]
        UploadErr = 4,
        [Description("文件已存在")]
        FileExist = 5,
        [Description("用户云盘空间不足")]
        SpaceNoEnough = 6,
        [Description("存在关联禁止删除")]
        RelationExist = 7,
        [Description("手机号码格式错误")]
        PhoneNumErr= 8,
        [Description("邮箱格式错误")]
        EmailErr = 9,
        [Description("邮箱已存在")]
        EmailExist = 10,
        [Description("昵称不能包含除字母以外任何字符")]
        NameErr = 11,
        [Description("昵称已存在")]
        NameExist = 12,
        [Description("邮箱不存在")]
        EmailNoExist =13,
        [Description("用户不存在")]
        UserNoExist = 14,
    }

    public enum YunSpace
    {
        [Description("100M")]
        Level1 = 104857600,
        [Description("500M")]
        Level2 = 524288000,
        [Description("1G")]
        Level3 = 1048576000
    }

    public enum YunPath
    {
        [Description("")]
        Self=1,
        [Description("用户上传资源和场景的临时文件夹")]
        Temp=2,
        [Description("用户上传文件审核后的文件夹")]
        Files=3,
        [Description("资源文件夹")]
        Source=4,
        [Description("场景文件夹")]
        Scene=5
    }
}
