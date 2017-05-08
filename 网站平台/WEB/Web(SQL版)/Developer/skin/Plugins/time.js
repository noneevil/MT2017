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
        //Cookie.write('r', String.uniqueID());
        var today = new Date();
        if (this.secondSub != 0) today.setTime(today.getTime() + this.secondSub);
        this.attTo.innerHTML = this.task(today);
    }
});