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