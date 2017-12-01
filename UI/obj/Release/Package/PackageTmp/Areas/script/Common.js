var submitServer = {
    url: '',
    form: '',
    type: '',
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