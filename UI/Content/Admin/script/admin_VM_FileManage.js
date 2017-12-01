new Vue({
    el: '.f-body',
    data: M_formData,
    mounted: function () {
        selfA = this;
        $('#J_page_wrap').on('click', 'a[id!="J_go_to_page"]', function () {
            var pageIndex = $(this).attr('data');
            $(this).addClass('cur').siblings().removeClass('cur');
            M_formData.pageIndex = pageIndex;
            selfA.refresh();
        });
        selfA.refresh();
    },
    methods: {
        search:function(){
            selfA.refresh(selfA.searchName);
        },
        refresh: function (searchName) {
            M_formData.formData = [];
            $.ajax({
                type: "post",
                url: "../../admin/fileManage/getpaginglist",
                dataType: "json",
                data: JSON.stringify({ "pageIndex": M_formData.pageIndex, "pageSize": M_formData.pageSize, 'fileName': searchName }),
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    page.page_max = Math.ceil(data.Data.TotalCount / M_formData.pageSize);
                    page.calculate_page(M_formData.pageIndex);
                    for (var i = 0; i < data.Data.List.length; i++) {
                        M_formData.formData.push(data.Data.List[i]);
                    }
                }
            });
        }
    }
})