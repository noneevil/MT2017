/*
mt 倒计时
ex: new TimeSpan(el, { starttime: "\/Date(1357390680000)\/", endtime: "\/Date(1357401540000)\/" });
*/
var TimeSpan = new Class({
    Implements: [Options],
    attachto: null,
    StartTime: null,
    EndTime: null,
    /* options: {
    endtime: '',
    starttime: ''
    },*/
    initialize: function (el, options) {
        this.attachto = el;
        this.setOptions(options);

        this.EndTime = parseInt(options.endtime.substr(6));
        this.StartTime = parseInt(options.starttime.substr(6));

        this.tick();
        if (!this.timer) this.timer = this.tick.periodical(1000, this);
    },
    tick: function () {
        this.StartTime = this.StartTime + 1000;
        var left = parseInt((this.EndTime - this.StartTime) / 1000);

        if (left <= 0) {
            this.attachto.set('text', "抢购已结束");
            this.stop();
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

            this.attachto.set('text', day + "天" + hour + "时" + minute + "分" + second + "秒");
        }
    },
    stop: function () {
        clearInterval(this.timer);
    }
});