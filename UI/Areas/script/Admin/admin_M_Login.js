//======用户登录表单======
var m_userInfo = {
    base: {
        userName: '',
        pwd: '',
        validateNum: '',
        isRemember: false,
    },
    uNamePositon: 'uNamePositon1',
    pwdPosition: "pwdPosition1",
    validatePositon: 'validatePositon1',
    validateImg_src: '/Login/GetValidateCode?time=' + Math.random()
}
//======用户登录表单      end======

var m_Loading = {
    loading_seen: false,
    mask_seen: false,
    QrCode_src: '/Login/GetQrCode?time=' + Math.random(),
    QrValidateCode: ''
}