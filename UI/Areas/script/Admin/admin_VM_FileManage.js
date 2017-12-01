//表格
Vue.component('com_table', {
    template: '#com_form',
    props: ['rows'],
    methods: {
        verifyFile: function (id,type) {
            ajax("../../admin/fileManage/verifyFile", { "id": id,"type":type }, function (data) {
                if (data.Status == 1) {
                    refresh();
                }
            });
        }
    }
})

new Vue({
    mounted: function () {
        $('#J_page_wrap').on('click', 'a[id!="J_go_to_page"]', function () {
            var pageIndex = $(this).attr('data');
            $(this).addClass('cur').siblings().removeClass('cur');
            M_formData.pageIndex = pageIndex;
            refresh(M_formData.searchName);
        });
        refresh();
    },
    el: '.f-body',
    data: M_formData,
    mounted: function () {
        refresh(M_formData.searchName);
    },
    methods: {

    }
})

//页面刷新
function refresh(searchName) {
    //m_loading.loading_seen = true;
    var name = searchName == null ? '' : searchName;
    M_formData.formData = [];
    $.ajax({
        type: "post",
        url: "../../admin/fileManage/getpaginglist",
        dataType: "json",
        data: JSON.stringify({ "pageIndex": M_formData.pageIndex, "pageSize": M_formData.pageSize, 'fileName': name }),
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            page.page_max = Math.ceil(data.Data.TotalCount / M_formData.pageSize);
            page.calculate_page(M_formData.pageIndex);
            console.log(data);
            for (var i = 0; i < data.Data.List.length; i++) {
                M_formData.formData.push(data.Data.List[i]);
            }
        },
        complete: function () {
            //m_loading.loading_seen = false;
        }
    });
}
function ajax(url, data, success) {
    $.ajax({
        type: "post",
        url: url,
        dataType: "json",
        data: JSON.stringify(data),
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            success(data)
        }
    });
}
