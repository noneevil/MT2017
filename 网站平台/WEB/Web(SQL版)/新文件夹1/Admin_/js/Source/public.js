/*
**************************************************通用**************************************************
*/
window.addEvent('domready', function () {
    var tabs = $$('ul.ui-tabs-nav li');
    tabs.addEvents({
        click: function (e) {
            e.stop();
            tabs.removeClass('ui-state-active');
            $$('div[id^=tabs-]').addClass('ui-tabs-hide');
            this.addClass('ui-state-active');
            var link = this.getElement('a');
            link.set('hidefocus', 'hidefocus');
            var box = link.get('href').replace('#', '');
            $(box).removeClass('ui-tabs-hide');
        }
    });
    Asset.javascript('js/mooniform.js', {
        onLoad: function () {
            new Mooniform($$('select,input[type="radio"],input[type="checkbox"],input[type="file"]'));
        }
    });
});

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
/*
**************************************************日期**************************************************
*/
var DateTimeShow = new Class({
    Implements: [Options],
    options:
    {
        showDate: true,
        showDay: true,
        showTime: true
    },
    secondSub: 0,
    attTo: null,
    secondSub: 0,
    CnDays: ["星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"],
    initialize: function (el, options) {
        this.attTo = $(el);
        this.setOptions(options);
        this.show();
        this.show.bind(this).periodical(1000);
    },
    task: function (toDay) {
        var o = this.options;
        var dn, str = "";
        var hh = toDay.getHours();
        var mm = toDay.getMinutes();
        var ss = toDay.getSeconds();
        if (o.showDate) str = toDay.getFullYear() + "年" + (toDay.getMonth() + 1) + "月" + toDay.getDate() + "日";
        if (o.showDay) str += "&nbsp;" + this.CnDays[toDay.getDay()];
        if (o.showTime) {
            if (hh < 10) hh = '0' + hh;
            if (mm < 10) mm = '0' + mm;
            if (ss < 10) ss = '0' + ss;
            str += "&nbsp;" + hh + ":" + mm + ":" + ss;
        }
        return (str);
    },
    show: function () {
        Cookie.write('r', String.uniqueID());
        var today = new Date();
        if (this.secondSub != 0) today.setTime(today.getTime() + this.secondSub);
        this.attTo.innerHTML = this.task(today);
    }
});
/*
**************************************************MD5加密**************************************************
*/
function MD5(sMessage) {
    function RotateLeft(lValue, iShiftBits) {
        return (lValue << iShiftBits) | (lValue >>> (32 - iShiftBits));
    }
    function AddUnsigned(lX, lY) {
        var lX4, lY4, lX8, lY8, lResult;
        lX8 = (lX & 0x80000000);
        lY8 = (lY & 0x80000000);
        lX4 = (lX & 0x40000000);
        lY4 = (lY & 0x40000000);
        lResult = (lX & 0x3FFFFFFF) + (lY & 0x3FFFFFFF);
        if (lX4 & lY4) return (lResult ^ 0x80000000 ^ lX8 ^ lY8);
        if (lX4 | lY4) {
            if (lResult & 0x40000000) return (lResult ^ 0xC0000000 ^ lX8 ^ lY8);
            else return (lResult ^ 0x40000000 ^ lX8 ^ lY8);
        } else return (lResult ^ lX8 ^ lY8);
    }
    function F(x, y, z) {
        return (x & y) | ((~x) & z);
    }
    function G(x, y, z) {
        return (x & z) | (y & (~z));
    }
    function H(x, y, z) {
        return (x ^ y ^ z);
    }
    function I(x, y, z) {
        return (y ^ (x | (~z)));
    }
    function FF(a, b, c, d, x, s, ac) {
        a = AddUnsigned(a, AddUnsigned(AddUnsigned(F(b, c, d), x), ac));
        return AddUnsigned(RotateLeft(a, s), b);
    }
    function GG(a, b, c, d, x, s, ac) {
        a = AddUnsigned(a, AddUnsigned(AddUnsigned(G(b, c, d), x), ac));
        return AddUnsigned(RotateLeft(a, s), b);
    }
    function HH(a, b, c, d, x, s, ac) {
        a = AddUnsigned(a, AddUnsigned(AddUnsigned(H(b, c, d), x), ac));
        return AddUnsigned(RotateLeft(a, s), b);
    }
    function II(a, b, c, d, x, s, ac) {
        a = AddUnsigned(a, AddUnsigned(AddUnsigned(I(b, c, d), x), ac));
        return AddUnsigned(RotateLeft(a, s), b);
    }
    function ConvertToWordArray(sMessage) {
        var lWordCount;
        var lMessageLength = sMessage.length;
        var lNumberOfWords_temp1 = lMessageLength + 8;
        var lNumberOfWords_temp2 = (lNumberOfWords_temp1 - (lNumberOfWords_temp1 % 64)) / 64;
        var lNumberOfWords = (lNumberOfWords_temp2 + 1) * 16;
        var lWordArray = Array(lNumberOfWords - 1);
        var lBytePosition = 0;
        var lByteCount = 0;
        while (lByteCount < lMessageLength) {
            lWordCount = (lByteCount - (lByteCount % 4)) / 4;
            lBytePosition = (lByteCount % 4) * 8;
            lWordArray[lWordCount] = (lWordArray[lWordCount] | (sMessage.charCodeAt(lByteCount) << lBytePosition));
            lByteCount++;
        }
        lWordCount = (lByteCount - (lByteCount % 4)) / 4;
        lBytePosition = (lByteCount % 4) * 8;
        lWordArray[lWordCount] = lWordArray[lWordCount] | (0x80 << lBytePosition);
        lWordArray[lNumberOfWords - 2] = lMessageLength << 3;
        lWordArray[lNumberOfWords - 1] = lMessageLength >>> 29;
        return lWordArray;
    }
    function WordToHex(lValue) {
        var WordToHexValue = "",
        WordToHexValue_temp = "",
        lByte, lCount;
        for (lCount = 0; lCount <= 3; lCount++) {
            lByte = (lValue >>> (lCount * 8)) & 255;
            WordToHexValue_temp = "0" + lByte.toString(16);
            WordToHexValue = WordToHexValue + WordToHexValue_temp.substr(WordToHexValue_temp.length - 2, 2);
        }
        return WordToHexValue;
    }
    var x = Array();
    var k, AA, BB, CC, DD, a, b, c, d;
    var S11 = 7,
    S12 = 12,
    S13 = 17,
    S14 = 22;
    var S21 = 5,
    S22 = 9,
    S23 = 14,
    S24 = 20;
    var S31 = 4,
    S32 = 11,
    S33 = 16,
    S34 = 23;
    var S41 = 6,
    S42 = 10,
    S43 = 15,
    S44 = 21;
    // Steps 1 and 2.  Append padding bits and length and convert to words
    x = ConvertToWordArray(sMessage);
    // Step 3.  Initialise
    a = 0x67452301;
    b = 0xEFCDAB89;
    c = 0x98BADCFE;
    d = 0x10325476;
    // Step 4.  Process the message in 16-word blocks
    for (k = 0; k < x.length; k += 16) {
        AA = a;
        BB = b;
        CC = c;
        DD = d;
        a = FF(a, b, c, d, x[k + 0], S11, 0xD76AA478);
        d = FF(d, a, b, c, x[k + 1], S12, 0xE8C7B756);
        c = FF(c, d, a, b, x[k + 2], S13, 0x242070DB);
        b = FF(b, c, d, a, x[k + 3], S14, 0xC1BDCEEE);
        a = FF(a, b, c, d, x[k + 4], S11, 0xF57C0FAF);
        d = FF(d, a, b, c, x[k + 5], S12, 0x4787C62A);
        c = FF(c, d, a, b, x[k + 6], S13, 0xA8304613);
        b = FF(b, c, d, a, x[k + 7], S14, 0xFD469501);
        a = FF(a, b, c, d, x[k + 8], S11, 0x698098D8);
        d = FF(d, a, b, c, x[k + 9], S12, 0x8B44F7AF);
        c = FF(c, d, a, b, x[k + 10], S13, 0xFFFF5BB1);
        b = FF(b, c, d, a, x[k + 11], S14, 0x895CD7BE);
        a = FF(a, b, c, d, x[k + 12], S11, 0x6B901122);
        d = FF(d, a, b, c, x[k + 13], S12, 0xFD987193);
        c = FF(c, d, a, b, x[k + 14], S13, 0xA679438E);
        b = FF(b, c, d, a, x[k + 15], S14, 0x49B40821);
        a = GG(a, b, c, d, x[k + 1], S21, 0xF61E2562);
        d = GG(d, a, b, c, x[k + 6], S22, 0xC040B340);
        c = GG(c, d, a, b, x[k + 11], S23, 0x265E5A51);
        b = GG(b, c, d, a, x[k + 0], S24, 0xE9B6C7AA);
        a = GG(a, b, c, d, x[k + 5], S21, 0xD62F105D);
        d = GG(d, a, b, c, x[k + 10], S22, 0x2441453);
        c = GG(c, d, a, b, x[k + 15], S23, 0xD8A1E681);
        b = GG(b, c, d, a, x[k + 4], S24, 0xE7D3FBC8);
        a = GG(a, b, c, d, x[k + 9], S21, 0x21E1CDE6);
        d = GG(d, a, b, c, x[k + 14], S22, 0xC33707D6);
        c = GG(c, d, a, b, x[k + 3], S23, 0xF4D50D87);
        b = GG(b, c, d, a, x[k + 8], S24, 0x455A14ED);
        a = GG(a, b, c, d, x[k + 13], S21, 0xA9E3E905);
        d = GG(d, a, b, c, x[k + 2], S22, 0xFCEFA3F8);
        c = GG(c, d, a, b, x[k + 7], S23, 0x676F02D9);
        b = GG(b, c, d, a, x[k + 12], S24, 0x8D2A4C8A);
        a = HH(a, b, c, d, x[k + 5], S31, 0xFFFA3942);
        d = HH(d, a, b, c, x[k + 8], S32, 0x8771F681);
        c = HH(c, d, a, b, x[k + 11], S33, 0x6D9D6122);
        b = HH(b, c, d, a, x[k + 14], S34, 0xFDE5380C);
        a = HH(a, b, c, d, x[k + 1], S31, 0xA4BEEA44);
        d = HH(d, a, b, c, x[k + 4], S32, 0x4BDECFA9);
        c = HH(c, d, a, b, x[k + 7], S33, 0xF6BB4B60);
        b = HH(b, c, d, a, x[k + 10], S34, 0xBEBFBC70);
        a = HH(a, b, c, d, x[k + 13], S31, 0x289B7EC6);
        d = HH(d, a, b, c, x[k + 0], S32, 0xEAA127FA);
        c = HH(c, d, a, b, x[k + 3], S33, 0xD4EF3085);
        b = HH(b, c, d, a, x[k + 6], S34, 0x4881D05);
        a = HH(a, b, c, d, x[k + 9], S31, 0xD9D4D039);
        d = HH(d, a, b, c, x[k + 12], S32, 0xE6DB99E5);
        c = HH(c, d, a, b, x[k + 15], S33, 0x1FA27CF8);
        b = HH(b, c, d, a, x[k + 2], S34, 0xC4AC5665);
        a = II(a, b, c, d, x[k + 0], S41, 0xF4292244);
        d = II(d, a, b, c, x[k + 7], S42, 0x432AFF97);
        c = II(c, d, a, b, x[k + 14], S43, 0xAB9423A7);
        b = II(b, c, d, a, x[k + 5], S44, 0xFC93A039);
        a = II(a, b, c, d, x[k + 12], S41, 0x655B59C3);
        d = II(d, a, b, c, x[k + 3], S42, 0x8F0CCC92);
        c = II(c, d, a, b, x[k + 10], S43, 0xFFEFF47D);
        b = II(b, c, d, a, x[k + 1], S44, 0x85845DD1);
        a = II(a, b, c, d, x[k + 8], S41, 0x6FA87E4F);
        d = II(d, a, b, c, x[k + 15], S42, 0xFE2CE6E0);
        c = II(c, d, a, b, x[k + 6], S43, 0xA3014314);
        b = II(b, c, d, a, x[k + 13], S44, 0x4E0811A1);
        a = II(a, b, c, d, x[k + 4], S41, 0xF7537E82);
        d = II(d, a, b, c, x[k + 11], S42, 0xBD3AF235);
        c = II(c, d, a, b, x[k + 2], S43, 0x2AD7D2BB);
        b = II(b, c, d, a, x[k + 9], S44, 0xEB86D391);
        a = AddUnsigned(a, AA);
        b = AddUnsigned(b, BB);
        c = AddUnsigned(c, CC);
        d = AddUnsigned(d, DD);
    }
    // Step 5.  Output the 128 bit digest
    var temp = WordToHex(a) + WordToHex(b) + WordToHex(c) + WordToHex(d);
    return temp.toUpperCase();
}

/*
**************************************************消息提示组件**************************************************
*/
/**
* Dialog.js 
* @author netman8410@163.com
*/
//<meta http-equiv="X-UA-Compatible" content="IE=7" />  IE8透明度解决方案
//var location=window.location; 避免iframe跳转解决方案
(function () {
    if (window.Dialog) return;
    var objType = function (type) {
        return new Function('o', "return Object.prototype.toString.call(o)=='[object " + type + "]'")
    }; //判断元素类型
    var isArray = objType('Array'),
    isObj = objType('Object'); //判断元素是否数组、object
    window.Dialog = {
        version: '4.0',
        pubDate: '2009-03-02',
        apply: function (o, c, d) {
            if (d) Dialog.apply(o, d);
            if (o && c && isObj(c)) for (var p in c) o[p] = c[p];
            return o;
        },
        eventList: []
    };
    /*初始化可能在页面加载完成调用的接口，防止外部调用失败。_initFn:缓存初始调用传入的参数*/
    var initFn = ['setDefaultCfg', 'show'],
    _initFn = {},
    t;
    while (t = initFn.shift()) Dialog[t] = eval('0,function(){_initFn.' + t + '?_initFn.' + t + '.push(arguments):(_initFn.' + t + '=[arguments])}');
    /*以下为公用函数及变量*/
    var isIE = ! +'\v1'; //IE浏览器
    var isCompat = document.compatMode == 'CSS1Compat'; //浏览器当前解释模式
    var IE6 = isIE && /MSIE (\d)\./.test(navigator.userAgent) && parseInt(RegExp.$1) < 7; //IE6以下需要用iframe来遮罩
    var useFixed = !isIE || (!IE6 && isCompat); //滚动时，IE7+（标准模式）及其它浏览器使用Fixed定位
    var $ = function (id) {
        return document.getElementById(id)
    }; //获取元素
    var $height = function (obj) {
        return parseInt(obj.style.height) || obj.offsetHeight
    }; //获取元素高度
    var addEvent = (function () {
        return new Function('env', 'fn', 'obj', 'obj=obj||document;' + (window.attachEvent ? "obj.attachEvent('on'+env,fn)" : 'obj.addEventListener(env,fn,false)') + ';Dialog.eventList.push([env,fn,obj])')
    })(); //事件绑定
    var detachEvent = (function () {
        return new Function('env', 'fn', 'obj', 'obj=obj||document;' + (window.attachEvent ? "obj.detachEvent('on'+env,fn)" : 'obj.removeEventListener(env,fn,false)'))
    })(); //取消事件绑定
    //为元素的特定样式属性设定值
    var setStyle = function (el, n, v) {
        if (!el) return;
        if (isObj(n)) {
            for (var i in n) setStyle(el, i, n[i]);
            return;
        }
        /*dom数组或dom集合*/
        if (isArray(el) || /htmlcollection|nodelist/i.test('' + el)) {
            for (var i = el.length - 1; i >= 0; i--) setStyle(el[i], n, v);
            return;
        }
        try {
            el.style[n] = v
        } catch (e) { }
    };
    /*----------------和业务有关的公用函数-----------------*/
    var btnIndex = 0,
    btnCache, seed = 0; //当前焦点的按钮的索引、当前存在的按钮、id种子
    /*创建按钮*/
    var mkBtn = function (txt, sign, autoClose, id) {
        if (!txt) return;
        if (isArray(txt)) {
            /*无效按钮删除*/
            var item, t = [],
            dftBtn = {
                OK: [curCfg.okTxt, 'ok'],
                CANCEL: [curCfg.cancelTxt, 'cancel']
            };
            while (txt.length) (item = txt.shift()) && t[t.push(mkBtn.apply(null, dftBtn[item] || item)) - 1] || t.pop();
            return t;
        }
        id = id || 'ymPrompt_btn_' + seed++;
        autoClose = autoClose == undefined ? 'undefined' : !!autoClose;
        return {
            id: id,
            html: "<input type='button' id='" + id + "' onclick='Dialog.doHandler(\"" + sign + "\"," + autoClose + ")' style='cursor:pointer' class='btnStyle handler' value='" + txt + "' />"
        };
    };
    /*生成按钮组合的html*/
    var joinBtn = function (btn) {
        if (!btn) return btnCache = '';
        if (!isArray(btn)) btn = [btn];
        if (!btn.length) return btnCache = '';
        btnCache = btn.concat();
        var html = [];
        while (btn.length) html.push(btn.shift().html);
        return html.join('&nbsp;&nbsp;');
    };
    /*默认显示配置及用户当前配置*/
    var dftCfg = {
        message: '内容',
        //消息框内容
        width: 300,
        //消息框宽度
        height: 185,
        //消息框高度
        title: '标题',
        //消息框标题
        handler: function () { },
        //回调事件，默认空函数
        maskAlphaColor: '#000',
        //遮罩透明色，默认黑色
        maskAlpha: 0.1,
        //遮罩透明度，默认0.1
        iframe: false,
        //iframe模式，默认不是
        icoCls: '',
        //消息框左侧图标，默认无
        btn: null,
        //消息框显示的按钮，默认无
        autoClose: true,
        //点击关闭、确定等按钮是否自动关闭，默认自动关闭
        fixPosition: true,
        //是否随滚动条滚动，默认是
        dragOut: false,
        //是否允许拖出窗口范围，默认不允许
        titleBar: true,
        //是否显示标题栏，默认显示
        showMask: true,
        //是否显示遮罩，默认显示
        winPos: 'c',
        //消息框弹出的位置，默认在页面中间
        winAlpha: 1,
        //拖动时消息框的透明度，默认0.8
        closeBtn: true,
        //是否显示关闭按钮，默认显示
        showShadow: false,
        //是否显示消息框的阴影，默认不显示（IE支持）
        useSlide: false,
        //是否启用消息框的淡入淡出效果，默认不启用
        slideCfg: { //淡入淡出效果配置，useSlide=true时有效
            increment: 0.3,
            //每次渐变的值，值范围0-1
            interval: 50 //渐变的速度
        },
        closeTxt: '关闭',
        //关闭按钮的提示文本
        okTxt: ' 确 定 ',
        //确定按钮的提示文本
        cancelTxt: ' 取 消 ',
        //取消按钮的提示文本
        msgCls: 'ym-content',
        //消息框内容的class名称，用于自定义验尸官，默认为ym-content,仅在iframe:false时有效
        minBtn: false,
        //是否显示最小化按钮，默认不显示
        minTxt: '最小化',
        //最小化按钮的提示文本
        maxBtn: false,
        //是否显示最大化按钮，默认不显示
        maxTxt: '最大化',
        //最大化按钮的提示文本
        allowSelect: false,
        //是否允许选择消息框内容，默认不允许
        allowRightMenu: false //是否允许在消息框使用右键，默认不允许
    },
    curCfg = {};

    /*开始解析*/
    (function () {
        var rootEl = document.body,
        callee = arguments.callee;
        if (!rootEl || typeof rootEl != 'object') return addEvent('load', callee, window); //等待页面加载完成
        /*防止在IE下因document未就绪而报“IE无法打开INTERNET站点的错”的错*/
        if (isIE && document.readyState != 'complete') return addEvent('readystatechange',
        function () {
            document.readyState == "complete" && callee()
        });

        rootEl = isCompat ? document.documentElement : rootEl; //根据html Doctype获取html根节点，以兼容非xhtml的页面
        var frameset = document.getElementsByTagName('frameset').length; //是否frameset页面
        if (!isIE && frameset) return; //frameset页面且不是IE则直接返回，否则会出现错误。
        /*获取scrollLeft和scrollTop，在fixed定位时返回0，0*/
        var getScrollPos = function () {
            return curCfg.fixPosition && useFixed ? [0, 0] : [rootEl.scrollLeft, rootEl.scrollTop];
        };
        /*保存窗口定位信息，弹出窗口相对页面左上角的坐标信息*/
        var saveWinInfo = function () {
            var pos = getScrollPos();
            Dialog.apply(dragVar, {
                _offX: parseInt(ym_win.style.left) - pos[0],
                _offY: parseInt(ym_win.style.top) - pos[1]
            });
        };
        /*-------------------------创建弹窗html-------------------*/
        var maskStyle = 'position:absolute;top:0;left:0;display:none;text-align:center';
        var div = document.createElement('div');
        div.innerHTML = [
        /*遮罩*/
        "<div id='maskLevel' style=\'" + maskStyle + ';z-index:10000;\'></div>', IE6 ? ("<iframe id='maskIframe' src='javascript:false' style='" + maskStyle + ";z-index:9999;filter:alpha(opacity=0);opacity:0'></iframe>") : '',
        /*窗体*/
        "<div id='ym-window' style='position:absolute;z-index:10001;display:none'>", IE6 ? "<iframe src='javascript:false' style='width:100%;height:100%;position:absolute;top:0;left:0;z-index:-1'></iframe>" : '', "<div class='ym-tl' id='ym-tl'><div class='ym-tr'><div class='ym-tc' style='cursor:move;'><div class='ym-header-text'></div><div class='ym-header-tools'>", "<div class='ymPrompt_min' title='最小化'><strong>0</strong></div>", "<div class='ymPrompt_max' title='最大化'><strong>1</strong></div>", "<div class='ymPrompt_close' title='关闭'><strong>r</strong></div>", "</div></div></div></div>", "<div class='ym-ml' id='ym-ml'><div class='ym-mr'><div class='ym-mc'><div class='ym-body' style='position:relative'></div></div></div></div>", "<div class='ym-ml' id='ym-btnl'><div class='ym-mr'><div class='ym-btn'></div></div></div>", "<div class='ym-bl' id='ym-bl'><div class='ym-br'><div class='ym-bc'></div></div></div>", "</div>",
        /*阴影*/
        isIE ? "<div id='ym-shadow' style='position:absolute;z-index:10000;background:#808080;filter:alpha(opacity=80) progid:DXImageTransform.Microsoft.Blur(pixelradius=2);display:none'></div>" : ''].join('');
        document.body.appendChild(div);
        /*窗口上的对象*/
        /*mask、window*/
        var maskLevel = $('maskLevel');
        var ym_win = $('ym-window');
        var ym_shadow = $('ym-shadow');
        var ym_wins;
        /*header*/
        var ym_headbox = $('ym-tl');
        var ym_head = ym_headbox.firstChild.firstChild;
        var ym_hText = ym_head.firstChild;
        var ym_hTool = ym_hText.nextSibling;
        /*content*/
        var ym_body = $('ym-ml').firstChild.firstChild.firstChild;
        /*button*/
        var ym_btn = $('ym-btnl');
        var ym_btnContent = ym_btn.firstChild.firstChild;
        /*bottom*/
        var ym_bottom = $('ym-bl');
        var maskEl = [maskLevel]; //遮罩元素
        IE6 && maskEl.push($('maskIframe'));
        var ym_ico = ym_hTool.childNodes; //右上角的图标
        var dragVar = {};
        /*窗口的最大化最小化核心功能实现*/
        var cur_state = 'normal',
        cur_cord = [0, 0]; //cur_cord记录最大化前窗口的坐标
        var cal_cord = function () {
            var pos = getScrollPos();
            cur_cord = [parseInt(ym_win.style.left) - pos[0], parseInt(ym_win.style.top) - pos[1]]
        }; //保存坐标(相对页面左上角坐标)
        /*从常态到最大化*/
        var doMax = function () {
            cal_cord(); //记录坐标，便于还原时使用
            cur_state = 'max';
            ym_ico[1].firstChild.innerHTML = '2';
            ym_ico[1].className = 'ymPrompt_normal';
            setWinSize(rootEl.clientWidth, rootEl.clientHeight, [0, 0]);
        };
        /*从正常到最小化*/
        var doMin = function () {
            cal_cord();
            cur_state = 'min';
            ym_ico[0].firstChild.innerHTML = '2';
            ym_ico[0].className = 'ymPrompt_normal';
            setWinSize(0, $height(ym_headbox), cur_cord); //定位在当前坐标
        };
        var doNormal = function (init) { //init=true,弹出时调用该函数
            !init && cur_state == 'min' && cal_cord(); //从最小化过来重新获取坐标
            cur_state = 'normal';
            ym_ico[0].firstChild.innerHTML = '0';
            ym_ico[1].firstChild.innerHTML = '1';
            ym_ico[0].className = 'ymPrompt_min';
            ym_ico[1].className = 'ymPrompt_max';
            setWinSize.apply(this, init ? [] : [0, 0, cur_cord]);
        };
        var max, min;
        addEvent('click', min = function () {
            cur_state != 'normal' ? doNormal() : doMin();
        },
        ym_ico[0]); //最小化
        addEvent('click', max = function () {
            cur_state != 'normal' ? doNormal() : doMax();
        },
        ym_ico[1]); //最大化
        addEvent('dblclick',
        function (e) {
            /*如果操作元素是最大最小关闭按钮则不进行此处理*/
            curCfg.maxBtn && (e.srcElement || e.target).parentNode != ym_hTool && max()
        },
        ym_head);
        addEvent('click',
        function () {
            Dialog.doHandler('close');
        },
        ym_ico[2]); //关闭
        /*窗口最大化最小化核心部分结束*/
        /*getWinSize取得页面实际大小*/
        var getWinSize = function () {
            return [Math.max(rootEl.scrollWidth, rootEl.clientWidth), Math.max(rootEl.scrollHeight, rootEl.clientHeight)]
        };
        var winSize = getWinSize(); //保存当前页面的实际大小
        /*事件绑定部分*/
        var bindEl = ym_head.setCapture && ym_head; //绑定拖放事件的对象，只有Ie下bindEl有效
        /*窗体透明度控制*/
        var filterWin = function (v) {
            /*鼠标按下时取消窗体的透明度，IE标准模式下透明度为1则直接清除透明属性，防止iframe窗口不能拖动滚动条*/
            !frameset && setStyle(ym_win, v == 1 && isCompat ? {
                filter: '',
                opacity: ''
            } : { filter: 'Alpha(opacity=' + v * 100 + ')', opacity: v });
        };
        /*mousemove事件*/
        var mEvent = function (e) {
            var sLeft = dragVar.offX + e.clientX;
            var sTop = dragVar.offY + e.clientY;
            if (!curCfg.dragOut) { //页面可见区域内拖动
                var pos = getScrollPos(),
                sl = pos[0],
                st = pos[1];
                sLeft = Math.min(Math.max(sLeft, sl), rootEl.clientWidth - ym_win.offsetWidth + sl);
                sTop = Math.min(Math.max(sTop, st), rootEl.clientHeight - ym_win.offsetHeight + st);
            } else if (curCfg.showMask && '' + winSize != '' + getWinSize()) //及时调整遮罩大小
                resizeMask(true);
            setStyle(ym_wins, {
                left: sLeft + 'px',
                top: sTop + 'px'
            });
        };
        /*mouseup事件*/
        var uEvent = function () {
            filterWin(1);
            detachEvent("mousemove", mEvent, bindEl);
            detachEvent("mouseup", uEvent, bindEl);
            saveWinInfo(); //保存当前窗口的位置
            curCfg.iframe && setStyle(getPage().nextSibling, 'display', 'none');
            /*IE下窗口外部拖动*/
            bindEl && (detachEvent("losecapture", uEvent, bindEl), bindEl.releaseCapture());
        };
        addEvent('mousedown',
        function (e) {
            if ((e.srcElement || e.target).parentNode == ym_hTool) return false; //点击操作按钮不进行启用拖动处理
            filterWin(curCfg.winAlpha); //鼠标按下时窗体的透明度
            /*鼠标与弹出框的左上角的位移差*/
            Dialog.apply(dragVar, {
                offX: parseInt(ym_win.style.left) - e.clientX,
                offY: parseInt(ym_win.style.top) - e.clientY
            });
            addEvent("mousemove", mEvent, bindEl);
            addEvent("mouseup", uEvent, bindEl);
            if (curCfg.iframe) {
                var cfg = {
                    display: ''
                },
                pg = getPage();
                isCompat && IE6 && Dialog.apply(cfg, {
                    width: pg.offsetWidth,
                    height: pg.offsetHeight
                }); //IE6必须设置高度
                setStyle(pg.nextSibling, cfg)
            }
            /*IE下窗口外部拖动*/
            bindEl && (addEvent("losecapture", uEvent, bindEl), bindEl.setCapture());
        },
        ym_head);
        /*页面滚动弹出窗口滚动*/
        var scrollEvent = function () {
            setStyle(ym_win, {
                left: dragVar._offX + rootEl.scrollLeft + 'px',
                top: dragVar._offY + rootEl.scrollTop + 'px'
            });
        };
        /*键盘监听*/
        var keydownEvent = function (e) {
            var keyCode = e.keyCode;
            if (keyCode == 27) destroy(); //esc键
            if (btnCache) {
                var l = btnCache.length,
                nofocus;
                /*tab键/左右方向键切换焦点*/
                document.activeElement && document.activeElement.id != btnCache[btnIndex].id && (nofocus = true);
                if (keyCode == 9 || keyCode == 39) nofocus && (btnIndex = -1),
                $(btnCache[++btnIndex == l ? (--btnIndex) : btnIndex].id).focus();
                if (keyCode == 37) nofocus && (btnIndex = l),
                $(btnCache[--btnIndex < 0 ? (++btnIndex) : btnIndex].id).focus();
                if (keyCode == 13) return true;
            }
            /*禁止F1-F12/ tab 回车*/
            return keyEvent(e, (keyCode > 110 && keyCode < 123) || keyCode == 9 || keyCode == 13);
        };
        /*监听键盘事件*/
        var keyEvent = function (e, d) {
            e = e || event;
            /*允许对表单项进行操作*/
            if (!d && /input|select|textarea/i.test((e.srcElement || e.target).tagName)) return true;
            try {
                e.returnValue = false;
                e.keyCode = 0;
            } catch (ex) {
                e.preventDefault && e.preventDefault();
            }
            return false;
        };
        maskLevel.oncontextmenu = keyEvent; //禁止右键和选择
        /*重新计算遮罩的大小*/
        var resizeMask = function (noDelay) {
            setStyle(maskEl, 'display', 'none'); //先隐藏
            var size = getWinSize();
            var resize = function () {
                setStyle(maskEl, {
                    width: size[0] + 'px',
                    height: size[1] + 'px',
                    display: ''
                });
            };
            isIE ? noDelay === true ? resize() : setTimeout(resize, 0) : resize();
            cur_state == 'min' ? doMin() : cur_state == 'max' ? doMax() : setWinSize(); //最大化最小化状态还原
        };
        /*蒙版的显示隐藏,state:true显示,false隐藏，默认为true*/
        var maskVisible = function (visible) {
            if (!curCfg.showMask) return; //无遮罩
            (visible === false ? detachEvent : addEvent)("resize", resizeMask, window); //页面大小改变及时调整遮罩大小
            if (visible === false) return setStyle(maskEl, 'display', 'none'); //隐藏遮罩
            setStyle(maskLevel, {
                background: curCfg.maskAlphaColor,
                filter: 'Alpha(opacity=' + curCfg.maskAlpha * 100 + ')',
                opacity: curCfg.maskAlpha
            });
            resizeMask(true);
        };
        /*计算指定位置的坐标，返回数组*/
        var getPos = function (f) {
            /*传入有效的数组，则采用用户坐标（需要做简单处理），否则根据传入字符串到map中匹配，如果匹配不到则默认采用c配置*/
            f = isArray(f) && f.length == 2 ? (f[0] + '+{2},{3}+' + f[1]) : (posMap[f] || posMap['c']);
            var pos = [rootEl.clientWidth - ym_win.offsetWidth, rootEl.clientHeight - ym_win.offsetHeight].concat(getScrollPos());
            var arr = f.replace(/\{(\d)\}/g,
            function (s, s1) {
                return pos[s1]
            }).split(',');
            return [eval(arr[0]), eval(arr[1])];
        }; //9个常用位置常数
        var posMap = {
            c: '{0}/2+{2},{1}/2+{3}',
            l: '{2},{1}/2+{3}',
            r: '{0}+{2},{1}/2+{3}',
            t: '{0}/2+{2},{3}',
            b: '{0}/2,{1}+{3}',
            lt: '{2},{3}',
            lb: '{2},{1}+{3}',
            rb: '{0}+{2},{1}+{3}',
            rt: '{0}+{2},{3}'
        };
        /*设定窗口大小及定位*/
        var setWinSize = function (w, h, pos) {
            if (ym_win.style.display == 'none') return; //当前不可见则不处理
            /*默认使用配置的宽高*/
            h = parseInt(h) || curCfg.height;
            w = parseInt(w) || curCfg.width;
            setStyle(ym_wins, {
                width: w + 'px',
                height: h + 'px',
                left: 0,
                top: 0
            });
            pos = getPos(pos || curCfg.winPos); //支持自定义坐标，或者默认配置
            setStyle(ym_wins, {
                top: pos[1] + 'px',
                left: pos[0] + 'px'
            });
            saveWinInfo(); //保存当前窗口位置信息
            setStyle(ym_body, 'height', h - $height(ym_headbox) - $height(ym_btn) - $height(ym_bottom) + 'px'); //设定内容区的高度
            isCompat && IE6 && curCfg.iframe && setStyle(getPage(), {
                height: ym_body.clientHeight
            }); //IE6标准模式下要计算iframe高度
        };
        var _obj = []; //IE中可见的obj元素
        var cacheWin = []; //队列中的窗口
        var winVisible = function (visible) {
            var fn = visible === false ? detachEvent : addEvent;
            fn('scroll', curCfg.fixPosition && !useFixed ? scrollEvent : saveWinInfo, window);
            setStyle(ym_wins, 'position', curCfg.fixPosition && useFixed ? 'fixed' : 'absolute');
            fn('keydown', keydownEvent);
            if (visible === false) { //关闭
                setStyle(ym_shadow, 'display', 'none');
                /*关闭窗口执行的操作*/
                var closeFn = function () {
                    setStyle(ym_win, 'display', 'none');
                    setStyle(_obj, 'visibility', 'visible');
                    _obj = []; //把当前弹出移除
                    cacheWin.shift(); //读取队列中未执行的弹出
                    if (cacheWin.length) Dialog.show.apply(null, cacheWin[0].concat(true))
                };
                /*渐变方式关闭*/
                var alphaClose = function () {
                    var alpha = 1;
                    var hideFn = function () {
                        alpha = Math.max(alpha - curCfg.slideCfg.increment, 0);
                        filterWin(alpha);
                        if (alpha == 0) {
                            maskVisible(false);
                            closeFn();
                            clearInterval(it);
                        }
                    };
                    hideFn();
                    var it = setInterval(hideFn, curCfg.slideCfg.interval);
                };
                curCfg.useSlide ? alphaClose() : closeFn();
                return;
            }
            for (var o = document.getElementsByTagName('object'), i = o.length - 1; i > -1; i--) o[i].style.visibility != 'hidden' && _obj.push(o[i]) && (o[i].style.visibility = 'hidden');
            setStyle([ym_hText, ym_hTool], 'display', (curCfg.titleBar ? '' : 'none'));
            ym_head.className = 'ym-tc' + (curCfg.titleBar ? '' : ' ym-ttc'); //无标题栏
            ym_hText.innerHTML = curCfg.title; //标题
            for (var i = 0,
            c = ['min', 'max', 'close']; i < 3; i++) {
                ym_ico[i].style.display = curCfg[c[i] + 'Btn'] ? '' : 'none';
                ym_ico[i].title = curCfg[c[i] + 'Txt'];
            }
            /*iframe如果不加上opacity=100，则ym-win和用于遮罩iframe的div也透明时，iframe也就透明了*/
            var ifmStyle = 'position:absolute;width:100%;height:100%;top:0;left:0;opacity:1;filter:alpha(opacity=100)';
            ym_body.innerHTML = !curCfg.iframe ? ('<div class="' + curCfg.msgCls + '">' + curCfg.message + '</div>') : "<iframe style='" + ifmStyle + "' border='0' frameborder='0' src='" + curCfg.message + "'></iframe><div style='" + ifmStyle + ";background:#000;opacity:0.1;filter:alpha(opacity=10);display:none'></div>"; //内容
            (function (el, obj) {
                for (var i in obj) try {
                    el[i] = obj[i]
                } catch (e) { }
            })(ym_body.firstChild, curCfg.iframe); //为iframe添加自定义属性
            ym_body.className = "ym-body " + curCfg.icoCls; //图标类型
            setStyle(ym_btn, 'display', ((ym_btnContent.innerHTML = joinBtn(mkBtn(curCfg.btn))) ? '' : 'none')); //没有按钮则隐藏
            !curCfg.useSlide && curCfg.showShadow && setStyle(ym_shadow, 'display', '');
            setStyle(ym_win, 'display', '');
            doNormal(true);
            filterWin(curCfg.useSlide ? 0 : 1); //此处使用filter同时可以解决IE非标准模式下有时下边会出现1px空白，使内容与下部不衔接的问题
            /*渐变方式显示*/
            curCfg.useSlide && (function () {
                var alpha = 0;
                var showFn = function () {
                    alpha = Math.min(alpha + curCfg.slideCfg.increment, 1);
                    filterWin(alpha);
                    if (alpha == 1) {
                        clearInterval(it);
                        curCfg.showShadow && setStyle(ym_shadow, 'display', '')
                    }
                };
                showFn();
                var it = setInterval(showFn, curCfg.slideCfg.interval);
            })();
            btnCache && $(btnCache[btnIndex = 0].id).focus(); //第一个按钮获取焦点
            /*是否禁止选择、禁止右键*/
            ym_win.onselectstart = curCfg.allowSelect ? null : keyEvent;
            ym_win.oncontextmenu = curCfg.allowRightMenu ? null : keyEvent;
        }; //初始化
        var init = function () {
            ym_wins = [ym_win].concat(curCfg.showShadow ? ym_shadow : ''); //是否使用阴影
            maskVisible();
            winVisible();
        }; //销毁
        var destroy = function () {
            !curCfg.useSlide && maskVisible(false);
            winVisible(false);
        }; //取得iframe
        var getPage = function () {
            return curCfg.iframe ? ym_body.firstChild : null
        };
        Dialog.apply(Dialog, {
            close: destroy,
            max: max,
            min: min,
            normal: doNormal,
            getPage: getPage,
            /*显示消息框,fargs:优先配置，会覆盖args中的配置*/
            /*show 强制显示*/
            show: function (args, fargs, show) { //如果有窗口未关闭则将本次传入的信息放到队列里
                if (!show && cacheWin.push([args, fargs]) && cacheWin.length > 1) return;
                /*支持两种参数传入方式:(1)JSON方式 (2)多个参数传入*/
                var a = [].slice.call(args, 0),
                o = {},
                j = -1;
                if (!isObj(a[0])) {
                    for (var i in dftCfg) if (a[++j]) o[i] = a[j];
                } else {
                    o = a[0];
                }
                Dialog.apply(curCfg, Dialog.apply({},
                o, fargs), Dialog.setDefaultCfg()); //先还原默认配置
                /*修正curCfg中的无效值(null/undefined)改为默认值*/
                for (var i in curCfg) curCfg[i] = curCfg[i] != null ? curCfg[i] : Dialog.cfg[i];
                init();
            },
            doHandler: function (sign, autoClose, closeFirst) {
                if (autoClose == undefined ? curCfg.autoClose : autoClose) destroy();
                try {
                    (curCfg.handler)(sign)
                } catch (e) {
                    alert(e.message)
                };
            },
            resizeWin: setWinSize,
            /*设定默认配置*/
            setDefaultCfg: function (cfg) {
                return Dialog.cfg = Dialog.apply({},
                cfg, Dialog.apply({},
                Dialog.cfg, dftCfg));
            },
            getButtons: function () {
                var btns = btnCache || [],
                btn,
                rBtn = [];
                while (btn = btns.shift()) rBtn.push($(btn.id));
                return rBtn;
            }
        });
        Dialog.setDefaultCfg(); //初始化默认配置
        /*执行用户初始化时的调用*/
        var t;
        for (var i in _initFn) while (t = _initFn[i].shift()) Dialog[i].apply(null, t);
        /*取消事件绑定*/
        addEvent('unload',
        function () {
            while (Dialog.eventList.length) detachEvent.apply(null, Dialog.eventList.shift());
        },
        window);
    })();
})();
//各消息框的相同操作
Dialog.apply(Dialog, {
    alert: function () {
        Dialog.show(arguments, {
            icoCls: 'ymPrompt_alert',
            btn: ['OK']
        });
    },
    succeedInfo: function () {
        Dialog.show(arguments, {
            icoCls: 'ymPrompt_succeed',
            btn: ['OK']
        });
    },
    errorInfo: function () {
        Dialog.show(arguments, {
            icoCls: 'ymPrompt_error',
            btn: ['OK']
        });
    },
    confirmInfo: function () {
        Dialog.show(arguments, {
            icoCls: 'ymPrompt_confirm',
            btn: ['OK', 'CANCEL']
        });
    },
    win: function () {
        Dialog.show(arguments);
    }
});

/*
**************************************************日期时间选择**************************************************
*/
var Picker = new Class({
    Implements: [Options, Events],
    _attachto: '',
    options: {
        pickerClass: 'datepicker',
        inject: null,
        animationDuration: 400,
        useFadeInOut: true,
        positionOffset: {
            x: 0,
            y: 0
        },
        pickerPosition: 'bottom',
        draggable: true,
        showOnInit: true,
        columns: 1,
        footer: false
    },
    initialize: function (attachTo, options) {
        this._attachto = $(attachTo);
        this.setOptions(options);
        this.constructPicker();
        if (this.options.showOnInit) this.show()
    },
    constructPicker: function () {
        var options = this.options;
        var picker = this.picker = new Element('div', {
            'class': options.pickerClass,
            styles: {
                left: 0,
                top: 0,
                display: 'none',
                opacity: 0
            }
        }).inject(this._attachto, 'after');
        picker.addClass('column_' + options.columns);
        if (options.useFadeInOut) {
            picker.set('tween', {
                duration: options.animationDuration,
                link: 'cancel'
            })
        }
        var header = this.header = new Element('div.header').inject(picker);
        var title = this.title = new Element('div.title').inject(header);
        var titleID = this.titleID = 'pickertitle-' + String.uniqueID();
        this.titleText = new Element('div', {
            'role': 'heading',
            'class': 'titleText',
            'id': titleID,
            'aria-live': 'assertive',
            'aria-atomic': 'true'
        }).inject(title);
        this.closeButton = new Element('div.closeButton[text=x][role=button]').addEvent('click', this.close.pass(false, this)).inject(header);
        var body = this.body = new Element('div.body').inject(picker);
        if (options.footer) {
            this.footer = new Element('div.footer').inject(picker);
            picker.addClass('footer')
        }
        var slider = this.slider = new Element('div.slider', {
            styles: {
                position: 'absolute',
                top: 0,
                left: 0
            }
        }).set('tween', {
            duration: options.animationDuration,
            transition: Fx.Transitions.Quad.easeInOut
        }).inject(body);
        this.newContents = new Element('div', {
            styles: {
                position: 'absolute',
                top: 0,
                left: 0
            }
        }).inject(slider);
        this.oldContents = new Element('div', {
            styles: {
                position: 'absolute',
                top: 0
            }
        }).inject(slider);
        this.originalColumns = options.columns;
        this.setColumns(options.columns);
        var shim = this.shim = window['IframeShim'] ? new IframeShim(picker) : null;
        if (options.draggable && typeOf(picker.makeDraggable) == 'function') {
            this.dragger = picker.makeDraggable(shim ? {
                onDrag: shim.position.bind(shim)
            } : null);
            picker.setStyle('cursor', 'move')
        }
    },
    open: function (noFx) {
        if (this.opened == true) return this;
        this.opened = true;
        var picker = this.picker.setStyle('display', 'block').set('aria-hidden', 'false');
        if (this.shim) this.shim.show();
        this.fireEvent('open');
        if (this.options.useFadeInOut && !noFx) {
            picker.fade('in').get('tween').chain(this.fireEvent.pass('show', this));
        } else {
            picker.setStyle('opacity', 1);
            this.fireEvent('show');
        }
        return this;
    },
    show: function () {
        return this.open(true)
    },
    close: function (noFx) {
        if (this.opened == false) return this;
        this.opened = false;
        this.fireEvent('close');
        var self = this,
        picker = this.picker,
        hide = function () {
            picker.setStyle('display', 'none').set('aria-hidden', 'true');
            if (self.shim) self.shim.hide();
            self.fireEvent('hide')
        };
        if (this.options.useFadeInOut && !noFx) {
            picker.fade('out').get('tween').chain(hide)
        } else {
            picker.setStyle('opacity', 0);
            hide()
        }
        return this
    },
    hide: function () {
        return this.close(true)
    },
    toggle: function () {
        return this[this.opened == true ? 'close' : 'open']()
    },
    destroy: function () {
        this.picker.destroy();
        if (this.shim) this.shim.destroy()
    },
    position: function (x, y) {
        var offset = this.options.positionOffset;
        var elementCoords = x.getCoordinates();
        var _win = window.getScrollSize();
        var picker = this.picker.setStyles({
            display: 'block',
            top: -300,
            left: -300
        }).set('aria-hidden', 'false');
        var pickersize = picker.getSize();
        picker.setStyles({
            top: 0,
            left: 0
        });
        if ((_win.y - elementCoords.bottom) < pickersize.y) {
            offset.y = -pickersize.y - 1;
            offset.x = x.getPosition().x - picker.getPosition().x;
        } else {
            offset.x = x.getPosition().x - picker.getPosition().x;
            offset.y = (x.getPosition().y - picker.getPosition().y) + x.getSize().y + 1;
        }
        picker.setStyles({
            top: offset.y,
            left: offset.x
        });
        //            var offset = this.options.positionOffset,
        //                scroll = document.getScroll(),
        //                size = document.getSize(),
        //                pickersize = this.picker.getSize();
        //            if (typeOf(x) == 'element') {
        //                var element = x, where = y || this.options.pickerPosition;
        //                var elementCoords = element.getCoordinates();
        //                x = (where == 'left') ? elementCoords.left - pickersize.x : (where == 'bottom' || where == 'top') ? elementCoords.left : elementCoords.right;
        //                y = (where == 'bottom') ? elementCoords.bottom : (where == 'top') ? elementCoords.top - pickersize.y : elementCoords.top;
        //                alert(y);
        //            }
        //            x += offset.x * ((where && where == 'left') ? -1 : 1);
        //            y += offset.y * ((where && where == 'top') ? -1 : 1);
        //            if ((x + pickersize.x) > (size.x + scroll.x)) x = (size.x + scroll.x) - pickersize.x;
        //            if ((y + pickersize.y) > (size.y + scroll.y)) y = (size.y + scroll.y) - pickersize.y;
        //            if (x < 0) x = 0;
        //            if (y < 0) y = 0;
        //            //            alert(x + ":" + y);
        //            this.picker.setStyles({
        //                left: x,
        //                top: y
        //            });
        if (this.shim) this.shim.position();
        return this
    },
    setBodySize: function () {
        var bodysize = this.bodysize = this.body.getSize();
        this.slider.setStyles({
            width: 2 * bodysize.x,
            height: bodysize.y
        });
        this.oldContents.setStyles({
            left: bodysize.x,
            width: bodysize.x,
            height: bodysize.y
        });
        this.newContents.setStyles({
            width: bodysize.x,
            height: bodysize.y
        })
    },
    setColumnContent: function (column, content) {
        var columnElement = this.columns[column];
        if (!columnElement) return this;
        var type = typeOf(content);
        if (['string', 'number'].contains(type)) columnElement.set('text', content);
        else columnElement.empty().adopt(content);
        return this
    },
    setColumnsContent: function (content, fx) {
        var old = this.columns;
        this.columns = this.newColumns;
        this.newColumns = old;
        content.forEach(function (_content, i) {
            this.setColumnContent(i, _content)
        },
        this);
        return this.setContent(null, fx)
    },
    setColumns: function (columns) {
        var _columns = this.columns = new Elements,
        _newColumns = this.newColumns = new Elements;
        for (var i = columns; i--; ) {
            _columns.push(new Element('div.column').addClass('column_' + (columns - i)));
            _newColumns.push(new Element('div.column').addClass('column_' + (columns - i)))
        }
        var oldClass = 'column_' + this.options.columns,
        newClass = 'column_' + columns;
        this.picker.removeClass(oldClass).addClass(newClass);
        this.options.columns = columns;
        return this
    },
    setContent: function (content, fx) {
        if (content) return this.setColumnsContent([content], fx);
        var old = this.oldContents;
        this.oldContents = this.newContents;
        this.newContents = old;
        this.newContents.empty();
        this.newContents.adopt(this.columns);
        this.setBodySize();
        if (fx) {
            this.fx(fx)
        } else {
            this.slider.setStyle('left', 0);
            this.oldContents.setStyles({
                left: 0,
                opacity: 0
            });
            this.newContents.setStyles({
                left: 0,
                opacity: 1
            })
        }
        return this
    },
    fx: function (fx) {
        var oldContents = this.oldContents,
        newContents = this.newContents,
        slider = this.slider,
        bodysize = this.bodysize;
        if (fx == 'right') {
            oldContents.setStyles({
                left: 0,
                opacity: 1
            });
            newContents.setStyles({
                left: bodysize.x,
                opacity: 1
            });
            slider.setStyle('left', 0).tween('left', 0, -bodysize.x)
        } else if (fx == 'left') {
            oldContents.setStyles({
                left: bodysize.x,
                opacity: 1
            });
            newContents.setStyles({
                left: 0,
                opacity: 1
            });
            slider.setStyle('left', -bodysize.x).tween('left', -bodysize.x, 0)
        } else if (fx == 'fade') {
            slider.setStyle('left', 0);
            oldContents.setStyle('left', 0).set('tween', {
                duration: this.options.animationDuration / 2
            }).tween('opacity', 1, 0).get('tween').chain(function () {
                oldContents.setStyle('left', bodysize.x)
            });
            newContents.setStyles({
                opacity: 0,
                left: 0
            }).set('tween', {
                duration: this.options.animationDuration
            }).tween('opacity', 0, 1)
        }
    },
    toElement: function () {
        return this.picker
    },
    setTitle: function (content, fn) {
        if (!fn) fn = Function.from;
        this.titleText.empty().adopt(Array.from(content).map(function (item, i) {
            return typeOf(item) == 'element' ? item : new Element('div.column', {
                text: fn(item, this.options)
            }).addClass('column_' + (i + 1))
        },
        this));
        return this
    },
    setTitleEvent: function (fn) {
        this.titleText.removeEvents('click');
        if (fn) this.titleText.addEvent('click', fn);
        this.titleText.setStyle('cursor', fn ? 'pointer' : '');
        return this
    }
});
Picker.Attach = new Class({
    Extends: Picker,
    options: {
        togglesOnly: true,
        showOnInit: false,
        blockKeydown: true
    },
    initialize: function (attachTo, options) {
        this.parent(attachTo, options);
        this.attachedEvents = [];
        this.attachedElements = [];
        this.toggles = [];
        this.inputs = [];
        var documentEvent = function (event) {
            if (this.attachedElements.contains(event.target)) return;
            this.close()
        } .bind(this);
        var document = this.picker.getDocument().addEvent('click', documentEvent);
        var preventPickerClick = function (event) {
            event.stopPropagation();
            return false
        };
        this.picker.addEvent('click', preventPickerClick);
        if (this.options.toggleElements) this.options.toggle = document.getElements(this.options.toggleElements);
        this.attach(attachTo, this.options.toggle)
    },
    attach: function (attachTo, toggle) {
        if (typeOf(attachTo) == 'string') attachTo = document.id(attachTo);
        if (typeOf(toggle) == 'string') toggle = document.id(toggle);
        var elements = Array.from(attachTo),
        toggles = Array.from(toggle),
        allElements = [].append(elements).combine(toggles),
        self = this;
        var closeEvent = function (event) {
            var stopInput = self.options.blockKeydown && event.type == 'keydown' && !(['tab', 'esc'].contains(event.key)),
            isCloseKey = event.type == 'keydown' && (['tab', 'esc'].contains(event.key)),
            isA = event.target.get('tag') == 'a';
            if (stopInput || isA) event.preventDefault();
            if (isCloseKey || isA) self.close()
        };
        var getOpenEvent = function (element) {
            return function (event) {
                var tag = event.target.get('tag');
                if (tag == 'input' && event.type == 'click' && !element.match(':focus') || (self.opened && self.input == element)) return;
                if (tag == 'a') event.stop();
                self.position(element);
                self.open();
                self.fireEvent('attached', [event, element])
            }
        };
        var getToggleEvent = function (open, close) {
            return function (event) {
                if (self.opened) close(event);
                else open(event)
            }
        };
        allElements.each(function (element) {
            if (self.attachedElements.contains(element)) return;
            var events = {},
            tag = element.get('tag'),
            openEvent = getOpenEvent(element),
            toggleEvent = getToggleEvent(openEvent, closeEvent);
            if (tag == 'input') {
                if (!self.options.togglesOnly || !toggles.length) {
                    events = {
                        focus: openEvent,
                        click: openEvent,
                        keydown: closeEvent
                    }
                }
                self.inputs.push(element)
            } else {
                if (toggles.contains(element)) {
                    self.toggles.push(element);
                    events.click = toggleEvent
                } else {
                    events.click = openEvent
                }
            }
            element.addEvents(events);
            self.attachedElements.push(element);
            self.attachedEvents.push(events)
        });
        return this
    },
    detach: function (attachTo, toggle) {
        if (typeOf(attachTo) == 'string') attachTo = document.id(attachTo);
        if (typeOf(toggle) == 'string') toggle = document.id(toggle);
        var elements = Array.from(attachTo),
        toggles = Array.from(toggle),
        allElements = [].append(elements).combine(toggles),
        self = this;
        if (!allElements.length) allElements = self.attachedElements;
        allElements.each(function (element) {
            var i = self.attachedElements.indexOf(element);
            if (i < 0) return;
            var events = self.attachedEvents[i];
            element.removeEvents(events);
            delete self.attachedEvents[i];
            delete self.attachedElements[i];
            var toggleIndex = self.toggles.indexOf(element);
            if (toggleIndex != -1) delete self.toggles[toggleIndex];
            var inputIndex = self.inputs.indexOf(element);
            if (toggleIndex != -1) delete self.inputs[inputIndex]
        });
        return this
    },
    destroy: function () {
        this.detach();
        return this.parent()
    }
}); (function () {
    this.DatePicker = Picker.Date = new Class({
        Extends: Picker.Attach,
        options: {
            timePicker: false,
            timePickerOnly: false,
            timeWheelStep: 1,
            yearPicker: true,
            yearsPerPage: 20,
            startDay: 1,
            rtl: false,
            startView: 'days',
            openLastView: false,
            pickOnly: false,
            canAlwaysGoUp: ['months', 'days'],
            updateAll: false,
            weeknumbers: false,
            months_abbr: null,
            days_abbr: null,
            years_title: function (date, options) {
                var year = date.get('year');
                return year + '-' + (year + options.yearsPerPage - 1)
            },
            months_title: function (date, options) {
                return date.get('year')
            },
            days_title: function (date, options) {
                return date.format('%b %Y')
            },
            time_title: function (date, options) {
                return (options.pickOnly == 'time') ? Locale.get('DatePicker.select_a_time') : date.format('%d %B, %Y')
            }
        },
        initialize: function (attachTo, options) {
            this.parent(attachTo, options);
            this.setOptions(options);
            options = this.options; ['year', 'month', 'day', 'time'].some(function (what) {
                if (options[what + 'PickerOnly']) {
                    options.pickOnly = what;
                    return true
                }
                return false
            });
            if (options.pickOnly) {
                options[options.pickOnly + 'Picker'] = true;
                options.startView = options.pickOnly
            }
            var newViews = ['days', 'months', 'years']; ['month', 'year', 'decades'].some(function (what, i) {
                return (options.startView == what) && (options.startView = newViews[i])
            });
            options.canAlwaysGoUp = options.canAlwaysGoUp ? Array.from(options.canAlwaysGoUp) : [];
            if (options.minDate) {
                if (!(options.minDate instanceof Date)) options.minDate = Date.parse(options.minDate);
                options.minDate.clearTime()
            }
            if (options.maxDate) {
                if (!(options.maxDate instanceof Date)) options.maxDate = Date.parse(options.maxDate);
                options.maxDate.clearTime()
            }
            if (!options.format) {
                options.format = (options.pickOnly != 'time') ? Locale.get('Date.shortDate') : '';
                if (options.timePicker) options.format = (options.format) + (options.format ? ' ' : '') + Locale.get('Date.shortTime')
            }
            this.addEvent('attached',
            function (event, element) {
                if (!this.currentView || !options.openLastView) this.currentView = options.startView;
                this.date = limitDate(new Date(), options.minDate, options.maxDate);
                var tag = element.get('tag'),
                input;
                if (tag == 'input') input = element;
                else {
                    var index = this.toggles.indexOf(element);
                    if (this.inputs[index]) input = this.inputs[index]
                }
                this.getInputDate(input);
                this.input = input;
                this.setColumns(this.originalColumns)
            } .bind(this), true)
        },
        getInputDate: function (input) {
            this.date = new Date();
            if (!input) return;
            var date = Date.parse(input.get('value'));
            if (date == null || !date.isValid()) {
                var storeDate = input.retrieve('datepicker:value');
                if (storeDate) date = Date.parse(storeDate)
            }
            if (date != null && date.isValid()) this.date = date
        },
        constructPicker: function () {
            this.parent();
            if (!this.options.rtl) {
                this.previous = new Element('div.previous[html=&#171;]').inject(this.header);
                this.next = new Element('div.next[html=&#187;]').inject(this.header)
            } else {
                this.next = new Element('div.previous[html=&#171;]').inject(this.header);
                this.previous = new Element('div.next[html=&#187;]').inject(this.header)
            }
        },
        hidePrevious: function (_next, _show) {
            this[_next ? 'next' : 'previous'].setStyle('display', _show ? 'block' : 'none');
            return this
        },
        showPrevious: function (_next) {
            return this.hidePrevious(_next, true)
        },
        setPreviousEvent: function (fn, _next) {
            this[_next ? 'next' : 'previous'].removeEvents('click');
            if (fn) this[_next ? 'next' : 'previous'].addEvent('click', fn);
            return this
        },
        hideNext: function () {
            return this.hidePrevious(true)
        },
        showNext: function () {
            return this.showPrevious(true)
        },
        setNextEvent: function (fn) {
            return this.setPreviousEvent(fn, true)
        },
        setColumns: function (columns, view, date, viewFx) {
            var ret = this.parent(columns),
            method;
            if ((view || this.currentView) && (method = 'render' + (view || this.currentView).capitalize()) && this[method]) this[method](date || this.date.clone(), viewFx);
            return ret
        },
        renderYears: function (date, fx) {
            var options = this.options,
            pages = options.columns,
            perPage = options.yearsPerPage,
            _columns = [],
            _dates = [];
            this.dateElements = [];
            date = date.clone().decrement('year', date.get('year') % perPage);
            var iterateDate = date.clone().decrement('year', Math.floor((pages - 1) / 2) * perPage);
            for (var i = pages; i--; ) {
                var _date = iterateDate.clone();
                _dates.push(_date);
                _columns.push(renderers.years(timesSelectors.years(options, _date.clone()), options, this.date.clone(), this.dateElements,
                function (date) {
                    if (options.pickOnly == 'years') this.select(date);
                    else this.renderMonths(date, 'fade');
                    this.date = date
                } .bind(this)));
                iterateDate.increment('year', perPage)
            }
            this.setColumnsContent(_columns, fx);
            this.setTitle(_dates, options.years_title);
            var limitLeft = (options.minDate && date.get('year') <= options.minDate.get('year')),
            limitRight = (options.maxDate && (date.get('year') + options.yearsPerPage) >= options.maxDate.get('year'));
            this[(limitLeft ? 'hide' : 'show') + 'Previous']();
            this[(limitRight ? 'hide' : 'show') + 'Next']();
            this.setPreviousEvent(function () {
                this.renderYears(date.decrement('year', perPage), 'left')
            } .bind(this));
            this.setNextEvent(function () {
                this.renderYears(date.increment('year', perPage), 'right')
            } .bind(this));
            this.setTitleEvent(null);
            this.currentView = 'years'
        },
        renderMonths: function (date, fx) {
            var options = this.options,
            years = options.columns,
            _columns = [],
            _dates = [],
            iterateDate = date.clone().decrement('year', Math.floor((years - 1) / 2));
            this.dateElements = [];
            for (var i = years; i--; ) {
                var _date = iterateDate.clone();
                _dates.push(_date);
                _columns.push(renderers.months(timesSelectors.months(options, _date.clone()), options, this.date.clone(), this.dateElements,
                function (date) {
                    if (options.pickOnly == 'months') this.select(date);
                    else this.renderDays(date, 'fade');
                    this.date = date
                } .bind(this)));
                iterateDate.increment('year', 1)
            }
            this.setColumnsContent(_columns, fx);
            this.setTitle(_dates, options.months_title);
            var year = date.get('year'),
            limitLeft = (options.minDate && year <= options.minDate.get('year')),
            limitRight = (options.maxDate && year >= options.maxDate.get('year'));
            this[(limitLeft ? 'hide' : 'show') + 'Previous']();
            this[(limitRight ? 'hide' : 'show') + 'Next']();
            this.setPreviousEvent(function () {
                this.renderMonths(date.decrement('year', years), 'left')
            } .bind(this));
            this.setNextEvent(function () {
                this.renderMonths(date.increment('year', years), 'right')
            } .bind(this));
            var canGoUp = options.yearPicker && (options.pickOnly != 'months' || options.canAlwaysGoUp.contains('months'));
            var titleEvent = (canGoUp) ?
            function () {
                this.renderYears(date, 'fade')
            } .bind(this) : null;
            this.setTitleEvent(titleEvent);
            this.currentView = 'months'
        },
        renderDays: function (date, fx) {
            var options = this.options,
            months = options.columns,
            _columns = [],
            _dates = [],
            iterateDate = date.clone().decrement('month', Math.floor((months - 1) / 2));
            this.dateElements = [];
            for (var i = months; i--; ) {
                _date = iterateDate.clone();
                _dates.push(_date);
                _columns.push(renderers.days(timesSelectors.days(options, _date.clone()), options, this.date.clone(), this.dateElements,
                function (date) {
                    if (options.pickOnly == 'days' || !options.timePicker) this.select(date);
                    else this.renderTime(date, 'fade');
                    this.date = date
                } .bind(this)));
                iterateDate.increment('month', 1)
            }
            this.setColumnsContent(_columns, fx);
            this.setTitle(_dates, options.days_title);
            var yearmonth = date.format('%Y%m').toInt(),
            limitLeft = (options.minDate && yearmonth <= options.minDate.format('%Y%m')),
            limitRight = (options.maxDate && yearmonth >= options.maxDate.format('%Y%m'));
            this[(limitLeft ? 'hide' : 'show') + 'Previous']();
            this[(limitRight ? 'hide' : 'show') + 'Next']();
            this.setPreviousEvent(function () {
                this.renderDays(date.decrement('month', months), 'left')
            } .bind(this));
            this.setNextEvent(function () {
                this.renderDays(date.increment('month', months), 'right')
            } .bind(this));
            var canGoUp = options.pickOnly != 'days' || options.canAlwaysGoUp.contains('days');
            var titleEvent = (canGoUp) ?
            function () {
                this.renderMonths(date, 'fade')
            } .bind(this) : null;
            this.setTitleEvent(titleEvent);
            this.currentView = 'days'
        },
        renderTime: function (date, fx) {
            var options = this.options;
            this.setTitle(date, options.time_title);
            var originalColumns = this.originalColumns = options.columns;
            this.currentView = null;
            if (originalColumns != 1) this.setColumns(1);
            this.setContent(renderers.time(options, date.clone(),
            function (date) {
                this.select(date)
            } .bind(this)), fx);
            this.hidePrevious().hideNext().setPreviousEvent(null).setNextEvent(null);
            var canGoUp = options.pickOnly != 'time' || options.canAlwaysGoUp.contains('time');
            var titleEvent = (canGoUp) ?
            function () {
                this.setColumns(originalColumns, 'days', date, 'fade')
            } .bind(this) : null;
            this.setTitleEvent(titleEvent);
            this.currentView = 'time'
        },
        select: function (date, all) {
            this.date = date;
            var formatted = date.format(this.options.format),
            time = date.strftime(),
            inputs = (!this.options.updateAll && !all && this.input) ? [this.input] : this.inputs;
            inputs.each(function (input) {
                input.set('value', formatted).store('datepicker:value', time).fireEvent('change')
            },
            this);
            this.fireEvent('select', [date].concat(inputs));
            this.close();
            return this
        }
    });
    var timesSelectors = {
        years: function (options, date) {
            var times = [];
            for (var i = 0; i < options.yearsPerPage; i++) {
                times.push(+date);
                date.increment('year', 1)
            }
            return times
        },
        months: function (options, date) {
            var times = [];
            date.set('month', 0);
            for (var i = 0; i <= 11; i++) {
                times.push(+date);
                date.increment('month', 1)
            }
            return times
        },
        days: function (options, date) {
            var times = [];
            date.set('date', 1);
            while (date.get('day') != options.startDay) date.set('date', date.get('date') - 1);
            for (var i = 0; i < 42; i++) {
                times.push(+date);
                date.increment('day', 1)
            }
            return times
        }
    };
    var renderers = {
        years: function (years, options, currentDate, dateElements, fn) {
            var container = new Element('div.years'),
            today = new Date(),
            element,
            classes;
            years.each(function (_year, i) {
                var date = new Date(_year),
                year = date.get('year');
                classes = '.year.year' + i;
                if (year == today.get('year')) classes += '.today';
                if (year == currentDate.get('year')) classes += '.selected';
                element = new Element('div' + classes, {
                    text: year
                }).inject(container);
                dateElements.push({
                    element: element,
                    time: _year
                });
                if (isUnavailable('year', date, options)) element.addClass('unavailable');
                else element.addEvent('click', fn.pass(date))
            });
            return container
        },
        months: function (months, options, currentDate, dateElements, fn) {
            var today = new Date(),
            month = today.get('month'),
            thisyear = today.get('year'),
            selectedyear = currentDate.get('year'),
            container = new Element('div.months'),
            monthsAbbr = options.months_abbr || Locale.get('Date.months_abbr'),
            element,
            classes;
            months.each(function (_month, i) {
                var date = new Date(_month),
                year = date.get('year');
                classes = '.month.month' + (i + 1);
                if (i == month && year == thisyear) classes += '.today';
                if (i == currentDate.get('month') && year == selectedyear) classes += '.selected';
                element = new Element('div' + classes, {
                    text: monthsAbbr[i]
                }).inject(container);
                dateElements.push({
                    element: element,
                    time: _month
                });
                if (isUnavailable('month', date, options)) element.addClass('unavailable');
                else element.addEvent('click', fn.pass(date))
            });
            return container
        },
        days: function (days, options, currentDate, dateElements, fn) {
            var month = new Date(days[14]).get('month'),
            todayString = new Date().toDateString(),
            currentString = currentDate.toDateString(),
            weeknumbers = options.weeknumbers,
            container = new Element('table.days' + (weeknumbers ? '.weeknumbers' : ''), {
                role: 'grid',
                'aria-labelledby': this.titleID
            }),
            header = new Element('thead').inject(container),
            body = new Element('tbody').inject(container),
            titles = new Element('tr.titles').inject(header),
            localeDaysShort = options.days_abbr || Locale.get('Date.days_abbr'),
            day,
            classes,
            element,
            weekcontainer,
            dateString,
            where = options.rtl ? 'top' : 'bottom';
            if (weeknumbers) new Element('th.title.day.weeknumber', {
                text: Locale.get('DatePicker.week')
            }).inject(titles);
            for (day = options.startDay; day < (options.startDay + 7); day++) {
                new Element('th.title.day.day' + (day % 7), {
                    text: localeDaysShort[(day % 7)],
                    role: 'columnheader'
                }).inject(titles, where)
            }
            days.each(function (_date, i) {
                var date = new Date(_date);
                if (i % 7 == 0) {
                    weekcontainer = new Element('tr.week.week' + (Math.floor(i / 7))).set('role', 'row').inject(body);
                    if (weeknumbers) new Element('th.day.weeknumber', {
                        text: date.get('week'),
                        scope: 'row',
                        role: 'rowheader'
                    }).inject(weekcontainer)
                }
                dateString = date.toDateString();
                classes = '.day.day' + date.get('day');
                if (dateString == todayString) classes += '.today';
                if (date.get('month') != month) classes += '.otherMonth';
                element = new Element('td' + classes, {
                    text: date.getDate(),
                    role: 'gridcell'
                }).inject(weekcontainer, where);
                if (dateString == currentString) element.addClass('selected').set('aria-selected', 'true');
                else element.set('aria-selected', 'false');
                dateElements.push({
                    element: element,
                    time: _date
                });
                if (isUnavailable('date', date, options)) element.addClass('unavailable');
                else element.addEvent('click', fn.pass(date.clone()))
            });
            return container
        },
        time: function (options, date, fn) {
            var container = new Element('div.time'),
            initMinutes = (date.get('minutes') / options.timeWheelStep).round() * options.timeWheelStep;
            if (initMinutes >= 60) initMinutes = 0;
            date.set('minutes', initMinutes);
            var hoursInput = new Element('input.hour[type=text]', {
                title: Locale.get('DatePicker.use_mouse_wheel'),
                value: date.format('%H'),
                events: {
                    click: function (event) {
                        event.target.focus();
                        event.stop()
                    },
                    mousewheel: function (event) {
                        event.stop();
                        hoursInput.focus();
                        var value = hoursInput.get('value').toInt();
                        value = (event.wheel > 0) ? ((value < 23) ? value + 1 : 0) : ((value > 0) ? value - 1 : 23);
                        date.set('hours', value);
                        hoursInput.set('value', date.format('%H'))
                    } .bind(this)
                },
                maxlength: 2
            }).inject(container);
            var minutesInput = new Element('input.minutes[type=text]', {
                title: Locale.get('DatePicker.use_mouse_wheel'),
                value: date.format('%M'),
                events: {
                    click: function (event) {
                        event.target.focus();
                        event.stop()
                    },
                    mousewheel: function (event) {
                        event.stop();
                        minutesInput.focus();
                        var value = minutesInput.get('value').toInt();
                        value = (event.wheel > 0) ? ((value < 59) ? (value + options.timeWheelStep) : 0) : ((value > 0) ? (value - options.timeWheelStep) : (60 - options.timeWheelStep));
                        if (value >= 60) value = 0;
                        date.set('minutes', value);
                        minutesInput.set('value', date.format('%M'))
                    } .bind(this)
                },
                maxlength: 2
            }).inject(container);
            new Element('div.separator[text=:]').inject(container);
            new Element('input.ok[type=submit]', {
                value: '确定',
                events: {
                    click: function (event) {
                        event.stop();
                        date.set({
                            hours: hoursInput.get('value').toInt(),
                            minutes: minutesInput.get('value').toInt()
                        });
                        fn(date.clone())
                    }
                }
            }).inject(container);
            return container
        }
    };
    Picker.Date.defineRenderer = function (name, fn) {
        renderers[name] = fn;
        return this
    };
    var limitDate = function (date, min, max) {
        if (min && date < min) return min;
        if (max && date > max) return max;
        return date
    };
    var isUnavailable = function (type, date, options) {
        var minDate = options.minDate, maxDate = options.maxDate, availableDates = options.availableDates, year, month, day, ms;
        if (!minDate && !maxDate && !availableDates) return false;
        date.clearTime();
        if (type == 'year') {
            year = date.get('year');
            return ((minDate && year < minDate.get('year')) || (maxDate && year > maxDate.get('year')) || ((availableDates != null && !options.invertAvailable) && (availableDates[year] == null || Object.getLength(availableDates[year]) == 0 || Object.getLength(Object.filter(availableDates[year],
            function (days) {
                return (days.length > 0)
            })) == 0)))
        }
        if (type == 'month') {
            year = date.get('year');
            month = date.get('month') + 1;
            ms = date.format('%Y%m').toInt();
            return ((minDate && ms < minDate.format('%Y%m').toInt()) || (maxDate && ms > maxDate.format('%Y%m').toInt()) || ((availableDates != null && !options.invertAvailable) && (availableDates[year] == null || availableDates[year][month] == null || availableDates[year][month].length == 0)))
        }
        year = date.get('year');
        month = date.get('month') + 1;
        day = date.get('date');
        var dateAllow = (minDate && date < minDate) || (minDate && date > maxDate);
        if (availableDates != null) {
            dateAllow = dateAllow || availableDates[year] == null || availableDates[year][month] == null || !availableDates[year][month].contains(day);
            if (options.invertAvailable) dateAllow = !dateAllow
        }
        return dateAllow
    }
})();
/*
**************************************************下拉框**************************************************
*/
var DropList = new Class({
    Implements: [Options],
    options: {
        container: '',
        multiple: false,
        parent: false
    },
    element: null,
    coord: {},
    size: {},
    loadurl: false,
    initialize: function (options) {
        this.setOptions(options);
        this.element = $(options.container + '_txt');
        if (!this.element) return;
        this.src = '/Developer/DropMenu.aspx?container=' + options.container + '&multiple=' + options.multiple + '&parent=' + options.parent;
        this.element.setStyle('background', 'url(/Developer/images/ico/DropDownList.gif) no-repeat right bottom');
        this.element.addEvents({
            click: this.Show.bind(this),
            mouseenter: function () {
                this.element.setStyle('background', 'url(/Developer/images/ico/DropDownList.gif) no-repeat right top');
            } .bind(this),
            mouseleave: function () {
                this.element.setStyle('background', 'url(/Developer/images/ico/DropDownList.gif) no-repeat right bottom');
            } .bind(this)
        });

        this.mask = new Element('div', {
            styles: {
                top: 0,
                left: 0,
                opacity: 0,
                width: '100%',
                display: 'none',
                background: '#000',
                position: 'absolute'
            },
            events: {
                click: this.Hide.bind(this)
            }
        }).inject(document.body, 'top');

        var size = this.element.getSize();
        this.iframe = new IFrame({
            //src: this.src,
            frameborder: 0,
            styles: {
                top: 0,
                left: 0,
                width: size.x - (this.element.getStyle('border').toInt() * 2),
                height: 100,
                position: 'absolute',
                border: '1px solid #add2da',
                'background-color': '#fff'
            }
        }).inject(this.element, 'after');
        this.iframe.setStyles({
            top: (this.element.getPosition().y - this.iframe.getPosition().y) + size.y + 1,
            left: this.element.getPosition().x - this.iframe.getPosition().x,
            height: 0,
            opacity: 0,
            display: 'none'
        });
    },
    Hide: function (event) {
        event.stop();
        if (this.transition) this.transition.cancel();
        this.transition = new Fx.Morph(this.iframe, {
            duration: 50,
            transition: Fx.Transitions.linear,
            onComplete: function () {
                this.iframe.setStyles({
                    display: 'none'
                });
                this.mask.setStyles({
                    height: 0,
                    display: 'none'
                });
            } .bind(this)
        }).start({
            height: 0,
            opacity: 0
        });
    },
    Show: function (event) {
        event.stop();
        if (!this.loadurl) {
            this.loadurl = true;
            this.iframe.set('src', this.src);
        }
        this.size = window.getScrollSize();
        this.mask.setStyles({
            height: this.size.y,
            display: 'block'
        });

        this.iframe.setStyles({
            display: 'block'
        });
        if (this.transition) this.transition.cancel();
        this.transition = new Fx.Morph(this.iframe, {
            duration: 300,
            transition: Fx.Transitions.linear
        }).start({
            height: 300,
            opacity: 1
        });
    }
});
/*
**************************************************文件上传**************************************************
*/
var SWFUpload = new Class({
    Extends: Swiff,
    version: "2.5.0 2010-01-15 Beta 2",
    Values: [],
    element: null,
    movieElement: null,
    console: null,
    speed: 10,
    QUEUE_ERROR: {
        QUEUE_LIMIT_EXCEEDED: -100,
        FILE_EXCEEDS_SIZE_LIMIT: -110,
        ZERO_BYTE_FILE: -120,
        INVALID_FILETYPE: -130
    },
    UPLOAD_ERROR: {
        HTTP_ERROR: -200,
        MISSING_UPLOAD_URL: -210,
        IO_ERROR: -220,
        SECURITY_ERROR: -230,
        UPLOAD_LIMIT_EXCEEDED: -240,
        UPLOAD_FAILED: -250,
        SPECIFIED_FILE_ID_NOT_FOUND: -260,
        FILE_VALIDATION_FAILED: -270,
        FILE_CANCELLED: -280,
        UPLOAD_STOPPED: -290,
        RESIZE: -300
    },
    FILE_STATUS: {
        QUEUED: -1,
        IN_PROGRESS: -2,
        ERROR: -3,
        COMPLETE: -4,
        CANCELLED: -5
    },
    UPLOAD_TYPE: {
        NORMAL: -1,
        RESIZED: -2
    },
    BUTTON_ACTION: {
        SELECT_FILE: -100,
        SELECT_FILES: -110,
        START_UPLOAD: -120,
        JAVASCRIPT: -130,
        NONE: -130
    },
    CURSOR: {
        ARROW: -1,
        HAND: -2
    },
    RESIZE_ENCODING: {
        JPEG: -1,
        PNG: -2
    },

    initialize: function (src, arg) {
        this.element = $(arg.id);
        var events = {};
        for (var property in this.callBacks) {
            events[property] = this.callBacks[property].bind(this);
        }
        Object.append(arg, {
            id: null,
            params: {
                wMode: 'transparent'
            },
            callBacks: events
        });
        this.parent(src, arg);

        var size = this.element.getCoordinates();
        var _border = this.element.getStyle('border').toInt() * 2;
        this.box = new Element('div').inject(this.element, 'after');
        this.box.setProperties(this.element.getProperties('style', 'class'));
        this.box.setStyles({
            width: size.width - _border,
            height: size.height - _border,
            background: '#fff',
            overflow: 'hidden',
            border: 0,
            position: 'absolute',
            left: 0 - _border / 2,
            top: 0 - _border / 2
        });
        this.box.setStyles({
            top: this.element.getPosition().y - this.box.getPosition().y,
            left: this.element.getPosition().x - this.box.getPosition().x,
            display: 'none'
        });
        this.perc = new Element('div', {
            styles: {
                background: 'url(/Developer/images/ico/ProgressBar.gif) 0 center no-repeat',
                width: 0,
                height: '100%'
            }
        }).inject(this.box);
        this.txt = new Element('div', {
            styles: {
                width: '100%',
                height: '100%',
                'text-align': 'center',
                overflow: 'hidden',
                'line-height': 21,
                'white-space': 'nowrap',
                'z-index': 10,
                position: 'absolute',
                left: 0,
                top: 0
            }
        }).inject(this.box);

        this.console = $(this.id + "_Console");
    },
    set: function (to, file) {
        var _width = (this.box.getStyle('width').replace('px', '') * (to.toInt() / 100)).toInt();
        this.perc.set('morph', {
            duration: this.speed,
            link: 'cancel'
        }).morph({
            width: _width
        });
        var procc = to.toInt() + '%';
        if (file) procc = '文件：' + file.name + '      ' + procc;
        this.txt.set('text', procc);
    },
    callBacks: {
        flashReady: function () {

        },
        fileDialogStart: function () {

        },
        mouseClick: function () {

        },
        mouseOver: function () {

        },
        mouseOut: function () {

        },
        fileQueued: function (file) {

        },
        fileQueueError: function (file, errorCode, message) {
            switch (errorCode) {
                case this.QUEUE_ERROR.FILE_EXCEEDS_SIZE_LIMIT:
                case this.QUEUE_ERROR.ZERO_BYTE_FILE:
                case this.QUEUE_ERROR.INVALID_FILETYPE:
                    alert(file.name + ' 文件无效！');
                default:
                    alert(message);
                    break;
            }
        },
        fileDialogComplete: function (numFilesSelected, numFilesQueued, numFilesInQueue) {
            this.remote('StartUpload');
        },
        uploadResizeStart: function (file, resizeSettings) {

        },
        uploadStart: function (file) {
            this.set(0);
            this.box.setStyles({
                display: 'block',
                opacity: 1
            });
            this.remote('ReturnUploadStart', true);
        },
        uploadProgress: function (file, bytesComplete, bytesTotal) {
            var percent = Math.ceil((bytesComplete / bytesTotal) * 100);
            this.set(percent, file);
        },
        uploadError: function (file, errorCode, message) {

        },
        uploadSuccess: function (file, serverData, responseReceived) {
            this.Values.include(serverData);
            this.set(100, file);
        },
        uploadComplete: function (file) {
            if (this.remote('GetStats').files_queued > 0) {
                (function () {
                    this.remote('StartUpload');
                } .bind(this)).delay(100);
            } else {
                this.element.value = this.Values.join("@");
                this.txt.set('text', " 上传完成!"); (function () {
                    new Fx.Morph(this.box, {
                        duration: 500,
                        transition: Fx.Transitions.linear,
                        onComplete: function () {
                            this.box.setStyles({
                                display: 'none'
                            });
                        } .bind(this)
                    }).start({
                        opacity: 0
                    });
                } .bind(this)).delay(1000);
                if (this.options.vars.buttonAction == this.BUTTON_ACTION.SELECT_FILE) this.Values.empty();
            }
        },
        debug: function (message) {
            try {
                if (!this.console) {
                    this.console = new Element('textarea', {
                        id: this.id + "_Console",
                        wrap: 'off',
                        styles: {
                            'width': '100%',
                            'height': 200
                        }
                    }).inject(document.body, 'top');
                }
                this.console.value += message + "\n";
                this.console.scrollTop = this.console.scrollHeight - this.console.clientHeight;
            } catch (ex) {
                alert("Exception: " + ex.name + " Message: " + ex.message);
            }
        }
    }
});