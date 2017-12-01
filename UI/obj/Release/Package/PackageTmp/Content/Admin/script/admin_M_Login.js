//======用户登录表单======
var M_userInfo = {
    base: {
        //昵称
        nickName: '',
        //密码
        pwd: '',
        //验证码
        validateNum: '',
        //是否记住密码
        isRemember: false,
    },
    uNamePositon: 'uNamePositon1',
    pwdPosition: "pwdPosition1",
    validatePositon: 'validatePositon1',
    validateImg_src: '/Login/GetValidateCode?time=' + Math.random()//验证码
}
//======用户登录表单      end======

var M_Loading = {
    loading_seen: false,
}