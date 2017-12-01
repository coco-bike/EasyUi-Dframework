var app=new Vue({
    el: '.container',
    data: {
        base: {
            id: '',
            nickName: '',
            telNumber: '',
            eMail: '',
            role: '',
            organize: '',
            socialSoftware: '',
            headPicUrl:''
        },
        uName: '',
        telNumber:'',
        seen: true
    },
    mounted: function () {

        //var oBox = document.getElementById("headPic");
        //var oBar = document.getElementById("headPic");
        //startDrag(oBar, oBox);
            selfA= this;
            selfA.base.nickName = selfA.GetQueryString("nickName");
            selfA.refresh();
    },
    methods: {
        nav_Userinfo: function () {
            location.href = "../../../../Web/UserDetail/UserInfo?nickName=" + selfA.base.nickName;
        },
        nav_AccountLevel: function () {
            location.href = "../../../../Web/UserDetail/AccountLevel?nickName=" + selfA.base.nickName;
        },
        nav_MyCollection: function () {
            location.href = "../../../../Web/UserDetail/MyCollection?nickName=" + selfA.base.nickName;
        },
        nav_MyProduct: function () {
            location.href = "../../../../Web/UserDetail/MyProduct?nickName=" + selfA.base.nickName;
        },
        nav_DownloadHistory: function () {
            location.href = "../../../../Web/UserDetail/DownloadHistory?nickName=" + selfA.base.nickName;
        },
        uploadHeadPic: function () {
        
            selfA.previewFile();
                //$(".cropper").cropper("replace", self.base.headPicUrl);
        },
        //getObjectURL: function (file) {
        //    var url = null;
        //    // 下面函数执行的效果是一样的，只是需要针对不同的浏览器执行不同的 js 函数而已  
        //    if (window.createObjectURL != undefined) { // basic  headPicheadPicheadPic
        //        url = window.createObjectURL(file);
        //    } else if (window.URL != undefined) { // mozilla(firefox)  
        //        url = window.URL.createObjectURL(file);
        //    } else if (window.webkitURL != undefined) { // webkit or chrome  
        //        url = window.webkitURL.createObjectURL(file);
        //    }
        //    return url;
        //},
        previewFile:function(){
            var preview = document.querySelector('img');
            //var file = document.querySelector('input[type=file]').files[0];
            var file = document.getElementById("headPic").files[0]
            var reader = new FileReader();
            reader.onloadend = function () {
                preview.src = reader.result;
                selfA.base.headPicUrl = preview.src;
            }
            if (file) {
              reader.readAsDataURL(file);
            } else {
                preview.src = "";
            }
        },
        basicInfo: function () {
            selfA.seen = true;
        },
        editInfo: function () {
            selfA.seen = false;
        },
        GetQueryString: function (name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        },
        update: function () {
            //var dataURL = $(".cropper").cropper("getDataURL", "image/jpeg");
           // if (selfA.base.nickName == '' || selfA.uName == '' || selfA.telNumber == '' || selfA.base.eMail == '' || selfA.base.organize == '' || selfA.base.socialSoftware == '' || selfA.base.headPicUrl == '')
           // {
           //     return;
           //}
            $.ajax({
                type: "post",
                url: "../../../../Web/UserDetail/UserInfoUpdate",
                dataType: "json",
                data: JSON.stringify({ "id": selfA.base.id, "nickName": selfA.base.nickName, "uName": selfA.uName, "telNumber": selfA.telNumber, "eMail": selfA.base.eMail, "orgainzeName": selfA.base.organize, "socialSoftware": selfA.base.socialSoftware, "headPic": selfA.base.headPicUrl }),
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    if (data.Status == 1) {
                        selfA.refresh();
                        selfA.seen = true;
                    }
                }
            });
        },
        refresh: function () {
            $.ajax({
                type: "post",
                url: "../../../../Web/UserDetail/GetUserInfo",
                dataType: "json",
                data: JSON.stringify({ "nickName": selfA.base.nickName }),
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    if (data.Status == 1) {
                        console.log(data.Data);
                        selfA.uName = data.Data.UName;
                        selfA.base.nickName = data.Data.NickName;
                        //selfA.base.telNumber = data.Data.TelNumber;
                        selfA.telNumber = data.Data.TelNumber;
                        selfA.base.eMail = data.Data.EMail;
                        selfA.base.role = data.Data.RoleName;
                        selfA.base.organize = data.Data.OrganizeName;
                        selfA.base.socialSoftware = data.Data.SocialSoftware;
                        selfA.base.id = data.Data.Id;
                        selfA.base.headPicUrl = data.Data.HeadPicUrl;
                    }
                }
            });
        },
        //checkUName:function(){
        //    selfA.uName = selfA.uName.replace(/[^\u4E00-\u9FA5]/g, '');//只能输入中文
        //    selfA.uName = selfA.uName.slice(0, 4);
        //}
    },
    watch: {
   telNumber: function (val) {
       selfA.telNumber = val.replace(/[^\d]/g, '');//e.key当该dom元素获取焦点时，每次输入的键盘值
       selfA.telNumber = val.slice(0, 11);
        return selfA.telNumber;
   },
   uName: function (val) {
       selfA.uName = selfA.uName.replace(/[^\u4E00-\u9FA5]/g, '');//只能输入中文
       selfA.uName = selfA.uName.slice(0, 4);
       selfA.uName = selfA.uName == '' ? '&nbsp;' : selfA.uName;
       return selfA.uName
   }

    },
    //computed: {
    //    telNumber1: {
    //        get: function () {
    //            return selfA.telNumber;
    //        },
    //        set: function (newValue) {
    //            newValue = newValue.replace(/[^\d]/g, '');//e.key当该dom元素获取焦点时，每次输入的键盘值
    //            newValue = newValue.slice(0, 11);
    //            selfA.telNumber = newValue;
    //        }
    //    }
    //}
});