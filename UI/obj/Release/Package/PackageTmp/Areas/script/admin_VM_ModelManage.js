//Loading组件
var vm_player = new Vue({
    el: '.layer',
    data: m_loading,
    methods: {

    }
})
//Loading组件     end
//页面刷新
function refresh(searchName) {
    m_loading.loading_seen = true;
    var name = searchName == null ? '' : searchName;
    M_formData.formData = [];
    $.ajax({
        type: "post",
        url: "../../admin/models/getpaginglist",
        dataType: "json",
        data: JSON.stringify({ "pageIndex": M_formData.pageIndex, "pageSize": M_formData.pageSize, 'name': name }),
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

//页码框
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
    el: '.b-body',
    data: M_formData,
    methods: {
        upShop: function () {

        },
        addModels: function () {
            $.ajax({
                type: "post",
                url: "../../admin/models/GetTags",
                dataType: "json",
                data: JSON.stringify({}),
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    if (data.Status == 1) {
                        for (var i = 0; i < data.Data.length; i++) {
                            var j = '{"class":"noSelect","tagName":"' + data.Data[i].TagName + '"}';
                            addModels.tagArr.push(JSON.parse(j));
                        }
                    }
                }
            });
            $('.mask').css('visibility', 'visible');
            addFormEmpty();
        },
        search: function () {
            refresh(M_formData.searchName);
        }
    }
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
                            refresh();
                        }
                    }
                });
            }
            else {
                return;
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
                        refresh(M_formData.searchName);
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
                        refresh(M_formData.searchName);
                    }
                }
            });
        },
        //模型修改读
        updateReadModel: function (id) {
            addFormEmpty();
            $('.mask').css('visibility', 'visible');
            $.ajax({
                type: "post",
                url: "../../admin/models/updateReadModel",
                dataType: "json",
                data: JSON.stringify({ "id": id }),
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    if (data.Status == 1) {
                        console.log(data.Data);
                        addModels.id = data.Data.Goods.Id;
                        addModels.name = data.Data.Goods.GoodsName;
                        addModels.company = data.Data.Goods.Company;
                        addModels.description = data.Data.Goods.Description;
                        addModels.thumbUrl = data.Data.Goods.ThumbUrl;
                        addModels.picUrls.push(data.Data.Goods.ImgUrl1);
                        addModels.picUrls.push(data.Data.Goods.ImgUrl2);
                        addModels.picUrls.push(data.Data.Goods.ImgUrl3);
                        addModels.modelUrl = data.Data.Goods.ModelUrl;
                        addModels.typeName = data.Data.Goods.TypeModel.TypeName;
                        //获取该模型选中的所有标签
                        for (var i = 0; i < data.Data.SelTags.length ; i++) {
                            var tagName = data.Data.SelTags[i].TagName;
                            addModels.tagArrSub.push(tagName);
                        }
                        var tagArrTemp = [];//获取所有标签
                        for (var i = 0; i < data.Data.Tags.length; i++) {
                            var tagName = data.Data.Tags[i].TagName;
                            tagArrTemp.push(tagName);
                        }
                        for (var i = 0; i < data.Data.Tags.length; i++) {
                            if (addModels.tagArrSub.indexOf(tagArrTemp[i]) == -1) {
                                var j = '{"class":"noSelect","tagName":"' + tagArrTemp[i] + '"}';
                                addModels.tagArr.push(JSON.parse(j));
                            }
                            else {
                                var j = '{"class":"select","tagName":"' + tagArrTemp[i] + '"}';
                                addModels.tagArr.push(JSON.parse(j));
                            }
                        }

                    }
                },
                complete: function () {

                }
            });

        }
    }
})
//清空新增表单
function addFormEmpty() {
    document.getElementById('addModelForm').reset();
    $(".progressBar").val(0);
    $('.progressBar').attr('max', 0);
    $('.percentage').text('');
    addModels.name = '';
    addModels.company = '';
    addModels.description = '';
    addModels.thumbUrl = '';
    addModels.imgUrl = '';
    addModels.modelUrl = '';
    addModels.typeName = '';
    addModels.tagArrSub = [];
    addModels.tagArr = [];
    addModels.tag = '';
    addModels.id = '';
    $('.tagForm a').css('background-color', 'rgba(30,144,255,.3)')
}



var app = new Vue({
    el: '.mask',
    data: addModels,
    mounted: function () {
        this.self = this;
    },
    methods: {
        submit: function () {
            if (this.name == '' || this.company == '' || this.typeName == '' || this.tagArrSub.length == 0 || this.thumbUrl == '' || this.picUrls.length < 3 || this.modelUrl == '') {
                alert('尚有必填项未填写，禁止提交');
                return;
            }
            $.ajax({
                type: "post",
                url: "../../admin/models/AddModel",
                dataType: "json",
                data: JSON.stringify({ 'id': this.id, 'name': this.name, 'company': this.company, 'description': this.description, 'ThumbUrl': this.thumbUrl, 'PicUrls': this.picUrls, 'ModelUrl': this.modelUrl, "typeName": this.typeName, "tagArr": this.tagArrSub }),
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    if (data.Status == 1) {
                        M_formData.formData = [];
                        $('.mask').css('display', 'none');
                        $.ajax({
                            type: "post",
                            url: "../../admin/models/getpaginglist",
                            dataType: "json",
                            data: JSON.stringify({ "pageIndex": M_formData.pageIndex, "pageSize": 20, 'name': M_formData.searchName }),
                            contentType: "application/json;charset=utf-8",
                            success: function (data) {
                                location.reload();
                                page.page_max = Math.ceil(data.Data.TotalCount / 20);
                                page.calculate_page(1);
                                for (var i = 0; i < data.Data.GoodsList.length; i++) {
                                    M_formData.formData.push(data.Data.GoodsList[i]);
                                }
                            }
                        });
                    }
                }
            })
        },
        cancel: function () {
            $('.mask').css('visibility', 'hidden');
        },
        addTag: function () {
            if (addModels.tag != '') {
                $.ajax({
                    type: "post",
                    url: "../../admin/models/AddTag",
                    dataType: "json",
                    data: JSON.stringify({ "tagName": addModels.tag }),
                    contentType: "application/json;charset=utf-8",
                    success: function (data) {
                        if (data.Status == 1) {
                            var j = '{"class":"noSelect","tagName":"' + addModels.tag + '"}';
                            addModels.tagArr.push(JSON.parse(j));
                            addModels.tag = '';
                        }
                    }
                });
            }
        },
        selTag: function (e) {
            if ($(e.currentTarget).attr('class') == 'noSelect') {
                var tagName = $(e.currentTarget).attr('name');
                var index;
                for (var i = 0; i < addModels.tagArr.length; i++) {
                    if (addModels.tagArr[i].tagName == tagName) {
                        index = i;
                    }
                }
                addModels.tagArrSub.push(tagName);
                addModels.tagArr[index].class = 'select';
            } else {
                var tagName = $(e.currentTarget).attr('name');
                if (tagName != '' && tagName != null) {
                    var index;
                    for (var i = 0; i < addModels.tagArr.length; i++) {
                        if (addModels.tagArr[i].tagName == tagName) {
                            index = i;
                        }
                    }
                    var j = '{"class":"noSelect","tagName":"' + tagName + '"}';
                    addModels.tagArrSub.splice(addModels.tagArrSub.indexOf(tagName), 1);
                    addModels.tagArr[index].class = 'noSelect';
                }
            }
        },
        delTag: function (e) {
            var temp = $(e.currentTarget);
            var tagName = $(e.currentTarget).attr('name');
            var className = $(e.currentTarget).siblings().attr('class');
            $.ajax({
                type: "post",
                url: "../../admin/models/DelTag",
                dataType: "json",
                data: JSON.stringify({ "tagName": tagName }),
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    if (data.Status == 1) {
                        var res1 = addModels.self.searchByTag(addModels.tagArr, tagName);//存在则返回索引，不存在返回-1
                        var res2 = addModels.self.searchByTag(addModels.tagArrSub,tagName);
                        if (res1>-1) {
                            addModels.tagArr.splice(res1, 1);
                        }
                        if (res2>-1) {
                            addModels.tagArrSub.splice(res2, 1);
                        }
                    }
                    else if (data.Status == 6) {
                        alert("该标签下关联有模型");
                    }
                }
            });
        },
        searchByTag: function (arr, tagName) {
            if (arr==null||arr==undefined) { return -1 };
            for (var i = 0; i < arr.length; i++) {
                if (arr[i].tagName == tagName) {
                    return i;
                }
            }
            return -1;
        },
        thumbUpload: function (e) {
            $('#thumbTag').val(0);
            try {
                var fileObj = document.getElementById("thumbUpload").files[0]; // 获取文件对象
            }
            catch (e) {
                alert(e);
            }
            //$(e.currentTarget).siblings('.filename').val(fileObj.name);
            //addModels.thumbUrl = fileObj.name;
            //文件格式判断
            var fileName = fileObj.name.split('.');
            stuffic = fileName[fileName.length - 1];
            if (!(stuffic == 'jpg' || stuffic == 'png')) {
                return;
            }
            process.index = 0;
            var FileController = "../../admin/models/UploadThumb";//上传后台地址
            var form = new FormData();
            form.append("file", fileObj);                           // 文件对象
            var xhr = new XMLHttpRequest();
            xhr.open("post", FileController, true);
            $('#thumbTag').val(parseInt($('#thumbTag').val()) + 1);
            xhr.onreadystatechange = callBack;
            xhr.onload = process.uploadComplete; //请求完成
            xhr.onerror = process.uploadFailed; //请求失败
            xhr.upload.onprogress = process.progressFunction;//【上传进度调用方法实现】
            xhr.upload.onloadstart = function () {//上传开始执行方法
                ot = new Date().getTime();   //设置上传开始时间
                oloaded = 0;//设置上传开始时，以上传的文件大小为0
            };
            xhr.send(form);
            function callBack() {
                if (xhr.readyState == 4 && xhr.status == 200) {
                    var textData = xhr.responseText;
                    var obj = JSON.parse(textData);
                    if (obj.Status == 1 && (obj.Data != null && obj.Data != '')) {
                        $('#thumbTag').val(parseInt($('#thumbTag').val()) - 1);
                        addModels.thumbUrl = obj.Data;
                    }
                }
            }
        },
        //picUpload: function (e) {
        //    $('#picTag').val(0);
        //    var fileObj = document.getElementById("picUpload").files[0]; // 获取文件对象
        //    //$(e.currentTarget).siblings('.filename').val(fileObj.name);
        //    //addModels.imgUrl = fileObj.name;
        //    //文件格式判断
        //    var fileName = fileObj.name.split('.');
        //    stuffic = fileName[fileName.length - 1];
        //    if (!(stuffic == 'jpg' || stuffic == 'png')) {
        //        return;
        //    }
        //    process.index = 1;
        //    var FileController = "../../admin/models/UploadPic";//上传后台地址
        //    var form = new FormData();
        //    form.append("file", fileObj);                           // 文件对象
        //    var xhr = new XMLHttpRequest();
        //    xhr.open("post", FileController, true);
        //    $('#picTag').val(parseInt($('#picTag').val()) + 1);
        //    xhr.onreadystatechange = callBack;
        //    xhr.onload = process.uploadComplete; //请求完成
        //    xhr.onerror = process.uploadFailed; //请求失败
        //    xhr.upload.onprogress = process.progressFunction;//【上传进度调用方法实现】
        //    xhr.upload.onloadstart = function () {//上传开始执行方法
        //        ot = new Date().getTime();   //设置上传开始时间
        //        oloaded = 0;//设置上传开始时，以上传的文件大小为0
        //    };
        //    xhr.send(form);
        //    function callBack() {
        //        if (xhr.readyState == 4 && xhr.status == 200) {
        //            var textData = xhr.responseText;
        //            var obj = JSON.parse(textData);
        //            if (obj.Status == 1 && (obj.Data != null && obj.Data != '')) {
        //                $('#picTag').val(parseInt($('#picTag').val()) - 1);
        //                //$('#picUrlSave').val(obj.Data);
        //                addModels.imgUrl = obj.Data;
        //            }
        //        }
        //    }
        //},
        modelUpload: function (e) {
            $('#modelTag').val(0);
            var fileObj = document.getElementById("modelUpload").files[0]; // 获取文件对象
            //$(e.currentTarget).siblings('.filename').val(fileObj.name);
            //addModels.modelUrl = fileObj.name;
            //文件格式判断
            var fileName = fileObj.name.split('.');
            stuffic = fileName[fileName.length - 1];
            //if (!(stuffic == 'jpg' || stuffic == 'png')) {
            //    return;
            //}
            process.index = 1;
            var FileController = "../../admin/models/UploadModel";//上传后台地址
            var form = new FormData();
            form.append("file", fileObj);                           // 文件对象
            var xhr = new XMLHttpRequest();
            xhr.open("post", FileController, true);
            $('#modelTag').val(parseInt($('#modelTag').val()) + 1);
            xhr.onreadystatechange = callBack;
            xhr.onload = process.uploadComplete; //请求完成
            xhr.onerror = process.uploadFailed; //请求失败
            xhr.upload.onprogress = process.progressFunction;//【上传进度调用方法实现】
            xhr.upload.onloadstart = function () {//上传开始执行方法
                ot = new Date().getTime();   //设置上传开始时间
                oloaded = 0;//设置上传开始时，以上传的文件大小为0
            };
            xhr.send(form);
            function callBack() {
                if (xhr.readyState == 4 && xhr.status == 200) {
                    var textData = xhr.responseText;
                    var obj = JSON.parse(textData);
                    if (obj.Status == 1 && (obj.Data != null && obj.Data != '')) {
                        $('#modelTag').val(parseInt($('#modelTag').val()) - 1);
                        //$('#modelUrlSave').val(obj.Data);
                        addModels.modelUrl = obj.Data;
                    } else if (obj.Status == 5) {
                        $('#modelProBar').attr('max', '0');
                        $('#modelProBar').attr('value', '0');
                        $('#modelPer').text("");
                        alert('服务器已存在该文件');
                    }
                }
            }
        }
    }
})
//=====================进度条=====================
var process = {
    index: '',
    progressFunction: function (evt) {
        var progressBar = $('.progressBar')[parseInt(process.index)];
        var percentageDiv = $('.percentage')[parseInt(process.index)];
        if (evt.lengthComputable) {
            progressBar.max = evt.total;//进度条颜色进度
            progressBar.value = evt.loaded;
            percentageDiv.innerHTML = Math.round(evt.loaded / evt.total * 100) + "%";
            if (Math.round(evt.loaded / evt.total * 100) >= 100) {
                //$('#remark').css('display', 'block');
            }
        }
    },
    uploadComplete: function (evt) {

    },
    uploadFailed: function (evt) {

    }
}