new Vue({
    el: '.container',
    data: {
        base: {
            nickName: ''
        }
    },
    mounted: function () {
        self = this;
        self.base.nickName = self.GetQueryString('nickName');
    },
    methods: {
        GetQueryString: function (name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        },
        nav_Userinfo: function () {
            location.href = "../../../../Web/UserDetail/UserInfo?nickName="+self.base.nickName;
        },
        nav_AccountLevel: function () {
            location.href = "../../../../Web/UserDetail/AccountLevel?nickName=" + self.base.nickName;
        },
        nav_MyCollection:function(){
            location.href = "../../../../Web/UserDetail/MyCollection?nickName=" + self.base.nickName;
        },
        nav_MyProduct: function () {
            location.href = "../../../../Web/UserDetail/MyProduct?nickName=" + self.base.nickName;
        },
        nav_DownloadHistory: function () {
            location.href = "../../../../Web/UserDetail/DownloadHistory?nickName=" + self.base.nickName;
        }
    }
});