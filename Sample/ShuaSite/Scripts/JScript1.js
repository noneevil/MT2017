var nInterval;
var xInterval;
function chkRefresh(frm) {
    document.getElementById('btnBeginRefresh').disabled = true;
    shuago();
}
//===============================================================================
function shuago() {
    xxwin = window.open("Alexa.Asp", "xx", "");
    st = 20;
    timeshua();
}
function timeshua() {
    if (xxwin.closed == true) {
        if (document.getElementById("shuatime").style.display == 'none') {
            chkStop();
        } else {
            st = st - 1;
            document.getElementById("shuatime").style.display = 'block';
            var st_rand = st + 1;
            document.getElementById('shuatime').innerHTML = '<b>(离下次开始的时间值:' + st_rand + ')</b>';
            var nInterval_go = setTimeout("timeshua()", 1000);
            if (st < 0) {
                window.clearInterval(nInterval_go);
                go();
            }
        }
    } else {
        st = st - 1;
        document.getElementById("shuatime").style.display = 'block';
        var st_rand = st + 1;
        document.getElementById('shuatime').innerHTML = '<b>(离下次开始的时间值:' + st_rand + ')</b>';
        var nInterval_go = setTimeout("timeshua()", 1000);
        if (st < 0) {
            window.clearInterval(nInterval_go);
            go();
        }
    }
}
function go() {
    st = 20;
    timeshua();
    if (xxwin.closed == true) {
        xxwin = window.open("Alexa.Asp", "xx", "");
    } else {
        try {
            xxwin.location.href = "Alexa.Asp";
        } catch(err) {
            if (xxwin.close != true) {
                xxwin.close();
            }
            xxwin = window.open("Alexa.Asp", "xx", "");
        }
    }
}
function chkStop() {
    document.getElementById('btnBeginRefresh').disabled = false;
    window.clearInterval(nInterval);
    window.clearInterval(xInterval);
    isStartSurf = 0;
    document.getElementById("shuatime").style.display = 'none';
    if (xxwin.close != true) {
        xxwin.close();
    }
}
function xgo() {
    if (xxwin.closed == true) {
        chkStop();
    }
}
function doTryAlexa() {
    try {
        aborted();
        var jg = true;
        if (jg) {
            shuago();
        }
    } catch(e) {
        alert("您好，您的电脑当前没有安装Alexa工具条！\n\n为了更让您的网站快速的提升Alexa排名，请下载安装Alexa工具条!\n\n并在刷站过程中，开启Alexa工具条!\n\n谢谢合作！");
    }
}
//===============================================================================
function chkLogin(frm) {
    var sStation sStation = frm.txtStation.value.toLowerCase();
    if (sStation.indexOf("http://") == -1) {
        frm.txtStation.value = "http://" + sStation;
    }
    if (sStation == "") {
        alert("您的网站地址不能为空!");
        return false
    };
    frm.hiddenAction.value = "login";
    return true;
}

