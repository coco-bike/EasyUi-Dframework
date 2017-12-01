Vue.component('t-loading', {
    template: '#loding-component',
    props: ['seen']
});

//表格
Vue.component('com_table', {
    template: '#com_form',
    props: ['rows'],
    methods: {
        //删除模型
        deleteModel: function (id) {
            if (confirm("确定要删除吗?")) {
                $.ajax({
                    type: "post",
                    url: "../../admin/models/DeleteModel",
                    dataType: "json",
                    data: JSON.stringify({ "id": id }),
                    contentType: "application/json;charset=utf-8",
                    success: function (data) {
                        if (data.Status == 1) {
                            M_formData.formData = [];
                            vm_body.refresh();
                        }
                    }
                });
            }
        },
        //模型上架
        upModel: function (id) {
            $.ajax({
                type: "post",
                url: "../../admin/models/UpModel",
                dataType: "json",
                data: JSON.stringify({ "id": id }),
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    if (data.Status == 1) {
                        vm_body.refresh();
                    }
                }
            });
        },
        //模型下架
        downModel: function (id) {
            $.ajax({
                type: "post",
                url: "../../admin/models/DownModel",
                dataType: "json",
                data: JSON.stringify({ "id": id }),
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    if (data.Status == 1) {
                        vm_body.refresh();
                    }
                }
            });
        },
        //模型修改读
        updateReadModel: function (id) {
            M_formData.addOrUpdate = true;
            vm_body.addFormEmpty();
            //$('.mask').css('visibility', 'visible');
            addModels.maskStyle.visibility = "visible";
            $.ajax({
                type: "post",
                url: "../../admin/models/updateReadModel",
                dataType: "json",
                data: JSON.stringify({ "id": id }),
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    if (data.Status == 1) {
                        addModels.id = data.Data.Goods.Id;
                        addModels.name = data.Data.Goods.GoodsName;
                        addModels.oldName = data.Data.Goods.GoodsName;
                        addModels.company = data.Data.Goods.Company;
                        addModels.description = data.Data.Goods.Description;
                        addModels.thumbUrl = data.Data.Goods.ThumbUrl;
                        addModels.picUrls.push(data.Data.Goods.ImgUrl1);
                        addModels.picUrls.push(data.Data.Goods.ImgUrl2);
                        addModels.picUrls.push(data.Data.Goods.ImgUrl3);
                        addModels.modelUrl = data.Data.Goods.ModelUrl;
                        addModels.typeName = data.Data.Goods.TypeModel.TypeName;
                        //获取该模型选中的所有标签
                        var tagArrSelect = [];
                        for (var i = 0; i < data.Data.SelTags.length ; i++) {
                            var tagName = data.Data.SelTags[i].TagName;
                            tagArrSelect.push(tagName);
                        }
                        var tagArrTemp = [];//获取所有标签
                        for (var i = 0; i < data.Data.Tags.length; i++) {
                            var tagName = data.Data.Tags[i].TagName;
                            tagArrTemp.push(tagName);
                        }
                        for (var i = 0; i < tagArrTemp.length; i++) {
                            if (tagArrSelect.indexOf(tagArrSelect[i]) == -1) {
                                var j = '{"class":"noSelect","tagName":"' + tagArrTemp[i] + '"}';
                                addModels.tagArr.push(JSON.parse(j));
                            }
                            else {
                                var j = '{"class":"select","tagName":"' + tagArrTemp[i] + '"}';
                                addModels.tagArr.push(JSON.parse(j));
                            }
                        }
                    }
                }
            });
        }
    }
})