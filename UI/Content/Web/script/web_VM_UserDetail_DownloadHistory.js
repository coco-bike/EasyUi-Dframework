var app = new Vue({
    el: '.container',
    data: {
        base: {
            year: '-',
            month: '-',
            day: '-',
            nickName: ''
        },
        years: [],
        months: [],
        days: [],
        items: []
    },
    mounted: function () {
        self = this;
        self.base.nickName = this.GetQueryString('nickName');
        var date = new Date();
        var year = date.getYear();
        var month = date.getMonth();
        var day = date.getDay();
        self.refresh();
    },
    methods: {
        nav_Userinfo: function () {
            location.href = "../../../../Web/UserDetail/UserInfo?nickName=" + self.base.nickName;
        },
        nav_AccountLevel: function () {
            location.href = "../../../../Web/UserDetail/AccountLevel?nickName=" + self.base.nickName;
        },
        nav_MyCollection: function () {
            location.href = "../../../../Web/UserDetail/MyCollection?nickName=" + self.base.nickName;
        },
        nav_MyProduct: function () {
            location.href = "../../../../Web/UserDetail/MyProduct?nickName=" + self.base.nickName;
        },
        nav_DownloadHistory: function () {
            location.href = "../../../../Web/UserDetail/DownloadHistory?nickName=" + self.base.nickName;
        },
        showYear: function () {
            $('.yearshow').slideDown('fast', 'linear');
            $('.monthshow').slideUp('fast', 'linear');
            $('.dayshow').slideUp('fast', 'linear');
        },
        showMonth: function () {
            $('.monthshow').slideDown('fast', 'linear');
            $('.yearshow').slideUp('fast', 'linear');
            $('.dayshow').slideUp('fast', 'linear');
        },
        showDay: function () {
            $('.dayshow').slideDown('fast', 'linear');
            $('.monthshow').slideUp('fast', 'linear');
            $('.yearshow').slideUp('fast', 'linear');
        },
        selectYear: function (year) {
            self.base.year = year;
            self.refresh();
        },
        selectMonth: function (month) {
            self.base.month = month;
            self.refresh();
        },
        selectDay: function (day) {
            self.base.day = day;
            self.refresh();
        },
        jumpDownLoad: function (e) {
           var id= $(e.currentTarget).attr('data-id');
           location.href = "../../../Web/DownLoad/index?id=" + id + "&nickName=" + self.GetQueryString('nickName');
        },
        GetQueryString: function (name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        },
        refresh: function () {
            $.ajax({
                type: "post",
                url: "../../../../Web/UserDetail/GetTimeAxis",
                dataType: "json",
                data: JSON.stringify({ "nickName": this.base.nickName, "year": this.base.year, "month": this.base.month, "day": this.base.day }),
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    if (data.Status == 1) {
                        self.years = data.Data.TimeAxis.Years;
                        self.months = data.Data.TimeAxis.Months;
                        self.days = data.Data.TimeAxis.Days;
                        self.items = data.Data.UserDataInfos;
                    }
                }
            });
        }
    }
});