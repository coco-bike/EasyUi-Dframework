//表格
Vue.component('com_table', {
    template: '#com_form',
    props: ['rows'],
    methods: {
        enDisableUser: function (id, type) {
            $.ajax({
                type: "post",
                url: "../../admin/userManage/EnDisableUser",
                dataType: "json",
                data: JSON.stringify({ "id": id, "type": type }),
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    if (data.Status == 1) {
                        refresh();
                    } else {
                        alert("操作失败");
                    }
                }
            });
        },
        verify: function (id) {
            M_verifyForm.seen = true;
            ajax("../../admin/userManage/ReadVerifyInfo", { "id": id }, function (data) {
                if (data.Status == 1) {
                    M_verifyForm.id = data.Data.Id;
                    M_verifyForm.uName = data.Data.UName;
                    M_verifyForm.telNumber = data.Data.TelNumber;
                    M_verifyForm.eMail = data.Data.EMail;
                    M_verifyForm.organizeName = data.Data.OrganizeName;
                    M_verifyForm.roleId = data.Data.RoleId;
                    M_verifyForm.levelId = data.Data.LevelId;
                    M_verifyForm.organizeId = data.Data.OrganizeId;
                }
            });
        }
    }
})

new Vue({
    el: '.verifyMask',
    data: M_verifyForm,
    methods: {
        verifySuccess: function () {
            ajax("../../admin/userManage/VerifyUser", { "id": this.id, "uName": this.uName, "telNumber": this.telNumber, "eMail": this.eMail, "organizeName": this.organizeName, "organizeId": this.organizeId, "roleId": this.roleId, "levelId": this.levelId,"status":2 }, function (data) {
                if (data.Status == 1) {
                    refresh();
                    M_verifyForm.seen = false;
                } else {
                    alert("审核失败");
                }
            });
        },
        verifyFail: function () {
            ajax("../../admin/userManage/VerifyUser", { "id": this.id, "uName": this.uName, "telNumber": this.telNumber, "eMail": this.eMail, "organizeName": this.organizeName, "organizeId": this.organizeId, "roleId": this.roleId, "levelId": this.levelId, "status": 0 }, function (data) {
                if (data.Status == 1) {
                    refresh();
                    M_verifyForm.seen = false;
                } else {
                    alert("审核失败");
                }
            });
        },
        cancel: function () {
            this.seen = false;
        }
    },
    watch: {
        telNumber: function () {
            this.telNumber =this.telNumber.replace(/[^\d]/g, '');//e.key当该dom元素获取焦点时，每次输入的键盘值
            this.telNumber = this.telNumber.slice(0, 11);
            return this.telNumber;
        }
    }
});

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
    el: '.u-body',
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
        url: "../../admin/userManage/getpaginglist",
        dataType: "json",
        data: JSON.stringify({ "pageIndex": M_formData.pageIndex, "pageSize": M_formData.pageSize, 'nickName': name }),
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            page.page_max = Math.ceil(data.Data.TotalCount / M_formData.pageSize);
            page.calculate_page(M_formData.pageIndex);
            for (var i = 0; i < data.Data.List.length; i++) {
                M_formData.formData.push(data.Data.List[i]);
            }
        },
        complete: function () {
            //m_loading.loading_seen = false;
        }
    });
}
function ajax(url,data,success) {
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