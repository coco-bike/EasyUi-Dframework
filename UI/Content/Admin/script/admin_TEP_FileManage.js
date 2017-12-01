//表格
Vue.component('com_table', {
    template: '#com_form',
    props: ['rows'],
    methods: {
        verifyFile: function (id, type) {
            ajax("../../admin/fileManage/verifyFile", { "id": id, "type": type }, function (data) {
                if (data.Status == 1) {
                    refresh();
                }
            });
        }
    }
})