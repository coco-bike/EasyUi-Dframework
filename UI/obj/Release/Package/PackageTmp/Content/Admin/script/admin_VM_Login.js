//======用户登录表单======
new Vue({
    el: '.logIn-containner',
    data: M_userInfo,
    mounted:function(){
        selfA = this;
    },
    methods: {
        btn_login: function () {
            if (selfA.base.nickName == "" || selfA.base.pwd == "" || selfA.base.validateNum == "" || selfA.base.validateNum == "") {
                return;
            }
            M_Loading.loading_seen = true;
            $.ajax({
                type: "post",
                url: "../Login/CheckLoginInfo",
                dataType: "json",
                data: JSON.stringify({ "nickName": selfA.base.nickName, "pwd": selfA.base.pwd, "validateNum": selfA.base.validateNum, "autoLogin": selfA.base.isRemember, "verifyCode": selfA.base.validateNum }),
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    M_Loading.loading_seen = false;
                    if (data.Status == 1) {
                        location.href = '../ModelManage/index';
                    }else {
                        alert(data.Msg);
                    }
                }
            });
        },
        uName_Focus: function () {
            selfA.uNamePositon = 'uNamePositon2'
        },
        uName_Blur: function () {
            if (selfA.base.nickName == '') {
                selfA.uNamePositon = 'uNamePositon1'
            }
        },
        pwd_Focus: function () {
            selfA.pwdPosition = 'pwdPosition2';
        },
        pwd_Blur: function () {
            if (selfA.base.pwd == '') {
                selfA.pwdPosition = 'pwdPosition1'
            }
        },
        validateCode_Focus: function () {
            selfA.validatePositon = 'validatePositon2';
        },
        validateCode_Blur: function () {
            if (selfA.base.validateNum == '') {
                selfA.validatePositon = 'validatePositon1';
            }
        },
        changeCode: function () {
            selfA.validateImg_src = '/Login/GetValidateCode?time=' + Math.random()
        }
    }
});
//======用户登录表单      end======


//======加载控件======
new Vue({
    el: '.layer',
    data: M_Loading,
    mounted:function(){
        selfB = this;
    },
    methods: {
        GetCode: function (val) {
            selfB.QrValidateCode = val;
        }
    }
})
//======加载控件        end======


