var json = [
       {
           "name": "天文",
           "list": [
               {
                   "name": "星空",
               },
               {
                   "name": "天体",
               },
                    {
                        "name": "航天",
                    },
           ]
       },
       {
           "name": "生态",
           "list": [
               {
                   "name": "地理",
               },
                 {
                     "name": "环境",
                 },
                    {
                        "name": "生物",
                    }
           ]
       },
          {
              "name": "物理",
              "list": [
                  {
                      "name": "光声电力热",
                  }
              ]
          },
        {
            "name": "化学",
            "list": [
                {
                    "name": "微观",
                },
                   {
                       "name": "化工",
                   },
                      {
                          "name": "实验",
                      },
            ]
        },
        {
            "name": "医学",
            "list": [
                {
                    "name": "医疗",
                },
                 {
                     "name": "人体",
                 }
            ]
        },
        {
            "name": "艺术",
            "list": [
                {
                    "name": "美术",
                },
                  {
                      "name": "乐器",
                  },
                    {
                        "name": "建筑",
                    }
            ]
        },
           {
               "name": "生活",
               "list": [
                   {
                       "name": "交通",
                   },
                       {
                           "name": "起居",
                       },
                           {
                               "name": "用品",
                           }
               ]
           },
           {
               "name": "其他",
               "list": [
                   {
                       "name": "工具",
                   },
                          {
                              "name": "场景",
                          }
               ]
           }
];
/*递归实现获取无级树数据并生成DOM结构*/
var str = "<div><i class='home1' type='3'></i><a onclick=getListByType('首页')><span>首页<span></a></div>";
var forTree = function (o) {
    for (var i = 0; i < o.length; i++) {
        var urlstr = "";
        try {
            urlstr = "<div>" + "<i class='left1' type='1'></i><a onclick=getListByType('" + o[i]["name"] + "')><span>" + o[i]["name"] + "</span></a><ul>";
            str += urlstr;
            if (o[i]["list"] != null) {
                forTree(o[i]["list"]);
            }
            str += "</ul></div>";
        } catch (e) { }
    }
    return str;
}
/*添加无级树*/
document.getElementById("menuTree").innerHTML = forTree(json);

/*树形菜单*/
var menuTree = function () {
    //给有子对象的元素加
    $("#menuTree ul").each(function (index, element) {
        var ulContent = $(element).html();
        var spanContent = $(element).siblings("span").html();
        if (ulContent) {
            $(element).siblings("span").html(spanContent)
        }
    });

    $("#menuTree").find("div i").click(function () {
        var ul = $(this).siblings("ul");
        var spanStr = $(this).html();
        var spanContent = spanStr.substr(3, spanStr.length);
        console.log($(this).attr('type'));
        if ($(this).attr('type') == 1) {
            //$(this).html('<i class="down1" type="2"></i>');
            $(this).removeClass('left1').removeClass('left2').addClass('down1').removeClass('down2');
            $(this).attr('type', 2);
        }
        else if ($(this).attr('type') == 2) {
            //$(this).html('<i class="down1" type="1"></i>');
            $(this).removeClass('down1').removeClass('down2').removeClass('left1').addClass('left2');
            $(this).attr('type', 1);
        }
        if (ul.find("div").html() != null) {
            if (ul.css("display") == "none") {
                ul.show(300);
            } else {
                ul.hide(300);
            }
        }

    })

}();


function getListByType(typeName) {
    if (typeName == '首页') {
        M_MailData.typeName = '';
        location.href = "../../../Web/shoppingmall/index?nickName=" + GetQueryString("nickName");
    }
    else {
        M_MailData.typeName = typeName;
        vm_models.refresh();
    }
}
function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

(function domInitialize() {
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
})();