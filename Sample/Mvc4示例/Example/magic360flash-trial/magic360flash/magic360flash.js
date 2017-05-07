/*


Magic 360 Flash v1.1.10 DEMO
Copyright 2012 Magic Toolbox
Buy a license: www.magictoolbox.com/magic360flash/
License agreement: http://www.magictoolbox.com/license/


*/
(function () {
    if (window.magicJS) {
        return
    }
    var b = {
        version: "v2.6.3",
        UUID: 0,
        storage: {},
        $uuid: function (d) {
            return (d.$J_UUID || (d.$J_UUID = ++a.UUID))
        },
        getStorage: function (d) {
            return (a.storage[d] || (a.storage[d] = {}))
        },
        $F: function () { },
        $false: function () {
            return false
        },
        defined: function (d) {
            return (undefined != d)
        },
        exists: function (d) {
            return !!(d)
        },
        j1: function (d) {
            if (!a.defined(d)) {
                return false
            }
            if (d.$J_TYPE) {
                return d.$J_TYPE
            }
            if (!!d.nodeType) {
                if (1 == d.nodeType) {
                    return "element"
                }
                if (3 == d.nodeType) {
                    return "textnode"
                }
            }
            if (d.length && d.item) {
                return "collection"
            }
            if (d.length && d.callee) {
                return "arguments"
            }
            if ((d instanceof window.Object || d instanceof window.Function) && d.constructor === a.Class) {
                return "class"
            }
            if (d instanceof window.Array) {
                return "array"
            }
            if (d instanceof window.Function) {
                return "function"
            }
            if (d instanceof window.String) {
                return "string"
            }
            if (a.j21.trident) {
                if (a.defined(d.cancelBubble)) {
                    return "event"
                }
            } else {
                if (d === window.event || d.constructor == window.Event || d.constructor == window.MouseEvent || d.constructor == window.UIEvent || d.constructor == window.KeyboardEvent || d.constructor == window.KeyEvent) {
                    return "event"
                }
            }
            if (d instanceof window.Date) {
                return "date"
            }
            if (d instanceof window.RegExp) {
                return "regexp"
            }
            if (d === window) {
                return "window"
            }
            if (d === document) {
                return "document"
            }
            return typeof (d)
        },
        extend: function (j, h) {
            if (!(j instanceof window.Array)) {
                j = [j]
            }
            for (var g = 0, e = j.length; g < e; g++) {
                if (!a.defined(j)) {
                    continue
                }
                for (var f in (h || {})) {
                    try {
                        j[g][f] = h[f]
                    } catch (d) { }
                }
            }
            return j[0]
        },
        implement: function (h, g) {
            if (!(h instanceof window.Array)) {
                h = [h]
            }
            for (var f = 0, d = h.length; f < d; f++) {
                if (!a.defined(h[f])) {
                    continue
                }
                if (!h[f].prototype) {
                    continue
                }
                for (var e in (g || {})) {
                    if (!h[f].prototype[e]) {
                        h[f].prototype[e] = g[e]
                    }
                }
            }
            return h[0]
        },
        nativize: function (f, e) {
            if (!a.defined(f)) {
                return f
            }
            for (var d in (e || {})) {
                if (!f[d]) {
                    f[d] = e[d]
                }
            }
            return f
        },
        $try: function () {
            for (var f = 0, d = arguments.length; f < d; f++) {
                try {
                    return arguments[f]()
                } catch (g) { }
            }
            return null
        },
        $A: function (f) {
            if (!a.defined(f)) {
                return $mjs([])
            }
            if (f.toArray) {
                return $mjs(f.toArray())
            }
            if (f.item) {
                var e = f.length || 0,
				d = new Array(e);
                while (e--) {
                    d[e] = f[e]
                }
                return $mjs(d)
            }
            return $mjs(Array.prototype.slice.call(f))
        },
        now: function () {
            return new Date().getTime()
        },
        detach: function (h) {
            var f;
            switch (a.j1(h)) {
                case "object":
                    f = {};
                    for (var g in h) {
                        f[g] = a.detach(h[g])
                    }
                    break;
                case "array":
                    f = [];
                    for (var e = 0, d = h.length; e < d; e++) {
                        f[e] = a.detach(h[e])
                    }
                    break;
                default:
                    return h
            }
            return a.$(f)
        },
        $: function (e) {
            if (!a.defined(e)) {
                return null
            }
            if (e.$J_EXTENDED) {
                return e
            }
            switch (a.j1(e)) {
                case "array":
                    e = a.nativize(e, a.extend(a.Array, {
                        $J_EXTENDED: a.$F
                    }));
                    e.j14 = e.forEach;
                    return e;
                    break;
                case "string":
                    var d = document.getElementById(e);
                    if (a.defined(d)) {
                        return a.$(d)
                    }
                    return null;
                    break;
                case "window":
                case "document":
                    a.$uuid(e);
                    e = a.extend(e, a.Doc);
                    break;
                case "element":
                    a.$uuid(e);
                    e = a.extend(e, a.Element);
                    break;
                case "event":
                    e = a.extend(e, a.Event);
                    break;
                case "textnode":
                    return e;
                    break;
                case "function":
                case "array":
                case "date":
                default:
                    break
            }
            return a.extend(e, {
                $J_EXTENDED: a.$F
            })
        },
        $new: function (d, f, e) {
            return $mjs(a.doc.createElement(d)).setProps(f || {}).j6(e || {})
        }
    };
    var a = b;
    window.magicJS = b;
    window.$mjs = b.$;
    a.Array = {
        $J_TYPE: "array",
        indexOf: function (g, h) {
            var d = this.length;
            for (var e = this.length, f = (h < 0) ? Math.max(0, e + h) : h || 0; f < e; f++) {
                if (this[f] === g) {
                    return f
                }
            }
            return -1
        },
        contains: function (d, e) {
            return this.indexOf(d, e) != -1
        },
        forEach: function (d, g) {
            for (var f = 0, e = this.length; f < e; f++) {
                if (f in this) {
                    d.call(g, this[f], f, this)
                }
            }
        },
        filter: function (d, j) {
            var h = [];
            for (var g = 0, e = this.length; g < e; g++) {
                if (g in this) {
                    var f = this[g];
                    if (d.call(j, this[g], g, this)) {
                        h.push(f)
                    }
                }
            }
            return h
        },
        map: function (d, h) {
            var g = [];
            for (var f = 0, e = this.length; f < e; f++) {
                if (f in this) {
                    g[f] = d.call(h, this[f], f, this)
                }
            }
            return g
        }
    };
    a.implement(String, {
        $J_TYPE: "string",
        j26: function () {
            return this.replace(/^\s+|\s+$/g, "")
        },
        eq: function (d, e) {
            return (e || false) ? (this.toString() === d.toString()) : (this.toLowerCase().toString() === d.toLowerCase().toString())
        },
        j22: function () {
            return this.replace(/-\D/g,
			function (d) {
			    return d.charAt(1).toUpperCase()
			})
        },
        dashize: function () {
            return this.replace(/[A-Z]/g,
			function (d) {
			    return ("-" + d.charAt(0).toLowerCase())
			})
        },
        j17: function (d) {
            return parseInt(this, d || 10)
        },
        toFloat: function () {
            return parseFloat(this)
        },
        j18: function () {
            return !this.replace(/true/i, "").j26()
        },
        has: function (e, d) {
            d = d || "";
            return (d + this + d).indexOf(d + e + d) > -1
        }
    });
    b.implement(Function, {
        $J_TYPE: "function",
        j24: function () {
            var e = a.$A(arguments),
			d = this,
			f = e.shift();
            return function () {
                return d.apply(f || null, e.concat(a.$A(arguments)))
            }
        },
        j16: function () {
            var e = a.$A(arguments),
			d = this,
			f = e.shift();
            return function (g) {
                return d.apply(f || null, $mjs([g || window.event]).concat(e))
            }
        },
        j27: function () {
            var e = a.$A(arguments),
			d = this,
			f = e.shift();
            return window.setTimeout(function () {
                return d.apply(d, e)
            },
			f || 0)
        },
        j28: function () {
            var e = a.$A(arguments),
			d = this;
            return function () {
                return d.j27.apply(d, e)
            }
        },
        interval: function () {
            var e = a.$A(arguments),
			d = this,
			f = e.shift();
            return window.setInterval(function () {
                return d.apply(d, e)
            },
			f || 0)
        }
    });
    var c = navigator.userAgent.toLowerCase();
    a.j21 = {
        features: {
            xpath: !!(document.evaluate),
            air: !!(window.runtime),
            query: !!(document.querySelector)
        },
        touchScreen: function () {
            try {
                if (document.createEvent) {
                    document.createEvent("TouchEvent", "touchend");
                    return true
                }
                return false
            } catch (d) {
                return false
            }
        } (),
        engine: (window.opera) ? "presto" : !!(window.ActiveXObject) ? "trident" : (undefined != document.getBoxObjectFor || null != window.mozInnerScreenY) ? "gecko" : (null != window.WebKitPoint || !navigator.taintEnabled) ? "webkit" : "unknown",
        version: "",
        ieMode: 0,
        platform: c.match(/ip(?:ad|od|hone)/) ? "ios" : (c.match(/(?:webos|android)/) || navigator.platform.match(/mac|win|linux/i) || ["other"])[0].toLowerCase(),
        backCompat: document.compatMode && "backcompat" == document.compatMode.toLowerCase(),
        getDoc: function () {
            return (document.compatMode && "backcompat" == document.compatMode.toLowerCase()) ? document.body : document.documentElement
        },
        ready: false,
        onready: function () {
            if (a.j21.ready) {
                return
            }
            a.j21.ready = true;
            a.body = $mjs(document.body);
            a.win = $mjs(window);
            $mjs(document).raiseEvent("domready")
        }
    }; (function () {
        function d() {
            return !!(arguments.callee.caller)
        }
        a.j21.version = ("presto" == a.j21.engine) ? !!(document.head) ? 270 : !!(window.applicationCache) ? 260 : !!(window.localStorage) ? 250 : (a.j21.features.query) ? 220 : ((d()) ? 211 : ((document.getElementsByClassName) ? 210 : 200)) : ("trident" == a.j21.engine) ? !!(window.msPerformance || window.performance) ? 900 : !!(window.XMLHttpRequest && window.postMessage) ? 6 : ((window.XMLHttpRequest) ? 5 : 4) : ("webkit" == a.j21.engine) ? ((a.j21.features.xpath) ? ((a.j21.features.query) ? 525 : 420) : 419) : ("gecko" == a.j21.engine) ? !!(document.head) ? 200 : !!document.readyState ? 192 : !!(window.localStorage) ? 191 : ((document.getElementsByClassName) ? 190 : 181) : "";
        a.j21[a.j21.engine] = a.j21[a.j21.engine + a.j21.version] = true;
        if (window.chrome) {
            a.j21.chrome = true
        }
        a.j21.ieMode = (!a.j21.trident) ? 0 : (document.documentMode) ? document.documentMode : function () {
            var e = 0;
            switch (a.j21.version) {
                case 4:
                    e = 6;
                    break;
                case 5:
                    e = 7;
                    break;
                case 6:
                    e = 8;
                    break;
                case 900:
                    e = 9;
                    break
            }
            return e
        } ()
    })();
    a.Element = {
        j13: function (d) {
            return this.className.has(d, " ")
        },
        j2: function (d) {
            if (d && !this.j13(d)) {
                this.className += (this.className ? " " : "") + d
            }
            return this
        },
        j3: function (d) {
            d = d || ".*";
            this.className = this.className.replace(new RegExp("(^|\\s)" + d + "(?:\\s|$)"), "$1").j26();
            return this
        },
        j4: function (d) {
            return this.j13(d) ? this.j3(d) : this.j2(d)
        },
        j5: function (f) {
            f = (f == "float" && this.currentStyle) ? "styleFloat" : f.j22();
            var d = null,
			e = null;
            if (this.currentStyle) {
                d = this.currentStyle[f]
            } else {
                if (document.defaultView && document.defaultView.getComputedStyle) {
                    e = document.defaultView.getComputedStyle(this, null);
                    d = e ? e.getPropertyValue([f.dashize()]) : null
                }
            }
            if (!d) {
                d = this.style[f]
            }
            if ("opacity" == f) {
                return a.defined(d) ? parseFloat(d) : 1
            }
            if (/^(border(Top|Bottom|Left|Right)Width)|((padding|margin)(Top|Bottom|Left|Right))$/.test(f)) {
                d = parseInt(d) ? d : "0px"
            }
            return ("auto" == d ? null : d)
        },
        j6Prop: function (f, d) {
            try {
                if ("opacity" == f) {
                    this.j23(d);
                    return this
                }
                if ("float" == f) {
                    this.style[("undefined" === typeof (this.style.styleFloat)) ? "cssFloat" : "styleFloat"] = d;
                    return this
                }
                this.style[f.j22()] = d + (("number" == a.j1(d) && !$mjs(["zIndex", "zoom"]).contains(f.j22())) ? "px" : "")
            } catch (g) { }
            return this
        },
        j6: function (e) {
            for (var d in e) {
                this.j6Prop(d, e[d])
            }
            return this
        },
        j19s: function () {
            var d = {};
            a.$A(arguments).j14(function (e) {
                d[e] = this.j5(e)
            },
			this);
            return d
        },
        j23: function (h, e) {
            e = e || false;
            h = parseFloat(h);
            if (e) {
                if (h == 0) {
                    if ("hidden" != this.style.visibility) {
                        this.style.visibility = "hidden"
                    }
                } else {
                    if ("visible" != this.style.visibility) {
                        this.style.visibility = "visible"
                    }
                }
            }
            if (a.j21.trident) {
                if (!this.currentStyle || !this.currentStyle.hasLayout) {
                    this.style.zoom = 1
                }
                try {
                    var g = this.filters.item("DXImageTransform.Microsoft.Alpha");
                    g.enabled = (1 != h);
                    g.opacity = h * 100
                } catch (d) {
                    this.style.filter += (1 == h) ? "" : "progid:DXImageTransform.Microsoft.Alpha(enabled=true,opacity=" + h * 100 + ")"
                }
            }
            this.style.opacity = h;
            return this
        },
        setProps: function (d) {
            for (var e in d) {
                this.setAttribute(e, "" + d[e])
            }
            return this
        },
        hide: function () {
            return this.j6({
                display: "none",
                visibility: "hidden"
            })
        },
        show: function () {
            return this.j6({
                display: "block",
                visibility: "visible"
            })
        },
        j7: function () {
            return {
                width: this.offsetWidth,
                height: this.offsetHeight
            }
        },
        j10: function () {
            return {
                top: this.scrollTop,
                left: this.scrollLeft
            }
        },
        j11: function () {
            var d = this,
			e = {
			    top: 0,
			    left: 0
			};
            do {
                e.left += d.scrollLeft || 0;
                e.top += d.scrollTop || 0;
                d = d.parentNode
            }
            while (d);
            return e
        },
        j8: function () {
            if (a.defined(document.documentElement.getBoundingClientRect)) {
                var d = this.getBoundingClientRect(),
				f = $mjs(document).j10(),
				h = a.j21.getDoc();
                return {
                    top: d.top + f.y - h.clientTop,
                    left: d.left + f.x - h.clientLeft
                }
            }
            var g = this,
			e = t = 0;
            do {
                e += g.offsetLeft || 0;
                t += g.offsetTop || 0;
                g = g.offsetParent
            }
            while (g && !(/^(?:body|html)$/i).test(g.tagName));
            return {
                top: t,
                left: e
            }
        },
        j9: function () {
            var e = this.j8();
            var d = this.j7();
            return {
                top: e.top,
                bottom: e.top + d.height,
                left: e.left,
                right: e.left + d.width
            }
        },
        changeContent: function (f) {
            try {
                this.innerHTML = f
            } catch (d) {
                this.innerText = f
            }
            return this
        },
        j33: function () {
            return (this.parentNode) ? this.parentNode.removeChild(this) : this
        },
        kill: function () {
            a.$A(this.childNodes).j14(function (d) {
                if (3 == d.nodeType || 8 == d.nodeType) {
                    return
                }
                $mjs(d).kill()
            });
            this.j33();
            this.je3();
            if (this.$J_UUID) {
                a.storage[this.$J_UUID] = null;
                delete a.storage[this.$J_UUID]
            }
            return null
        },
        append: function (g, e) {
            e = e || "bottom";
            var d = this.firstChild; ("top" == e && d) ? this.insertBefore(g, d) : this.appendChild(g);
            return this
        },
        j32: function (f, e) {
            var d = $mjs(f).append(this, e);
            return this
        },
        enclose: function (d) {
            this.append(d.parentNode.replaceChild(this, d));
            return this
        },
        hasChild: function (d) {
            if (!(d = $mjs(d))) {
                return false
            }
            return (this == d) ? false : (this.contains && !(a.j21.webkit419)) ? (this.contains(d)) : (this.compareDocumentPosition) ? !!(this.compareDocumentPosition(d) & 16) : a.$A(this.byTag(d.tagName)).contains(d)
        }
    };
    a.Element.j19 = a.Element.j5;
    a.Element.j20 = a.Element.j6;
    if (!window.Element) {
        window.Element = a.$F;
        if (a.j21.engine.webkit) {
            window.document.createElement("iframe")
        }
        window.Element.prototype = (a.j21.engine.webkit) ? window["[[DOMElement.prototype]]"] : {}
    }
    a.implement(window.Element, {
        $J_TYPE: "element"
    });
    a.Doc = {
        j7: function () {
            if (a.j21.presto925 || a.j21.webkit419) {
                return {
                    width: self.innerWidth,
                    height: self.innerHeight
                }
            }
            return {
                width: a.j21.getDoc().clientWidth,
                height: a.j21.getDoc().clientHeight
            }
        },
        j10: function () {
            return {
                x: self.pageXOffset || a.j21.getDoc().scrollLeft,
                y: self.pageYOffset || a.j21.getDoc().scrollTop
            }
        },
        j12: function () {
            var d = this.j7();
            return {
                width: Math.max(a.j21.getDoc().scrollWidth, d.width),
                height: Math.max(a.j21.getDoc().scrollHeight, d.height)
            }
        }
    };
    a.extend(document, {
        $J_TYPE: "document"
    });
    a.extend(window, {
        $J_TYPE: "window"
    });
    a.extend([a.Element, a.Doc], {
        j29: function (g, e) {
            var d = a.getStorage(this.$J_UUID),
			f = d[g];
            if (undefined != e && undefined == f) {
                f = d[g] = e
            }
            return (a.defined(f) ? f : null)
        },
        j30: function (f, e) {
            var d = a.getStorage(this.$J_UUID);
            d[f] = e;
            return this
        },
        j31: function (e) {
            var d = a.getStorage(this.$J_UUID);
            delete d[e];
            return this
        }
    });
    if (!(window.HTMLElement && window.HTMLElement.prototype && window.HTMLElement.prototype.getElementsByClassName)) {
        a.extend([a.Element, a.Doc], {
            getElementsByClassName: function (d) {
                return a.$A(this.getElementsByTagName("*")).filter(function (g) {
                    try {
                        return (1 == g.nodeType && g.className.has(d, " "))
                    } catch (f) { }
                })
            }
        })
    }
    a.extend([a.Element, a.Doc], {
        byClass: function () {
            return this.getElementsByClassName(arguments[0])
        },
        byTag: function () {
            return this.getElementsByTagName(arguments[0])
        }
    });
    a.Event = {
        $J_TYPE: "event",
        stop: function () {
            if (this.stopPropagation) {
                this.stopPropagation()
            } else {
                this.cancelBubble = true
            }
            if (this.preventDefault) {
                this.preventDefault()
            } else {
                this.returnValue = false
            }
            return this
        },
        j15: function () {
            var e,
			d;
            e = ((/touch/i).test(this.type)) ? this.changedTouches[0] : this;
            return (!a.defined(e)) ? {
                x: 0,
                y: 0
            } : {
                x: e.pageX || e.clientX + a.j21.getDoc().scrollLeft,
                y: e.pageY || e.clientY + a.j21.getDoc().scrollTop
            }
        },
        getTarget: function () {
            var d = this.target || this.srcElement;
            while (d && 3 == d.nodeType) {
                d = d.parentNode
            }
            return d
        },
        getRelated: function () {
            var e = null;
            switch (this.type) {
                case "mouseover":
                    e = this.relatedTarget || this.fromElement;
                    break;
                case "mouseout":
                    e = this.relatedTarget || this.toElement;
                    break;
                default:
                    return e
            }
            try {
                while (e && 3 == e.nodeType) {
                    e = e.parentNode
                }
            } catch (d) {
                e = null
            }
            return e
        },
        getButton: function () {
            if (!this.which && this.button !== undefined) {
                return (this.button & 1 ? 1 : (this.button & 2 ? 3 : (this.button & 4 ? 2 : 0)))
            }
            return this.which
        }
    };
    a._event_add_ = "addEventListener";
    a._event_del_ = "removeEventListener";
    a._event_prefix_ = "";
    if (!document.addEventListener) {
        a._event_add_ = "attachEvent";
        a._event_del_ = "detachEvent";
        a._event_prefix_ = "on"
    }
    a.extend([a.Element, a.Doc], {
        je1: function (g, f) {
            var i = ("domready" == g) ? false : true,
			e = this.j29("events", {});
            e[g] = e[g] || {};
            if (e[g].hasOwnProperty(f.$J_EUID)) {
                return this
            }
            if (!f.$J_EUID) {
                f.$J_EUID = Math.floor(Math.random() * a.now())
            }
            var d = this,
			h = function (j) {
			    return f.call(d)
			};
            if ("domready" == g) {
                if (a.j21.ready) {
                    f.call(this);
                    return this
                }
            }
            if (i) {
                h = function (j) {
                    j = a.extend(j || window.e, {
                        $J_TYPE: "event"
                    });
                    return f.call(d, $mjs(j))
                };
                this[a._event_add_](a._event_prefix_ + g, h, false)
            }
            e[g][f.$J_EUID] = h;
            return this
        },
        je2: function (g) {
            var i = ("domready" == g) ? false : true,
			e = this.j29("events");
            if (!e || !e[g]) {
                return this
            }
            var h = e[g],
			f = arguments[1] || null;
            if (g && !f) {
                for (var d in h) {
                    if (!h.hasOwnProperty(d)) {
                        continue
                    }
                    this.je2(g, d)
                }
                return this
            }
            f = ("function" == a.j1(f)) ? f.$J_EUID : f;
            if (!h.hasOwnProperty(f)) {
                return this
            }
            if ("domready" == g) {
                i = false
            }
            if (i) {
                this[a._event_del_](a._event_prefix_ + g, h[f], false)
            }
            delete h[f];
            return this
        },
        raiseEvent: function (h, f) {
            var m = ("domready" == h) ? false : true,
			l = this,
			j;
            if (!m) {
                var g = this.j29("events");
                if (!g || !g[h]) {
                    return this
                }
                var i = g[h];
                for (var d in i) {
                    if (!i.hasOwnProperty(d)) {
                        continue
                    }
                    i[d].call(this)
                }
                return this
            }
            if (l === document && document.createEvent && !l.dispatchEvent) {
                l = document.documentElement
            }
            if (document.createEvent) {
                j = document.createEvent(h);
                j.initEvent(f, true, true)
            } else {
                j = document.createEventObject();
                j.eventType = h
            }
            if (document.createEvent) {
                l.dispatchEvent(j)
            } else {
                l.fireEvent("on" + f, j)
            }
            return j
        },
        je3: function () {
            var d = this.j29("events");
            if (!d) {
                return this
            }
            for (var e in d) {
                this.je2(e)
            }
            this.j31("events");
            return this
        }
    }); (function () {
        if (a.j21.webkit && a.j21.version < 420) {
            (function () {
                ($mjs(["loaded", "complete"]).contains(document.readyState)) ? a.j21.onready() : arguments.callee.j27(50)
            })()
        } else {
            if (a.j21.trident && a.j21.ieMode < 9 && window == top) {
                (function () {
                    (a.$try(function () {
                        a.j21.getDoc().doScroll("left");
                        return true
                    })) ? a.j21.onready() : arguments.callee.j27(50)
                })()
            } else {
                $mjs(document).je1("DOMContentLoaded", a.j21.onready);
                $mjs(window).je1("load", a.j21.onready)
            }
        }
    })();
    a.Class = function () {
        var h = null,
		e = a.$A(arguments);
        if ("class" == a.j1(e[0])) {
            h = e.shift()
        }
        var d = function () {
            for (var l in this) {
                this[l] = a.detach(this[l])
            }
            if (this.constructor.$parent) {
                this.$parent = {};
                var o = this.constructor.$parent;
                for (var n in o) {
                    var j = o[n];
                    switch (a.j1(j)) {
                        case "function":
                            this.$parent[n] = a.Class.wrap(this, j);
                            break;
                        case "object":
                            this.$parent[n] = a.detach(j);
                            break;
                        case "array":
                            this.$parent[n] = a.detach(j);
                            break
                    }
                }
            }
            var i = (this.init) ? this.init.apply(this, arguments) : this;
            delete this.caller;
            return i
        };
        if (!d.prototype.init) {
            d.prototype.init = a.$F
        }
        if (h) {
            var g = function () { };
            g.prototype = h.prototype;
            d.prototype = new g;
            d.$parent = {};
            for (var f in h.prototype) {
                d.$parent[f] = h.prototype[f]
            }
        } else {
            d.$parent = null
        }
        d.constructor = a.Class;
        d.prototype.constructor = d;
        a.extend(d.prototype, e[0]);
        a.extend(d, {
            $J_TYPE: "class"
        });
        return d
    };
    b.Class.wrap = function (d, e) {
        return function () {
            var g = this.caller;
            var f = e.apply(d, arguments);
            return f
        }
    };
    a.win = $mjs(window);
    a.doc = $mjs(document)
})();
var MagicImage = (function (c) {
    if (!c) {
        throw "MagicJS not found";
        return
    }
    var b = c.$;
    var a = new c.Class({
        self: null,
        ready: false,
        options: {
            onload: c.$F,
            onabort: c.$F,
            onerror: c.$F
        },
        width: 0,
        height: 0,
        border: {
            left: 0,
            right: 0,
            top: 0,
            bottom: 0
        },
        margin: {
            left: 0,
            right: 0,
            top: 0,
            bottom: 0
        },
        padding: {
            left: 0,
            right: 0,
            top: 0,
            bottom: 0
        },
        _timer: null,
        _handlers: {
            onload: function (d) {
                if (d) {
                    b(d).stop()
                }
                this._unbind();
                if (this.ready) {
                    return
                }
                this.ready = true;
                this.calc();
                this._cleanup();
                this.options.onload.j27(1)
            },
            onabort: function (d) {
                if (d) {
                    b(d).stop()
                }
                this._unbind();
                this.ready = false;
                this._cleanup();
                this.options.onabort.j27(1)
            },
            onerror: function (d) {
                if (d) {
                    b(d).stop()
                }
                this._unbind();
                this.ready = false;
                this._cleanup();
                this.options.onerror.j27(1)
            }
        },
        _bind: function () {
            b(["load", "abort", "error"]).j14(function (d) {
                this.self.je1(d, this._handlers["on" + d].j16(this).j28(1))
            },
			this)
        },
        _unbind: function () {
            b(["load", "abort", "error"]).j14(function (d) {
                this.self.je2(d)
            },
			this)
        },
        _cleanup: function () {
            if (this.self.j29("new")) {
                var d = this.self.parentNode;
                this.self.j33().j31("new").j6({
                    position: "static",
                    top: "auto"
                });
                d.kill()
            }
        },
        init: function (f, e) {
            this.options = c.extend(this.options, e);
            var d = this.self = b(f) || c.$new("img", {},
			{
			    "max-width": "none",
			    "max-height": "none",
			    width: "auto",
			    height: "auto"
			}).j32(c.$new("div").j2("magic-image-tmp-box").j6({
			    position: "absolute",
			    top: -10000,
			    width: 10,
			    height: 10,
			    overflow: "hidden"
			}).j32(c.body)).j30("new", true),
			g = function () {
			    if (this.isReady()) {
			        this._handlers.onload.call(this)
			    } else {
			        this._handlers.onerror.call(this)
			    }
			    g = null
			} .j24(this);
            this._bind();
            if (!f.src) {
                d.src = f
            } else {
                d.src = f.src
            }
            if (d && d.complete) {
                this._timer = g.j27(100)
            }
        },
        destroy: function () {
            if (this._timer) {
                try {
                    clearTimeout(this._timer)
                } catch (d) { }
                this._timer = null
            }
            this._unbind();
            this._cleanup();
            this.ready = false;
            return this
        },
        isReady: function () {
            var d = this.self;
            return (d.naturalWidth) ? (d.naturalWidth > 0) : (d.readyState) ? ("complete" == d.readyState) : d.width > 0
        },
        calc: function () {
            this.width = this.self.naturalWidth || this.self.width;
            this.height = this.self.naturalHeight || this.self.height;
            b(["left", "right", "top", "bottom"]).j14(function (d) {
                this.margin[d] = this.self.j5("padding-" + d).j17();
                this.padding[d] = this.self.j5("padding-" + d).j17();
                this.border[d] = this.self.j5("border-" + d + "-width").j17()
            },
			this)
        }
    });
    return a
})(magicJS);
function M360_AC_AddExtension(b, a) {
    if (b.indexOf("?") != -1) {
        return b.replace(/\?/, a + "?")
    } else {
        return b + a
    }
}
function M360_AC_Generateobj(h, g, a) {
    var e = (navigator.appVersion.indexOf("MSIE") != -1) ? true : false;
    var c = (navigator.appVersion.toLowerCase().indexOf("win") != -1) ? true : false;
    var b = (navigator.userAgent.indexOf("Opera") != -1) ? true : false;
    var f = "";
    if (e && c && !b) {
        f += "<object ";
        for (var d in h) {
            f += d + '="' + h[d] + '" '
        }
        f += ">";
        for (var d in g) {
            f += '<param name="' + d + '" value="' + g[d] + '" /> '
        }
        f += "</object>"
    } else {
        f += "<embed ";
        for (var d in a) {
            f += d + '="' + a[d] + '" '
        }
        f += "> </embed>"
    }
    return f
}
function M360_AC_FL_RunContent() {
    var a = M360_AC_GetArgs(arguments, ".swf", "movie", "clsid:d27cdb6e-ae6d-11cf-96b8-444553540000", "application/x-shockwave-flash");
    return M360_AC_Generateobj(a.objAttrs, a.params, a.embedAttrs)
}
function M360_AC_SW_RunContent() {
    var a = M360_AC_GetArgs(arguments, ".dcr", "src", "clsid:166B1BCA-3F9C-11CF-8075-444553540000", null);
    return M360_AC_Generateobj(a.objAttrs, a.params, a.embedAttrs)
}
function M360_AC_GetArgs(b, e, g, d, h) {
    var a = new Object();
    a.embedAttrs = new Object();
    a.params = new Object();
    a.objAttrs = new Object();
    for (var c = 0; c < b.length; c = c + 2) {
        var f = b[c].toLowerCase();
        switch (f) {
            case "classid":
                break;
            case "pluginspage":
                a.embedAttrs[b[c]] = b[c + 1];
                break;
            case "src":
            case "movie":
                b[c + 1] = M360_AC_AddExtension(b[c + 1], e);
                a.embedAttrs.src = b[c + 1];
                a.params[g] = b[c + 1];
                break;
            case "onafterupdate":
            case "onbeforeupdate":
            case "onblur":
            case "oncellchange":
            case "onclick":
            case "ondblclick":
            case "ondrag":
            case "ondragend":
            case "ondragenter":
            case "ondragleave":
            case "ondragover":
            case "ondrop":
            case "onfinish":
            case "onfocus":
            case "onhelp":
            case "onmousedown":
            case "onmouseup":
            case "onmouseover":
            case "onmousemove":
            case "onmouseout":
            case "onkeypress":
            case "onkeydown":
            case "onkeyup":
            case "onload":
            case "onlosecapture":
            case "onpropertychange":
            case "onreadystatechange":
            case "onrowsdelete":
            case "onrowenter":
            case "onrowexit":
            case "onrowsinserted":
            case "onstart":
            case "onscroll":
            case "onbeforeeditfocus":
            case "onactivate":
            case "onbeforedeactivate":
            case "ondeactivate":
            case "type":
            case "codebase":
            case "id":
                a.objAttrs[b[c]] = b[c + 1];
                break;
            case "width":
            case "height":
            case "align":
            case "vspace":
            case "hspace":
            case "class":
            case "title":
            case "accesskey":
            case "name":
            case "tabindex":
                a.embedAttrs[b[c]] = a.objAttrs[b[c]] = b[c + 1];
                break;
            default:
                a.embedAttrs[b[c]] = a.params[b[c]] = b[c + 1]
        }
    }
    a.objAttrs.classid = d;
    if (h) {
        a.embedAttrs.type = h
    }
    return a
}
var M360_ua = "msie";
var W = navigator.userAgent.toLowerCase();
if (W.indexOf("opera") != -1) {
    M360_ua = "opera"
} else {
    if (W.indexOf("msie") != -1) {
        M360_ua = "msie"
    } else {
        if (W.indexOf("safari") != -1) {
            M360_ua = "safari"
        } else {
            if (W.indexOf("mozilla") != -1) {
                M360_ua = "gecko"
            }
        }
    }
}
var m360ATags = new Array();
var Magic360Flash = (function ($J) {
    var $j = $J.$;
    var Magic360Flash = {
        version: "v1.1.10",
        codeVersion: "v1.1.10",
        options: {},
        defaults: {
            spin: "hover",
            autostart: "false",
            "pause-on-click": "true",
            smoothing: 10,
            blur: 10,
            speed: 10,
            magnifier: "circle",
            "magnifier-size": "66%",
            "magnifier-size-x": "66%",
            "magnifier-size-y": "66%",
            "magnifier-effect": "fade",
            "magnifier-filter": "glow",
            "magnifier-time": 30,
            "magnifier-simulate": "",
            logo: "",
            "logo-position-x": "right",
            "logo-position-y": "top",
            "logo-margin": "20",
            "message-loading-spin": "Loading 360 spin",
            "message-loading-magnifier": "Loading magnifier",
            "border-color": "#CCCCCC",
            "border-width": 0,
            "magnifier-border-color": "#CCCCCC",
            "magnifier-border-width": 1,
            "progress-color": "#CCCCCC",
            "progress-height": 10,
            "progress-text": 1,
            "progress-text-color": "#777777",
            "pause-change-direction": "false",
            delimiter: "*",
            status: "58723627fcebc230ab0d53ddf5f16e34",
            background: "#fff",
            wmode: ($J.j21.engine == "webkit") ? "window" : "opaque"
        },
        getBaseUrl: function () {
            var baseurl = "";
            var M360_elements = document.getElementsByTagName("script");
            for (var i = 0; i < M360_elements.length; i++) {
                if (M360_elements[i].src && (/Magic360Flash.js/i.test(M360_elements[i].src))) {
                    var src = M360_elements[i].src;
                    srcMode = (src.indexOf("_src") != -1) ? "_src" : "";
                    src = src.substring(0, src.lastIndexOf("/"));
                    baseurl = src + "/";
                    if (baseurl == "/") {
                        baseurl = ""
                    }
                    break
                }
            }
            return baseurl
        },
        init: function (params) {
            this.params = params;
            var httppref = document.location + "";
            httppref = (httppref.indexOf("https") != -1) ? "https" : "http";
            this.baseurl = this.getBaseUrl();
            var Magic360FlashDefaultParams = new Array();
            for (var i in this.defaults) {
                Magic360FlashDefaultParams[i] = this.defaults[i]
            }
            var Magic360FlashOptionsInn = new Array();
            for (var i in this.options) {
                Magic360FlashOptionsInn[i] = this.options[i]
            }
            for (var i in Magic360FlashOptionsInn) {
                if (Magic360FlashDefaultParams.hasOwnProperty(i)) {
                    Magic360FlashDefaultParams[i] = Magic360FlashOptionsInn[i];
                    if (i == "magnifier-size") {
                        Magic360FlashDefaultParams["magnifier-size-x"] = Magic360FlashDefaultParams["magnifier-size-y"] = Magic360FlashDefaultParams["magnifier-size"]
                    }
                }
            }
            for (var i in Magic360FlashOptionsInn[this.params.id]) {
                if (Magic360FlashDefaultParams.hasOwnProperty(i)) {
                    Magic360FlashDefaultParams[i] = Magic360FlashOptionsInn[this.params.id][i];
                    if (i == "magnifier-size") {
                        Magic360FlashDefaultParams["magnifier-size-x"] = Magic360FlashDefaultParams["magnifier-size-y"] = Magic360FlashDefaultParams["magnifier-size"]
                    }
                }
            }
            for (var i in Magic360FlashDefaultParams) {
                if (Magic360FlashDefaultParams.hasOwnProperty(i)) {
                    var value = Magic360FlashDefaultParams[i].toString();
                    if (value.toLowerCase() == "true" || Magic360FlashDefaultParams[i] === true) {
                        Magic360FlashDefaultParams[i] = 1
                    }
                    if (value.toLowerCase() == "none" || value.toLowerCase() == "false" || Magic360FlashDefaultParams[i] === false) {
                        Magic360FlashDefaultParams[i] = 0
                    }
                }
            }
            var swfFileName = "Magic360Flash";
            this.swfUrl = this.baseurl + swfFileName.toLowerCase();
            Magic360FlashDefaultParams["border-color"] = this.color(Magic360FlashDefaultParams["border-color"]);
            Magic360FlashDefaultParams["progress-color"] = this.color(Magic360FlashDefaultParams["progress-color"]);
            Magic360FlashDefaultParams["progress-text-color"] = this.color(Magic360FlashDefaultParams["progress-text-color"]);
            if (Magic360FlashDefaultParams.background.indexOf("#") == 0) {
                Magic360FlashDefaultParams.background = this.color(Magic360FlashDefaultParams.background)
            }
            Magic360FlashDefaultParams["magnifier-border-color"] = this.color(Magic360FlashDefaultParams["magnifier-border-color"]);
            if (Magic360FlashDefaultParams["magnifier-filter"].toLowerCase() == "shadow") {
                Magic360FlashDefaultParams["magnifier-filter"] = "shadow,4,60,#000,0.3,6,6,2,3,false"
            } else {
                if (Magic360FlashDefaultParams["magnifier-filter"].toLowerCase() == "glow") {
                    Magic360FlashDefaultParams["magnifier-filter"] = "glow,#000,0.5,5,5,2,100,false"
                }
            }
            if (this.params.big_images == null) {
                this.params.big_images = ""
            }
            if (this.params.small_images == null) {
                this.params.small_images = ""
            }
            if (this.params.big_images != "") {
                var imgs = this.params.big_images.split(Magic360FlashDefaultParams.delimiter);
                for (var i = 0; i < imgs.length; i++) {
                    imgs[i] = this.toAbs(imgs[i], document.location.href)
                }
                this.params.big_images = imgs.join(Magic360FlashDefaultParams.delimiter)
            }
            if (this.params.small_images != "") {
                var imgs = this.params.small_images.split(Magic360FlashDefaultParams.delimiter);
                for (var i = 0; i < imgs.length; i++) {
                    imgs[i] = this.toAbs(imgs[i], document.location.href)
                }
                this.params.small_images = imgs.join(Magic360FlashDefaultParams.delimiter)
            }
            var flashVars = "_p_big_images=" + this.escapeParam(this.params.big_images) + "&_p_small_images=" + this.escapeParam(this.params.small_images) + "&_p_width=" + this.escapeParam(this.params.width) + "&_p_height=" + this.escapeParam(this.params.height);
            for (var i in Magic360FlashDefaultParams) {
                if (Magic360FlashDefaultParams.hasOwnProperty(i)) {
                    flashVars += "&_p_" + i.replace(/-/g, "_") + "=" + this.escapeParam(Magic360FlashDefaultParams[i])
                }
            }
            flashVars += "&_p_img_id=" + this.params.img_id;
            return M360_AC_FL_RunContent("codebase", httppref + "://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,0,0", "FlashVars", flashVars, "width", this.params.width, "height", this.params.height, "name", this.params.name, "id", this.params.name, "src", this.swfUrl, "quality", "high", "pluginspage", "http://www.adobe.com/go/getflashplayer", "movie", this.swfUrl, "allowScriptAccess", "always", "base", ".", "wmode", Magic360FlashDefaultParams.wmode, "scale", "noscale", "allowFullScreen", "true", "salign", "lt", "swliveConnect", "true")
        },
        escapeParam: function (val) {
            return encodeURIComponent(val)
        },
        sendMouseUp: function () {
            for (var i in m360ATags) {
                if (m360ATags.hasOwnProperty(i)) {
                    var id = i.toString();
                    var movie = Magic360Flash.getFlashMovieObject(id.replace("m360ObjCont", "m360Obj"));
                    if (movie && movie.getMouseUp) {
                        movie.getMouseUp()
                    }
                }
            }
        },
        sOpacity: function (elm, opacity) {
            if (elm.style) {
                elm.style.opacity = parseFloat(opacity / 100);
                elm.style["-moz-opacity"] = parseFloat(opacity / 100);
                elm.style["-html-opacity"] = parseFloat(opacity / 100);
                elm.style.filter = "alpha(Opacity=" + opacity + ")"
            }
        },
        getFlashMovieObject: function (movieName) {
            if (window.document[movieName]) {
                return window.document[movieName]
            }
            if (navigator.appName.indexOf("Microsoft Internet") == -1) {
                if (document.embeds && document.embeds[movieName]) {
                    return document.embeds[movieName]
                }
            } else {
                return document.getElementById(movieName)
            }
        },
        removeImg: function (id, opacity) {
            elm = document.getElementById(id);
            if (elm == undefined) {
                return
            }
            opacity -= 5;
            this.sOpacity(elm, opacity);
            if (opacity <= 0) {
                elm.parentNode.removeChild(elm)
            } else {
                setTimeout(function () {
                    Magic360Flash.removeImg(id, opacity)
                },
				1)
            }
        },
        color: function (color) {
            color = color.replace("#", "");
            if (color.length == 3) {
                color = color + color
            }
            return "0x" + color
        },
        stop: function () {
            for (var id in m360ATags) {
                var elm = document.getElementById(id);
                if (elm) {
                    var p = elm.parentNode;
                    p.replaceChild(m360ATags[id], elm)
                }
            }
            m360ATags = new Array()
        },
        toAbs: function (link, host) {
            var lparts = link.split("/");
            if (/http:|https:|ftp:/.test(lparts[0])) {
                return link
            }
            var i,
			hparts = host.split("/");
            if (hparts.length > 3) {
                hparts.pop()
            }
            if (lparts[0] === "") {
                host = hparts[0] + "//" + hparts[2];
                hparts = host.split("/");
                delete lparts[0]
            }
            for (i = 0; i < lparts.length; i++) {
                if (lparts[i] === "..") {
                    if (typeof lparts[i - 1] !== "undefined") {
                        delete lparts[i - 1]
                    } else {
                        if (hparts.length > 3) {
                            hparts.pop()
                        }
                    }
                    delete lparts[i]
                }
                if (lparts[i] === ".") {
                    delete lparts[i]
                }
            }
            var newlinkparts = [];
            for (i = 0; i < lparts.length; i++) {
                if (typeof lparts[i] !== "undefined") {
                    newlinkparts[newlinkparts.length] = lparts[i]
                }
            }
            return hparts.join("/") + "/" + newlinkparts.join("/")
        },
        getVer: function () {
            var flashVer = -1;
            var isIE = (navigator.appVersion.indexOf("MSIE") != -1) ? true : false;
            var isWin = (navigator.appVersion.toLowerCase().indexOf("win") != -1) ? true : false;
            var isOpera = (navigator.userAgent.indexOf("Opera") != -1) ? true : false;
            if (navigator.plugins != null && navigator.plugins.length > 0) {
                if (navigator.plugins["Shockwave Flash 2.0"] || navigator.plugins["Shockwave Flash"]) {
                    var swVer2 = navigator.plugins["Shockwave Flash 2.0"] ? " 2.0" : "";
                    var flashDescription = navigator.plugins["Shockwave Flash" + swVer2].description;
                    var descArray = flashDescription.split(" ");
                    var tempArrayMajor = descArray[2].split(".");
                    var versionMajor = tempArrayMajor[0];
                    var versionMinor = tempArrayMajor[1];
                    if (descArray[3] != "") {
                        tempArrayMinor = descArray[3].split("r")
                    } else {
                        tempArrayMinor = descArray[4].split("r")
                    }
                    var versionRevision = tempArrayMinor[1] > 0 ? tempArrayMinor[1] : 0;
                    var flashVer = versionMajor
                }
            } else {
                if (navigator.userAgent.toLowerCase().indexOf("webtv/2.6") != -1) {
                    flashVer = 4
                } else {
                    if (navigator.userAgent.toLowerCase().indexOf("webtv/2.5") != -1) {
                        flashVer = 3
                    } else {
                        if (navigator.userAgent.toLowerCase().indexOf("webtv") != -1) {
                            flashVer = 2
                        } else {
                            if (isIE && isWin && !isOpera) {
                                var version;
                                var axo;
                                var e;
                                try {
                                    axo = new ActiveXObject("ShockwaveFlash.ShockwaveFlash.7");
                                    version = axo.GetVariable("$version");
                                    version = 7
                                } catch (e) { }
                                if (!version) {
                                    try {
                                        axo = new ActiveXObject("ShockwaveFlash.ShockwaveFlash.6");
                                        version = "WIN 6,0,21,0";
                                        axo.AllowScriptAccess = "always";
                                        version = axo.GetVariable("$version");
                                        version = 6
                                    } catch (e) { }
                                }
                                if (!version) {
                                    try {
                                        axo = new ActiveXObject("ShockwaveFlash.ShockwaveFlash.3");
                                        version = axo.GetVariable("$version");
                                        version = 3
                                    } catch (e) { }
                                }
                                if (!version) {
                                    try {
                                        axo = new ActiveXObject("ShockwaveFlash.ShockwaveFlash.3");
                                        version = "WIN 3,0,18,0";
                                        version = 3
                                    } catch (e) { }
                                }
                                if (!version) {
                                    try {
                                        axo = new ActiveXObject("ShockwaveFlash.ShockwaveFlash");
                                        version = "WIN 2,0,0,11";
                                        version = 2
                                    } catch (e) {
                                        version = -1
                                    }
                                }
                                return version
                            }
                        }
                    }
                }
            }
            return flashVer
        },
        start: function () {
            var aels = $J.$A(window.document.getElementsByTagName("*"));
            var flVer = this.getVer();
            if (parseInt(flVer) < 7) {
                return
            }
            for (var i = 0, l = aels.length; i < l; i++) {
                if (/Magic360Flash/.test(aels[i].className)) {
                    while (aels[i].firstChild) {
                        if (aels[i].firstChild.tagName != "IMG") {
                            aels[i].removeChild(aels[i].firstChild)
                        } else {
                            break
                        }
                    }
                    if (aels[i].firstChild && aels[i].firstChild.tagName != "IMG" || aels[i].firstChild == undefined && aels[i].tagName != "IMG") {
                        throw "Invalid Magic 360 Flash invocation!"
                    }
                    if (this.options["disable-auto-start"] && arguments.length == 0) {
                        $mjs(aels[i]).je1("click",
						function (e) {
						    $mjs(e).stop();
						    this.start(true);
						    return false
						});
                        continue
                    }
                    var elm = (aels[i].tagName == "IMG") ? aels[i] : aels[i].firstChild;
                    var img = new MagicImage(elm, {
                        onload: function (ael, el, options) {
                            var width = parseInt(el.width);
                            var height = parseInt(el.height);
                            if (width == 0 || height == 0) {
                                width = parseInt(el.style.width);
                                height = parseInt(el.style.height)
                            }
                            var div = document.createElement("SPAN");
                            div.className = "Magic360FlashContainer";
                            var iid = m360ATags.length;
                            var params = {
                                width: width,
                                height: height,
                                big_images: (ael.tagName == "A") ? ael.getAttribute("rel") : "",
                                small_images: ((ael.tagName == "IMG") ? ael : ael.firstChild).getAttribute("rel") || ((ael.tagName == "IMG") ? ael : ael.firstChild).getAttribute("longdesc"),
                                name: "m360Obj" + iid,
                                img_id: "mtImg" + iid,
                                id: ael.id
                            };
                            div.id = "m360ObjCont" + iid;
                            $mjs(div).j6({
                                position: "relative",
                                overflow: "hidden",
                                display: "inline-block",
                                width: width + "px",
                                height: height + "px"
                            });
                            var img = document.createElement("SPAN");
                            $mjs(img).j6({
                                position: "absolute",
                                display: "block",
                                top: "0px",
                                left: "0px",
                                "z-index": "200"
                            });
                            img.className = "";
                            img.id = "mtImg" + iid;
                            img.appendChild(ael.cloneNode(true));
                            var loaderURL = (options.loader == undefined) ? Magic360Flash.getBaseUrl() + "loader.gif" : options.loader;
                            if (ael.id && options[ael.id] && options[ael.id]["loader"] != undefined) {
                                loaderURL = options[ael.id]["loader"]
                            }
                            var loader = document.createElement("SPAN");
                            $mjs(loader).j6({
                                position: "absolute",
                                display: "block",
                                top: "0px",
                                left: "0px",
                                background: "url(" + loaderURL + ") center center no-repeat",
                                width: width + "px",
                                height: height + "px"
                            });
                            Magic360Flash.sOpacity(loader, 70);
                            img.appendChild(loader);
                            $mjs(img).je1("click",
							function (e) {
							    $mjs(e).stop();
							    if (M360_ua != "msie") {
							        this.blur()
							    } else {
							        window.focus()
							    }
							    return false
							} .j16(img));
                            var divel = document.createElement("SPAN");
                            $mjs(divel).j6({
                                display: "block",
                                top: "0px",
                                left: "0px",
                                "z-index": "100",
                                width: width + "px",
                                height: height + "px",
                                position: "absolute"
                            });
                            div.appendChild(divel);
                            if (document.location.protocol.indexOf("http") == 0) {
                                div.appendChild(img)
                            }
                            var p = ael.parentNode;
                            m360ATags[div.id] = ael;
                            p.replaceChild(div, ael);
                            var str = '<SCRIPT event="FSCommand(command,args)" for="m360Obj' + iid + '">M360_DoFSCommand(command, args);</SCRIPT>';
                            m360Swf(divel, Magic360Flash.init(params) + str);
                            eval("window.m360Obj" + iid + "_DoFSCommand = function(command,arguments){ M360_DoFSCommand(command,arguments) };");
                            var webkit = navigator.userAgent.indexOf("AppleWebKit/") > -1;
                            var gecko = navigator.userAgent.indexOf("Gecko") > -1 && navigator.userAgent.indexOf("KHTML") == -1;
                            var movie = divel.getElementsByTagName("OBJECT")[0];
                            if (movie == undefined) {
                                movie = divel.getElementsByTagName("EMBED")[0]
                            }
                            if (gecko || webkit) {
                                $mjs(div).je1(((gecko) ? "DOMMouseScroll" : "mousewheel"),
								function (e, m) {
								    var value = (e.detail) ? -e.detail : e.wheelDelta;
								    if (Math.abs(value)) {
								        if ($J.j21.engine == "webkit") {
								            value = 3 * value / Math.abs(value)
								        } else { }
								        if (m.onWheel) {
								            m.onWheel(value)
								        }
								    }
								    $mjs(e).stop()
								} .j16(div, movie))
                            }
                            if ($J.j21.engine == "presto") {
                                movie.onmousewheel = function (e) {
                                    var value = (e.detail) ? -e.detail : e.wheelDelta;
                                    if (Math.abs(value)) {
                                        if (this.onWheel) {
                                            this.onWheel(value)
                                        }
                                    }
                                    $mjs(e).stop()
                                }
                            }
                            $mjs(movie).je1("mouseout",
							function (e, m) {
							    if (m && m.getMouseOut) {
							        m.getMouseOut()
							    }
							    $mjs(e).stop()
							} .j16(div, movie));
                            $mjs(movie).je1("mousedown",
							function (e, m) {
							    if (m && m.getMouseDown) {
							        m.getMouseDown("movie.mousedown")
							    }
							    if ($J.j21.engine != "webkit") {
							        $mjs(e).stop()
							    }
							} .j16(div, movie));
                            $mjs(movie).je1("mouseover",
							function (e, m) {
							    if (m && m.getMouseOver) {
							        m.getMouseOver()
							    }
							    $mjs(e).stop()
							} .j16(div, movie))
                        } .j24(null, aels[i], elm, this.options)
                    })
                }
            }
        }
    };
    return Magic360Flash
})(magicJS);
function M360_DoFSCommand(b, a) {
    if ("Magic360_removeImg" == b) {
        Magic360Flash.removeImg(a, 100)
    }
}
$mjs(document).je1("domready",
function () {
    Magic360Flash.start()
});
$mjs(document).je1("mouseup", Magic360Flash.sendMouseUp);
window.m360Swf = function (o, s) { o.innerHTML = s; };

