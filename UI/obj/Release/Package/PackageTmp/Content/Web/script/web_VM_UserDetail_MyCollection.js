var app= new Vue({
    el: '.container',
    data: {
        base: {
            nickName: '',
            pageIndex: 1,
            pageSize: 12,
            pageCount: '',
            ids:[]
        },
       items:[]
    },
    mounted: function () {
        self = this;
        self.base.nickName = self.GetQueryString('nickName');
        $('#J_page_wrap').on('click', 'a[id!="J_go_to_page"]', function () {
            var type = $(this).attr('type');
            var pageIndex;
            switch (type) {
                case "previousPage":
                    pageIndex = page.page_now - 1 < 1 ? 1 : page.page_now - 1;
                    break;
                case "lastPage":
                    pageIndex = page.page_now + 1 > page.page_max ? page.page_max : page.page_now + 1;
                    break;
                default:
                    pageIndex = $(this).attr('data');
                    break;
            }
            $(this).addClass('cur').siblings().removeClass('cur');
            self.base.pageIndex = pageIndex;
            self.refresh();
        });
        self.refresh();
    },
    methods: {
        refresh: function () {
            $('.imgGroup span').removeClass('checkBox').removeClass('checkBox-hover ');
            $.ajax({
                type: "post",
                url: "../../../../Web/UserDetail/GetMyCollectionList",
                dataType: "json",
                data: JSON.stringify({ "pageIndex": self.base.pageIndex, "pageSize": self.base.pageSize, "nickName": self.base.nickName }),
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    page.page_max = Math.ceil(data.Data.TotalCount / self.base.pageSize);
                    self.base.pageCount = Math.ceil(data.Data.TotalCount / self.base.pageSize);
                    page.calculate_page(self.base.pageIndex);
                    self.items = data.Data.List;
                }
            });
        },
        jumpDownLoad: function (e) {
            var id = $(e.currentTarget).attr('data-id');
            location.href='../../../Web/DownLoad/index?id=' + id + '&nickName=' + self.GetQueryString('nickName');
        },
        GetQueryString: function (name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        },
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
        editCollect: function () {
            $('.imgGroup span').addClass('checkBox');
        },
        check: function (e) {
            console.log(e.currentTarget);
            $(e.currentTarget).toggleClass('checkBox-hover');
        },
        closeEdit: function () {
            $('.imgGroup span').removeClass('checkBox').removeClass('checkBox-hover');
        },
        selectAll: function () {
            if ($('.imgGroup span').hasClass('checkBox')) {
                if ($('.imgGroup span').hasClass('checkBox-hover')) {
                    $('.imgGroup span').removeClass('checkBox-hover');
                } else {
                    $('.imgGroup span').addClass('checkBox-hover');
                }
            }
        },
        deleteCollect: function () {
            $('.imgGroup span').each(function (index) {
                if ($(this).hasClass('checkBox-hover')) {
                    self.base.ids.push($(this).attr('data-id'));
                }
            });
            $.ajax({
                type: "post",
                url: "../../../../Web/UserDetail/DeleteMyCollection",
                dataType: "json",
                data: JSON.stringify({ "ids": self.base.ids }),
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    self.refresh();
                }
            });
        }
    }
});