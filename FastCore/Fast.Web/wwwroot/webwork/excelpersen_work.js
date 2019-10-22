


var _idlist;//所有导出的记录ID 
var _url;//进度条url
var _index;//当前进度索引
var _key;//传输键


self.onmessage = function (pars) {
    _idlist = pars.data.ids;
    _url = pars.data.url;
    _index = 0;
    _key = pars.data.key;
    setTimeout(persenrun,300);
};


function persenrun() {

    if (_idlist && _idlist.length > 0) {

        if (_idlist && _index > _idlist.length - 1) {
            postMessage({ status: true, message: "完成", complete: true, persen: 100, key: _key });
            return;
        }
        var purl = _url + "?reportid=" + _idlist[_index] + "&key=" + _key;
        var ajax = new XMLHttpRequest();
        ajax.open('get', purl);
        ajax.send();
        ajax.onreadystatechange = function () {
            if (ajax.readyState === 4 && ajax.status === 200) {
                if (ajax.responseText !== null) {
                    var res = JSON.parse(ajax.responseText);
                    res.persen = parseFloat((_index + 1) * 100 / _idlist.length);
                    _key = res.key;
                    postMessage(res);
                    if (res.status) {
                        _index += 1;
                        setTimeout(persenrun, 90);
                    }
                }
            }
        };
    }
    else postMessage({ status: false, message: "没有数据可以导出" });
    
}