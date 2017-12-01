var app = new Vue({
    el: '.downLoad',
    data: M_DownLodData,
    mounted: function () {
        self = this;
        self.refresh();
        var i = 0;
        setInterval(function () {//图片自动切换
            if (i >= 3) { i = 0 }
            app.btnChange(i, $('.change-btn').children().eq(i).children());
            ++i;
        }, 2000);
    },
    methods: {
        //收藏与取消收藏
        collectModel: function () {
            $.ajax({
                type: "post",
                url: "../../../../Web/DownLoad/CollectModel",
                dataType: "json",
                data: JSON.stringify({ 'id': this.id, 'nickName': this.nickName, "type": this.whetherCollect }),
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    if (data.Status == 1) {
                        $('.whetherCollect').removeClass('no-collectBtn').addClass('collectBtn');
                        this.whetherCollect = true;
                        self.refresh();
                    }
                }
            });
            //ajax("../../../../Web/DownLoad/CollectModel", { 'id': this.id, 'nickName': this.nickName, "type": this.whetherCollect }, function () {
            //            if (data.Status == 1) {
            //                $('.whetherCollect').removeClass('no-collectBtn').addClass('collectBtn');
            //                this.whetherCollect = true;
            //                self.refresh();
            //            } 
            //});
        },
        //图片手动切换
        btnChange: function (index, e) {
            var obj = e.currentTarget == null ? e : e.currentTarget;
            $(obj).parent().siblings().children().removeClass('change-btn-select');
            $(obj).addClass('change-btn-select');
            if (index == 0) {
                $('.pic-change li').eq(0).css('display', 'block');
                $('.pic-change li').eq(0).siblings('.pic').css('display', 'none')
            } else if (index == 1) {
                $('.pic-change li').eq(1).css('display', 'block');
                $('.pic-change li').eq(1).siblings('.pic').css('display', 'none')
            } else if (index == 2) {
                $('.pic-change li').eq(2).css('display', 'block');
                $('.pic-change li').eq(2).siblings('.pic').css('display', 'none')
            }
        },
        //下载模型
        downModel: function () {
            //$.ajax({
            //    type: "post",
            //    url: "../../../../Web/DownLoad/DownLodModel",
            //    dataType: "json",
            //    data: JSON.stringify({ 'id': this.id }),
            //    contentType: "application/json;charset=utf-8",
            //    success: function (data) {
            //        if (data.Status == 1) {
            //            //var host = "http://" + window.location.host;//域名
            //            var host = "http://" + window.location.host;//域名
            //            var filePath = data.Data;
            //            //DownLoad(host + filePath);
            //        }
            //    }
            //});
            this.ajax("../../../../Web/DownLoad/DownLodModel", { 'id': this.id,'nickName':this.nickName }, function (data) {
                if (data.Status == 1) {
                            if (data.Status == 1) {
                                //var host = "http://" + window.location.host;//域名
                                var host = "http://" + window.location.host;//域名
                                var filePath = data.Data;
                                DownLoad(host + filePath);
                            }
                }
            });
        },
        GetQueryString: function (name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        },
        refresh: function (searchName) {
            this.id = this.GetQueryString("id");
            this.nickName = this.GetQueryString("nickName");
            $.ajax({
                type: "post",
                url: "../../../../Web/DownLoad/GetModel",
                dataType: "json",
                data: JSON.stringify({ 'id': this.id, 'nickName': this.nickName }),
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    $('#tags').text('');
                    $('#modelName').text(data.Data.GoodsInfo.GoodsName);
                    $('#modelIntroduce').text(data.Data.GoodsInfo.Description);
                    var host = "http://" + window.location.host;
                    var img1File = host + data.Data.GoodsInfo.ImgUrl1;
                    var img2File = host + data.Data.GoodsInfo.ImgUrl2;
                    var img3File = host + data.Data.GoodsInfo.ImgUrl3;
                    $('#pic1').attr('src', img1File);
                    $('#pic2').attr('src', img2File);
                    $('#pic3').attr('src', img3File);
                    $('#company').text(data.Data.GoodsInfo.Company);
                    var stringBuilder = '';
                    for (var i = 0; i < data.Data.GoodsInfo.TagModels.length ; i++) {
                        stringBuilder += '<li><a>' + data.Data.GoodsInfo.TagModels[i].TagName + '</a></li>';
                    }
                    $('#tags').append(stringBuilder);
                    if (data.Data.WhetherCollect) {
                        app.whetherCollect = true;
                        $('.whetherCollect').removeClass('no-collectBtn').addClass('collectBtn');
                    } else {
                        app.whetherCollect = false;
                        $('.whetherCollect').removeClass('collectBtn').addClass('no-collectBtn');
                    }
                }
            });
        },
        ajax: function (url, data, success) {
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
    }
});