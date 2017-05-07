/*
mt 倒计时
ex: new TimeSpan(el, { ServerTime: "\/Date(1357390680000)\/", endtime: "\/Date(1357401540000)\/" });
*/
var TimeSpan = new Class({
    Implements: [Options],
    attachto: null,
    options: {
        onStart: function () { },
        onEnd: function () { },
        showDay: false,
        showHour: false,
        showMinute: false,
        showSecond: true,
        endtime: '',
        servertime: ''
    },
    initialize: function (el, options) {
        this.attachto = el;
        this.setOptions(options);
        this.options.endtime = Date.parse(options.endtime);
        this.options.servertime = Date.parse(options.servertime);

        this.tick();
        this.options.onStart();
        if (!this.timer) this.timer = this.tick.periodical(1000, this);
    },
    tick: function () {
        var o = this.options;
        o.servertime = o.servertime + 1000;
        var left = parseInt((o.endtime - o.servertime) / 1000);

        if (left <= 0) {
            this.stop();
            this.options.onEnd();
        }
        else {
            var day = parseInt(left / 3600 / 24); //计算天
            var hour = parseInt((left / 3600) % 24); //计算小时
            var minute = parseInt((left / 60) % 60); //计算分
            var second = parseInt(left % 60); // 计算秒
            var milliseconds = parseInt((left * 10) % 10); //计算100毫秒  

            hour = hour < 10 ? "0" + hour : hour;
            minute = minute < 10 ? "0" + minute : minute;
            second = second < 10 ? "0" + second : second;
            //milliseconds = milliseconds < 10 ? "0" + milliseconds : milliseconds;
            var str = "";
            if (o.showDay) str += "<b>" + day + "</b>" + "天";
            if (o.showHour) str += "<b>" + hour + "</b>" + "时";
            if (o.showMinute) str += "<b>" + minute + "</b>" + "分";
            if (o.showSecond) str += "<b>" + second + "</b>" + "秒";

            this.attachto.set('html', str);
        }
    },
    stop: function () {
        clearInterval(this.timer);
    }
});