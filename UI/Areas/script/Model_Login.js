//======模态框======
var m_mask_Layer = {
    seen: false,
    QrCode_src: '/Login/GetQrCode?time=' + Math.random(),
    QrValidateCode: '',
    laryer_currentHead: '',
    laryer_currentBody: '',
    laryer_currentFooter: '',
}
//======模态框     end======

//======用户登录表单======
var m_userInfo = {
    base: {
        userName: '',
        pwd: '',
        validateNum: '',
        isRemember: '',
    },
    uNamePositon: 'uNamePositon1',
    pwdPosition: "pwdPosition1",
    validatePositon: 'validatePositon1',
    validateImg_src: '/Login/GetValidateCode?time=' + Math.random()
}
//======用户登录表单      end======