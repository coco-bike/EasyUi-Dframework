var vm_models = new Vue({
    el: '.modelInfo',
    data: {
        self: '',
        items: []
    },
    mounted: function () {
        self = this;
        self.refresh();
        self.pageBarInitialize();
    },
    methods: {
        refresh: function () {
            $('.line').html('');
            $.ajax({
                type: "post",
                url: "../../../../Web/ShoppingMall/GetPagingList",
                dataType: "json",
                data: JSON.stringify({ "pageIndex": M_MailData.pageIndex, "pageSize": M_MailData.pageSize, 'modelName': M_MailData.modelName, 'typeName': M_MailData.typeName }),
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    page.page_max = Math.ceil(data.Data.TotalCount / M_MailData.pageSize);
                    M_MailData.pageCount = Math.ceil(data.Data.TotalCount / M_MailData.pageSize);
                    page.calculate_page(M_MailData.pageIndex);
                    self.items = data.Data.List;
                }
            });
        },
        jumpDownLoad: function (id) {
            var nickName = GetQueryString("nickName");
            if (nickName == null) { return }
            M_MailData.nickName = GetQueryString("nickName");
            var url = "../../../../Web/DownLoad/index?id=" + id + "&nickName=" + M_MailData.nickName;
            var host = "http://" + window.location.host;//域名
            ChangeUrl(host + url);
            window.location.href = "../../../.." + url;
        },
        pageBarInitialize: function () {
            $('#J_page_wrap').on('click', 'a[id!="J_go_to_page"]', function () {

                var type = $(this).attr('type');
                var pageIndex;
                switch (type) {
                    case "previousPage":
                        M_MailData.pageIndex = page.page_now - 1 < 1 ? 1 : page.page_now - 1;
                        break;
                    case "lastPage":
                        M_MailData.pageIndex = page.page_now + 1 > page.page_max ? page.page_max : page.page_now + 1;
                        break;
                    default:
                        M_MailData.pageIndex = $(this).attr('data');
                        break;
                }
                self.refresh();
            });
        }
    }
});