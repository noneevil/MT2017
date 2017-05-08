window.addEvent('domready', function () {
    if (Browser.ie) {
        $$("html")[0].addEvent('keydown', function () {
            if (event.altKey && event.keyCode == 37) {
                getIFrame().history.go(-1);
                event.keyCode = 0;
                event.returnValue = false;
            }
            else if (event.altKey && event.keyCode == 39) {
                getIFrame().history.forward();
                event.keyCode = 0;
                event.returnValue = false;
            }
            else if (event.keyCode == 116) {
                getIFrame().location.reload();
                event.keyCode = 0;
                event.returnValue = false;
            }
        });
    } else {
        $$("html")[0].addEvent('keydown', function (event) {
            if (event.code === 116) {
                getIFrame().location.reload();
                event.stop();
            }
        });
    }
});
jQuery(function () {
    document.documentElement.focus();
    jq(':button,:submit').button();
    jq("#icons li").hover(function () {
        jQuery(this).addClass("ui-state-hover");
    }, function () {
        jQuery(this).removeClass("ui-state-hover");
    });

    jq(".table tbody tr").hover(function () {
        jq(this).css("background-color", '#FFF7CE;');
    }, function () {
        jq(this).css("background-color", '');
    });
    jq("[type=password],[type=text],textarea").bind({
        focus: function () {
            jq(this).css({ "background-color": '#FCFAF0;', 'borderColor': '#FF9334' });
        },
        blur: function () {
            jq(this).css({ "background-color": '', 'borderColor': '#ADD2DA' });
        }
    });
});
/*调用__doPostBack*/
function CallPostBack(el, data) {
    __doPostBack(jq(el).attr('name'), data)
}
/*删除提示对话框*/
function dialogConfirm(arg) {//msg, css, callback
    arg.css = arg.css || 'failure';
    var html = '<table cellpadding="0" cellspacing="0"><tr><td width="55" valign="middle">';
    html += '<img src="/Developer/images/dialog/' + arg.css + '.gif" />';
    html += '</td><td valign="middle">' + unescape(arg.text) + '</td></tr></table>';

    jq('<div class="' + arg.css + '">' + html + '</div>').dialog({
        modal: true,
        title: "提示",
        resizable: false,
        buttons: {
            "确定": function () {
                jq(this).dialog("close");
                var target = jq(arg.el).attr('name');
                __doPostBack(target, arg.data);
            },
            "取消": function () {
                jq(this).dialog("close");
            }
        },
        close: function () {
            jq(this).dialog("destroy");
        }
    });
    return false;
}
/*消息提示*/
function dialogMessage(txt, css) {
    css = css || 'prompt';
    var html = '<table cellpadding="0" cellspacing="0"><tr><td width="55" valign="middle">';
    html += '<img src="/Developer/images/dialog/' + css + '.gif" />';
    html += '</td><td valign="middle">' + txt + '</td></tr></table>';
    jq('<div class="' + css + '">' + html + '</div>').dialog({
        modal: true,
        title: "提示",
        resizable: false,
        buttons: {
            "确定": function () {
                jq(this).dialog("close");
            }
        },
        close: function () {
            jq(this).dialog("destroy");
        }
    });
}
/*编辑对话框*/
function dialogIFrame(arg) {
    var iframe = new IFrame({
        src: arg.url,
        frameborder: 0,
        styles: {
            width: '100%',
            height: '100%',
            border: 0
        }
    });
    var width = arg.width || 350;
    var height = arg.height || 280;
    view = jq('<div style="padding:0;"/>').dialog({
        width: width,
        height: height,
        modal: true,
        resizable: false,
        title: arg.title,
        create: function () {
            jq(this).append(iframe);
        },
        close: function () {
            jq(this).dialog("destroy");
        }
    });
}
//document.onselectstart=function(e){return false;};
var getIFrame = function () {
    return top.$("winFrame").contentDocument || top.document.frames["winFrame"];
};
function loading(state) {//提交数据时设置控件状态
    $$('input,select,textarea').each(function (item) { item.disabled = state; });
    $$('[name=edit],[name=del],[name=asave],[name=acanel],[name=online]').each(function (item) {
        if (state) {
            item.src = item.src.replace(".gif", "_.gif");
        }
        else {
            item.src = item.src.replace("_.", ".");
        }
    });
    if (state) {
        top.Dialog.win({ message: '<div class="loading">数据加载中,请稍后...</div>', width: 180, height: 30, autoClose: false, titleBar: false });
        window.status = "数据加载中,请稍后...";
    }
    else {
        top.Dialog.close();
        window.status = "完成";
    }
}