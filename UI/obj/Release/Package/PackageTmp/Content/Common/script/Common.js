var submitServer = {
    url: '',
    form: '',
    type: 'post',
    send: function (successCallback, errorCallback) {
        var xhr = new XMLHttpRequest();
        xhr.open(this.type, this.url, true);
        xhr.onload = function () {
            if (xhr.readyState == 4 && xhr.status == 200) {
                var textData = xhr.responseText;
                var obj = JSON.parse(textData);
                if (obj.Status == 1) {
                    if (successCallback) {
                        successCallback();
                    }

                }
                else {
                    if (errorCallback) {
                        errorCallback();
                    }
                }
            }
        }
        xhr.send(this.form);
    }
}

//返回对象的类型名称
function classOf(val) {
    if (val === null) return 'null';
    if (val === undefined) return 'undefined';
    return Object.prototype.toString.call(val).slice(8, -1);
}

function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

//建立一個可存取到該file的url  
// self.base.headPicUrl = self.getObjectURL( document.getElementById("headPic").files[0]); // 获取文件对象);
function getObjectURL(file) {
    var url = null;
    // 下面函数执行的效果是一样的，只是需要针对不同的浏览器执行不同的 js 函数而已  
    if (window.createObjectURL != undefined) { // basic  
        url = window.createObjectURL(file);
    } else if (window.URL != undefined) { // mozilla(firefox)  
        url = window.URL.createObjectURL(file);
    } else if (window.webkitURL != undefined) { // webkit or chrome  
        url = window.webkitURL.createObjectURL(file);
    }
    return url;
}


//控件拖拽
//http://www.zhangxinxu.com/wordpress/2010/03/javascript%E5%AE%9E%E7%8E%B0%E6%9C%80%E7%AE%80%E5%8D%95%E7%9A%84%E6%8B%96%E6%8B%BD%E6%95%88%E6%9E%9C/


