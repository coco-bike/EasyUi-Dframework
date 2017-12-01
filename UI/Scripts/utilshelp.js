var Utils =
{
    //字符串格式化
    StringFormat: function () {
        if (arguments.length == 0)
            return null;
        var str = arguments[0];
        for (var i = 1; i < arguments.length; i++) {
            var re = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
            str = str.replace(re, arguments[i]);
        }
        return str;
    },

    // val为经json直接序列化后的C#的DateTime类型的数据
    FormatTime: function (val, format) {
        if (!val) {
            return "";
        }

        var re = /-?\d+/;
        var m = re.exec(val);
        var d = new Date(parseInt(m[0]));
        // 按【2012-02-13 09:09:09】的格式返回日期

        var defaultFormat = "yyyy-MM-dd hh:mm:ss";
        if (format && typeof format == "string") {
            defaultFormat = format;
        }
        return d.Format(defaultFormat);
    },

    //判断是否是有效的日期格式字符串
    IsDataString: function (strData) {
        /// <summary>
        /// 编辑/新增
        /// </summary>
        /// <param name="strData">需要校验的字符串</param>
        /// <returns></returns>

        //日期格式正则
        var dataFilter = /^([1-9]\d{3}-((0?[1-9])|(1[0-2]))-((0[1-9])|([1-2]?\d)|(3[0-1])))?$/
        return dataFilter.test(strData);
    },

    //判断是否是有效的日期时间格式字符串
    IsDataTimeString: function (strDataTime) {
        /// <summary>
        /// 判断是否是有效的日期时间格式字符串
        /// </summary>
        /// <param name="strDataTime">需要校验的字符串</param>
        /// <returns></returns>

        //日期时间格式正则
        var timeFilter = /^[1-9][0-9]-(0?[1-9]|1[0|1|2])-(0?[1-9]|[1|2][0-9]|3[0|1])s(0?[1-9]|1[0-9]|2[0-3]):(0?[0-9]|[1|2|3|4|5][0-9]):(0?[0-9]|[1|2|3|4|5][0-9])$/;
        return timeFilter.test(strDataTime);
    },

    /// <summary>
    ///将时间转换为c#需要的反序列化的格式 （默认处理时区）
    /// <summary>
    /// <param name="strDateTime">时间</param>
    /// <param name="isDoOffset">bool类型，true处理时区，false处理时区</param>
    GetDateSerializeString: function (strDateTime, isDoOffset) {
        var dataFilter = /^([1-9]\d{3}-((0?[1-9])|(1[0-2]))-((0[1-9])|([1-2]?\d)|(3[0-1])))$/
        if (dataFilter.test(strDateTime)) {
            strDateTime = strDateTime.replace(/-/g, "/");
            var localOffset = new Date().getTimezoneOffset() * 60000;//时区偏差
            if (typeof (localOffset) != undefined && isDoOffset == false) {
                localOffset = 0;
            }
            return "\/Date(" + (new Date(strDateTime).valueOf() - localOffset) + ")\/";
        }
        else {
            return null;
        }
    },

    //文字超长省略显示
    OverflowFormatter: function (value) {
        return "<div class='OverflowFormatter'>" + $.trim(value) + "</div>"
    },

    //json时间格式转字符串
    JsonDateFormatter: function (strJsonTime) {
        if (!strJsonTime || strJsonTime == "") {
            return "";
        }
        var date;
        //newtonsoft类型
        if (strJsonTime.indexOf("T") > 0) {
            strJsonTime = strJsonTime.replace("T", " ");
            date = new Date(Date.parse(strJsonTime.replace(/-/g, "/")));
        }
        else if (strJsonTime.indexOf("Date") > 0) {
            //微软类型
            var re = /-?\d+/;
            var m = re.exec(strJsonTime);
            date = new Date(parseInt(m[0]));
        }
        if (date) {
            return date.Format("yyyy-MM-dd");
        }
        else {
            return "";
        }
    },

    //配置jqGrid表格根据页面宽度动态适配，两个参数分别为表格id与表格所在父容器的id，通过动态匹配父容器宽度来实现
    //第三个参数为宽度偏移值，为了适配不同的页面
    SetGridWidthDynamic: function (tableId, parentContainerId, offsetValue) {
        if ($.browser.msie && parseInt($.browser.version, 10) < 8) {
            return;
        }

        $(window).resize(function () {
            Utils.SetGridWidth(tableId, parentContainerId, offsetValue);
        });
    },

    //手动触发表格的宽度自适应
    SetGridWidth: function (tableId, parentContainerId, offsetValue) {
        var grid = $(tableId);
        if (grid && grid.length == 1) {
            var parentWidth = $(parentContainerId).width();
            if (parentWidth > 1680) { //避免出现不断拉宽的问题
                return;
            }
            var gridWidth = grid.width();

            if (parentWidth > 0 && Math.abs(parentWidth - gridWidth) > 10) {
                grid.setGridWidth(parentWidth - offsetValue);
            }
        }
    },

    //设置jqGrid表格中Tip提示框
    //fromColIndex: 从哪一列开始进行Tip提示，必填
    //toColIndex: Tip提示到哪一列截止，非必填，如果不填表示从fromColIndex开始到最后所有的列
    //注：序号需要考虑隐藏列，从1开始，同时需要引入tooltip插件
    SetCellTipInfo: function (tableId, fromColIndex, toColIndex) {
        var tableObj = $("#" + tableId);
        if (!tableObj) {
            return;
        }

        toColIndex = (!toColIndex) ? 100 : toColIndex;

        tableObj.find("tr").each(function () {
            if (!$(this).hasClass("jqgfirstrow")) {
                $(this).find("td").each(function (index, item) {
                    if (index >= fromColIndex && index <= toColIndex) {
                        if ($(item).find("div").length > 0) {
                            var oldContent = $(item).find("div").html();
                            var newContent = Utils.StringFormat("<div class='tipClass' style='max-height:11px;margin:5px 5px;' title='{0}'>{1}</div>", $(item).find("div").attr("title"), $.trim(oldContent));
                            $(item).empty().html(newContent);
                        } else {
                            var oldContent = $(item).html();
                            var newContent = Utils.StringFormat("<div class='tipClass' style='max-height:11px;margin:5px 5px;' title='{0}'>{1}</div>", $(item).attr("title"), $.trim(oldContent));
                            $(item).removeAttr("title").empty().html(newContent);
                        }
                    }
                });
            }
        });

        $(".tipClass").tooltip({ track: true, delay: 0, showURL: false, showBody: " - ", fade: 300 });
    },

    //设置左侧树的高度与右侧区域相同的高度,offsetHeight为局部调整高度
    SetTreeHeight: function (treeId, rightDivId, minHeight, offsetHeight) {
        var treeH = $(treeId).height();
        var rightH = $(rightDivId).height();

        //修改左侧树动态高度逻辑
        if (rightH < minHeight) {
            treeH = minHeight;
            offsetHeight = 0;
        }
        else {
            treeH = rightH;
        }

        $(treeId).height(treeH - offsetHeight);
    },

    // 复制对像
    Clone: function (myObj) {
        if (typeof (myObj) != 'object') { return myObj; }
        if (myObj == null) { return myObj; }

        var myNewObj = new Object();

        for (var i in myObj) {
            myNewObj[i] = this.Clone(myObj[i]);
        }

        return myNewObj;
    },

    //日志打印
    Log: function (msg) {
        if (typeof console != undefined) {
            var str = Utils.StringFormat("【{0}】{1}", (new Date()).Format("yyyy-MM-dd hh:mm:ss"), msg);
            console.log(str);
        }
    },

    HtmlEncode: function (str) {
        var s = "";
        if (str.length == 0)
            return "";
        s = str.replace(/&/g, "&amp;");
        s = s.replace(/</g, "&lt;");
        s = s.replace(/>/g, "&gt;");
        s = s.replace(/ /g, "&nbsp;");
        s = s.replace(/\'/g, "&#39;");
        s = s.replace(/\"/g, "&quot;");
        return s;
    },

    HtmlDecode: function (str) {
        var s = "";
        if (str.length == 0)
            return "";
        s = str.replace(/&amp;/g, "&");
        s = s.replace(/&lt;/g, "<");
        s = s.replace(/&gt;/g, ">");
        s = s.replace(/&nbsp;/g, " ");
        s = s.replace(/&#39;/g, "\'");
        s = s.replace(/&quot;/g, "\"");
        return s;
    },

    // 导出文件
    DownloadFile: function (url) {
        if (navigator.userAgent.indexOf("MSIE 8.0") > 0 || navigator.userAgent.indexOf("MSIE 7.0") > 0) {
            location.href = url;
        }
        else {
            var frame = $("#__ipalDownloadFrame");
            if (frame.length == 0) {
                frame = $("<iframe id='__ipalDownloadFrame' style='display:none'>");
                $("body").append(frame);
            }
            frame.attr("src", url);
        }
    },

    // str:源中英文字符串 len:要截取的长度
    SubString: function (str, len) {
        var newLength = 0;
        var newStr = "";
        var chineseRegex = /[^\x00-\xff]/g;
        var singleChar = "";
        var strLength = str.replace(chineseRegex, "**").length;
        for (var i = 0; i < strLength; i++) {
            singleChar = str.charAt(i).toString();
            if (singleChar.match(chineseRegex) != null) {
                newLength += 2;
            }
            else {
                newLength++;
            }
            if (newLength > len) {
                break;
            }
            newStr += singleChar;
        }

        if (strLength > len) {
            newStr += "...";
        }
        return newStr;
    },

    /*重新选中下拉框中的option项, option对应value*/
    SelectOption: function (selectId, option) {
        var options = $("#" + selectId).find("option");
        if (options.length == 0) {
            return;
        }

        var newOptions = "";
        $.each(options, function (index, opt) {
            if ($(opt).val() == option) {
                newOptions += "<option value='" + $(opt).val() + "' selected='selected'>" + $(opt).text() + "</option>";
            }
            else {
                newOptions += "<option value='" + $(opt).val() + "'>" + $(opt).text() + "</option>";
            }
        });

        $("#" + selectId).empty().html(newOptions);
    },

    GetJsonFromParams: function (params) {
        if (!params) {
            return {};
        }

        var result = new Object();
        var str = params;
        strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {
            result[strs[i].split("=")[0]] = decodeURIComponent(strs[i].split("=")[1]);
        }

        return result;
    },

    FormValidate: function (selector, settings, custSettings, validCallback) {
        var set = {};
        if (settings) {
            set = settings;
        }

        var defaultSettings = {
            errorClass: "validate-error",
            submitHandler: function (form) {
                var result = $(form).valid();
                if (result == true && validCallback) {
                    var fp = Utils.GetJsonFromParams($(form).serialize());
                    validCallback(fp);
                }
            },
            errorPlacement: function (error, element) {
                var target = element;
                var name = $(element).attr("name");
                if (custSettings && custSettings.hasOwnProperty(name) && custSettings[name].error) {
                    var result = custSettings[name].error($(error).html(), element, "validate-error");
                    target = result ? result : target;
                }

                $(target).attr('original-title', $(error).html());
                $(target).tipsy({
                    trigger: "manual",
                    gravity: "w"
                });

                $(target).tipsy("show");
            },
            success: function (error, element) {
                var target = element;
                var name = $(element).attr("name");
                if (custSettings && custSettings.hasOwnProperty(name) && custSettings[name].success) {
                    var result = custSettings[name].success($(error).html(), element, "validate-error");
                    target = result ? result : target;
                }

                $(target).tipsy("hide");
            }
        };

        $.extend(set, defaultSettings);

        var validater = $(selector).validate(set);

        function _hideTips(selectors) {
            var $validateErrorElements = [];
            if (arguments.length == 0) {
                $validateErrorElements = $(selector).find("[class*='validate-error']");
            }
            else {
                var ids = Array.prototype.slice.call(arguments).join(",");
                $validateErrorElements = $(ids);
            }

            $validateErrorElements.each(function (index, element) {
                $(element).removeClass("validate-error");
                $(element).tipsy("hide");
            });
        }

        return {
            resetForm: function () {
                validater.resetForm();
                _hideTips();
            },
            hideTips: function (selector) {
                _hideTips.apply(this, arguments);
            },
            hideAllTips: function () {
                _hideTips();
            }
        };
    }
}