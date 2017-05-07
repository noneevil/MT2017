/*


Magic Magnify v4.3.2 DEMO
Copyright 2012 Magic Toolbox
Buy a license: www.magictoolbox.com/magicmagnify/
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
})(); (function (a) {
    if (!a) {
        throw "MagicJS not found";
        return
    }
    if (a.FX) {
        return
    }
    var b = a.$;
    a.FX = new a.Class({
        options: {
            fps: 50,
            duration: 500,
            transition: function (c) {
                return -(Math.cos(Math.PI * c) - 1) / 2
            },
            onStart: a.$F,
            onComplete: a.$F,
            onBeforeRender: a.$F,
            roundCss: true
        },
        styles: null,
        init: function (d, c) {
            this.el = $mjs(d);
            this.options = a.extend(this.options, c);
            this.timer = false
        },
        start: function (c) {
            this.styles = c;
            this.state = 0;
            this.curFrame = 0;
            this.startTime = a.now();
            this.finishTime = this.startTime + this.options.duration;
            this.timer = this.loop.j24(this).interval(Math.round(1000 / this.options.fps));
            this.options.onStart.call();
            return this
        },
        stop: function (c) {
            c = a.defined(c) ? c : false;
            if (this.timer) {
                clearInterval(this.timer);
                this.timer = false
            }
            if (c) {
                this.render(1);
                this.options.onComplete.j27(10)
            }
            return this
        },
        calc: function (e, d, c) {
            return (d - e) * c + e
        },
        loop: function () {
            var d = a.now();
            if (d >= this.finishTime) {
                if (this.timer) {
                    clearInterval(this.timer);
                    this.timer = false
                }
                this.render(1);
                this.options.onComplete.j27(10);
                return this
            }
            var c = this.options.transition((d - this.startTime) / this.options.duration);
            this.render(c)
        },
        render: function (c) {
            var d = {};
            for (var e in this.styles) {
                if ("opacity" === e) {
                    d[e] = Math.round(this.calc(this.styles[e][0], this.styles[e][1], c) * 100) / 100
                } else {
                    d[e] = this.calc(this.styles[e][0], this.styles[e][1], c);
                    if (this.options.roundCss) {
                        d[e] = Math.round(d[e])
                    }
                }
            }
            this.options.onBeforeRender(d);
            this.set(d)
        },
        set: function (c) {
            return this.el.j6(c)
        }
    });
    a.FX.Transition = {
        linear: function (c) {
            return c
        },
        sineIn: function (c) {
            return -(Math.cos(Math.PI * c) - 1) / 2
        },
        sineOut: function (c) {
            return 1 - a.FX.Transition.sineIn(1 - c)
        },
        expoIn: function (c) {
            return Math.pow(2, 8 * (c - 1))
        },
        expoOut: function (c) {
            return 1 - a.FX.Transition.expoIn(1 - c)
        },
        quadIn: function (c) {
            return Math.pow(c, 2)
        },
        quadOut: function (c) {
            return 1 - a.FX.Transition.quadIn(1 - c)
        },
        cubicIn: function (c) {
            return Math.pow(c, 3)
        },
        cubicOut: function (c) {
            return 1 - a.FX.Transition.cubicIn(1 - c)
        },
        backIn: function (d, c) {
            c = c || 1.618;
            return Math.pow(d, 2) * ((c + 1) * d - c)
        },
        backOut: function (d, c) {
            return 1 - a.FX.Transition.backIn(1 - d)
        },
        elasticIn: function (d, c) {
            c = c || [];
            return Math.pow(2, 10 * --d) * Math.cos(20 * d * Math.PI * (c[0] || 1) / 3)
        },
        elasticOut: function (d, c) {
            return 1 - a.FX.Transition.elasticIn(1 - d, c)
        },
        bounceIn: function (e) {
            for (var d = 0, c = 1; 1; d += c, c /= 2) {
                if (e >= (7 - 4 * d) / 11) {
                    return c * c - Math.pow((11 - 6 * d - 11 * e) / 4, 2)
                }
            }
        },
        bounceOut: function (c) {
            return 1 - a.FX.Transition.bounceIn(1 - c)
        },
        none: function (c) {
            return 0
        }
    }
})(magicJS); (function (a) {
    if (!a) {
        throw "MagicJS not found";
        return
    }
    if (!a.FX) {
        throw "MagicJS.FX not found";
        return
    }
    if (a.FX.Slide) {
        return
    }
    var b = a.$;
    a.FX.Slide = new a.Class(a.FX, {
        options: {
            mode: "vertical"
        },
        init: function (d, c) {
            this.el = $mjs(d);
            this.options = a.extend(this.$parent.options, this.options);
            this.$parent.init(d, c);
            this.wrapper = this.el.j29("slide:wrapper");
            this.wrapper = this.wrapper || a.$new("DIV").j6(a.extend(this.el.j19s("margin-top", "margin-left", "margin-right", "margin-bottom", "position", "top", "float"), {
                overflow: "hidden"
            })).enclose(this.el);
            this.el.j30("slide:wrapper", this.wrapper).j6({
                margin: 0
            })
        },
        vertical: function () {
            this.margin = "margin-top";
            this.layout = "height";
            this.offset = this.el.offsetHeight
        },
        horizontal: function (c) {
            this.margin = "margin-" + (c || "left");
            this.layout = "width";
            this.offset = this.el.offsetWidth
        },
        right: function () {
            this.horizontal()
        },
        left: function () {
            this.horizontal("right")
        },
        start: function (e, h) {
            this[h || this.options.mode]();
            var g = this.el.j5(this.margin).j17(),
			f = this.wrapper.j5(this.layout).j17(),
			c = {},
			i = {},
			d;
            c[this.margin] = [g, 0],
			c[this.layout] = [0, this.offset],
			i[this.margin] = [g, -this.offset],
			i[this.layout] = [f, 0];
            switch (e) {
                case "in":
                    d = c;
                    break;
                case "out":
                    d = i;
                    break;
                case "toggle":
                    d = (0 == f) ? c : i;
                    break
            }
            this.$parent.start(d);
            return this
        },
        set: function (c) {
            this.el.j6Prop(this.margin, c[this.margin]);
            this.wrapper.j6Prop(this.layout, c[this.layout]);
            return this
        },
        slideIn: function (c) {
            return this.start("in", c)
        },
        slideOut: function (c) {
            return this.start("out", c)
        },
        hide: function (d) {
            this[d || this.options.mode]();
            var c = {};
            c[this.layout] = 0,
			c[this.margin] = -this.offset;
            return this.set(c)
        },
        show: function (d) {
            this[d || this.options.mode]();
            var c = {};
            c[this.layout] = this.offset,
			c[this.margin] = 0;
            return this.set(c)
        },
        toggle: function (c) {
            return this.start("toggle", c)
        }
    })
})(magicJS);
function MT_AC_AddExtension(b, a) {
    if (b.indexOf("?") != -1) {
        return b.replace(/\?/, a + "?")
    } else {
        return b + a
    }
}
function MT_AC_Generateobj(h, g, a) {
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
function MT_AC_FL_RunContent() {
    var a = MT_AC_GetArgs(arguments, ".swf", "movie", "clsid:d27cdb6e-ae6d-11cf-96b8-444553540000", "application/x-shockwave-flash");
    return MT_AC_Generateobj(a.objAttrs, a.params, a.embedAttrs)
}
function MT_AC_SW_RunContent() {
    var a = MT_AC_GetArgs(arguments, ".dcr", "src", "clsid:166B1BCA-3F9C-11CF-8075-444553540000", null);
    return MT_AC_Generateobj(a.objAttrs, a.params, a.embedAttrs)
}
function MT_AC_GetArgs(b, e, g, d, h) {
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
                b[c + 1] = MT_AC_AddExtension(b[c + 1], e);
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
var mATags = new Array();
var mSWF2aTags = new Array();
var mtATagsNum = new Array();
var mSelectors = new Array();
var mSelectorsObj = new Array();
var mIDSTags = new Array();
var MagicMagnify = (function ($J) {
    var $j = $J.$;
    var MagicMagnify = {
        version: "v4.3.2",
        codeVersion: "v2.3.3",
        options: {},
        defaults: {
            "border-color": "#9b9b9b",
            "border-width": 0,
            magnifier: "circle",
            "magnifier-size": "66%",
            "magnifier-size-x": "66%",
            "magnifier-size-y": "66%",
            "magnifier-border-width": 1,
            "magnifier-border-color": "#9b9b9b",
            "magnifier-effect": "fade",
            "magnifier-filter": "glow",
            "magnifier-time": 200,
            "magnifier-simulate": "",
            "lense-url": "",
            "lense-offset-x": 0,
            "lense-offset-y": 0,
            "lense-position": "top",
            "hide-cursor": ("MagicMagnify" == "MagicMagnify"),
            callback: "",
            "thumb-change": "click",
            "thumb-change-delay": 200,
            "thumb-change-time": 500,
            "link-url": "",
            "link-window": "_self",
            blur: false,
            transparency: "100%",
            "show-immediately": false,
            "show-immediately-x": 0,
            "show-immediately-y": 0,
            "disable-auto-start": false,
            containerDisplay: "block",
            "disable-expand": false,
            "pause-on-click": false,
            "init-on-click": true,
            "change-time": 500,
            background: "",
            "thumb-id": "",
            "progress-color": "#CCCCCC",
            "progress-height": 0,
            wmode: "transparent",
            "disable-image-clone": true,
            "secure-domain": "",
            "disable-crossdomain": false,
            "wheel-effect": "20%"
        },
        init: function (params) {
            this.params = params;
            httppref = (document.location.protocol.indexOf("https") != -1) ? "https" : "http";
            this.baseurl = "";
            var MagicMagnify_elements = document.getElementsByTagName("script");
            for (var i = 0; i < MagicMagnify_elements.length; i++) {
                if (MagicMagnify_elements[i].src && (/MagicMagnify.js/i.test(MagicMagnify_elements[i].src))) {
                    var src = MagicMagnify_elements[i].src;
                    srcMode = (src.indexOf("_src") != -1) ? "_src" : "";
                    src = src.substring(0, src.lastIndexOf("/"));
                    this.baseurl = src + "/";
                    if (this.baseurl == "/") {
                        this.baseurl = ""
                    }
                    break
                }
            }
            var MagicMagnifyDefaultParams = params.params;
            var swfFileName = "MagicMagnify";
            this.swfUrl = this.baseurl + swfFileName.toLowerCase();
            var flashVars = "_p_big_images=" + this.escapeParam(this.params.bigImageUrl) + "&_p_small_images=" + this.escapeParam(this.params.smallImageUrl) + "&_p_width=" + this.params.width + "&_p_status=58723627fcebc230ab0d53ddf5f16e34&_p_height=" + this.params.height;
            if ("MagicMagnify" != "MagicMagnifyPlus" || MagicMagnifyDefaultParams["disable-expand"]) {
                MagicMagnifyDefaultParams["thumb-id"] = ""
            }
            if (MagicMagnifyDefaultParams["secure-domain"] == "") {
                MagicMagnifyDefaultParams["secure-domain"] = document.location.hostname
            }
            if (MagicMagnifyDefaultParams["magnifier-simulate"] != "") {
                MagicMagnifyDefaultParams["magnifier-simulate"] = parseInt(MagicMagnifyDefaultParams["magnifier-simulate"])
            }
            for (var i in MagicMagnifyDefaultParams) {
                if (flashVars != "") {
                    flashVars += "&"
                }
                flashVars += "_p_" + i.replace(/-/g, "_") + "=" + this.escapeParam(MagicMagnifyDefaultParams[i])
            }
            flashVars += "&_p_baseurl=" + this.escapeParam(this.baseurl) + "&_p_img_id=" + this.params.img_id;
            return MT_AC_FL_RunContent("codebase", httppref + "://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,0,0", "FlashVars", flashVars, "width", "100%", "height", "100%", "name", this.params.name, "id", this.params.name, "src", this.swfUrl, "quality", "high", "pluginspage", "http://www.adobe.com/go/getflashplayer", "movie", this.swfUrl, "allowScriptAccess", "always", "base", ".", "wmode", MagicMagnifyDefaultParams.wmode, "allowFullScreen", "true", "salign", "lt", "scale", "exactFit", "swliveConnect", "true")
        },
        escapeParam: function (val) {
            return encodeURIComponent(val)
        },
        createParamsList: function (elm) {
            var p = new Array();
            for (var i in this.defaults) {
                p[i] = this.defaults[i]
            }
            var MagicMagnifyOptionsInn = new Array();
            for (var i in this.options) {
                MagicMagnifyOptionsInn[i] = this.options[i]
            }
            if (elm.rel != undefined && elm.rel != "") {
                for (var i in p) {
                    var str = this.getParam("(^|[ ;]{1,})" + i.replace("-", "\\-") + "(\\s+)*:(\\s+)*([^;$]*)", elm.rel, "*");
                    if (str != "*") {
                        MagicMagnifyOptionsInn[i] = str
                    }
                }
            }
            for (var i in MagicMagnifyOptionsInn) {
                if (p.hasOwnProperty(i)) {
                    p[i] = MagicMagnifyOptionsInn[i];
                    if (i == "magnifier-size") {
                        p["magnifier-size-x"] = p["magnifier-size-y"] = p["magnifier-size"]
                    }
                }
            }
            for (var i in MagicMagnifyOptionsInn[elm.id]) {
                if (p.hasOwnProperty(i)) {
                    p[i] = MagicMagnifyOptionsInn[elm.id][i];
                    if (i == "magnifier-size") {
                        p["magnifier-size-x"] = p["magnifier-size-y"] = p["magnifier-size"]
                    }
                }
            }
            for (var i in p) {
                if (p.hasOwnProperty(i)) {
                    var value = p[i].toString();
                    if (/\-?[0-9]*px/i.test(value)) {
                        p[i] = value.replace("px", "")
                    }
                    if (/^[0-9]{1,}$/i.test(value)) {
                        p[i] = value.j17()
                    }
                    if (value.toLowerCase() == "yes" || value.toLowerCase() == "true" || p[i] === true) {
                        p[i] = 1
                    }
                    if (value.toLowerCase() == "no" || value.toLowerCase() == "none" || value.toLowerCase() == "false" || p[i] === false) {
                        p[i] = 0
                    }
                }
            }
            p["border-color"] = this.color(p["border-color"]);
            p["magnifier-border-color"] = this.color(p["magnifier-border-color"]);
            p["progress-color"] = this.color(p["progress-color"]);
            p.background = this.color(p.background);
            var s = p["magnifier-filter"] + "";
            if (s.toLowerCase() == "shadow") {
                p["magnifier-filter"] = "shadow,4,60,#000,0.3,6,6,2,3,false"
            } else {
                if (s.toLowerCase() == "glow") {
                    p["magnifier-filter"] = "glow,#000,0.5,5,5,2,100,false"
                }
            }
            if (p["lense-url"] != "") {
                var img = new Image();
                img.src = p["lense-url"];
                p["lense-url"] = img.src
            }
            return p
        },
        getBaseUrl: function () {
            var baseurl = "";
            var MagicMagnify_elements = document.getElementsByTagName("script");
            for (var i = 0; i < MagicMagnify_elements.length; i++) {
                if (MagicMagnify_elements[i].src && (/MagicMagnify.js/i.test(MagicMagnify_elements[i].src))) {
                    var src = MagicMagnify_elements[i].src;
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
        getParam: function (pattern, string, defaultValue) {
            var re = new RegExp(pattern, "i");
            var matches = re.exec(string);
            return (matches) ? matches[4] : defaultValue
        },
        sOpacity: function (elm, opacity) {
            if (elm.style && $mjs(elm).j6) {
                $mjs(elm).j6({
                    opacity: opacity / 100
                });
                if ($J.j21.trident && elm.firstChild) {
                    var imgs = $J.$A(elm.getElementsByTagName("IMG"));
                    for (var i = 0, l = imgs.length; i < l; i++) {
                        this.sOpacity(imgs[i], opacity)
                    }
                }
            }
        },
        smoothShow: function (elm, opacity) {
            opacity += 5;
            this.sOpacity(elm, opacity);
            if (opacity >= 100) {
                this.sOpacity(elm, 100)
            } else {
                setTimeout(function () {
                    this.smoothShow(elm, opacity)
                },
				1)
            }
        },
        setThumbnail: function (img) { },
        removeImg: function (id, opacity) {
            elm = document.getElementById(id);
            if (elm == undefined) {
                return
            }
            opacity -= 5;
            this.sOpacity(elm, opacity);
            if (opacity <= 0) {
                elm.parentNode.removeChild(elm);
                var movie = this.getFlashMovieObject(id.replace("mImg", "mObj"));
                if (mSelectors[id] && mSelectors[id].length) {
                    for (var i in mSelectors[id]) {
                        if (mSelectors[id].hasOwnProperty(i)) {
                            movie.initAdditionalImage(mSelectors[id][i]["sm_img"], mSelectors[id][i]["bg_img"])
                        }
                    }
                }
                if ("MagicMagnify" != "MagicMagnifyPlus" && this.parentNode) { }
            } else {
                setTimeout(function () {
                    MagicMagnify.removeImg(id, opacity)
                },
				1)
            }
        },
        color: function (color) {
            if (color == "") {
                return color
            }
            color = color.replace("#", "");
            if (color.length == 3) {
                color = color + color
            }
            return "0x" + color
        },
        changeThumbnail: function (p) {
            var p = p.split("*");
            var img_id = p[0];
            var w = p[1];
            var h = p[2];
            var movie = this.getFlashMovieObject(img_id.replace("mImg", "mObj"));
            var tha = movie.parentNode.tha;
            var resize = {};
            resize.width = [movie.parentNode.j7().width, w.j17()];
            resize.height = [movie.parentNode.j7().height, h.j17()];
            if (movie.parentNode.fx) {
                movie.parentNode.fx.stop()
            }
            if (movie.parentNode.parentNode.fx) {
                movie.parentNode.parentNode.fx.stop()
            }
            if (tha && tha.fx) {
                tha.fx.stop()
            }
            movie.parentNode.fx = new $J.FX(movie.parentNode, {
                duration: tha.params["thumb-change-time"]
            });
            movie.parentNode.fx.start(resize);
            movie.parentNode.parentNode.fx = new $J.FX(movie.parentNode.parentNode, {
                duration: tha.params["thumb-change-time"]
            });
            movie.parentNode.parentNode.fx.start(resize);
            if (tha) {
                tha.fx = new $J.FX(tha, {
                    duration: tha.params["thumb-change-time"]
                });
                tha.fx.start(resize)
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
        stop: function () {
            for (var id in mATags) {
                var elm = document.getElementById(id);
                if (elm) {
                    var p = elm.parentNode;
                    p.replaceChild(mATags[id], elm)
                }
            }
            mATags = new Array();
            mIDSTags = new Array();
            mtATagsNum = new Array();
            mSelectors = new Array();
            mSelectorsObj = new Array();
            mSWF2aTags = new Array();
            if ("MagicMagnify" == "MagicMagnifyPlus") {
                if (MagicThumb) {
                    MagicThumb.stop()
                }
            }
        },
        findSelectors: function (div, id, params, s_bigImageUrl, s_smallImageUrl, s_divElm, thumbContainer, tha, aTagOriginal) {
            var p = new Array();
            var oldId = "";
            var done = false;
            for (var j in params) {
                p[j] = params[j]
            }
            var aels = $J.$A(window.document.getElementsByTagName("A"));
            for (var i = 0, l = aels.length; i < l; i++) {
                if (aels[i].rel == id) {
                    aels[i].rel = "magnifier-id:" + id
                }
                var magnifier_id = this.getParam("(^|[ ;]{1,})magnifier-id(s+)?:(s+)?([^;$ ]{1,})", aels[i].rel, "");
                if (magnifier_id == id) {
                    if (aels[i].rev == "") {
                        continue
                    }
                    done = true;
                    if (aels[i].rel.indexOf("thumb-id:") == -1) {
                        aels[i].rel += ";thumb-id:" + tha.id
                    }
                    p.smallImageUrl = aels[i].rev;
                    p.bigImageUrl = aels[i].href;
                    var thumbA = document.createElement("A");
                    $mjs(thumbA).j6({
                        position: "absolute",
                        top: "0px",
                        left: "0px"
                    });
                    thumbA = tha;
                    var aTag = aTagOriginal.cloneNode(true);
                    $mjs(aTag).j6({
                        position: "absolute",
                        display: "block",
                        top: "0px",
                        left: "0px",
                        "z-index": "1"
                    });
                    aTag.className = "MagicMagnify";
                    aTag.id = tha.id + "-" + mSelectors[p.img_id].length;
                    aTag.rel = "stop";
                    var elm = new Image();
                    if (aTag.firstChild.getAttribute("width")) {
                        elm.setAttribute("width", aTag.firstChild.getAttribute("width"))
                    }
                    if (aTag.firstChild.getAttribute("height")) {
                        elm.setAttribute("height", aTag.firstChild.getAttribute("height"))
                    }
                    if (aTag.firstChild.getAttribute("style")) {
                        elm.setAttribute("style", aTag.firstChild.getAttribute("style"))
                    }
                    aTag.replaceChild(elm, aTag.firstChild);
                    thumbContainer.appendChild(aTag);
                    elm.src = p.smallImageUrl;
                    p.smallImageUrl = elm.src;
                    mSelectors[p.img_id][mSelectors[p.img_id].length] = {
                        sm_img: p.smallImageUrl,
                        bg_img: p.bigImageUrl
                    };
                    mSelectorsObj[mSelectorsObj.length] = aels[i];
                    var img = new MagicImage(elm, {
                        onload: function (elm, div, id, params, smallImageUrl, bigImageUrl, s_divElm, thumbContainer, ael, thumbA, aTagOriginal) {
                            var width = elm.j7().width;
                            var height = elm.j7().height;
                            elm.parentNode.parentNode.removeChild(elm.parentNode);
                            movie = s_divElm.getElementsByTagName("OBJECT")[0];
                            if (movie == undefined) {
                                movie = s_divElm.getElementsByTagName("EMBED")[0]
                            }
                            movie.parentNode.tha = tha;
                            if (aTagOriginal.params["thumb-change"] == "click") {
                                $mjs(ael).je1("click",
								function (e, movie, surl, burl, w, h, tha) {
								    MagicMagnify.swap(e, movie, surl, burl, w, h, null, tha, "click")
								} .j16(null, movie, smallImageUrl, bigImageUrl, width, height, tha))
                            } else {
                                $mjs(ael).je1("mouseout",
								function (e, elm) {
								    elm.swapTimer = false
								} .j16(null, elm));
                                $mjs(ael).je1("mouseover",
								function (e, movie, surl, burl, w, h, elm, aTagOriginal, tha) {
								    elm.swapTimer = true;
								    MagicMagnify.swap.j24(null, e, movie, surl, burl, w, h, elm, tha, "mouseover").j27(aTagOriginal.params["thumb-change-delay"])
								} .j16(null, movie, smallImageUrl, bigImageUrl, width, height, elm, aTagOriginal, tha));
                                $mjs(ael).je1("click",
								function (e, movie, surl, burl, w, h, tha) {
								    $mjs(e).stop();
								    return false
								} .j16(null, movie, smallImageUrl, bigImageUrl, width, height, tha))
                            }
                        } .j24(null, elm, div, id, params, p.smallImageUrl, p.bigImageUrl, s_divElm, thumbContainer, aels[i], thumbA, aTagOriginal)
                    })
                }
            }
            if (!done) {
                movie = s_divElm.getElementsByTagName("OBJECT")[0];
                if (movie == undefined) {
                    movie = s_divElm.getElementsByTagName("EMBED")[0]
                }
                movie.parentNode.tha = tha
            }
            return
        },
        findSelectors4Thumb: function (id) {
            var aels = $J.$A(window.document.getElementsByTagName("A"));
            for (var i = 0, l = aels.length; i < l; i++) {
                if (aels[i].rel == id) {
                    aels[i].rel = "magnifier-id:" + id
                }
                var magnifier_id = this.getParam("(^|[ ;]{1,})magnifier-id(s+)?:(s+)?([^;$ ]{1,})", aels[i].rel, "");
                if (magnifier_id == id) {
                    if (aels[i].rel.indexOf("thumb-id:") == -1) {
                        aels[i].rel += ";thumb-id:" + id
                    }
                }
            }
            return
        },
        update: function (id, burl, surl) {
            var elm = new Image();
            elm.src = burl;
            burl = elm.src;
            var elm = new Image();
            elm.src = surl;
            surl = elm.src;
            $mjs(elm).j6({
                position: "absolute",
                top: "-50000px"
            });
            document.body.appendChild(elm);
            var pimg = mSWF2aTags[id].replace("mObj", "mImg");
            mSelectors[pimg][mSelectors[pimg].length] = {
                sm_img: surl,
                bg_img: burl
            };
            mSelectorsObj[mSelectorsObj.length] = id;
            var img = new MagicImage(elm, {
                onload: function (elm, pimg, surl, burl, id) {
                    var width = elm.j7().width;
                    var height = elm.j7().height;
                    var movie = MagicMagnify.getFlashMovieObject(pimg.replace("mImg", "mObj"));
                    if (movie.initAdditionalImage) {
                        movie.initAdditionalImage(surl, burl, width, height)
                    }
                    if ("MagicMagnify" == "MagicMagnifyPlus") {
                        $mjs("mt-" + id + "-0").href = burl;
                        MagicThumb.refresh("mt-" + id + "-0")
                    }
                    document.body.removeChild(elm)
                } .j24(null, elm, pimg, surl, burl, id)
            })
        },
        swap: function (e, movie, surl, burl, w, h, elm, tha, etype) {
            if (etype != "click" && !elm.swapTimer) {
                return
            }
            if (etype == "click") {
                $mjs(e).stop()
            }
            if (movie) {
                movie.parentNode.tha = tha;
                if (movie.changeImage) {
                    movie.changeImage(surl, burl, w, h)
                }
            }
            return false
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
            var flVer = this.getVer();
            var MagicThumbOnly = false;
            if ("MagicMagnify" == "MagicMagnifyPlus") {
                MagicThumb.options = $J.extend($J.detach(this.options), MagicThumb.options)
            }
            var aels = $J.$A(window.document.getElementsByTagName("A"));
            for (var i = 0, l = aels.length; i < l; i++) {
                if (/MagicMagnify/i.test(aels[i].className.toLowerCase()) && aels[i].rel != "stop") {
                    if (parseInt(flVer) < 7 && "MagicMagnify" == "MagicMagnifyPlus") {
                        aels[i].className = "MagicThumb";
                        MagicMagnify.findSelectors4Thumb(aels[i].id);
                        MagicThumbOnly = true;
                        MagicThumb.refresh(aels[i]);
                        continue
                    } else {
                        if (parseInt(flVer) < 7 && "MagicMagnify" == "MagicMagnify") {
                            break
                        }
                    }
                    var thumbA = aels[i].cloneNode(true);
                    while (aels[i].firstChild) {
                        if (aels[i].firstChild.tagName != "IMG") {
                            aels[i].removeChild(aels[i].firstChild)
                        } else {
                            break
                        }
                    }
                    if (arguments.length == 2 && !aels[i].clicked) {
                        continue
                    }
                    aels[i].params = MagicMagnify.createParamsList(aels[i]);
                    if (aels[i].firstChild.tagName != "IMG") {
                        throw "Invalid MagicMagnify invocation!"
                    }
                    aels[i].params["show-immediately"] = (false || aels[i].clicked || aels[i].params["show-immediately"]) ? 1 : 0;
                    if (aels[i].params["show-immediately"]) {
                        aels[i].params["show-immediately-x"] = aels[i].clickedX || aels[i].params["show-immediately-x"];
                        aels[i].params["show-immediately-y"] = aels[i].clickedY || aels[i].params["show-immediately-y"]
                    }
                    aels[i].clicked = false;
                    aels[i].clickedX = aels[i].clickedY = 0;
                    if ((aels[i].params["disable-auto-start"]) && arguments.length == 0) {
                        $mjs(aels[i]).je1("click",
						function (e) {
						    this.clicked = true;
						    $mjs(e).stop();
						    var imgElm;
						    for (var id in this.childNodes) {
						        if (this.childNodes[id].tagName == "IMG") {
						            imgElm = this.childNodes[id];
						            break
						        }
						    }
						    var rect = $mjs(imgElm).j9();
						    var ieBody = (document.compatMode && "backcompat" != document.compatMode.toLowerCase()) ? document.documentElement : document.body;
						    var eX = e.clientX + parseInt((self.pageXOffset) ? self.pageXOffset : ieBody.scrollLeft);
						    var eY = e.clientY + parseInt((self.pageYOffset) ? self.pageYOffset : ieBody.scrollTop);
						    this.clickedX = parseInt(eX - rect.left);
						    this.clickedY = parseInt(eY - rect.top);
						    MagicMagnify.start(true, true);
						    return false
						});
                        continue
                    }
                    var elm = aels[i].firstChild;
                    var img = new MagicImage(elm, {
                        onload: MagicMagnify.subInit.j24(null, aels[i], elm, thumbA)
                    })
                }
            }
            if (MagicThumbOnly) {
                MagicThumb.start()
            }
            window.onunload = function () {
                window.__flash__removeCallback = function (instance, name) {
                    return
                }
            }
        },
        subInit: function (ael, el, thumbA) {
            if (!ael.parentNode) {
                return
            }
            var width = el.j7().width;
            var height = el.j7().height;
            if (width == 0) {
                if (el.width) {
                    width = el.width
                }
                if (el.style.width) {
                    width = parseInt(el.style.width)
                }
            }
            if (height == 0) {
                if (el.height) {
                    height = el.height
                }
                if (el.style.height) {
                    height = parseInt(el.style.height)
                }
            }
            if (width == 0 || height == 0) {
                if ((el.j7().top == el.j7().bottom)) {
                    MagicMagnify.subInit.j24(null, ael, el, thumbA).j27(500)
                }
                return
            }
            var div = document.createElement("SPAN");
            div.className = "MagicMagnifyContainer";
            var iid;
            if (ael.id && !(/[^a-zA-Z0-9_]/i.test(ael.id)) && !(mIDSTags.hasOwnProperty(ael.id))) {
                iid = ael.id
            } else {
                iid = mtATagsNum.length
            }
            mIDSTags[iid] = iid;
            if (ael.id) {
                mSWF2aTags[ael.id] = "mObj" + iid
            }
            var params = {
                width: width,
                height: height,
                bigImageUrl: ael.href,
                smallImageUrl: ael.firstChild.src,
                name: "mObj" + iid,
                img_id: "mImg" + iid,
                rel: ael.rel,
                params: ael.params
            };
            div.id = "mObjCont" + iid;
            $mjs(div).j6({
                width: width + "px",
                height: height + "px",
                position: "relative",
                display: "block",
                overflow: "hidden",
                "z-index": "2"
            });
            var thumbContainer = document.createElement("SPAN");
            $mjs(thumbContainer).j6({
                width: "0px",
                height: "0px",
                position: "absolute",
                display: "block",
                overflow: "hidden",
                "z-index": "1"
            });
            thumbA.className = "MagicMagnify MagicThumb";
            thumbA.params = ael.params;
            thumbA.id = "mt-" + iid + "-0";
            if (ael.params["thumb-change"] == "mouseover") {
                thumbA.rel += ";swap-image:mouseover;swap-image-delay:" + ael.params["thumb-change-delay"]
            }
            thumbContainer.appendChild(thumbA);
            $mjs(thumbA).j6({
                width: width + "px",
                height: height + "px",
                display: "block",
                overflow: "hidden"
            });
            params.params["thumb-id"] = thumbA.id;
            var img = ael.cloneNode(true);
            $mjs(img).j6({
                position: "absolute",
                display: "block",
                top: "0px",
                left: "0px",
                "z-index": (document.location.protocol.indexOf("http") != -1) ? 300 : 200
            });
            img.className = "";
            img.id = "mImg" + iid;
            var loaderURL = (ael.params.loader == undefined) ? MagicMagnify.getBaseUrl() + "loader.gif" : ael.params.loader;
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
            img.appendChild(loader);
            $mjs(img).je1("click",
			function (e) {
			    $mjs(e).stop();
			    return false
			} .j16(img));
            var divElm = document.createElement("SPAN");
            $mjs(divElm).j6({
                display: "block",
                top: "0px",
                left: "0px",
                position: "absolute",
                width: width + "px",
                height: height + "px",
                "z-index": (document.location.protocol.indexOf("http") != -1) ? 200 : 300
            });
            div.appendChild(divElm);
            div.appendChild(img);
            div.appendChild(thumbContainer);
            var p = ael.parentNode;
            var id = ael.id;
            var bImgUrl = ael.href;
            var sImgUrl = ael.firstChild.src;
            mATags[div.id] = ael;
            mtATagsNum[mtATagsNum.length] = ael;
            p.replaceChild(div, ael);
            var str = '<SCRIPT event="FSCommand(command,args)" for="mObj' + iid + '">MagicMagnify_DoFSCommand(command, args);</SCRIPT>';
            MagicMagnifySwf(divElm, MagicMagnify.init(params) + str);
            eval("window.mObj" + iid + "_DoFSCommand = function(command,arguments){ MagicMagnify_DoFSCommand(command,arguments) };");
            mSelectors[params.img_id] = new Array();
            if (id != "") {
                MagicMagnify.findSelectors(div, id, params, bImgUrl, sImgUrl, divElm, thumbContainer, thumbA, ael)
            }
            if ("MagicMagnify" == "MagicMagnifyPlus") {
                MagicThumb.start(thumbA)
            }
            var webkit = navigator.userAgent.indexOf("AppleWebKit/") > -1;
            var gecko = navigator.userAgent.indexOf("Gecko") > -1 && navigator.userAgent.indexOf("KHTML") == -1;
            movie = divElm.getElementsByTagName("OBJECT")[0];
            if (movie == undefined) {
                movie = divElm.getElementsByTagName("EMBED")[0]
            }
            var wheeleffect = parseInt(new String(ael.params["wheel-effect"]).replace("%", ""));
            if (wheeleffect) {
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
            }
            $mjs(movie).je1("mouseout",
			function (e, m) {
			    if (m && m.getMouseOut) {
			        try {
			            m.getMouseOut()
			        } catch (x) { }
			    }
			    $mjs(e).stop()
			} .j16(div, movie));
            $mjs(movie).je1("mouseover",
			function (e, m) {
			    if (m && m.getMouseOver) {
			        var c = $mjs(e).j15();
			        var c1 = $mjs(m).j8();
			        try {
			            m.getMouseOver(parseInt(c.x - c1.left), parseInt((c.y - c1.top)))
			        } catch (x) { }
			    }
			    $mjs(e).stop()
			} .j16(div, movie))
        }
    };
    return MagicMagnify
})(magicJS);
function MagicMagnify_DoFSCommand(command, arguments) {
    if ("MagicMagnify_removeImg" == command) {
        MagicMagnify.removeImg(arguments, 100)
    }
    if ("MagicMagnify_setThumbnail" == command) {
        MagicMagnify.setThumbnail(arguments, arguments[1])
    }
    if ("MagicMagnify" == "MagicMagnifyPlus") {
        if ("MagicMagnify_expandImage" == command) {
            MagicThumb.expand(arguments)
        }
    }
    if ("MagicMagnify_changeThumbnail" == command) {
        MagicMagnify.changeThumbnail(arguments, arguments[1])
    }
    if ("MagicMagnify_callback" == command) {
        eval(arguments, arguments[1])
    }
}
$mjs(document).je1("domready",
function () {
    MagicMagnify.start()
});
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
window.MagicMagnifySwf = function (o, s) { o.innerHTML = s; };
