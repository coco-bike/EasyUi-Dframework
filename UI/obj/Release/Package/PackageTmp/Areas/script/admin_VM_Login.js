//======用户登录表单======
new Vue({
    el: '.logIn-containner',
    data: m_userInfo,
    methods: {
        btn_login: function () {
            m_Loading.loading_seen = true;
            $.ajax({
                type: "post",
                url: "../Login/CheckLoginInfo",
                dataType: "json",
                data: JSON.stringify({ "uName": this.base.userName, "pwd": this.base.pwd, "validateNum": this.base.validateNum, "autoLogin": this.base.isRemember, "verifyCode": this.base.validateNum }),
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    if (data.Status == 1) {
                        location.href = '../Models/index';
                        M_Loading.loading_seen = false;
                    }
                }
            });
        },
        uName_Focus: function () {
            this.uNamePositon = 'uNamePositon2'
        },
        uName_Blur: function () {
            if (this.base.userName == '') {
                this.uNamePositon = 'uNamePositon1'
            }
        },
        pwd_Focus: function () {
            this.pwdPosition = 'pwdPosition2';
        },
        pwd_Blur: function () {
            if (this.base.pwd == '') {
                this.pwdPosition = 'pwdPosition1'
            }
        },
        validateCode_Focus: function () {
            this.validatePositon = 'validatePositon2';
        },
        validateCode_Blur: function () {
            if (this.base.validateNum == '') {
                this.validatePositon = 'validatePositon1';
            }
        },
        mark_layer_show: function () {
            //vm_mask_Layer.seen = true;
            vm_player.mask_seen = true;
            m_mask_Layer.laryer_currentHead = QrHead;
            m_mask_Layer.laryer_currentBody = QrBody;
            m_mask_Layer.laryer_currentFooter = QrFootter;
        },
        changeCode: function () {
            m_userInfo.validateImg_src = '/Login/GetValidateCode?time=' + Math.random()
        }
    }
});

Vue.component('t-loading', {
    template: '#loding-component',
    props: ['seen']
})

var vm_player=new Vue({
    el: '.layer',
    data: m_Loading,
    methods: {
        GetCode: function (val) {//标号20170601002
            this.QrValidateCode = val;
        }
    }
})

//======用户登录表单      end======

