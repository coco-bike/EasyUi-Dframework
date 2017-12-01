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
 var str = "<div><i class='home1' type='1'></i><a onclick=getListByType('首页')><span>首页<span></a></div>";
//var str = "<div><strong><img src='/Imgs/nav/home1.png'/></strong><a><span>首页<span></a></div>";
var forTree = function (o) {
    for (var i = 0; i < o.length; i++) {
        var urlstr = "";
        try {
            //if (o[i].list) {
            //    if (typeof o[i]["url"] == "undefined") {
            //        urlstr = "<div><strong><i class='left2' type='1'></i></strong><a href='#'><span>" + o[i]["name"] + "</span></a><ul>";
            //    } else {
            //        urlstr = "<div><strong><i class='left2' type='1'></i></strong>" + "<a href=" + o[i]["url"] + "><span>" + o[i]["name"] + "</span></a><ul>";
            //    }
            //}
            //else {
            //    if (typeof o[i]["url"] == "undefined") {
                    //urlstr = "<div><a href='#'><i class='left2' type='1'></i><span>" + o[i]["name"] + "</span></a><ul>";
                //} else {
            urlstr = "<div>" + "<i class='left1' type='1'></i><a onclick=getListByType('" + o[i]["name"] + "')><span>" + o[i]["name"] + "</span></a><ul>";
                //}
            //}
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
                // $(this).html("[-]" + spanContent);
            } else {
                ul.hide(300);
                // $(this).html("[+] " + spanContent);
            }
        }

    })

}();