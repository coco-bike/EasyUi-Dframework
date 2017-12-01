//=====================Loading组件=====================
var vm_player = new Vue({
    el: '.layer',
    data: M_loading,
    methods: {
    }
})
//=====================Loading组件     end=====================


//=====================页面主体=====================
var vm_body = new Vue({
    el: '.b-body',
    data: M_pageData,
    mounted: function () {
        selfA = this;
        ///绑定页码框的页面的点击事件
        $('#J_page_wrap').on('click', 'a[id!="J_go_to_page"]', function () {
            var pageIndex = $(this).attr('data');
            $(this).addClass('cur').siblings().removeClass('cur');
            M_formData.pageIndex = pageIndex;
            selfA.refresh();
        });
        selfA.refresh();
    },
    methods: {
        /// 页面刷新
        /// <param name="searchName">搜索名称</param>
        refresh: function (searchName) {
            M_loading.loading_seen = true;
            //var name = searchName == null ? '' : searchName;
            selfA.formData = [];
            $.ajax({
                type: "post",
                url: "../../admin/modelmanage/getpaginglist",
                dataType: "json",
                data: JSON.stringify({ "pageIndex": selfA.pageIndex, "pageSize": selfA.pageSize, 'name': searchName }),
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    if (data.Status == 1) {
                        page.page_max = Math.ceil(data.Data.TotalCount / selfA.pageSize);
                        page.calculate_page(selfA.pageIndex);
                        for (var i = 0; i < data.Data.List.length; i++) {
                            selfA.formData.push(data.Data.List[i]);
                        }
                    }
                    M_loading.loading_seen = false;//Loading组件消失
                }
            });
        },
        ///新增模型
        addModels: function () {
            selfA.addOrUpdate = false;
            selfA.addFormEmpty();//清空表单
            $.ajax({
                type: "post",
                url: "../../admin/modelmanage/GetTags",
                dataType: "json",
                data: JSON.stringify({}),
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    if (data.Status == 1) {
                        for (var i = 0; i < data.Data.length; i++) {
                            var j = '{"class":"noSelect","tagName":"' + data.Data[i].TagName + '"}';
                            M_addForm.tagArr.push(JSON.parse(j));
                        }
                    }
                }
            });
            M_addForm.maskStyle.visibility = "visible";
        },
        ///查询
        search: function () {
            selfA.refresh(selfA.searchName);
        },
        ////打开新增模态清空表单
        addFormEmpty: function () {
            document.getElementById('addModelForm').reset();
            $(".progressBar").val(0);
            $('.progressBar').attr('max', 0);
            $('.percentage').text('');
            M_addForm.name = '';
            M_addForm.company = '';
            M_addForm.description = '';
            M_addForm.thumbUrl = '';
            M_addForm.imgUrl = '';
            M_addForm.modelUrl = '';
            M_addForm.typeName = '';
            M_addForm.tagArrSub = [];
            M_addForm.tagArr = [];
            M_addForm.tag = '';
            M_addForm.id = '';
            $('.tagForm a').css('background-color', 'rgba(30,144,255,.3)')
        }
    }
});
//=====================页面主体         end=====================

//=====================新增模态=====================
var vm_mask = new Vue({
    el: '.mask',
    data: M_addForm,
    mounted: function () {
        selfB = this;
    },
    methods: {
        ///提交新增表单
        submit: function () {
            var data={};
            var url = "";
            var tagArrSub = [];
            for (var i = 0; i < selfB.tagArr.length; i++) {
                if (selfB.tagArr[i].class == "select") {
                    tagArrSub.push(selfB.tagArr[i].tagName);
                }
            }
            if (selfB.name == '' || selfB.company == '' || selfB.typeName == '' || tagArrSub.length == 0 || selfB.thumbUrl == '' || selfB.picUrls.length < 3 || selfB.modelUrl == '') {
                alert('尚有必填项未填写，禁止提交');
                return;
            }
            if (M_formData.addOrUpdate) {
                var mark = selfB.name == selfB.oldName;
                data = data = { 'id': selfB.id, 'name': selfB.name, 'company': selfB.company, 'description': selfB.description, 'ThumbUrl': selfB.thumbUrl, 'PicUrls': selfB.picUrls, 'ModelUrl': selfB.modelUrl, "typeName": selfB.typeName, "tagArr": tagArrSub, 'mark': mark }
                url = "../../admin/modelmanage/UpdateModel";
            } else {
                data = { 'name': selfB.name, 'company': selfB.company, 'description': selfB.description, 'ThumbUrl': selfB.thumbUrl, 'PicUrls': selfB.picUrls, 'ModelUrl': selfB.modelUrl, "typeName": selfB.typeName, "tagArr": selfB.tagArrSub };
                url = "../../admin/modelmanage/AddModel";
            }
            $.ajax({
                type: "post",
                url: url,
                dataType: "json",
                data: JSON.stringify(data),
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    if (data.Status == 1) {
                        M_formData.formData = [];
                        selfB.maskStyle.visibility = "hidden";
                        vm_body.refresh();
                        var files = uploader.getFiles();
                        for (var i = 0; i < files.length; i++) {
                            uploader.removeFile(files[i]);
                        }
                    }
                    else {
                        alert(data.Data);
                    }
                }
            })
        },
        ///关闭新增模态
        cancel: function () {
            var files = uploader.getFiles();
            for (var i = 0; i < files.length; i++) {
                uploader.removeFile(files[i]);
            }
            selfB.maskStyle.visibility = "hidden";
        },
        ///添加标签
        addTag: function () {
            if (selfB.tag != '') {
                $.ajax({
                    type: "post",
                    url: "../../admin/modelmanage/AddTag",
                    dataType: "json",
                    data: JSON.stringify({ "tagName": selfB.tag }),
                    contentType: "application/json;charset=utf-8",
                    success: function (data) {
                        if (data.Status == 1) {
                            var j = '{"class":"noSelect","tagName":"' + selfB.tag + '"}';
                            selfB.tagArr.push(JSON.parse(j));
                            selfB.tag = '';
                        }
                    }
                });
            }
        },
        ///点击选择标签
        selTag: function (e) {
            if ($(e.currentTarget).attr('class') == 'noSelect') {
                var tagName = $(e.currentTarget).attr('name');
                var index;
                for (var i = 0; i < selfB.tagArr.length; i++) {
                    if (selfB.tagArr[i].tagName == tagName) {
                        index = i;
                    }
                }
                selfB.tagArrSub.push(tagName);
                selfB.tagArr[index].class = 'select';
            } else {
                var tagName = $(e.currentTarget).attr('name');
                if (tagName != '' && tagName != null) {
                    var index;
                    for (var i = 0; i < selfB.tagArr.length; i++) {
                        if (selfB.tagArr[i].tagName == tagName) {
                            index = i;
                        }
                    }
                    var j = '{"class":"noSelect","tagName":"' + tagName + '"}';
                    selfB.tagArrSub.splice(selfB.tagArrSub.indexOf(tagName), 1);
                    selfB.tagArr[index].class = 'noSelect';
                }
            }
        },
        ///删除标签
        delTag: function (e) {
            var temp = $(e.currentTarget);
            var tagName = $(e.currentTarget).attr('name');
            var className = $(e.currentTarget).siblings().attr('class');
            $.ajax({
                type: "post",
                url: "../../admin/modelmanage/DelTag",
                dataType: "json",
                data: JSON.stringify({ "tagName": tagName }),
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    if (data.Status == 1) {
                        var res1 = selfB.searchByTag(selfB.tagArr, tagName);//存在则返回索引，不存在返回-1
                        if (res1 > -1) {
                            selfB.tagArr.splice(res1, 1);
                        }
                    } else  {
                        alert(data.Data);
                    }
                }
            });
        },
        ///
        searchByTag: function (arr, tagName) {
            if (arr == null || arr == undefined) { return -1 };
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
                alert("只能上传后缀名为.jpg或者.png文件");
                return;
            }
            selfB.upload(0, "../../admin/modelmanage/UploadThumb", $('#thumbTag'), fileObj, function (xhr) {
                    if (xhr.readyState == 4 && xhr.status == 200) {
                        var textData = xhr.responseText;
                        var obj = JSON.parse(textData);
                        if (obj.Status == 1 && (obj.Data != null && obj.Data != '')) {
                            $('#thumbTag').val(parseInt($('#thumbTag').val()) - 1);
                            selfB.thumbUrl = obj.Data;
                        }
                    }
            });
        },
        modelUpload: function (e) {
            $('#modelTag').val(0);
            var fileObj = document.getElementById("modelUpload").files[0]; // 获取文件对象
            //$(e.currentTarget).siblings('.filename').val(fileObj.name);
            //addModels.modelUrl = fileObj.name;
            //文件格式判断
            var fileName = fileObj.name.split('.');
            stuffic = fileName[fileName.length - 1];
            if (!(stuffic == 'zip')) {
                alert("只能上传后缀名为.zip文件");
                return;
            }
            selfB.upload(1, "../../admin/modelmanage/UploadModel", $('#modelTag'), fileObj, function (xhr) {
                if (xhr.readyState == 4 && xhr.status == 200) {
                            var textData = xhr.responseText;
                            var obj = JSON.parse(textData);
                            if (obj.Status == 1 && (obj.Data != null && obj.Data != '')) {
                                $('#modelTag').val(parseInt($('#modelTag').val()) - 1);
                                //$('#modelUrlSave').val(obj.Data);
                                selfB.modelUrl = obj.Data;
                            } else if (obj.Status == 5) {
                                $('#modelProBar').attr('max', '0');
                                $('#modelProBar').attr('value', '0');
                                $('#modelPer').text("");
                                alert('服务器已存在该文件');
                            }
                        }
            });
        },
        ///缩略图和模型上传方法
        /// <param name="index">0：缩略图上传，1：模型上传</param>
        /// <param name="$fileTag">文件上传是否成功标志</param>
        /// <param name="fileObj">文件上传控件的js对象</param>
        /// <param name="callBack">回调</param>
        upload: function (index,url, $fileTag,fileObj, callBack) {
            process.index = index;
            var FileController = url;//上传后台地址
            var form = new FormData();
            form.append("file", fileObj);                           // 文件对象
            var xhr = new XMLHttpRequest();
            xhr.open("post", FileController, true);
            $fileTag.val(parseInt($fileTag.val()) + 1);
            xhr.onreadystatechange = call_Back;
            xhr.onload = process.uploadComplete; //请求完成
            xhr.onerror = process.uploadFailed; //请求失败
            xhr.upload.onprogress = process.progressFunction;//【上传进度调用方法实现】
            xhr.upload.onloadstart = function () {//上传开始执行方法
                ot = new Date().getTime();   //设置上传开始时间
                oloaded = 0;//设置上传开始时，以上传的文件大小为0
            };
            xhr.send(form);
            function call_Back() {
                callBack(xhr);
            }
        }
    }
})
//=====================新增模态         end=====================

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