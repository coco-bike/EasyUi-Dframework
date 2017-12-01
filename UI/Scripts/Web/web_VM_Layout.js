$(function () {
    domInitialize();
    pageBarInitialize();
    refresh();
});


function domInitialize() {
    $('.menuTree div').hover(function () {
        var jiantou = $(this).children()[0];
        $(jiantou).children('i').css('color', '#ffb900');
    }, function () {
        var jiantou = $(this).children()[0];
        $(jiantou).children('i').css('color', '#fff');
    });

    $('.search input').focus(function () {
        $('.search').css('border', '3px solid #3076BB');
        $('.search button').css('background-image', 'url("../../Imgs/search2.jpg")');
    })

    $('.search input').blur(function () {
        $('.search').css('border', '3px solid #666');
        $('.search button').css('background-image', 'url("../../Imgs/search1.jpg")');
    })

    $('#menuTree div:not(:first-child)').hover(function () {
        if ($(this).children('i').attr('type') == 1) {
            $(this).children('i').removeClass('left1').addClass('left2');
        } else if ($(this).children('i').attr('type') == 2) {
            $(this).children('i').removeClass('down1').addClass('down2');
        }

    }, function () {
        if ($(this).children('i').attr('type') == 1) {
            $(this).children('i').removeClass('left2').addClass('left1');
        } else if ($(this).children('i').attr('type') == 2) {
            $(this).children('i').removeClass('down2').addClass('down1');
        }
    })
    $('#menuTree div:first-child').hover(function () {
        $(this).children('i').removeClass('home1').addClass('home2');
    }, function () {
        $(this).children('i').removeClass('home2').addClass('home1');
    })
    $('#menuTree ul div').hover(function () {
        $(this).children('i').removeClass('left1').addClass('left2');
    }, function () {
        $(this).children('i').removeClass('left2').addClass('left1');
    })
}

function pageBarInitialize() {
    $('#J_page_wrap').on('click', 'a[id!="J_go_to_page"]', function () {
        if ($(this).attr('data') == "01") {
            M_MailData.pageIndex = M_MailData.pageIndex - 1 < 1 ? 1 : M_MailData.pageIndex - 1;
        } else if ($(this).attr('data') == "02") {
            M_MailData.pageIndex = M_MailData.pageIndex + 1 > M_MailData.pageCount ? M_MailData.pageCount : M_MailData.pageIndex + 1;
        } else {
            M_MailData.pageIndex = $(this).attr('data');
        }
        refresh();
    });
}



function search() {
    M_MailData.modelName = $('#searchName').val();
    refresh();
}

function getListByType(typeName) {
    if (typeName == '首页') {
        M_MailData.typeName = '';
    }
    else {
        M_MailData.typeName = typeName;
    }
    refresh();
}

function refresh() {
    $('.line').html('');
    $.ajax({
        type: "post",
        url: "../../../../ShoppingMall/GetPagingList",
        dataType: "json",
        data: JSON.stringify({ "pageIndex": M_MailData.pageIndex, "pageSize": M_MailData.pageSize, 'modelName': M_MailData.modelName, 'typeName': M_MailData.typeName }),
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            page.page_max = Math.ceil(data.Data.TotalCount / M_MailData.pageSize);
            M_MailData.pageCount = Math.ceil(data.Data.TotalCount / M_MailData.pageSize);
            page.calculate_page(M_MailData.pageIndex);
            var stringBuilder = '';
            for (var i = 0; i < data.Data.GoodsList.length; i++) {
                stringBuilder += '<div onclick="jumpDownLoad(\'' + data.Data.GoodsList[i].Id + '\')"><img style="width:77px;height:78px;" src="../../../' + data.Data.GoodsList[i].ThumbUrl + '" /><p class="introduce"><span>' + data.Data.GoodsList[i].GoodsName + '</span><br />';
                var count = data.Data.GoodsList[i].TagModels.length > 3 ? 3 : data.Data.GoodsList[i].TagModels.length;
                for (var j = 0; j < count ; j++) {
                    stringBuilder += '<span class="tag">' + data.Data.GoodsList[i].TagModels[j].TagName + '</span>';
                }
                stringBuilder += '<br /><span>' + data.Data.GoodsList[i].Company + '</span></p></div>';
            }
            $('.line').append(stringBuilder);
        }
    });
}

function jumpDownLoad(id){
   var nickName= GetQueryString("nickName");
   if (nickName==null) { return }
    M_MailData.nickName = GetQueryString("nickName");
    var url = "/DownLoad/index?id=" + id + "&nickName=" + M_MailData.nickName;
    var host = "http://" + window.location.host;//域名
    //ChangeUrl(host + url);
    window.location.href ="../../../.."+ url;
}

function GetQueryString (name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}