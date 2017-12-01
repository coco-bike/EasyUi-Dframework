//======模态框======
//var vm_mask_Layer = new Vue({
//    el: '.mask_Layer',
//    data: m_mask_Layer,
//    methods: {
//        Qrvc_change: function (val) {
//            this.QrValidateCode = val;
//        }
//    }
//});
//======模态框     end======

//======用户登录表单======
new Vue({
    el: '.logIn-containner',
    data: m_userInfo,
    methods: {
        btn_login: function () {

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

Vue.component('t-mask', {
    template: '#mask-component',
    props: ['seen', 'src', 'code'],
    methods:{
        qr_change: function (val) {//标号20170601001
            this.$emit("qrvc_change", val);//传至标号20170601002处
        }
    },
    components: {
        'layer-header': {
            template: "#layerheader-component",
        },
        'layer-body': {
            template: "#layerbody-component",
            props: ['mysrc', 'mycode'],
            data: function () {
                return {
                    db_code: this.mycode
                }
            },
            methods: {
                QrVC_change: function () {
                    this.$emit("qrvc_change", this.db_code);//子组件向父组件传数据，如果多层，则需要一层一层传；传至标号20170601001处
                }
            }
        },
        'layer-footer': {
            template: '#layerfooter-component',
                   methods: {
                    canel: function () {
                        vm_player.mask_seen = false;
                    },
                    confirm: function () {
                        var QrValidateCode = vm_player.QrValidateCode;
                        var form = new FormData();
                        form.append('QrValidateCode', QrValidateCode);
                        submitServer.form = form;
                        submitServer.url = '/Login/QrLogin';
                        submitServer.type = 'post';
                        submitServer.send(function () {
                              vm_player.mask_seen = false;
                            location.href = '/home/index';
                        });
                    }
                }
        }
    }
});


Vue.component('t-loading', {
    template: '#loding-component',
    props: ['seen']
})

var vm_player=new Vue({
    el: '.layer',
    data: {
        loading_seen: false,
        mask_seen: false,
        QrCode_src: '/Login/GetQrCode?time=' + Math.random(),
        QrValidateCode:''
    },
    methods: {
        GetCode: function (val) {//标号20170601002
            this.QrValidateCode = val;
        }
    }
})

//======用户登录表单      end======

