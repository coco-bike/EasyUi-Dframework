 var json = [
        {
            "name": "天文",
            "id": "3",
            "list": [
                {
                    "name": "星空",
                    "id": "5",
                },
                {
                    "name": "天体",
                    "id": "6",
                },
                     {
                         "name": "航天",
                         "id": "7",
                     },
            ]
        },
        {
            "name": "生态",
            "id": "8",
            "list": [
                {
                    "name": "地理",
                    "id": "9",
                },
                  {
                      "name": "环境",
                      "id": "10",
                  },
                     {
                         "name": "生物",
                         "id": "11",
                     }
            ]
        },
           {
               "name": "物理",
               "id": "12",
               "list": [
                   {
                       "name": "光声电力热",
                       "id": "13",
                   }
               ]
           },
         {
             "name": "化学",
             "id": "14",
             "list": [
                 {
                     "name": "微观",
                     "id": "15",
                 },
                    {
                        "name": "化工",
                        "id": "17",
                    },
                       {
                           "name": "实验",
                           "id": "18",
                       },
             ]
         },
         {
             "name": "医学",
             "id": "19",
             "list": [
                 {
                     "name": "医疗",
                     "id": "20",
                 },
                  {
                      "name": "人体",
                      "id": "21",
                  }
             ]
         },
         {
             "name": "艺术",
             "id": "22",
             "list": [
                 {
                     "name": "美术",
                     "id": "23",
                 },
                   {
                       "name": "乐器",
                       "id": "24",
                   },
                     {
                         "name": "建筑",
                         "id": "25",
                     }
             ]
         },
            {
                "name": "生活",
                "id": "26",
                "list": [
                    {
                        "name": "交通",
                        "id": "27",
                    },
                        {
                            "name": "起居",
                            "id": "28",
                        },
                            {
                                "name": "用品",
                                "id": "29",
                            }
                ]
            },
            {
                "name": "其他",
                "id": "30",
                "list": [
                    {
                        "name": "工具",
                        "id": "32",
                    },
                           {
                               "name": "场景",
                               "id": "33",
                           }
                ]
            }
    ];
/*递归实现获取无级树数据并生成DOM结构*/
 var str = "";
//var str = "<div><strong><img src='/Imgs/nav/home1.png'/></strong><a><span>首页<span></a></div>";
var forTree = function (o) {
    for (var i = 0; i < o.length; i++) {
        var urlstr = "";
        try {
            urlstr = "<div>" + "<i class='left1' type='1'></i><a id=" + o[i]["id"] + "\"" + " name=" + "\"" + o[i]["name"] + "\"" + "onclick='setType("+"\""+o[i]["name"]+"\""+')\''+"><span>" + o[i]["name"] + "</span></a><ul>";
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
            $(this).removeClass('left1').removeClass('left2').addClass('down1').removeClass('down2');
            $(this).attr('type', 2);
        }
        else if ($(this).attr('type') == 2) {
            $(this).removeClass('down1').removeClass('down2').removeClass('left1').addClass('left2');
            $(this).attr('type', 1);
        }
        if (ul.find("div").html() != null) {
            if (ul.css("display") == "none") {
                ul.slideDown(300);
            } else {
                ul.slideUp(300);
            }
        }

    })

}();

function setType(typeName) {
    addModels.typeName = typeName;
}
                 