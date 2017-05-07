window.addEvent('domready', function () {

});
var tick = 10;
var timer;
var T_Start = function () {
    tick = 10;
    win = window.open("/Home/Alexa", "xx", "");
    timer = go.periodical(1000);
    $('btn_Start').disabled = true;
    $('btn_Stop').disabled = false;
};
var T_Stop = function () {
    clearInterval(timer);
    $('btn_Start').disabled = false;
    $('btn_Stop').disabled = true;
    if (win.closed != true) win.close();
};

var go = function () {
    if (win.closed == true) {
        T_Stop();
        //        win = window.open("/Home/Alexa", "xx", "");
    }
    else {
        if (tick < 1) {
            tick = 10;
            win.location.href = "/Home/Alexa";
        }
        else {
            tick = tick - 1;
            $('time').set('text', '离下次开始的时间值' + tick);
        }
    }
};