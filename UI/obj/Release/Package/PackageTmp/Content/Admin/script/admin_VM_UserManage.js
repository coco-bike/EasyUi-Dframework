new Vue({
    el: '.verifyMask',
    data: M_verifyForm,
    mounted:function(){
        selfA = this;
    },
    methods: {
        verifySuccess: function () {
            ajax("../../admin/userManage/VerifyUser", { "id": selfA.id, "uName": selfA.uName, "telNumber": selfA.telNumber, "eMail": selfA.eMail, "organizeName": selfA.organizeName, "organizeId": selfA.organizeId, "roleId": selfA.roleId, "levelId": selfA.levelId, "status": 2 }, function (data) {
                if (data.Status == 1) {
                    refresh();
                    selfA.seen = false;
                } else {
                    alert("审核失败");
                }
            });
        },
        verifyFail: function () {
            ajax("../../admin/userManage/VerifyUser", { "id": selfA.id, "uName": selfA.uName, "telNumber": selfA.telNumber, "eMail": selfA.eMail, "organizeName": selfA.organizeName, "organizeId": this.organizeId, "roleId": selfA.roleId, "levelId": selfA.levelId, "status": 0 }, function (data) {
                if (data.Status == 1) {
                    refresh();
                    selfA.seen = false;
                } else {
                    alert("审核失败");
                }
            });
        },
        cancel: function () {
            selfA.seen = false;
        }
    },
    watch: {
        telNumber: function () {
            selfA.telNumber = selfA.telNumber.replace(/[^\d]/g, '');//e.key当该dom元素获取焦点时，每次输入的键盘值
            selfA.telNumber = selfA.telNumber.slice(0, 11);
            return selfA.telNumber;
        }
    }
});

new Vue({
    mounted: function () {
        $('#J_page_wrap').on('click', 'a[id!="J_go_to_page"]', function () {
            var pageIndex = $(this).attr('data');
            $(this).addClass('cur').siblings().removeClass('cur');
            M_formData.pageIndex = pageIndex;
            selfB.refresh();
        });
        selfB.refresh();
    },
    el: '.u-body',
    data: M_formData,
    mounted: function () {
        selfB = this;
        selfB.refresh();
    },
    methods: {
        search:function(){
            selfB.refresh(selfB.searchName );
        },
        refresh: function (searchName) {
            selfB.formData = [];
            $.ajax({
                type: "post",
                url: "../../admin/userManage/getpaginglist",
                dataType: "json",
                data: JSON.stringify({ "pageIndex": selfB.pageIndex, "pageSize": selfB.pageSize, 'nickName': searchName }),
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    page.page_max = Math.ceil(data.Data.TotalCount / selfB.pageSize);
                    page.calculate_page(selfB.pageIndex);
                    for (var i = 0; i < data.Data.List.length; i++) {
                        selfB.formData.push(data.Data.List[i]);
                    }
                }
            });
        }
    }
})