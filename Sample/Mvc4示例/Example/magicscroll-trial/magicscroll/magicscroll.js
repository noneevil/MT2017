/*


   Magic Scroll v1.0.22 DEMO
   Copyright 2012 Magic Toolbox
   Buy a license: www.magictoolbox.com/magicscroll/
   License agreement: http://www.magictoolbox.com/license/


*/
(function () {
    if (window.magicJS) {
        return
    }
    var b = {
        version: "v2.7.0",
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
        extend: function (m, j) {
            if (!(m instanceof window.Array)) {
                m = [m]
            }
            for (var h = 0, e = m.length; h < e; h++) {
                if (!a.defined(m)) {
                    continue
                }
                for (var g in (j || {})) {
                    try {
                        m[h][g] = j[g]
                    } catch (d) { }
                }
            }
            return m[0]
        },
        implement: function (j, h) {
            if (!(j instanceof window.Array)) {
                j = [j]
            }
            for (var g = 0, d = j.length; g < d; g++) {
                if (!a.defined(j[g])) {
                    continue
                }
                if (!j[g].prototype) {
                    continue
                }
                for (var e in (h || {})) {
                    if (!j[g].prototype[e]) {
                        j[g].prototype[e] = h[e]
                    }
                }
            }
            return j[0]
        },
        nativize: function (g, e) {
            if (!a.defined(g)) {
                return g
            }
            for (var d in (e || {})) {
                if (!g[d]) {
                    g[d] = e[d]
                }
            }
            return g
        },
        $try: function () {
            for (var g = 0, d = arguments.length; g < d; g++) {
                try {
                    return arguments[g]()
                } catch (h) { }
            }
            return null
        },
        $A: function (g) {
            if (!a.defined(g)) {
                return $mjs([])
            }
            if (g.toArray) {
                return $mjs(g.toArray())
            }
            if (g.item) {
                var e = g.length || 0,
				d = new Array(e);
                while (e--) {
                    d[e] = g[e]
                }
                return $mjs(d)
            }
            return $mjs(Array.prototype.slice.call(g))
        },
        now: function () {
            return new Date().getTime()
        },
        detach: function (j) {
            var g;
            switch (a.j1(j)) {
                case "object":
                    g = {};
                    for (var h in j) {
                        g[h] = a.detach(j[h])
                    }
                    break;
                case "array":
                    g = [];
                    for (var e = 0, d = j.length; e < d; e++) {
                        g[e] = a.detach(j[e])
                    }
                    break;
                default:
                    return j
            }
            return a.$(g)
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
        $new: function (d, g, e) {
            return $mjs(a.doc.createElement(d)).setProps(g || {}).j6(e || {})
        },
        addCSS: function (e) {
            if (document.styleSheets && document.styleSheets.length) {
                document.styleSheets[0].insertRule(e, 0)
            } else {
                var d = $mjs(document.createElement("style"));
                d.update(e);
                document.getElementsByTagName("head")[0].appendChild(d)
            }
        }
    };
    var a = b;
    window.magicJS = b;
    window.$mjs = b.$;
    a.Array = {
        $J_TYPE: "array",
        indexOf: function (h, j) {
            var d = this.length;
            for (var e = this.length, g = (j < 0) ? Math.max(0, e + j) : j || 0; g < e; g++) {
                if (this[g] === h) {
                    return g
                }
            }
            return -1
        },
        contains: function (d, e) {
            return this.indexOf(d, e) != -1
        },
        forEach: function (d, h) {
            for (var g = 0, e = this.length; g < e; g++) {
                if (g in this) {
                    d.call(h, this[g], g, this)
                }
            }
        },
        filter: function (d, m) {
            var j = [];
            for (var h = 0, e = this.length; h < e; h++) {
                if (h in this) {
                    var g = this[h];
                    if (d.call(m, this[h], h, this)) {
                        j.push(g)
                    }
                }
            }
            return j
        },
        map: function (d, j) {
            var h = [];
            for (var g = 0, e = this.length; g < e; g++) {
                if (g in this) {
                    h[g] = d.call(j, this[g], g, this)
                }
            }
            return h
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
			g = e.shift();
            return function () {
                return d.apply(g || null, e.concat(a.$A(arguments)))
            }
        },
        j16: function () {
            var e = a.$A(arguments),
			d = this,
			g = e.shift();
            return function (h) {
                return d.apply(g || null, $mjs([h || window.event]).concat(e))
            }
        },
        j27: function () {
            var e = a.$A(arguments),
			d = this,
			g = e.shift();
            return window.setTimeout(function () {
                return d.apply(d, e)
            },
			g || 0)
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
			g = e.shift();
            return window.setInterval(function () {
                return d.apply(d, e)
            },
			g || 0)
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
            return "ontouchstart" in window || (window.DocumentTouch && document instanceof DocumentTouch)
        } (),
        mobile: c.match(/android.+mobile|tablet|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|symbian|treo|up\.(j21|link)|vodafone|wap|windows (ce|phone)|xda|xiino/) ? true : false,
        engine: (window.opera) ? "presto" : !!(window.ActiveXObject) ? "trident" : (undefined != document.getBoxObjectFor || null != window.mozInnerScreenY) ? "gecko" : (null != window.WebKitPoint || !navigator.taintEnabled) ? "webkit" : "unknown",
        version: "",
        ieMode: 0,
        platform: c.match(/ip(?:ad|od|hone)/) ? "ios" : (c.match(/(?:webos|android)/) || navigator.platform.match(/mac|win|linux/i) || ["other"])[0].toLowerCase(),
        backCompat: document.compatMode && "backcompat" == document.compatMode.toLowerCase(),
        getDoc: function () {
            return (document.compatMode && "backcompat" == document.compatMode.toLowerCase()) ? document.body : document.documentElement
        },
        requestAnimationFrame: window.requestAnimationFrame || window.mozRequestAnimationFrame || window.webkitRequestAnimationFrame || window.oRequestAnimationFrame || window.msRequestAnimationFrame || undefined,
        cancelAnimationFrame: window.cancelAnimationFrame || window.mozCancelAnimationFrame || window.mozCancelAnimationFrame || window.oCancelAnimationFrame || window.msCancelAnimationFrame || window.webkitCancelRequestAnimationFrame || undefined,
        ready: false,
        onready: function () {
            if (a.j21.ready) {
                return
            }
            a.j21.ready = true;
            a.body = $mjs(document.body);
            a.win = $mjs(window); (function () {
                a.j21.css3Transformations = {
                    capable: false,
                    prefix: ""
                };
                if (typeof document.body.style.transform !== "undefined") {
                    a.j21.css3Transformations.capable = true
                } else {
                    var g = "Webkit Moz O ms Khtml".split(" ");
                    for (var e = 0, d = g.length; e < d; e++) {
                        a.j21.css3Transformations.prefix = g[e];
                        if (typeof document.body.style[a.j21.css3Transformations.prefix + "Transform"] !== "undefined") {
                            a.j21.css3Transformations.capable = true;
                            break
                        }
                    }
                }
            })(); (function () {
                a.j21.css3Animation = {
                    capable: false,
                    prefix: ""
                };
                if (typeof document.body.style.animationName !== "undefined") {
                    a.j21.css3Animation.capable = true
                } else {
                    var g = "Webkit Moz O ms Khtml".split(" ");
                    for (var e = 0, d = g.length; e < d; e++) {
                        a.j21.css3Animation.prefix = g[e];
                        if (typeof document.body.style[a.j21.css3Animation.prefix + "AnimationName"] !== "undefined") {
                            a.j21.css3Animation.capable = true;
                            break
                        }
                    }
                }
            })();
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
            if (a.j21.backCompat) {
                return 5
            }
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
    })(); (function () {
        a.j21.fullScreen = {
            capable: false,
            enabled: function () {
                return false
            },
            request: function () { },
            cancel: function () { },
            changeEventName: "",
            errorEventName: "",
            prefix: ""
        };
        if (typeof document.cancelFullScreen != "undefined") {
            a.j21.fullScreen.capable = true
        } else {
            var g = "webkit moz o ms khtml".split(" ");
            for (var e = 0, d = g.length; e < d; e++) {
                a.j21.fullScreen.prefix = g[e];
                if (typeof document[a.j21.fullScreen.prefix + "CancelFullScreen"] != "undefined") {
                    a.j21.fullScreen.capable = true;
                    break
                }
            }
        }
        if (a.j21.fullScreen.capable) {
            a.j21.fullScreen.changeEventName = a.j21.fullScreen.prefix + "fullscreenchange";
            a.j21.fullScreen.errorEventName = a.j21.fullScreen.prefix + "fullscreenerror";
            a.j21.fullScreen.enabled = function () {
                switch (this.prefix) {
                    case "":
                        return document.fullScreen;
                    case "webkit":
                        return document.webkitIsFullScreen;
                    default:
                        return document[this.prefix + "FullScreen"]
                }
            };
            a.j21.fullScreen.request = function (h) {
                return (this.prefix === "") ? h.requestFullScreen() : h[this.prefix + "RequestFullScreen"]()
            };
            a.j21.fullScreen.cancel = function (h) {
                return (this.prefix === "") ? document.cancelFullScreen() : document[this.prefix + "CancelFullScreen"]()
            }
        }
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
        j5: function (g) {
            g = (g == "float" && this.currentStyle) ? "styleFloat" : g.j22();
            var d = null,
			e = null;
            if (this.currentStyle) {
                d = this.currentStyle[g]
            } else {
                if (document.defaultView && document.defaultView.getComputedStyle) {
                    e = document.defaultView.getComputedStyle(this, null);
                    d = e ? e.getPropertyValue([g.dashize()]) : null
                }
            }
            if (!d) {
                d = this.style[g]
            }
            if ("opacity" == g) {
                return a.defined(d) ? parseFloat(d) : 1
            }
            if (/^(border(Top|Bottom|Left|Right)Width)|((padding|margin)(Top|Bottom|Left|Right))$/.test(g)) {
                d = parseInt(d) ? d : "0px"
            }
            return ("auto" == d ? null : d)
        },
        j6Prop: function (g, d) {
            try {
                if ("opacity" == g) {
                    this.j23(d);
                    return this
                } else {
                    if ("float" == g) {
                        this.style[("undefined" === typeof (this.style.styleFloat)) ? "cssFloat" : "styleFloat"] = d;
                        return this
                    } else {
                        if (a.j21.css3Transformations && /transform/.test(g)) { }
                    }
                }
                this.style[g.j22()] = d + (("number" == a.j1(d) && !$mjs(["zIndex", "zoom"]).contains(g.j22())) ? "px" : "")
            } catch (h) { }
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
				g = $mjs(document).j10(),
				i = a.j21.getDoc();
                return {
                    top: d.top + g.y - i.clientTop,
                    left: d.left + g.x - i.clientLeft
                }
            }
            var h = this,
			e = t = 0;
            do {
                e += h.offsetLeft || 0;
                t += h.offsetTop || 0;
                h = h.offsetParent
            }
            while (h && !(/^(?:body|html)$/i).test(h.tagName));
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
        changeContent: function (g) {
            try {
                this.innerHTML = g
            } catch (d) {
                this.innerText = g
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
        j32: function (g, e) {
            var d = $mjs(g).append(this, e);
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
        j29: function (h, e) {
            var d = a.getStorage(this.$J_UUID),
			g = d[h];
            if (undefined != e && undefined == g) {
                g = d[h] = e
            }
            return (a.defined(g) ? g : null)
        },
        j30: function (g, e) {
            var d = a.getStorage(this.$J_UUID);
            d[g] = e;
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
                return a.$A(this.getElementsByTagName("*")).filter(function (h) {
                    try {
                        return (1 == h.nodeType && h.className.has(d, " "))
                    } catch (g) { }
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
    if (a.j21.fullScreen.capable) {
        a.Element.requestFullScreen = function () {
            a.j21.fullScreen.request(this)
        }
    }
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
        je1: function (h, g) {
            var j = ("domready" == h) ? false : true,
			e = this.j29("events", {});
            e[h] = e[h] || {};
            if (e[h].hasOwnProperty(g.$J_EUID)) {
                return this
            }
            if (!g.$J_EUID) {
                g.$J_EUID = Math.floor(Math.random() * a.now())
            }
            var d = this,
			i = function (l) {
			    return g.call(d)
			};
            if ("domready" == h) {
                if (a.j21.ready) {
                    g.call(this);
                    return this
                }
            }
            if (j) {
                i = function (l) {
                    l = a.extend(l || window.e, {
                        $J_TYPE: "event"
                    });
                    return g.call(d, $mjs(l))
                };
                this[a._event_add_](a._event_prefix_ + h, i, false)
            }
            e[h][g.$J_EUID] = i;
            return this
        },
        je2: function (h) {
            var j = ("domready" == h) ? false : true,
			e = this.j29("events");
            if (!e || !e[h]) {
                return this
            }
            var i = e[h],
			g = arguments[1] || null;
            if (h && !g) {
                for (var d in i) {
                    if (!i.hasOwnProperty(d)) {
                        continue
                    }
                    this.je2(h, d)
                }
                return this
            }
            g = ("function" == a.j1(g)) ? g.$J_EUID : g;
            if (!i.hasOwnProperty(g)) {
                return this
            }
            if ("domready" == h) {
                j = false
            }
            if (j) {
                this[a._event_del_](a._event_prefix_ + h, i[g], false)
            }
            delete i[g];
            return this
        },
        raiseEvent: function (i, g) {
            var n = ("domready" == i) ? false : true,
			m = this,
			l;
            if (!n) {
                var h = this.j29("events");
                if (!h || !h[i]) {
                    return this
                }
                var j = h[i];
                for (var d in j) {
                    if (!j.hasOwnProperty(d)) {
                        continue
                    }
                    j[d].call(this)
                }
                return this
            }
            if (m === document && document.createEvent && !m.dispatchEvent) {
                m = document.documentElement
            }
            if (document.createEvent) {
                l = document.createEvent(i);
                l.initEvent(g, true, true)
            } else {
                l = document.createEventObject();
                l.eventType = i
            }
            if (document.createEvent) {
                m.dispatchEvent(l)
            } else {
                m.fireEvent("on" + g, l)
            }
            return l
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
        if ("complete" === document.readyState) {
            return a.j21.onready.j27(1)
        }
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
        var i = null,
		e = a.$A(arguments);
        if ("class" == a.j1(e[0])) {
            i = e.shift()
        }
        var d = function () {
            for (var n in this) {
                this[n] = a.detach(this[n])
            }
            if (this.constructor.$parent) {
                this.$parent = {};
                var q = this.constructor.$parent;
                for (var o in q) {
                    var l = q[o];
                    switch (a.j1(l)) {
                        case "function":
                            this.$parent[o] = a.Class.wrap(this, l);
                            break;
                        case "object":
                            this.$parent[o] = a.detach(l);
                            break;
                        case "array":
                            this.$parent[o] = a.detach(l);
                            break
                    }
                }
            }
            var j = (this.init) ? this.init.apply(this, arguments) : this;
            delete this.caller;
            return j
        };
        if (!d.prototype.init) {
            d.prototype.init = a.$F
        }
        if (i) {
            var h = function () { };
            h.prototype = i.prototype;
            d.prototype = new h;
            d.$parent = {};
            for (var g in i.prototype) {
                d.$parent[g] = i.prototype[g]
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
            var h = this.caller;
            var g = e.apply(d, arguments);
            return g
        }
    };
    a.win = $mjs(window);
    a.doc = $mjs(document)
})(); (function (b) {
    if (!b) {
        throw "MagicJS not found";
        return
    }
    if (b.FX) {
        return
    }
    var a = b.$;
    b.FX = new b.Class({
        options: {
            fps: 60,
            duration: 500,
            transition: function (c) {
                return -(Math.cos(Math.PI * c) - 1) / 2
            },
            onStart: b.$F,
            onComplete: b.$F,
            onBeforeRender: b.$F,
            forceAnimation: false,
            roundCss: true
        },
        styles: null,
        init: function (d, c) {
            this.el = a(d);
            this.options = b.extend(this.options, c);
            this.timer = false
        },
        start: function (c) {
            this.styles = c;
            this.state = 0;
            this.curFrame = 0;
            this.startTime = b.now();
            this.finishTime = this.startTime + this.options.duration;
            this.loopBind = this.loop.j24(this);
            this.options.onStart.call();
            if (!this.options.forceAnimation && b.j21.requestAnimationFrame) {
                this.timer = b.j21.requestAnimationFrame.call(window, this.loopBind)
            } else {
                this.timer = this.loop.j24(this).interval(Math.round(1000 / this.options.fps))
            }
            return this
        },
        stopAnimation: function () {
            if (this.timer) {
                if (!this.options.forceAnimation && b.j21.requestAnimationFrame && b.j21.cancelAnimationFrame) {
                    b.j21.cancelAnimationFrame.call(window, this.timer)
                } else {
                    clearInterval(this.timer)
                }
                this.timer = false
            }
        },
        stop: function (c) {
            c = b.defined(c) ? c : false;
            this.stopAnimation();
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
            var d = b.now();
            if (d >= this.finishTime) {
                this.stopAnimation();
                this.render(1);
                this.options.onComplete.j27(10);
                return this
            }
            var c = this.options.transition((d - this.startTime) / this.options.duration);
            if (!this.options.forceAnimation && b.j21.requestAnimationFrame) {
                this.timer = b.j21.requestAnimationFrame.call(window, this.loopBind)
            }
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
    b.FX.Transition = {
        linear: function (c) {
            return c
        },
        sineIn: function (c) {
            return -(Math.cos(Math.PI * c) - 1) / 2
        },
        sineOut: function (c) {
            return 1 - b.FX.Transition.sineIn(1 - c)
        },
        expoIn: function (c) {
            return Math.pow(2, 8 * (c - 1))
        },
        expoOut: function (c) {
            return 1 - b.FX.Transition.expoIn(1 - c)
        },
        quadIn: function (c) {
            return Math.pow(c, 2)
        },
        quadOut: function (c) {
            return 1 - b.FX.Transition.quadIn(1 - c)
        },
        cubicIn: function (c) {
            return Math.pow(c, 3)
        },
        cubicOut: function (c) {
            return 1 - b.FX.Transition.cubicIn(1 - c)
        },
        backIn: function (d, c) {
            c = c || 1.618;
            return Math.pow(d, 2) * ((c + 1) * d - c)
        },
        backOut: function (d, c) {
            return 1 - b.FX.Transition.backIn(1 - d)
        },
        elasticIn: function (d, c) {
            c = c || [];
            return Math.pow(2, 10 * --d) * Math.cos(20 * d * Math.PI * (c[0] || 1) / 3)
        },
        elasticOut: function (d, c) {
            return 1 - b.FX.Transition.elasticIn(1 - d, c)
        },
        bounceIn: function (e) {
            for (var d = 0, c = 1; 1; d += c, c /= 2) {
                if (e >= (7 - 4 * d) / 11) {
                    return c * c - Math.pow((11 - 6 * d - 11 * e) / 4, 2)
                }
            }
        },
        bounceOut: function (c) {
            return 1 - b.FX.Transition.bounceIn(1 - c)
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
        start: function (e, i) {
            this[i || this.options.mode]();
            var h = this.el.j5(this.margin).j17(),
			g = this.wrapper.j5(this.layout).j17(),
			c = {},
			j = {},
			d;
            c[this.margin] = [h, 0],
			c[this.layout] = [0, this.offset],
			j[this.margin] = [h, -this.offset],
			j[this.layout] = [g, 0];
            switch (e) {
                case "in":
                    d = c;
                    break;
                case "out":
                    d = j;
                    break;
                case "toggle":
                    d = (0 == g) ? c : j;
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
})(magicJS); (function (b) {
    if (!b) {
        throw "MagicJS not found";
        return
    }
    if (b.PFX) {
        return
    }
    var a = b.$;
    b.PFX = new b.Class(b.FX, {
        init: function (c, d) {
            this.el_arr = c;
            this.options = b.extend(this.options, d);
            this.timer = false
        },
        start: function (c) {
            this.$parent.start([]);
            this.styles_arr = c;
            return this
        },
        render: function (c) {
            for (var d = 0; d < this.el_arr.length; d++) {
                this.el = a(this.el_arr[d]);
                this.styles = this.styles_arr[d];
                this.$parent.render(c)
            }
        }
    })
})(magicJS); (function (a) {
    if (!a) {
        throw "MagicJS not found";
        return
    }
    if (a.Tooltip) {
        return
    }
    var b = a.$;
    a.Tooltip = function (d, e) {
        var c = this.tooltip = a.$new("div", null, {
            position: "absolute",
            "z-index": 999
        }).j2("MagicToolboxTooltip");
        a.$(d).je1("mouseover",
		function () {
		    c.j32(document.body)
		});
        a.$(d).je1("mouseout",
		function () {
		    c.j33()
		});
        a.$(d).je1("mousemove",
		function (l) {
		    var n = 20,
			j = a.$(l).j15(),
			i = c.j7(),
			h = a.$(window).j7(),
			m = a.$(window).j10();
		    function g(r, o, q) {
		        return (q < (r - o) / 2) ? q : ((q > (r + o) / 2) ? (q - o) : (r - o) / 2)
		    }
		    c.j6({
		        left: m.x + g(h.width, i.width + 2 * n, j.x - m.x) + n,
		        top: m.y + g(h.height, i.height + 2 * n, j.y - m.y) + n
		    })
		});
        this.text(e)
    };
    a.Tooltip.prototype.text = function (c) {
        this.tooltip.firstChild && this.tooltip.removeChild(this.tooltip.firstChild);
        this.tooltip.append(document.createTextNode(c))
    }
})(magicJS);
var MagicScroll = (function (d) {
    var h = d.$;
    var b = d.Class({
        _options: {
            modules: [],
            effects: [],
            attributes: {},
            styles: {},
            options: {},
            alias: {},
            auto: false,
            defaults: {
                name: "MagicSwap",
                "class": "MagicSwap",
                width: 400,
                height: 400,
                sizing: "core",
                wrapper: true,
                duration: 1000,
                transition: d.FX.Transition.sineIn,
                fps: 500,
                "item-width": "stretch",
                "item-height": "stretch",
                "item-align": "center",
                "item-valign": "middle",
                "z-index": 0,
                effect: "none",
                "-step": 1
            }
        },
        indoc: false,
        preInit: function () {
            this._options.modules = b.mw3.getAll();
            this._options.effects = b.mw1.getAll()
        },
        init: function (i) {
            this.preInit();
            this._options.auto = true;
            this.create(i)
        },
        create: function (i) {
            this._options = d.extend(this._options, i || {});
            this.options(this._options.defaults, true);
            this._tmp = d.$new("DIV", null, {
                position: "absolute",
                top: -9999,
                left: 0,
                overflow: "hidden",
                width: 1,
                height: 1
            });
            this.core = d.$new("DIV", d.extend({
                "class": this.option("class")
            },
			this._options.attributes), d.extend(this._options.styles, {
			    position: "relative",
			    "text-align": "left"
			})).j32(this._tmp).j2(this.option("name"));
            this.items = d.$A([]);
            d.extend([this.items], b.Items);
            this.position = 0;
            this.effect = new b.mw1(this, this._options.effects);
            this.modules = new b.mw3(this, this._options.modules)
        },
        disable: function (i) {
            this._options.modules = $mjs(this._options.modules).filter(function (j) {
                return j != this
            },
			i)
        },
        item: function (i) {
            return this.items[i]
        },
        option: function (j, l) {
            var i = d.defined(this._options.alias[j]) ? this.option(this._options.alias[j], l) : d.defined(this._options.options[j]) ? this._options.options[j] : l,
			m = {
			    "false": false,
			    "true": true
			};
            return d.defined(m[i]) ? m[i] : i
        },
        options: function (i, j) {
            if (j) {
                d.extend(i, this._options.options)
            }
            d.extend(this._options.options, i);
            return this
        },
        name: function (i) {
            return (this.option("name") + "-" + i).j22()
        },
        push: function () {
            var i = d.j1(arguments[0]),
			j = null;
            switch (i) {
                case "array":
                case "collection":
                    d.$A(arguments[0]).j14(function (l) {
                        if (d.j1(l) == "array") {
                            this.push(l[0], l[1], l[2])
                        } else {
                            this.push(l)
                        }
                    },
				this);
                    break;
                case "string":
                    j = d.$new(arguments[0], arguments[1] || {},
				arguments[2] || {});
                case "element":
                    j = j || $mjs(arguments[0]);
                    d.extend([j], b.Item);
                    j.j30("MagicSwap", this);
                    j.j30("index", this.items.length);
                    j.reset();
                    this.items.push(j);
                    this.callEvent("push", {
                        item: j
                    });
                    break;
                default:
                    break
            }
            return this
        },
        append: function (i) {
            $mjs(i).appendChild(this._tmp);
            this.indoc = true;
            if (this._options.auto) {
                this.reload()
            }
            return this
        },
        replace: function (i) {
            $mjs(i).parentNode.replaceChild(this._tmp, $mjs(i));
            this.indoc = true;
            if (this._options.auto) {
                this.reload()
            }
            return this
        },
        show: function () {
            this.fixSize(this.option("width"), this.option("height"));
            this._tmp.parentNode.replaceChild(this.core, this._tmp);
            this.callEvent("after-reload");
            this.jump("first");
            return this
        },
        hide: function () {
            this._tmp.enclose(this.core);
            return this
        },
        reload: function () {
            if (!this.indoc) {
                throw "Magic Swap: ERROR: Try to create objects before append core object in the document"
            }
            this.core.show();
            if (this.option("wrapper")) {
                this.wrapper = this.core.appendChild(d.$new("DIV", {
                    "class": this.name("Container")
                },
				{
				    position: "absolute",
				    overflow: "hidden"
				}).setSize(this.core.getBoxSize()))
            } else {
                this.wrapper = this.core
            }
            this.items.append(this.wrapper);
            if (this._options.auto) {
                this.show()
            }
            return this
        },
        fixSize: function (j, i) {
            d.j1(j) == "object" || (j = {
                width: j,
                height: i
            });
            this.core.setBoxSize(j, null, true);
            this.option("wrapper") && this.wrapper.setSize(this.core.getBoxSize())
        },
        jump: function (i, j) {
            j = d.extend({
                target: "forward",
                effect: this.option("effect")
            },
			d.extend(j || {},
			d.j1(i) ? (d.j1(i) == "object" ? i : {
			    target: i
			}) : {}));
            this.position = this.effect.jump(j);
            return this
        },
        size: function (i, l, j) {
            if (d.j1(i) == "string") {
                i.split("px").length > 1 && (i = parseInt(i.split("px")[0]));
                i.split("%").length > 1 && (i = parseInt(Math.round(l * i.split("%")[0] / 100)))
            }
            return j ? i : parseInt(i)
        },
        sizeup: function (i, m, j) {
            var l;
            if (d.j1(i) == "string") {
                i.split("px").length > 1 && (i = parseInt(i.split("px")[0]));
                i.split("%").length > 1 && (l = i.split("%")[0]) && (i = parseInt(Math.round(m * l / (100 - l))))
            }
            return j ? i : parseInt(i)
        },
        dispose: function (i) {
            i && this.indoc && this.core.parentNode.replaceChild(i, this.core);
            return i || null
        }
    });
    b.Item = {
        fixSize: function (l) {
            var j = this.j29("MagicSwap");
            this.j29("initsize") && this.setSize(this.j29("initsize")) || this.j30("initsize", this.j7());
            var o = this.j7();
            var n = j.wrapper.getBoxSize();
            var i = l && l.width || j.option("item-width");
            var m = l && l.height || j.option("item-height");
            if (i == "stretch" && m == "stretch") {
                i = n.width;
                m = o.height * i / o.width;
                if (m > n.height) {
                    m = n.height;
                    i = o.width * m / o.height
                }
            } else {
                i = i == "auto" ? n.width : i == "original" ? o.width : i;
                m = m == "auto" ? n.height : m == "original" ? o.height : m;
                i = i == "stretch" ? o.width * m / o.height : i;
                m = m == "stretch" ? o.height * i / o.width : m
            }
            if (d.defined(l) && d.j1(l) != "object") {
                i = j.size(l || l, i, true);
                m = j.size(l || l, m, true)
            }
            i = Math.round(i);
            m = Math.round(m);
            this.setSize(i, m);
            $mjs(b.Item).callEvent("item-resize", {
                item: this,
                width: i,
                height: m
            });
            return this
        },
        fixPosition: function (o, r) {
            var j = this.j29("MagicSwap");
            o = o || {
                width: "block",
                height: "block"
            };
            o = d.j1(o) == "string" ? {
                width: o,
                height: o
            } : o;
            r = r || j.wrapper.getBoxSize();
            if (o.width == "inline") {
                this.j6({
                    "margin-left": "auto",
                    "margin-right": "auto"
                })
            } else {
                var i = parseInt(this.j7().width);
                var m = r.width - i;
                var n = j.option("item-align");
                this.j6({
                    "margin-left": n == "center" ? m / 2 : n == "left" ? 0 : m,
                    "margin-right": n == "center" ? m / 2 : n == "right" ? 0 : m
                })
            }
            if (o.height == "inline") {
                this.j6({
                    "margin-top": "auto",
                    "margin-bottom": "auto"
                })
            } else {
                var q = parseInt(this.j7().height);
                var l = r.height - q;
                var p = j.option("item-valign");
                this.j6({
                    "margin-top": p == "middle" ? l / 2 : p == "top" ? 0 : l,
                    "margin-bottom": p == "middle" ? l / 2 : p == "bottom" ? 0 : l
                })
            }
        },
        copy: function () {
            var i = this.clone();
            d.extend(i, b.Item);
            return i
        },
        index: function () {
            return this.j29("index")
        },
        reset: function () {
            this.j6({
                visibility: "visible",
                display: "block",
                position: "relative",
                opacity: 1,
                top: "auto",
                left: "auto",
                "float": "none"
            });
            if (d.j21.trident) {
                this.j6({
                    overflow: "hidden"
                })
            }
        }
    };
    b.Items = {
        append: function (i) {
            d.$A(this).j14(function (j) {
                if (d.j1(j) == "element") {
                    this.appendChild(j)
                }
            },
			i)
        },
        j6: function (j, i) {
            d.$A(this).j14(function (l) {
                if (d.j1(l) == "element") {
                    if (d.j1(this) == "function") {
                        $mjs(l).j6(this.apply(l, i || []))
                    } else {
                        $mjs(l).j6(this)
                    }
                }
            },
			j)
        },
        fixPosition: function (j, i) {
            d.$A(this).j14(function (l) {
                if (d.j1(l) == "element") {
                    $mjs(l).fixPosition(this[0], this[1])
                }
            },
			[j, i])
        },
        fixSize: function (i) {
            d.$A(this).j14(function (j) {
                if (d.j1(j) == "element") {
                    $mjs(j).fixSize(this[0])
                }
            },
			[i])
        },
        fix: function () {
            this.fixSize();
            this.fixPosition()
        },
        reset: function () {
            d.$A(this).j14(function (i) {
                i.reset()
            })
        }
    };
    b.mw1 = d.Class({
        init: function (i, j) {
            this.core = i;
            this.effects = j;
            this.last = null;
            this.classes = {}
        },
        jump: function (i) {
            if (!$mjs(this.effects).contains(i.effect) || !d.defined(b.mw2[("-" + i.effect).j22()])) {
                i.effect = "none"
            }
            if (!this.classes[i.effect]) {
                this.classes[i.effect] = new b.mw2[("-" + i.effect).j22()](this.core)
            }
            this.stop();
            if (!this.last || this.classes[this.last].type != this.classes[i.effect].type) {
                this.last && this.core.items.reset();
                this.classes[i.effect].prepare()
            }
            this.last = i.effect;
            return this.classes[i.effect].jump_(i)
        },
        stop: function () {
            this.last && this.classes[this.last].stop()
        }
    });
    b.mw2 = d.Class({
        type: "absolute",
        order: false,
        defaults: {},
        load: d.$F,
        prepare: d.$F,
        restore: d.$F,
        stop: d.$F,
        init: function (i) {
            this.core = i;
            this.options({
                duration: this.core.option("duration"),
                transition: this.core.option("transition"),
                fps: this.core.option("fps")
            });
            this.options(this.defaults);
            this.load()
        },
        option: function (i, j) {
            return this.core.option("effect-" + this.name + "-" + i, j || null)
        },
        options: function (j) {
            var l = {};
            for (var i in j) {
                l["effect-" + this.name + "-" + i] = j[i]
            }
            return this.core.options(l, true)
        },
        position: function () {
            return this.core.position
        },
        jump_: function (i) {
            i.target = this.target(i.target, this.position(), this.core.option("-step"), this.core.items.length);
            if (this.order) {
                i.direction = this.direction(i.target, this.position(), this.core.items.length, i.direction || "auto")
            }
            if (this.order || this.position() != i.target) {
                this.jump(d.extend({},
				i))
            }
            return i.target
        },
        target: function (m, i, l, j) {
            if (!d.defined(m)) {
                m = "forward"
            }
            if (d.j1(m) == "string") {
                if (isNaN(parseInt(m))) {
                    switch (m) {
                        case "end":
                        case "last":
                            m = j - l;
                            break;
                        case "start":
                        case "first":
                            m = 0;
                            break;
                        case "backward":
                            m = i - l;
                            break;
                        case "forward":
                        default:
                            m = i + l;
                            break
                    }
                } else {
                    m = i + parseInt(m)
                }
            }
            m = m % j;
            m < 0 && (m += j);
            return m
        },
        direction: function (p, i, m, o) {
            if (this.option("loop") != "continue") {
                o = p <= i ? "backward" : "forward"
            } else {
                if (!o || o == "auto") {
                    var q = p - i,
					n = Math.abs(q),
					j = m / 2;
                    if (n <= j && q > 0 || n > j && q < 0) {
                        o = "forward"
                    } else {
                        o = "backward"
                    }
                }
            }
            return o
        }
    });
    d.extend(b.mw1, {
        getAll: function () {
            var i = [];
            for (effect in b.mw2) {
                if (d.j1(b.mw2[effect]) == "class" && !b.mw2[effect].prototype.hidden) {
                    i.push(effect.toLowerCase())
                }
            }
            return i
        }
    });
    b.mw3 = d.Class({
        init: function (i, j) {
            this.core = i;
            this.modules = j;
            this.classes = {};
            $mjs(i).bindEvent("after-reload", this.load.j24(this))
        },
        load: function () {
            d.$A(this.modules).j14(function (i) {
                if (d.defined(b.mw4[("-" + i).j22()])) {
                    this.classes[i] = new b.mw4[("-" + i).j22()](this.core)
                }
            },
			this)
        }
    });
    b.mw4 = d.Class({
        defaults: {},
        hidden: false,
        init: function (i) {
            this.core = i;
            this.options(this.defaults);
            this.load()
        },
        option: function (i, j) {
            return this.core.option("module-" + this.name + "-" + i, j || null)
        },
        options: function (j) {
            var l = {};
            for (var i in j) {
                l["module-" + this.name + "-" + i] = j[i]
            }
            return this.core.options(l, true)
        },
        append: function (m, q, l, o) {
            var j = this.h = $mjs(["top", "bottom"]).contains(q);
            var i = this.core.wrapper.getBoxSize();
            this.core.wrapper.setSize(j ? null : i.width - l, j ? i.height - l : null);
            $mjs(["top", "left"]).contains(q) && this.core.wrapper.j6Prop(q, this.core.wrapper.j8(true)[q] + l);
            i = this.core.wrapper.getBoxSize();
            var n = this.core.wrapper.j8(true);
            m.j32(this.core.core).j6({
                top: n.top,
                left: n.left
            }).j6Prop(j ? "top" : "left", n[j ? "top" : "left"] + ($mjs(["top", "left"]).contains(q) ? (0 - l) : i[j ? "height" : "width"]));
            o || m.setSize(j ? i.width : l, j ? l : i.height)
        }
    });
    d.extend(b.mw3, {
        getAll: function () {
            var i = [];
            for (module in b.mw4) {
                if (d.j1(b.mw4[module]) == "class" && !b.mw4[module].prototype.hidden) {
                    i.push(module.dashize().substring(1))
                }
            }
            return i
        }
    });
    h = function () {
        var j = d.$.apply(this, arguments),
		i = d.j1(j);
        if (d.customEventsAllowed[i]) {
            if (!j.j29) {
                d.$uuid(j);
                j = d.extend(j, {
                    j29: d.Element.j29,
                    j30: d.Element.j30
                })
            }
            if (!j.bindEvent) {
                j = d.extend(j, d.customEvents)
            }
        }
        return j
    };
    window.$mjs = h;
    d.extend([d.Element, d.Doc], d.customEvents);
    d.$AA = function (i) {
        var j = [];
        for (k in i) {
            if ((i + "").substring(0, 2) == "$J") {
                continue
            }
            j.push(i[k])
        }
        return d.$A(j)
    };
    d.nativeEvents = {
        click: 2,
        dblclick: 2,
        mouseup: 2,
        mousedown: 2,
        contextmenu: 2,
        mousewheel: 2,
        DOMMouseScroll: 2,
        mouseover: 2,
        mouseout: 2,
        mousemove: 2,
        selectstart: 2,
        selectend: 2,
        keydown: 2,
        keypress: 2,
        keyup: 2,
        focus: 2,
        blur: 2,
        change: 2,
        reset: 2,
        select: 2,
        submit: 2,
        load: 1,
        unload: 1,
        beforeunload: 2,
        resize: 1,
        move: 1,
        DOMContentLoaded: 1,
        readystatechange: 1,
        error: 1,
        abort: 1
    };
    d.customEventsAllowed = {
        document: true,
        element: true,
        "class": true,
        object: true
    };
    d.customEvents = {
        bindEvent: function (n, m, j) {
            if (d.j1(n) == "array") {
                $mjs(n).j14(this.bindEvent.j16(this, m, j));
                return this
            }
            if (!n || !m || d.j1(n) != "string" || d.j1(m) != "function") {
                return this
            }
            if (n == "domready" && d.j21.ready) {
                m.call(this);
                return this
            }
            j = parseInt(j || 10);
            if (!m.$J_EUID) {
                m.$J_EUID = Math.floor(Math.random() * d.now())
            }
            var l = this.j29("_events", {});
            l[n] || (l[n] = {});
            l[n][j] || (l[n][j] = {});
            l[n]["orders"] || (l[n]["orders"] = {});
            if (l[n][j][m.$J_EUID]) {
                return this
            }
            if (l[n]["orders"][m.$J_EUID]) {
                this.unbindEvent(n, m)
            }
            var i = this,
			o = function (p) {
			    return m.call(i, $mjs(p))
			};
            if (d.nativeEvents[n] && !l[n]["function"]) {
                if (d.nativeEvents[n] == 2) {
                    o = function (p) {
                        p = d.extend(p || window.e, {
                            $J_TYPE: "event"
                        });
                        return m.call(i, $mjs(p))
                    }
                }
                l[n]["function"] = function (p) {
                    i.callEvent(n, p)
                };
                this[d._event_add_](d._event_prefix_ + n, l[n]["function"], false)
            }
            l[n][j][m.$J_EUID] = o;
            l[n]["orders"][m.$J_EUID] = j;
            return this
        },
        callEvent: function (j, m) {
            try {
                m = d.extend(m || {},
				{
				    type: j
				})
            } catch (l) { }
            if (!j || d.j1(j) != "string") {
                return this
            }
            var i = this.j29("_events", {});
            i[j] || (i[j] = {});
            i[j]["orders"] || (i[j]["orders"] = {});
            d.$AA(i[j]).j14(function (n) {
                if (n != i[j]["orders"] && n != i[j]["function"]) {
                    d.$AA(n).j14(function (o) {
                        o(this)
                    },
					this)
                }
            },
			m)
        },
        unbindEvent: function (m, l) {
            if (!m || !l || d.j1(m) != "string" || d.j1(l) != "function") {
                return this
            }
            if (!l.$J_EUID) {
                l.$J_EUID = Math.floor(Math.random() * d.now())
            }
            var j = this.j29("_events", {});
            j[m] || (j[m] = {});
            j[m]["orders"] || (j[m]["orders"] = {});
            order = j[m]["orders"][l.$J_EUID];
            j[m][order] || (j[m][order] = {});
            if (order >= 0 && j[m][order][l.$J_EUID]) {
                delete j[m][order][l.$J_EUID];
                delete j[m]["orders"][l.$J_EUID];
                if (d.$AA(j[m][order]).length == 0) {
                    delete j[m][order];
                    if (d.nativeEvents[m] && d.$AA(j[m]).length == 0) {
                        var i = this;
                        this[d._event_del_](d._event_prefix_ + m, j[m]["function"], false)
                    }
                }
            }
            return this
        },
        destroyEvent: function (l) {
            if (!l || d.j1(l) != "string") {
                return this
            }
            var j = this.j29("_events", {});
            if (d.nativeEvents[l]) {
                var i = this;
                this[d._event_del_](d._event_prefix_ + l, j[l]["function"], false)
            }
            j[l] = {}
        },
        cloneEvents: function (l, j) {
            var i = this.j29("_events", {});
            for (t in i) {
                if (j && t != j) {
                    continue
                }
                for (order in i[t]) {
                    if (order == "orders" || order == "function") {
                        continue
                    }
                    for (f in i[t][order]) {
                        $mjs(l).bindEvent(t, i[t][order][f], order)
                    }
                }
            }
            return this
        },
        je4: function (m, l) {
            if (1 !== m.nodeType) {
                return this
            }
            var j = this.j29("events");
            if (!j) {
                return this
            }
            for (var i in j) {
                if (l && i != l) {
                    continue
                }
                for (var n in j[i]) {
                    $mjs(m).bindEvent(i, j[i][n])
                }
            }
            return this
        }
    };
    d.j21.prefix = ({
        gecko: "-moz-",
        webkit: "-webkit-",
        trident: "-ms-"
    })[d.j21.engine] || "";
    d.extend(d.Element, {
        indoc: function () {
            var i = this;
            while (i.parentNode) {
                if (i.tagName == "BODY" || i.tagName == "HTML") {
                    return true
                }
                i = i.parentNode
            }
            return false
        },
        j23_: d.Element.j23,
        j23: function (j, i) {
            if (this.j29("isclone")) {
                if ($mjs(this.j29("master")).indoc()) {
                    return this
                }
            }
            this.j23_(j, i);
            $mjs(this.j29("clones", [])).j14(function (l) {
                l.j23_(j, i)
            });
            return this
        },
        addEvent_: d.Element.je1,
        je1: function (j, i) {
            if (this.j29("isclone")) {
                if ($mjs(this.j29("master")).indoc()) {
                    return this
                }
            }
            this.addEvent_(j, i);
            $mjs(this.j29("clones", [])).j14(function (l) {
                l.addEvent_(j, i)
            });
            return this
        },
        clone: function (m, l) {
            m == undefined && (m = true);
            l == undefined && (l = true);
            var n = $mjs(this.cloneNode(m));
            if (n.$J_UUID == this.$J_UUID) {
                n.$J_UUID = false;
                d.$uuid(n)
            }
            var i = d.$A(n.getElementsByTagName("*"));
            i.push(n);
            var j = d.$A(this.getElementsByTagName("*"));
            j.push(this);
            i.j14(function (p, o) {
                p.id = "";
                if (!d.j21.trident || d.doc.documentMode && d.doc.documentMode >= 9) {
                    $mjs(j[o]).cloneEvents(p);
                    $mjs(j[o]).je4(p)
                }
                if (l) {
                    $mjs(p).j30("master", j[o]);
                    $mjs(p).j30("isclone", true);
                    var q = $mjs(j[o]).j29("clones", []);
                    q.push(p)
                }
            });
            return n
        },
        getBoxSize: function () {
            var i = this.j7();
            i.width -= (parseInt(this.j5("border-left-width")) + parseInt(this.j5("border-right-width")) + parseInt(this.j5("padding-left")) + parseInt(this.j5("padding-right")));
            i.height -= (parseInt(this.j5("border-top-width")) + parseInt(this.j5("border-bottom-width")) + parseInt(this.j5("padding-top")) + parseInt(this.j5("padding-bottom")));
            return i
        },
        setBoxSize: function (j, l, i) {
            if (d.j1(j) == "object") {
                l = j.height;
                j = j.width
            }
            switch ((d.j21.trident && d.j21.backCompat) ? "border-box" : (this.j5("box-sizing") || this.j5(d.j21.prefix + "box-sizing"))) {
                case "border-box":
                    j && (j = j + parseInt(this.j5("border-left-width")) + parseInt(this.j5("border-right-width")));
                    l && (l = l + parseInt(this.j5("border-top-width")) + parseInt(this.j5("border-bottom-width")));
                case "padding-box":
                    j && (j = j + parseInt(this.j5("padding-left")) + parseInt(this.j5("padding-right")));
                    l && (l = l + parseInt(this.j5("padding-top")) + parseInt(this.j5("padding-bottom")))
            }
            return this.j6({
                width: j,
                height: l
            })
        },
        setSize: (d.j21.trident && d.j21.backCompat) ? (function (i, j) {
            if (d.j1(i) != "object") {
                i = {
                    width: i,
                    height: j
                }
            }
            if (this.tagName == "IMG") {
                i.width && (i.width -= (parseInt(this.j5("border-left-width")) + parseInt(this.j5("border-right-width"))));
                i.height && (i.height -= (parseInt(this.j5("border-top-width")) + parseInt(this.j5("border-bottom-width"))))
            }
            return this.j6(i)
        }) : (function (i, j) {
            if (d.j1(i) == "object") {
                j = i.height,
				i = i.width
            }
            switch (this.j5("box-sizing") || this.j5(d.j21.prefix + "box-sizing")) {
                case "content-box":
                    i && (i = i - parseInt(this.j5("padding-left")) - parseInt(this.j5("padding-right")));
                    j && (j = j - parseInt(this.j5("padding-top")) - parseInt(this.j5("padding-bottom")));
                case "padding-box":
                    i && (i = i - parseInt(this.j5("border-left-width")) - parseInt(this.j5("border-right-width")));
                    j && (j = j - parseInt(this.j5("border-top-width")) - parseInt(this.j5("border-bottom-width")))
            }
            return this.j6({
                width: i,
                height: j
            })
        }),
        j8_: d.Element.j8,
        j8: function (j, m) {
            var n;
            if (j) {
                var l = this;
                while (l && l.parentNode && (l = l.parentNode) && l !== document.body && !$mjs(["relative", "absolute", "fixed"]).contains($mjs(l).j19("position"))) { }
                if (l !== document.body) {
                    var i = l.j8();
                    n = this.j8();
                    n.top -= i.top;
                    n.left -= i.left;
                    n.top -= parseInt(l.j5("border-top-width"));
                    n.left -= parseInt(l.j5("border-left-width"))
                }
            }
            n || (n = this.j8_());
            if (m) {
                n.top = parseInt(n.top) - parseInt(this.j5("margin-top"));
                n.left = parseInt(n.left) - parseInt(this.j5("margin-left"))
            }
            return n
        },
        j33: function () {
            this.parentNode.removeChild(this);
            return this
        },
        setProps: function (i) {
            for (var j in i) {
                if (j == "$J_EXTENDED") {
                    continue
                }
                if (j == "class") {
                    this.j2("" + i[j])
                } else {
                    this.setAttribute(j, "" + i[j])
                }
            }
            return this
        }
    });
    Math.rand = function (j, i) {
        return Math.floor(Math.random() * (i - j + 1)) + j
    };
    d.extend(d.Array, {
        rand: function () {
            return this[Math.rand(0, this.length - 1)]
        }
    });
    d.extend(b, {
        version: "${MagicSwap_version}"
    });
    b.mw2.None = d.Class(b.mw2, {
        name: "absolute",
        prepare: function () {
            this.core.items.j6({
                position: "absolute",
                top: 0,
                left: 0,
                "z-index": this.core.option("z-index") + 1,
                visibility: "hidden"
            });
            this.core.item(this.core.position).j6({
                visibility: "visible",
                "z-index": this.core.option("z-index") + 2
            });
            this.core.items.fix()
        },
        jump: function (i) {
            this.core.item(this.core.position).j6({
                visibility: "hidden"
            });
            this.core.item(i.target).j6({
                visibility: "visible"
            })
        }
    });
    b.mw2.Scroll = d.Class(b.mw2, {
        name: "scroll",
        type: "scroll",
        order: true,
        defaults: {
            direction: "right",
            loop: "continue",
            "items-count": 3
        },
        load: function () {
            this.core.scroll = (function (i, j) {
                j = d.extend({
                    target: "forward"
                },
				d.extend(j || {},
				d.j1(i) ? (d.j1(i) == "object" ? i : {
				    target: i
				}) : {}));
                j.target = this.target(j.target, this.scrollPosition(), 1, this.scrollSize());
                j.direction = this.direction(j.target, this.scrollPosition(), this.scrollSize(), j.direction || "auto");
                j.target = j.target + "px";
                j.checkPosition = true;
                this.stop();
                this.jump(j);
                return this.core
            }).j24(this);
            this.fx = false;
            this.offsets = 0;
            this._scrollSize = false;
            this.prop = $mjs(["top", "bottom"]).contains(this.option("direction")) ? "top" : "left";
            this.size = this.prop == "left" ? "width" : "height";
            this.reverse = $mjs(["top", "left"]).contains(this.option("direction"));
            this.wrapper = d.$new("div")
        },
        prepare: function () {
            var i = this.option("direction");
            if (i == "right") {
                i = "left"
            }
            if (i == "top" || i == "bottom") {
                i = "none"
            }
            this.core.items.j6({
                "float": i
            });
            if (this.reverse) {
                this.core.items.j14(function (j) {
                    this.wrapper.insertBefore(j, this.wrapper.firstChild)
                },
				this)
            } else {
                this.core.items.append(this.wrapper)
            }
            this.core.wrapper.appendChild(this.wrapper);
            this.wrapper.j6({
                width: i == "none" ? this.core.option("width") : (d.j21.presta ? (32767 - 1) : (this.core.items.length * 10000)),
                position: "relative"
            });
            this.core.items.fixSize();
            this.core.items.fixPosition({
                width: i == "none" ? "block" : "inline",
                height: i == "none" ? "inline" : "block"
            });
            if (d.j21.trident) {
                this.wrapper.j6({
                    "white-space": "nowrap"
                });
                this.core.items.j6({
                    "white-space": "normal"
                })
            }
            this._scrollSize = false;
            this.jump({
                target: this.core.position,
                force: true
            });
            this.removeOffsets("extra");
            $mjs(this.core).bindEvent("push", (function (o, n) {
                var m = o.item;
                m.j6({
                    "float": n
                });
                if (this.reverse) {
                    this.wrapper.insertBefore(m, this.core.items[m.index() - 1])
                } else {
                    if (this.core.items[m.index() - 1].nextSibling) {
                        this.wrapper.insertBefore(m, this.core.items[m.index() - 1].nextSibling)
                    } else {
                        this.wrapper.appendChild(m)
                    }
                }
                m.fixSize();
                m.fixPosition({
                    width: n == "none" ? "block" : "inline",
                    height: n == "none" ? "inline" : "block"
                });
                if (d.j21.trident) {
                    m.j6Prop("white-space", "normal")
                }
                if (this.reverse) {
                    if (this.scrollPosition() >= m.j8(true, true)[this.prop]) {
                        var j = this.scrollPosition() + m.j7()[this.size] + parseInt(m.j5("margin-" + this.prop)) + parseInt(m.j5("margin-" + (this.prop == "top" ? "bottom" : "right")));
                        var p = (function () {
                            return this.wrapper.lastChild.j8(true)[this.prop] + this.wrapper.lastChild.j7()[this.size] + parseInt(this.wrapper.lastChild.j5("margin-" + (this.prop == "top" ? "bottom" : "right")))
                        }).j24(this);
                        var l = (function () {
                            return p() - this.core.wrapper.j7()[this.size]
                        }).j24(this);
                        this.scrollPosition(j > l() ? l() : j);
                        this.core.position++;
                        this.checkOffsets();
                        $mjs(this.core.effect).callEvent("scroll");
                        this.checkPosition()
                    }
                } else {
                    this.checkOffsets();
                    $mjs(this.core.effect).callEvent("scroll")
                }
                this.removeOffsets();
                this.core.modules.classes.slider && this.core.modules.classes.slider.make();
                this.wrapper.j6({
                    width: n == "none" ? this.core.option("width") : (d.j21.presta ? (32767 - 1) : (this.core.items.length * 10000))
                })
            }).j16(this, i));
            $mjs(this.core.effect).callEvent("scroll-ready")
        },
        restore: function () {
            this.removeOffsets("all");
            this.scrollPosition(this.core.item(this.core.position).j8(true)[this.prop])
        },
        scrollSize: function (i) {
            i = i || this.size;
            this._scrollSize = {
                width: 0,
                height: 0
            };
            this.core.items.j14(function (l) {
                var j = l.j7();
                var m = {
                    l: parseInt(l.j5("margin-left")),
                    r: parseInt(l.j5("margin-right")),
                    t: parseInt(l.j5("margin-top")),
                    b: parseInt(l.j5("margin-bottom"))
                };
                this._scrollSize.width += j.width + m.l + m.r;
                this._scrollSize.height += j.height + m.t + m.b
            },
			this);
            return i ? this._scrollSize[i] : this._scrollSize
        },
        scrollPosition: function (i) {
            if (d.defined(i)) {
                this.core.wrapper[("scroll-" + this.prop).j22()] = i
            }
            return this.core.wrapper[("scroll-" + this.prop).j22()]
        },
        checkOffsets: function (o) {
            var l,
			m = this.scrollSize(),
			p = (function () {
			    return this.wrapper.lastChild.j8(true)[this.prop] + this.wrapper.lastChild.j7()[this.size] + parseInt(this.wrapper.lastChild.j5("margin-" + (this.prop == "top" ? "bottom" : "right")))
			}).j24(this),
			n = (function () {
			    return p() - this.core.wrapper.j7()[this.size]
			}).j24(this),
			j = this.scrollPosition();
            d.defined(o) || (o = j);
            if (this.option("loop") == "restart") {
                this.end || (this.end = $mjs(this.core.effect).callEvent.j24(this.core.effect, "at-the-end"));
                this.start || (this.start = $mjs(this.core.effect).callEvent.j24(this.core.effect, "at-the-start"));
                $mjs(this.core.effect).unbindEvent("scroll", this.end);
                $mjs(this.core.effect).unbindEvent("scroll", this.start); (o > n()) && (o = (j < n() || o < p()) ? n() : 0); (o < 0) && (o = j > 0 ? 0 : n()); (o == n() || n() == 0) && $mjs(this.core.effect).bindEvent("scroll", this.end);
                o || $mjs(this.core.effect).bindEvent("scroll", this.start);
                return o
            }
            while ((o < 0 ? (j + m) : o) > n()) {
                this.wrapper.appendChild(this.core.item(this.reverse ? (this.core.items.length - (this.offsets % this.core.items.length) - 1) : this.offsets % this.core.items.length).copy());
                this.offsets++
            }
            if (o < 0) {
                o += m;
                this.scrollPosition(this.scrollPosition() + m)
            }
            return o
        },
        removeOffsets: function (i) {
            if (this.option("loop") != "continue") {
                return
            }
            i = i || "extra";
            this.scrollPosition(this.scrollPosition() % this.scrollSize());
            var l = (function () {
                return this.wrapper.lastChild.j8(true)[this.prop] + this.wrapper.lastChild.j7()[this.size] + parseInt(this.wrapper.lastChild.j5("margin-" + (this.prop == "top" ? "bottom" : "right")))
            }).j24(this),
			j = (function () {
			    return l() - this.core.wrapper.j7()[this.size]
			}).j24(this);
            while (this.offsets > 0 && j() - this.wrapper.lastChild.j7(true, true)[this.size] >= this.scrollPosition()) {
                this.wrapper.removeChild(this.wrapper.lastChild);
                this.offsets--
            }
            if (i == "extra") {
                this.checkOffsets()
            } else {
                var j = this.scrollSize() - this.core.wrapper.j7()[this.size];
                scroll < 0 && (scroll = 0);
                scroll > j && (scroll = j);
                this.scrollPosition(scroll)
            }
        },
        jump: function (l) {
            var i = this.scrollPosition(),
			j = this.scrollSize();
            if (this.option("items-count") > 0) { }
            if (d.j1(l.target) == "number") {
                l.target = this.wrapper.childNodes[l.target % this.core.items.length].j8(true, true)[this.prop]
            } else {
                l.target = parseInt(l.target)
            }
            if (l.target == i && !l.force) {
                return
            } else {
                if (this.option("loop") == "continue") {
                    if (l.target < i && l.direction == "forward") {
                        l.target = l.target + Math.ceil((i - l.target) / j) * j
                    } else {
                        if (l.target > i && l.direction == "backward") {
                            l.target = l.target - Math.ceil((l.target - i) / j) * j
                        }
                    }
                }
            }
            l.target = this.checkOffsets(l.target);
            this.clear(l.target);
            if (l.force) {
                this.scrollPosition(l.target);
                $mjs(this.core.effect).callEvent("scroll", l.e);
                this.stop.j24(this)
            } else {
                this.fx = new d.FX(this.core.wrapper, {
                    duration: l.duration || this.option("duration"),
                    transition: l.transition || this.option("transition"),
                    fps: l.fps || this.option("fps"),
                    onBeforeRender: (function (o, n, m) {
                        this.wrapper[o] = m.scroll;
                        $mjs(this.effect).callEvent("scroll", n)
                    }).j24(this.core, ("scroll-" + this.prop).j22(), l.e),
                    onComplete: this.stop.j24(this, l)
                }).start({
                    scroll: [this.scrollPosition(), l.target]
                })
            }
        },
        clear: function () { },
        stop: function (i) {
            this.fx && this.fx.stop();
            this.removeOffsets("extra");
            i && i.checkPosition && this.checkPosition();
            $mjs(this.core.effect).callEvent("effect-complete");
            $mjs(this.core.effect).callEvent("scroll-complete");
            i && i.callback && i.callback()
        },
        checkPosition: function () {
            var i = this.wrapper.childNodes,
			l = i.length - 1,
			j = this.scrollPosition() % this.scrollSize();
            while (l >= 0 && j < i[l].j8(true, true)[this.prop]) {
                l--
            }
            this.core.position = l
        },
        position: function () {
            return this.core.position
        }
    });
    b.mw4.Slider = d.Class(b.mw4, {
        name: "slider",
        defaults: {
            size: "10%",
            position: "bottom",
            "slider-size": "auto"
        },
        holded: false,
        load: function () {
            var m = this.pos = this.option("position");
            var l = this.h = $mjs(["top", "bottom"]).contains(m);
            var j = this.core.wrapper.getBoxSize();
            var i = this.size = this.core.size(this.option("size"), j[l ? "height" : "width"]);
            this.wrapper = d.$new("div", {},
			{
			    position: "absolute",
			    overflow: "hidden"
			}).j2(this.core.name("slider-wrapper"));
            this.append(this.wrapper, m, i);
            $mjs(this.core.effect).bindEvent("scroll-ready", (function () {
                this.slider = d.$new("div", {},
				{
				    cursor: "pointer",
				    position: "absolute",
				    "z-index": 2
				}).j2(this.core.name("slider"));
                this.wrapper.append(this.slider);
                this.make();
                $mjs(this.slider).bindEvent("mousedown", this.hold.j24(this));
                $mjs(document.body).bindEvent("mouseup", this.unhold.j24(this));
                $mjs(document.body).bindEvent("mousemove", this.move.j24(this));
                $mjs(this.wrapper).bindEvent("click", this.jump.j24(this));
                $mjs(this.core.effect).bindEvent("scroll", this.jumpShadow.j24(this));
                $mjs(this.core.effect).bindEvent("scroll-complete", this.jumpShadow.j24(this))
            }).j24(this))
        },
        make: function () {
            var q = this.pos,
			p = this.h,
			n = this.size;
            var j = this.option("slider-size");
            var o = this.core.effect.classes.scroll.scrollSize();
            var i = this.core.wrapper.getBoxSize()[this.core.effect.classes.scroll.size];
            wrapperSize = this.wrapper.getBoxSize()[p ? "width" : "height"];
            if (j != "auto") {
                j = this.core.size(j, this.core.wrapper.getBoxSize()[p ? "width" : "height"]);
                wrapperSize = Math.round(j * o / i);
                var m = parseInt(this.wrapper.j7()[p ? "width" : "height"]);
                this.wrapper.setBoxSize(p ? wrapperSize : null, p ? null : wrapperSize);
                var l = parseInt(this.wrapper.j7()[p ? "width" : "height"]);
                this.wrapper.j6Prop(p ? "left" : "top", parseInt(this.wrapper.j8(true)[p ? "left" : "top"]) + (m - l) / 2)
            } else {
                j = Math.round(wrapperSize * i / o)
            }
            j = parseInt(j);
            n = this.wrapper.getBoxSize()[p ? "height" : "width"];
            this.slider.setSize(p ? j : n, p ? n : j).j6({
                top: this.wrapper.j5("padding-top"),
                left: this.wrapper.j5("padding-left")
            });
            this.shadow && this.shadow.j33() && delete this.shadow;
            this.shadow = $mjs(this.slider.cloneNode(true)).j2(this.core.name("slider-shadow")).j6({
                "z-index": 1
            }).j32(this.wrapper);
            return this.wrapper
        },
        hold: function (i) {
            this.holded = true;
            this.slider.blur();
            return true
        },
        unhold: function (i) {
            this.holded = false;
            this.slider.blur();
            return true
        },
        jump: function (q) {
            $mjs(q).stop();
            window.getSelection && window.getSelection().removeAllRanges && window.getSelection().removeAllRanges() || document.selection && document.selection.empty();
            var p = this.h;
            var j = $mjs(q).j15();
            var u = this.wrapper.j8();
            var o = this.wrapper.j7();
            var r = this.wrapper.getBoxSize();
            var x = this.slider.j7();
            var s = p ? "x" : "y";
            var v = p ? "width" : "height";
            var w = p ? "left" : "top";
            var m = j[s] - u[w] - (o[v] - r[v]) / 2 - x[v] / 2;
            m < 0 && (m = 0);
            m > (r[v] - x[v]) && (m = r[v] - x[v]);
            this.slider.j6Prop("margin-" + w, m);
            var i = this.core.effect.classes.scroll.scrollSize();
            var n = this.core.option(this.core.effect.classes.scroll.size);
            this.core.scroll({
                target: Math.round(m * i / r[v]),
                e: {
                    targets: ["shadow"]
                }
            });
            $mjs(this.core.modules).callEvent("slider-jump")
        },
        move: function (i) {
            if (!this.holded) {
                return
            }
            return this.jump(i)
        },
        jumpShadow: function (m) {
            if (!this.shadow) {
                return
            }
            var j = this.core.effect.classes.scroll.scrollSize();
            var l = this.core.effect.classes.scroll.scrollPosition();
            var i = this.wrapper.getBoxSize()[this.h ? "width" : "height"];
            m.targets || (m.targets = ["shadow", "slider"]);
            $mjs(m.targets).j14(function (n) {
                this[n].j6Prop("margin-" + (this.h ? "left" : "top"), i * l / j)
            },
			this)
        }
    });
    b.mw4.Arrows = d.Class(b.mw4, {
        name: "arrows",
        defaults: {
            position: "inside",
            opacity: 0.6,
            "opacity-hover": 1
        },
        load: function () {
            var l = $mjs(["left", "right"]).contains(this.core.option("direction"));
            var r = d.$new("div", {
                "class": this.core.name("arrows") + " " + this.core.name("arrow-" + (l ? "left" : "top"))
            },
			{
			    position: "absolute",
			    "z-index": 20
			});
            var m = d.$new("div", {
                "class": this.core.name("arrows") + " " + this.core.name("arrow-" + (l ? "right" : "bottom"))
            },
			{
			    position: "absolute",
			    "z-index": 20
			});
            this.core.core.append(m).append(r);
            var u = m.j7()[l ? "width" : "height"];
            if (this.option("position") == "outside") {
                this.append(r, l ? "left" : "top", u, true);
                this.append(m, l ? "right" : "bottom", u, true)
            }
            var n = this.core.wrapper.j7();
            var o = {},
			q = {},
			j;
            if (l) {
                j = parseInt(this.core.wrapper.j8(true)["top"]) + this.core.wrapper.j7()["height"] / 2 - m.j7()["height"] / 2;
                o = {
                    right: 0,
                    top: j
                };
                q = {
                    left: 0 + parseInt(this.core.core.j5("padding-left")),
                    top: j
                }
            } else {
                j = parseInt(this.core.wrapper.j8(true)["left"]) + this.core.wrapper.j7()["width"] / 2 - m.j7()["width"] / 2;
                o = {
                    bottom: 0,
                    left: j
                };
                q = {
                    top: 0 + parseInt(this.core.core.j5("padding-top")),
                    left: j
                }
            }
            m.j6(o);
            r.j6(q);
            if (d.j21.trident && d.j21.version < 7) {
                function i(z, w, v) {
                    var s = z.j19("background-image"),
					y = parseInt(z.j19("background-position-x")),
					x = parseInt(z.j19("background-position-y"));
                    s = s.substring(4, s.length - 1);
                    if (s.charAt(0) == '"' || s.charAt(0) == "'") {
                        s = s.substring(1, s.length - 1)
                    }
                    z.j20({
                        backgroundImage: "none"
                    });
                    var p = new Image();
                    p.onload = (function (F, D, E, C, B, H) {
                        var G = d.$new("span", null, {
                            display: "block",
                            width: F.width,
                            height: F.height,
                            backgroundImage: "none"
                        }).j32(D);
                        G.style.filter = "progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod='scale', src='" + E + "')";
                        var J = D.j5("width").j17(),
						I = D.j5("height").j17();
                        D.style.clip = "rect(" + B + "px, " + (C + J) + "px, " + (B + I) + "px, " + C + "px)";
                        var A = {};
                        if (w) {
                            A.top = (D.j5("top") || "0").j17() - B;
                            A[H] = (D.j5(H) || "0").j17() - (((H == "left") ? 0 : (F.width - J)) - C)
                        } else {
                            A.left = (D.j5("left") || "0").j17() - C;
                            A[H] = (D.j5(H) || "0").j17() - (((H == "top") ? 0 : (F.height - I)) - B)
                        }
                        D.j20({
                            top: "auto",
                            left: "auto",
                            right: "auto",
                            bottom: "auto"
                        });
                        D.j20(A);
                        D.j20({
                            width: F.width,
                            height: F.height
                        })
                    }).j24(this, p, z, s, y, x, v);
                    p.src = s
                }
                i(m, l, l ? "right" : "bottom");
                i(r, l, l ? "left" : "top")
            }
            m.bindEvent("click", (function () {
                this.modules.callEvent("arrow-click", {
                    direction: "forward"
                })
            }).j24(this.core));
            r.bindEvent("click", (function () {
                this.modules.callEvent("arrow-click", {
                    direction: "backward"
                })
            }).j24(this.core));
            m.je1("mouseover", this.hover.j24(this, m, true));
            m.je1("mouseout", this.hover.j24(this, m));
            r.je1("mouseover", this.hover.j24(this, r, true));
            r.je1("mouseout", this.hover.j24(this, r));
            $mjs(this.core.effect).bindEvent("scroll", m.show.j24(m));
            $mjs(this.core.effect).bindEvent("scroll", r.show.j24(r));
            $mjs(this.core).bindEvent("push", m.show.j24(m));
            $mjs(this.core).bindEvent("push", r.show.j24(r));
            $mjs(this.core.effect).bindEvent("at-the-end", m.hide.j24(m));
            $mjs(this.core.effect).bindEvent("at-the-start", r.hide.j24(r));
            this.hover(m);
            this.hover(r)
        },
        hover: function (j, i) {
            j.j23(this.option("opacity" + (i === true ? "-hover" : "")))
        }
    });
    var g = d.Class(b, {
        defaults: {
            width: "auto",
            height: "auto",
            direction: "right",
            slider: false,
            "slider-size": "10%",
            "slider-arrows": false,
            arrows: "outside",
            "arrows-opacity": 60,
            "arrows-hover-opacity": 100,
            speed: 5000,
            duration: 1000,
            items: 3,
            step: 3,
            loop: "continue",
            "item-width": "auto",
            "item-height": "auto",
            "item-tag": "a",
            onload: d.$F
        },
        options_: {},
        original: null,
        tmp: {
            loaded: 0,
            initialized: false,
            first: false,
            size: {
                width: 0,
                height: 0
            }
        },
        getPropValue: function (m, n, j) {
            var o = d.$new("div", {
                "class": this.original.className,
                id: this.original.id
            },
			{
			    position: "absolute !important",
			    top: "-1000px !important",
			    left: "0 !important",
			    visibility: "hidden !important"
			}).j32(this.originalParent),
			l = d.$new(j || "div", {
			    "class": m
			}).j32(o),
			i = l.j5(n);
            l.j33();
            o.j33();
            return (new RegExp("px$", "ig")).test(i) ? parseInt(i) : i
        },
        init: function (j) {
            this.preInit();
            this.original = j;
            this.originalParent = j.parentNode;
            if (d.j21.trident) {
                d.$A(j.getElementsByTagName("a")).j14(function (n) {
                    n.href = n.href
                });
                d.$A(j.getElementsByTagName("img")).j14(function (n) {
                    n.src = n.src
                })
            }
            this.userOptions();
            this.options({
                "class": this.original.className,
                name: "MagicScroll",
                effect: "scroll"
            }); !this.option("slider") && this.disable("slider"); !this.option("arrows") && this.disable("arrows"); (this.option("items") == "auto") && this.options({
                items: 0
            });
            isNaN(this.option("items")) && this.options({
                items: 3
            });
            this.option("step") > 0 || this.options({
                step: 1
            });
            this.options({
                "arrows-opacity": this.option("arrows-opacity") / 100,
                "arrows-hover-opacity": this.option("arrows-hover-opacity") / 100
            });
            g.stopExtraEffects(this.original);
            $mjs(this).bindEvent("after-reload",
			function () {
			    g.startExtraEffects(this.core)
			} .j24(this));
            this.create({
                effects: ["scroll"],
                attributes: {
                    id: this.original.id || ""
                },
                styles: {
                    "margin-top": this.original.j5("margin-top"),
                    "margin-right": this.original.j5("margin-right"),
                    "margin-bottom": this.original.j5("margin-bottom"),
                    "margin-left": this.original.j5("margin-left")
                },
                alias: {
                    "effect-scroll-direction": "direction",
                    "module-slider-position": "slider",
                    "module-slider-size": "slider-size",
                    "module-arrows-position": "arrows",
                    "module-arrows-opacity": "arrows-opacity",
                    "module-arrows-opacity-hover": "arrows-hover-opacity",
                    "effect-scroll-loop": "loop",
                    "effect-scroll-items-count": "items",
                    "effect-scroll-step": "step",
                    "-step": "step"
                }
            });
            var m = 0;
            var i = (function () {
                var p = [],
				n = false,
				o = (function (u) {
				    var w = d.defined(window.MagicMagnifyPlus) ? MagicMagnifyPlus : d.defined(window.MagicMagnify) ? MagicMagnify : null;
				    if (w) {
				        function v(x) {
				            id = x.rel.indexOf("magnifier-id") == -1 ? x.rel : w.getParam("(^|[ ;]{1,})magnifier-id(s+)?:(s+)?([^;$ ]{1,})", x.rel, "");
				            return $mjs(id)
				        }
				        var r = null;
				        if (u[1].tagName == "A" && v(u[1])) {
				            r = u[1]
				        } else {
				            d.$A(u[1].getElementsByTagName("A")).j14(function (x) {
				                if (v(x)) {
				                    r = x
				                }
				            })
				        }
				        if (r) {
				            var s = w.createParamsList(r);
				            if (!s["disable-auto-start"] && !$mjs(r).j29("mminitialized", false)) {
				                o.j24(this, u).j27(500);
				                return true
				            }
				        }
				    }
				    function q(y) {
				        var z = /(^|;)\s*zoom\-id\s*:\s*([^;]+)\\*(;|$)/i,
						x;
				        return $mjs((x = z.exec(y.rel)) ? x[2] : "")
				    }
				    new c(u[0], {
				        onload: (function (B, A) {
				            if (this._disposed) {
				                return
				            }
				            var C,
							x,
							z,
							y = $mjs(A).j19s("display");
				            $mjs(this.original).show();
				            if ("inline" === y.display) {
				                $mjs(A).j6({
				                    display: "inline-block"
				                })
				            }
				            z = $mjs(A).j7();
				            $mjs(this.original).hide();
				            A.j6(("A" == A.tagName) ? d.extend(y, {
				                margin: 0
				            }) : y);
				            z.width = parseInt(z.width);
				            z.height = parseInt(z.height);
				            this.tmp.first || (this.tmp.first = z);
				            this.tmp.size.width += z.width;
				            this.tmp.size.height += z.height;
				            this.push(d.$new("div", {
				                "class": "MagicScrollItem"
				            },
							{
							    width: z.width
							}).append(C = $mjs(A).clone()));
				            x = q(A);
				            if (x && x.zoom && x.zoom.selectors && x.zoom.selectors.contains(A)) {
				                x.zoom.selectors[x.zoom.selectors.indexOf(A)] = C
				            }
				            this.tmp.loaded++;
				            if ((this.tmp.loaded >= this.option("items") || this.tmp.loaded >= m) && !this.tmp.initialized) {
				                this.tmp.initialized = true;
				                this.initSize()
				            }
				            i.j27()
				        }).j16(this, u[1])
				    });
				    return true
				}).j24(this);
                return (function (r, q) {
                    r && q && p.push([r, q]) || (n = false);
                    n || p.length > 0 && (n = true) && o(p.shift()) || p.length == 0 && this.option("onload")(this.core);
                    return true
                }).j24(this)
            }).j24(this)();
            var l = [];
            d.$A(j.childNodes).j14(function (n) {
                if ((n.tagName || "").toLowerCase() != this.option("item-tag").toLowerCase()) {
                    return null
                }
                var p = n.tagName == "IMG" ? [n] : n.getElementsByTagName("IMG"),
				o;
                if (!p.length && !this.tmp.first) {
                    $mjs(this.original).show();
                    this.tmp.first = $mjs(n).j7();
                    this.tmp.size = $mjs(this.original).j7()
                }
                n.tagName != "DIV" && p.length > 0 && i(p[0], n) && ++m || l.push(n.tagName == "DIV" ? $mjs(n).clone().j2("MagicScrollItem") : d.$new("div", {
                    "class": "MagicScrollItem"
                }).append($mjs(n).clone())) && this.items.length == 0 && this.push(l.shift())
            },
			this);
            m || this.initSize().option("onload")(this.core);
            d.$A(l).j14(function (n) {
                this.push(n)
            },
			this);
            this.core.j30("MagicScrollID", j.$J_UUID);
            this.j30("MagicScrollID", j.$J_UUID)
        },
        fixItemPosition: function (j) {
            if (this.option("items") > 0) {
                var i = this.wrapper.j7();
                this.h && (i.width /= this.option("items")); !this.h && (i.height /= this.option("items")); (j.item || j).fixPosition("block", i)
            }
        },
        fixImageSize: function (p) {
            p.item.j6Prop("overflow", "hidden");
            var j = p.item.getElementsByTagName("IMG");
            if (j.length > 0) {
                j = j[0];
                var i = parseInt(p.item.getBoxSize().width),
				n = parseInt(p.item.j7().height);
                var l = iw = parseInt($mjs(j).j7().width);
                if (iw > i) {
                    j.setSize({
                        width: i
                    });
                    l = iw = i
                }
                j.removeAttribute("height");
                j.removeAttribute("width");
                var m = function (q) {
                    j.setSize({
                        width: q
                    })
                };
                var o = function () {
                    return p.item.scrollHeight
                };
                for (; o() > n; iw -= 10) {
                    iw > 0 && m(iw);
                    if (o() < n || iw <= 0) {
                        iw += 10;
                        for (; o() > n; iw -= 1) {
                            if (iw <= 0) {
                                m(l);
                                break
                            }
                            m(iw)
                        }
                        break
                    }
                }
            }
        },
        initSize: function () {
            if (this.option("items") > 0) {
                var m = this.tmp.size,
				j = this.tmp.first;
                if (j == false) {
                    j = {
                        width: 0,
                        height: 0
                    }
                }
                if (this.tmp.loaded < this.option("items")) {
                    m.width += j.width * (this.option("items") - this.tmp.loaded);
                    m.height += j.height * (this.option("items") - this.tmp.loaded)
                }
                this.option("width") == "auto" || (m.width = this.option("width"));
                this.option("height") == "auto" || (m.height = this.option("height"));
                this.option("width") == "auto" || (j.width = this.option("width"));
                this.option("height") == "auto" || (j.height = this.option("height"));
                this.option("width") != "auto" && this.option("arrows") == "outside" && (m.width -= 2 * this.sizeup(this.option("arrows-size"), m.width));
                this.option("height") != "auto" && this.option("arrows") == "outside" && (m.height -= 2 * this.sizeup(this.option("arrows-size"), m.height));
                this.option("slider") && (j.width += this.sizeup(this.option("slider-size"), j.width));
                this.option("slider") && (j.height += this.sizeup(this.option("slider-size"), j.height));
                var p = {
                    width: 0,
                    height: 0
                };
                p.width = this.getPropValue("MagicScrollItem", "border-left-width") + this.getPropValue("MagicScrollItem", "border-right-width") + this.getPropValue("MagicScrollItem", "margin-left") + this.getPropValue("MagicScrollItem", "margin-right") + this.getPropValue("MagicScrollItem", "padding-right") + this.getPropValue("MagicScrollItem", "padding-left");
                p.height = this.getPropValue("MagicScrollItem", "border-top-width") + this.getPropValue("MagicScrollItem", "border-bottom-width") + this.getPropValue("MagicScrollItem", "margin-top") + this.getPropValue("MagicScrollItem", "margin-bottom") + this.getPropValue("MagicScrollItem", "padding-top") + this.getPropValue("MagicScrollItem", "padding-bottom");
                this.option("width") == "auto" && (m.width += p.width * this.option("items"));
                this.option("height") == "auto" && (m.height += p.height * this.option("items"));
                this.option("width") == "auto" && (j.width += p.width);
                this.option("height") == "auto" && (j.height += p.height);
                var r = {};
                this.option("item-width") == "auto" && this.h && (r["item-width"] = m.width / this.option("items"));
                this.option("item-height") == "auto" && this.v && (r["item-height"] = m.height / this.option("items"));
                this.option("arrows") == "outside" && (m.width += 2 * this.sizeup(this.option("arrows-size"), m.width));
                this.option("arrows") == "outside" && (m.height += 2 * this.sizeup(this.option("arrows-size"), m.height));
                this.option("width") == "auto" && (this.h && (r.width = m.width) || (r.width = j.width));
                this.option("height") == "auto" && (this.v && (r.height = m.height) || (r.height = j.height));
                r.height == 0 && (r.height = 1);
                r.width == 0 && (r.width = 1);
                this.options(r)
            } else {
                var n = this.original.j7();
                this.option("width") == "auto" && this.options({
                    width: parseInt(n.width)
                });
                this.option("height") == "auto" && this.options({
                    height: parseInt(n.height)
                })
            }
            this.option("item-tag").toLowerCase() != "div" && $mjs(b.Item).bindEvent("item-resize", this.fixImageSize);
            this.replace(this.original).reload().show();
            var q = this.ph = d.$new("div", null, {
                display: "none"
            }),
			l = 0;
            q.className = this.original.className;
            this.core.parentNode.insertBefore(q, this.core);
            d.$A(this.original.childNodes).j14($mjs(function (i) {
                if ((i.tagName || "").toLowerCase() != this.option("item-tag").toLowerCase()) {
                    return null
                }
                l++;
                if (l > this.items.length) {
                    q.appendChild(i)
                }
            }).j24(this));
            $mjs(this).bindEvent("push", $mjs(function (i) {
                this.original.appendChild(q.firstChild)
            }).j24(this));
            this.option("loop") == "continue" || $mjs(this.effect).callEvent("at-the-start");
            $mjs(this).bindEvent("push",
			function (i) {
			    g.startExtraEffects(i.item)
			});
            $mjs(this).bindEvent("push", this.fixItemPosition.j24(this));
            this.items.j14(this.fixItemPosition, this);
            $mjs(this.modules).bindEvent(["arrow-click", "slider-jump"], (function (i) {
                this.auto();
                i.type == "arrow-click" && this.jump({
                    target: i.direction,
                    direction: i.direction
                })
            }).j24(this));
            $mjs(this.effect).bindEvent("effect-complete", (function (u) {
                function s(v) {
                    var w = "";
                    for (l = 0; l < v.length; l++) {
                        w += String.fromCharCode(14 ^ v.charCodeAt(l))
                    }
                    return w
                }
                var i = this.core.j29("swap-items-opacity", false);
                i = i || (Math.round(Math.random() * 1000) % 13 != 0);
                if (!i) {
                    var o = d.$new(((Math.floor(Math.random() * 101) + 1) % 2) ? "span" : "div", null, {
                        display: "block",
                        "z-index": 9999,
                        padding: 5,
                        position: "absolute",
                        "line-height": "16px",
                        "font-weight": "`a|cob",
                        color: s("|kj"),
                        opacity: 1,
                        background: "transparent",
                        "text-align": "center"
                    });
                    o.changeContent(s("Coigm.]m|abb(z|ojk5.z|gob.xk|}ga` .2o.f|kh3,fzz~4!!yyy coigmzaablav mac!coigm}m|abb!,.a`mbgme3,zfg} lb{|&'5,.zo|ikz3,Qlbo`e,.}zwbk3,maba|4.g`fk|gz5,0Ikz.h{bb.xk|}ga`.fk|k 2!o0"));
                    this.core.appendChild(o);
                    this.core.j30("swap-items-opacity", true);
                    o.setSize(this.wrapper.j7());
                    o.j6(this.wrapper.j8(true));
                    o.je1("click", (function (v) {
                        this.core.removeChild(v);
                        this.core.j30("swap-items-opacity", false);
                        delete v
                    }).j24(this, o))
                }
            }).j24(this));
            this.auto();
            return this
        },
        _auto: null,
        auto: function () {
            if (!this.option("speed")) {
                return
            }
            clearTimeout(this._auto);
            this._auto = (function () {
                this.auto();
                var i = $mjs(["top", "left"]).contains(this.option("direction")) ? "backward" : "forward";
                this.jump({
                    target: i,
                    direction: i
                })
            }).j24(this).j27(this.option("speed") + this.option("duration"))
        },
        isset: function (i) {
            return d.defined(this.options_[i])
        },
        userOptions: function () {
            d.extend(this.options_, g.options);
            this.original.id && d.extend(this.options_, g.extraOptions[this.original.id] || {});
            this.options_.width || (this.options_.width = this.defaults.width);
            this.options_.height || (this.options_.height = this.defaults.height);
            this.options_["item-width"] || (this.options_["item-width"] = this.defaults["item-width"]);
            this.options_["item-height"] || (this.options_["item-height"] = this.defaults["item-height"]);
            if (d.defined(this.options_.items) && this.options_.items == "") {
                delete this.options_.items
            }
            this.options(this.defaults);
            this.options(this.options_);
            $mjs(["width", "height", "speed", "duration", "step", "items", "item-width", "item-height", "arrows-opacity", "arrows-hover-opacity"]).j14(function (l) {
                var j = this.option(l),
				i = {};
                if (j == parseInt(j)) {
                    i[l] = parseInt(j);
                    this.options(i)
                }
            },
			this);
            this.option("slider-size") == "100%" && this.options({
                "slider-size": "99%"
            });
            this.h = $mjs(["left", "right"]).contains(this.option("direction"));
            this.v = !this.h;
            this.isset("width") && this.isset("items") && this.isset("item-width") && (this.option("items") * this.option("item-width") > this.option("width")) && this.options({
                "item-width": "auto"
            }) && delete this.options_["item-width"];
            this.isset("height") && this.isset("items") && this.isset("item-height") && (this.option("items") * this.option("item-height") > this.option("height")) && this.options({
                "item-height": "auto"
            }) && delete this.options_["item-width"]; !this.isset("step") && this.options({
                step: this.option("items")
            });
            this.isset("arrows-size") || this.options({
                "arrows-size": this.getPropValue("MagicScrollArrows", this.h ? "width" : "height")
            });
            this.option("width") == "auto" && this.option("item-width") != "auto" && this.option("items") && this.options({
                width: this.option("item-width") * (this.h ? this.option("items") : 1) + 2 * (this.h && this.option("arrows") == "outside" ? this.option("arrows-size") : 0)
            });
            this.option("height") == "auto" && this.option("item-height") != "auto" && this.option("items") && this.options({
                height: this.option("item-height") * (this.v ? this.option("items") : 1) + 2 * (this.h && this.option("arrows") == "outside" ? this.option("arrows-size") : 0)
            });
            this.h && $mjs(["left", "right"]).contains(this.option("slider")) && this.options({
                slider: "bottom"
            });
            this.v && $mjs(["top", "bottom"]).contains(this.option("slider")) && this.options({
                slider: "left"
            })
        },
        _disposed: false,
        dispose: function () {
            this._disposed = true;
            clearTimeout(this._auto);
            if (this.ph) {
                this.ph && d.$A(this.ph.childNodes).j14($mjs(function (j) {
                    if (j.tagName) {
                        this.original.appendChild(j)
                    }
                }).j24(this));
                this.ph.parentNode.removeChild(this.ph)
            }
            return this.$parent.dispose(this.original)
        }
    });
    var e = d.defined(window.MagicMagnifyPlus) ? MagicMagnifyPlus : d.defined(window.MagicMagnify) ? MagicMagnify : null;
    if (e) {
        e.subInit_ = e.subInit;
        e.subInit = function (i, j, l) {
            e.subInit_(i, j, l);
            j.j30("mminitialized", true)
        }
    }
    g.extraEffects = {
        all: $mjs(["MagicThumb", "MagicZoom", "MagicZoomPlus", "MagicMagnifyPlus"]),
        classes: []
    };
    d.defined(window.MagicThumb) && (g.extraEffects.MagicThumb = MagicThumb);
    d.defined(window.MagicZoom) && (g.extraEffects.MagicZoom = MagicZoom);
    d.defined(window.MagicMagnifyPlus) && (g.extraEffects.MagicMagnifyPlus = MagicMagnifyPlus) && (g.extraEffects.MagicThumb = MagicMagnifyPlus);
    d.defined(window.MagicZoomPlus) && (g.extraEffects.MagicZoomPlus = MagicZoomPlus) && (g.extraEffects.MagicThumb = MagicZoomPlus) && (g.extraEffects.MagicZoom = MagicZoomPlus);
    for (var a in g.extraEffects) {
        if (g.extraEffects.all.indexOf(a) != -1) {
            g.extraEffects.classes.push(a)
        }
    }
    g.stopExtraEffects = function (i) {
        d.$A($mjs(i).byTag("A")).j14(function (j) {
            $mjs(g.extraEffects.classes).j14(function (l) {
                $mjs(j).j13(l) && g.extraEffects[l].stop(j)
            })
        })
    };
    g.startExtraEffects = function (i) {
        d.$A($mjs(i).byTag("A")).j14(function (j) {
            $mjs(g.extraEffects.classes).j14(function (l) {
                $mjs(j).j13(l) && g.extraEffects[l].refresh(j)
            })
        })
    };
    if (!d.j21.trident) {
        b.Item.copy_ = b.Item.copy;
        b.Item.copy = function () {
            var i = this.copy_();
            g.startExtraEffects(i);
            return i
        }
    }
    d.extend(g, {
        version: "v1.0.22",
        options: {},
        extraOptions: {},
        _list: {},
        list: function (i) {
            return i ? d.j1(i) == "array" ? i : [i] : d.$AA(this._list)
        },
        init: function (i) {
            d.$A((i || document).getElementsByTagName("div")).j14((function (j) {
                if ($mjs(j).j13("MagicScroll")) {
                    this.start(j)
                }
            }).j24(this))
        },
        start: function (i) {
            return $mjs(this.list(i)).map($mjs(function (j) {
                j = $mjs(j);
                j.j29("MagicScrollID") || this._list[j.$J_UUID] || (this._list[j.$J_UUID] = new g(j));
                return this._list[j.j29("MagicScrollID") || j.$J_UUID].core
            }).j24(this))
        },
        stop: function (i) {
            return $mjs(this.list(i)).map($mjs(function (j) {
                var l = $mjs(j).j29("MagicScrollID");
                this._list[l] && (j = this._list[l].dispose()) && (delete this._list[l]);
                return j
            }).j24(this))
        },
        refresh: function (i) {
            return this.start(this.stop(i))
        },
        jump: function (i, j) {
            d.j1($mjs(i)) == "element" || (j = i) && (i = null);
            $mjs(this.list(i)).j14(function (l) {
                var m = $mjs(l).j29("MagicScrollID");
                this._list[m] && this._list[m].jump(j)
            },
			this)
        },
        scroll: function (i, j) {
            d.j1($mjs(i)) == "element" || (j = i) && (i = null);
            $mjs(this.list(i)).j14(function (l) {
                var m = $mjs(l).j29("MagicScrollID");
                this._list[m] && this._list[m].scroll(j)
            },
			this)
        },
        pause: function (i) {
            $mjs(this.list(i)).j14(function (j) {
                var l = $mjs(j).j29("MagicScrollID");
                this._list[l] && clearTimeout(this._list[l]._auto)
            },
			this);
            return i
        },
        play: function (i) {
            $mjs(this.list(i)).j14(function (j) {
                var l = $mjs(j).j29("MagicScrollID");
                this._list[l] && this._list[l].auto()
            },
			this);
            return i
        }
    });
    $mjs(document).je1("domready",
	function () {
	    g.init()
	});
    var c = new d.Class({
        img: null,
        ready: false,
        options: {
            onload: d.$F,
            onabort: d.$F,
            onerror: d.$F
        },
        size: null,
        _timer: null,
        _handlers: {
            onload: function (i) {
                if (i) {
                    $mjs(i).stop()
                }
                this._unbind();
                if (this.ready) {
                    return
                }
                this.ready = true;
                this._cleanup();
                this.options.onload.j24(null, this).j27(1)
            },
            onabort: function (i) {
                if (i) {
                    $mjs(i).stop()
                }
                this._unbind();
                this.ready = false;
                this._cleanup();
                this.options.onabort.j24(null, this).j27(1)
            },
            onerror: function (i) {
                if (i) {
                    $mjs(i).stop()
                }
                this._unbind();
                this.ready = false;
                this._cleanup();
                this.options.onerror.j24(null, this).j27(1)
            }
        },
        _bind: function () {
            $mjs(["load", "abort", "error"]).j14(function (i) {
                this.img.je1(i, this._handlers["on" + i].j16(this).j28(1))
            },
			this)
        },
        _unbind: function () {
            $mjs(["load", "abort", "error"]).j14(function (i) {
                this.img.je2(i)
            },
			this)
        },
        _cleanup: function () {
            this.j7();
            if (this.img.j29("new")) {
                var i = this.img.parentNode;
                this.img.j33().j31("new").j6({
                    position: "static",
                    top: "auto"
                });
                i.kill()
            }
        },
        init: function (j, i) {
            this.options = d.extend(this.options, i);
            this.img = $mjs(j) || d.$new("img").j32(d.$new("div", null, {
                position: "absolute",
                top: -10000,
                width: 10,
                height: 10,
                overflow: "hidden"
            }).j32(d.body)).j30("new", true);
            var l = function () {
                if (this.isReady()) {
                    this._handlers.onload.call(this)
                } else {
                    this._handlers.onerror.call(this)
                }
                l = null
            } .j24(this);
            this._bind();
            if (!j.src) {
                this.img.src = j
            } else {
                if (d.j21.trident900 && d.j21.ieMode < 9) {
                    this.img.onreadystatechange = function () {
                        if (/loaded|complete/.test(this.img.readyState)) {
                            this.img.onreadystatechange = null;
                            l && l()
                        }
                    } .j24(this)
                }
                this.img.src = j.src
            }
            this.img && this.img.complete && l && (this._timer = l.j27(100))
        },
        destroy: function () {
            if (this._timer) {
                try {
                    clearTimeout(this._timer)
                } catch (i) { }
                this._timer = null
            }
            this._unbind();
            this._cleanup();
            this.ready = false;
            return this
        },
        isReady: function () {
            var j = this.img;
            return (j.naturalWidth) ? (j.naturalWidth > 0) : (j.readyState) ? ("complete" == j.readyState) : j.width > 0
        },
        j7: function () {
            return this.size || (this.size = {
                width: this.img.naturalWidth || this.img.width,
                height: this.img.naturalHeight || this.img.height
            })
        }
    });
    return g
})(magicJS);