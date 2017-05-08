//document.onselectstart=function(e){return false;};
var getIFrame = function() {
    return top.$("winFrame").contentDocument || top.document.frames["winFrame"];
};
function loading(state) {//提交数据时设置控件状态
    $$('input,select,textarea').each(function(item) { item.disabled = state; });
    $$('[name=edit],[name=del],[name=asave],[name=acanel],[name=online]').each(function(item) {
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
window.addEvents({
    'domready': function() {
        if (top.$("winFrame")) {
            $$('.button').addEvents({
                mouseenter: function() { this.addClass('hover'); },
                mouseleave: function() { this.removeClass('hover'); }
            });
            $$('[type=password],[type=text],textarea').addEvents({
                blur: function() { this.css({ borderColor: '#ADD2DA', 'background-color': '' }); },
                focus: function() { this.css({ borderColor: '#FF9334', 'background-color': '#FCFAF0' }); }
            });
            $$('.table tbody tr').event({
                mouseenter: function() { this.css({ backgroundColor: '#FFF7CE' }); },
                mouseleave: function() { this.css({ backgroundColor: '' }); }
            });
        }
    },
    'load': function() {
        if (top.$("winFrame")) {
            document.addEvent('keydown', function(event) {
                if (Browser.ie6 || Browser.ie7 || Browser.ie8) {
                    event = window.event;
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
                    else if ((event.keyCode == 116) || (event.ctrlKey && event.keyCode == 82) || (event.ctrlKey && event.keyCode == 116)) {
                        getIFrame().location.reload();
                        event.keyCode = 0;
                        event.returnValue = false;
                    }
                }
                else {
                    if (event.alt && event.key == 'left') {
                        getIFrame().history.go(-1);
                        event.stop();
                    }
                    else if (event.alt && event.key == 'right') {
                        getIFrame().history.forward();
                        event.stop();
                    }
                    if ((event.key == 'f5') || (event.control && event.key == 'r') || (event.control && event.key == 'f5')) {
                        getIFrame().location.reload();
                        event.stop();
                    }
                }
            });
        }
    }
});