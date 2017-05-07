var rotate = function (o) {
    /*
    *this.x,this.y 轴心坐标（均相对于div的坐标来计算）
    *this.r        轴心到每个list中心点的距离
    *this.angle    每次旋转角度
    *this.time     执行坐标更改的时间间隔
    */
    this.m = document.getElementById(o.id),
    this.n = o.num,
    this.r = o.r || 100,
    this.time = o.time || 15,
    this.angle = o.angle || 1;
    this.y = !isNaN(o.y) ? o.y : this.m.offsetHeight / 2,
    this.x = !isNaN(o.x) ? o.x : this.m.offsetWidth / 2,
    this.c = this.m.children[0].children;
    this.w = this.c[0].offsetWidth / 2,
    this.h = this.c[0].offsetHeight / 2;
}
rotate.prototype = {
    init: function () { //只初始化位置
        var p = this._getpos();
        rotate.setpos(this.c, p);
    },
    binds: function (s, f) { //给所有li绑定一个事件
        var o = this.c;
        for (var i in o) {
            (o[i].addEventListener) ? o[i].addEventListener(s, f, false) : (o[i].attachEvent) ? o[i].attachEvent(s, f) : null;
        }
    },
    bind: function (s, f, i) { //单独给某个li绑定一个事件
        (this.c[i].addEventListener) ? this.c[i].addEventListener(s, f, false) : (this.c[i].attachEvent) ? this.c[i].attachEvent(s, f) : null;
    },
    turn: function () { //切换旋转方向
        this.angle = -this.angle;
    },
    pause: function () { //暂停
        this.tmp = this.angle;
        this.angle = 0;
    },
    goon: function () { //继续
        if (this.tmp) {
            this.angle = this.tmp;
        }
    },
    run: function () { //启动
        var i = 0;
        var o = this; (function () {
            var p = o._getpos(i);
            rotate.setpos(o.c, p);
            i = (i >= 360 || i <= -360) ? 0 : i + o.angle;
            setTimeout(arguments.callee, o.time);
        })();
    },
    _getpos: function (c) {
        var i, p = [],
        o = this.c,
        pi = Math.PI,
        e = 360 / this.n,
        c = c || 0;
        var m = 2 * pi / 360;
        for (i = 0; i < this.n; i++) {
            p.push(rotate.calpos((e * i + c) * m, this.r, this.x, this.y, this.w, this.h));
        }
        return p;
    }
}
rotate.setpos = function (o, p) {
    for (var i in o) {
        if (o[i].style) {
            o[i].style.left = p[i][0] + 'px';
            o[i].style.top = p[i][1] + 'px';
        }
    }
}
rotate.calpos = function (a, r, x, y, w, h) {
    return [parseInt(x + r * Math.cos(a) - w), parseInt(y + r * Math.sin(a) - h)];
}