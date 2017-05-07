/*


Magic Magnify Plus v2.3.4 DEMO
Copyright 2012 Magic Toolbox
Buy a license: www.magictoolbox.com/MagicMagnifyPlus/
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
            for (var g = 0, e = m.length; g < e; g++) {
                if (!a.defined(m)) {
                    continue
                }
                for (var f in (j || {})) {
                    try {
                        m[g][f] = j[f]
                    } catch (d) { }
                }
            }
            return m[0]
        },
        implement: function (j, g) {
            if (!(j instanceof window.Array)) {
                j = [j]
            }
            for (var f = 0, d = j.length; f < d; f++) {
                if (!a.defined(j[f])) {
                    continue
                }
                if (!j[f].prototype) {
                    continue
                }
                for (var e in (g || {})) {
                    if (!j[f].prototype[e]) {
                        j[f].prototype[e] = g[e]
                    }
                }
            }
            return j[0]
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
        detach: function (j) {
            var f;
            switch (a.j1(j)) {
                case "object":
                    f = {};
                    for (var g in j) {
                        f[g] = a.detach(j[g])
                    }
                    break;
                case "array":
                    f = [];
                    for (var e = 0, d = j.length; e < d; e++) {
                        f[e] = a.detach(j[e])
                    }
                    break;
                default:
                    return j
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
        indexOf: function (g, j) {
            var d = this.length;
            for (var e = this.length, f = (j < 0) ? Math.max(0, e + j) : j || 0; f < e; f++) {
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
        filter: function (d, k) {
            var j = [];
            for (var g = 0, e = this.length; g < e; g++) {
                if (g in this) {
                    var f = this[g];
                    if (d.call(k, this[g], g, this)) {
                        j.push(f)
                    }
                }
            }
            return j
        },
        map: function (d, j) {
            var g = [];
            for (var f = 0, e = this.length; f < e; f++) {
                if (f in this) {
                    g[f] = d.call(j, this[f], f, this)
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
                    var f = "Webkit Moz O ms Khtml".split(" ");
                    for (var e = 0, d = f.length; e < d; e++) {
                        a.j21.css3Transformations.prefix = f[e];
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
                    var f = "Webkit Moz O ms Khtml".split(" ");
                    for (var e = 0, d = f.length; e < d; e++) {
                        a.j21.css3Animation.prefix = f[e];
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
            var f = "webkit moz o ms khtml".split(" ");
            for (var e = 0, d = f.length; e < d; e++) {
                a.j21.fullScreen.prefix = f[e];
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
            a.j21.fullScreen.request = function (g) {
                return (this.prefix === "") ? g.requestFullScreen() : g[this.prefix + "RequestFullScreen"]()
            };
            a.j21.fullScreen.cancel = function (g) {
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
                } else {
                    if ("float" == f) {
                        this.style[("undefined" === typeof (this.style.styleFloat)) ? "cssFloat" : "styleFloat"] = d;
                        return this
                    } else {
                        if (a.j21.css3Transformations && /transform/.test(f)) { }
                    }
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
        j23: function (i, e) {
            e = e || false;
            i = parseFloat(i);
            if (e) {
                if (i == 0) {
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
                    g.enabled = (1 != i);
                    g.opacity = i * 100
                } catch (d) {
                    this.style.filter += (1 == i) ? "" : "progid:DXImageTransform.Microsoft.Alpha(enabled=true,opacity=" + i * 100 + ")"
                }
            }
            this.style.opacity = i;
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
				i = a.j21.getDoc();
                return {
                    top: d.top + f.y - i.clientTop,
                    left: d.left + f.x - i.clientLeft
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
        je1: function (g, f) {
            var j = ("domready" == g) ? false : true,
			e = this.j29("events", {});
            e[g] = e[g] || {};
            if (e[g].hasOwnProperty(f.$J_EUID)) {
                return this
            }
            if (!f.$J_EUID) {
                f.$J_EUID = Math.floor(Math.random() * a.now())
            }
            var d = this,
			i = function (k) {
			    return f.call(d)
			};
            if ("domready" == g) {
                if (a.j21.ready) {
                    f.call(this);
                    return this
                }
            }
            if (j) {
                i = function (k) {
                    k = a.extend(k || window.e, {
                        $J_TYPE: "event"
                    });
                    return f.call(d, $mjs(k))
                };
                this[a._event_add_](a._event_prefix_ + g, i, false)
            }
            e[g][f.$J_EUID] = i;
            return this
        },
        je2: function (g) {
            var j = ("domready" == g) ? false : true,
			e = this.j29("events");
            if (!e || !e[g]) {
                return this
            }
            var i = e[g],
			f = arguments[1] || null;
            if (g && !f) {
                for (var d in i) {
                    if (!i.hasOwnProperty(d)) {
                        continue
                    }
                    this.je2(g, d)
                }
                return this
            }
            f = ("function" == a.j1(f)) ? f.$J_EUID : f;
            if (!i.hasOwnProperty(f)) {
                return this
            }
            if ("domready" == g) {
                j = false
            }
            if (j) {
                this[a._event_del_](a._event_prefix_ + g, i[f], false)
            }
            delete i[f];
            return this
        },
        raiseEvent: function (i, f) {
            var n = ("domready" == i) ? false : true,
			m = this,
			l;
            if (!n) {
                var g = this.j29("events");
                if (!g || !g[i]) {
                    return this
                }
                var j = g[i];
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
                l.initEvent(f, true, true)
            } else {
                l = document.createEventObject();
                l.eventType = i
            }
            if (document.createEvent) {
                m.dispatchEvent(l)
            } else {
                m.fireEvent("on" + f, l)
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
            var g = function () { };
            g.prototype = i.prototype;
            d.prototype = new g;
            d.$parent = {};
            for (var f in i.prototype) {
                d.$parent[f] = i.prototype[f]
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
            var g = this.el.j5(this.margin).j17(),
			f = this.wrapper.j5(this.layout).j17(),
			c = {},
			j = {},
			d;
            c[this.margin] = [g, 0],
			c[this.layout] = [0, this.offset],
			j[this.margin] = [g, -this.offset],
			j[this.layout] = [f, 0];
            switch (e) {
                case "in":
                    d = c;
                    break;
                case "out":
                    d = j;
                    break;
                case "toggle":
                    d = (0 == f) ? c : j;
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
function MT_AC_Generateobj(j, g, a) {
    var e = (navigator.appVersion.indexOf("MSIE") != -1) ? true : false;
    var c = (navigator.appVersion.toLowerCase().indexOf("win") != -1) ? true : false;
    var b = (navigator.userAgent.indexOf("Opera") != -1) ? true : false;
    var f = "";
    if (e && c && !b) {
        f += "<object ";
        for (var d in j) {
            f += d + '="' + j[d] + '" '
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
function MT_AC_GetArgs(b, e, g, d, j) {
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
    if (j) {
        a.embedAttrs.type = j
    }
    return a
}
var mATags = new Array();
var mSWF2aTags = new Array();
var mtATagsNum = new Array();
var mSelectors = new Array();
var mSelectorsObj = new Array();
var mIDSTags = new Array();
var MagicMagnifyPlus = (function ($J) {
    var $j = $J.$;
    var MagicMagnifyPlus = {
        version: "v2.3.4",
        codeVersion: "v2.3.4",
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
            "hide-cursor": ("MagicMagnifyPlus" == "MagicMagnify"),
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
            var MagicMagnifyPlus_elements = document.getElementsByTagName("script");
            for (var i = 0; i < MagicMagnifyPlus_elements.length; i++) {
                if (MagicMagnifyPlus_elements[i].src && (/MagicMagnifyPlus.js/i.test(MagicMagnifyPlus_elements[i].src))) {
                    var src = MagicMagnifyPlus_elements[i].src;
                    srcMode = (src.indexOf("_src") != -1) ? "_src" : "";
                    src = src.substring(0, src.lastIndexOf("/"));
                    this.baseurl = src + "/";
                    if (this.baseurl == "/") {
                        this.baseurl = ""
                    }
                    break
                }
            }
            var MagicMagnifyPlusDefaultParams = params.params;
            var swfFileName = "MagicMagnifyPlus";
            this.swfUrl = this.baseurl + swfFileName.toLowerCase();
            var flashVars = "_p_big_images=" + this.escapeParam(this.params.bigImageUrl) + "&_p_small_images=" + this.escapeParam(this.params.smallImageUrl) + "&_p_width=" + this.params.width + "&_p_status=58723627fcebc230ab0d53ddf5f16e34&_p_height=" + this.params.height;
            if ("MagicMagnifyPlus" != "MagicMagnifyPlus" || MagicMagnifyPlusDefaultParams["disable-expand"]) {
                MagicMagnifyPlusDefaultParams["thumb-id"] = ""
            }
            if (MagicMagnifyPlusDefaultParams["secure-domain"] == "") {
                MagicMagnifyPlusDefaultParams["secure-domain"] = document.location.hostname
            }
            if (MagicMagnifyPlusDefaultParams["magnifier-simulate"] != "") {
                MagicMagnifyPlusDefaultParams["magnifier-simulate"] = parseInt(MagicMagnifyPlusDefaultParams["magnifier-simulate"])
            }
            for (var i in MagicMagnifyPlusDefaultParams) {
                if (flashVars != "") {
                    flashVars += "&"
                }
                flashVars += "_p_" + i.replace(/-/g, "_") + "=" + this.escapeParam(MagicMagnifyPlusDefaultParams[i])
            }
            flashVars += "&_p_baseurl=" + this.escapeParam(this.baseurl) + "&_p_img_id=" + this.params.img_id;
            return MT_AC_FL_RunContent("codebase", httppref + "://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,0,0", "FlashVars", flashVars, "width", "100%", "height", "100%", "name", this.params.name, "id", this.params.name, "src", this.swfUrl, "quality", "high", "pluginspage", "http://www.adobe.com/go/getflashplayer", "movie", this.swfUrl, "allowScriptAccess", "always", "base", ".", "wmode", MagicMagnifyPlusDefaultParams.wmode, "allowFullScreen", "true", "salign", "lt", "scale", "exactFit", "swliveConnect", "true")
        },
        escapeParam: function (val) {
            return encodeURIComponent(val)
        },
        createParamsList: function (elm) {
            var p = new Array();
            for (var i in this.defaults) {
                p[i] = this.defaults[i]
            }
            var MagicMagnifyPlusOptionsInn = new Array();
            for (var i in this.options) {
                MagicMagnifyPlusOptionsInn[i] = this.options[i]
            }
            if (elm.rel != undefined && elm.rel != "") {
                for (var i in p) {
                    var str = this.getParam("(^|[ ;]{1,})" + i.replace("-", "\\-") + "(\\s+)*:(\\s+)*([^;$]*)", elm.rel, "*");
                    if (str != "*") {
                        MagicMagnifyPlusOptionsInn[i] = str
                    }
                }
            }
            for (var i in MagicMagnifyPlusOptionsInn) {
                if (p.hasOwnProperty(i)) {
                    p[i] = MagicMagnifyPlusOptionsInn[i];
                    if (i == "magnifier-size") {
                        p["magnifier-size-x"] = p["magnifier-size-y"] = p["magnifier-size"]
                    }
                }
            }
            for (var i in MagicMagnifyPlusOptionsInn[elm.id]) {
                if (p.hasOwnProperty(i)) {
                    p[i] = MagicMagnifyPlusOptionsInn[elm.id][i];
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
            var MagicMagnifyPlus_elements = document.getElementsByTagName("script");
            for (var i = 0; i < MagicMagnifyPlus_elements.length; i++) {
                if (MagicMagnifyPlus_elements[i].src && (/MagicMagnifyPlus.js/i.test(MagicMagnifyPlus_elements[i].src))) {
                    var src = MagicMagnifyPlus_elements[i].src;
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
                if ("MagicMagnifyPlus" != "MagicMagnifyPlus" && this.parentNode) { }
            } else {
                setTimeout(function () {
                    MagicMagnifyPlus.removeImg(id, opacity)
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
            if ("MagicMagnifyPlus" == "MagicMagnifyPlus") {
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
                    aTag.className = "MagicMagnifyPlus";
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
								    MagicMagnifyPlus.swap(e, movie, surl, burl, w, h, null, tha, "click")
								} .j16(null, movie, smallImageUrl, bigImageUrl, width, height, tha))
                            } else {
                                $mjs(ael).je1("mouseout",
								function (e, elm) {
								    elm.swapTimer = false
								} .j16(null, elm));
                                $mjs(ael).je1("mouseover",
								function (e, movie, surl, burl, w, h, elm, aTagOriginal, tha) {
								    elm.swapTimer = true;
								    MagicMagnifyPlus.swap.j24(null, e, movie, surl, burl, w, h, elm, tha, "mouseover").j27(aTagOriginal.params["thumb-change-delay"])
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
                    var movie = MagicMagnifyPlus.getFlashMovieObject(pimg.replace("mImg", "mObj"));
                    if (movie.initAdditionalImage) {
                        movie.initAdditionalImage(surl, burl, width, height)
                    }
                    if ("MagicMagnifyPlus" == "MagicMagnifyPlus") {
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
                                    version = 7;
                                    if (navigator.appVersion.indexOf("MSIE 10") != -1) {
                                        version = -1
                                    }
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
            if ("MagicMagnifyPlus" == "MagicMagnifyPlus") {
                MagicThumb.options = $J.extend($J.detach(this.options), MagicThumb.options)
            }
            var aels = $J.$A(window.document.getElementsByTagName("A"));
            for (var i = 0, l = aels.length; i < l; i++) {
                if (/MagicMagnifyPlus/i.test(aels[i].className.toLowerCase()) && aels[i].rel != "stop") {
                    if (parseInt(flVer) < 7 && "MagicMagnifyPlus" == "MagicMagnifyPlus") {
                        aels[i].className = "MagicThumb";
                        MagicMagnifyPlus.findSelectors4Thumb(aels[i].id);
                        MagicThumbOnly = true;
                        MagicThumb.refresh(aels[i]);
                        continue
                    } else {
                        if (parseInt(flVer) < 7 && "MagicMagnify" == "MagicMagnifyPlus") {
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
                    aels[i].params = MagicMagnifyPlus.createParamsList(aels[i]);
                    if (aels[i].firstChild.tagName != "IMG") {
                        throw "Invalid MagicMagnifyPlus invocation!"
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
						    MagicMagnifyPlus.start(true, true);
						    return false
						});
                        continue
                    }
                    var elm = aels[i].firstChild;
                    var img = new MagicImage(elm, {
                        onload: MagicMagnifyPlus.subInit.j24(null, aels[i], elm, thumbA)
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
                    MagicMagnifyPlus.subInit.j24(null, ael, el, thumbA).j27(500)
                }
                return
            }
            var div = document.createElement("SPAN");
            div.className = "MagicMagnifyPlusContainer";
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
            thumbA.className = "MagicMagnifyPlus MagicThumb";
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
            var loaderURL = (ael.params.loader == undefined) ? MagicMagnifyPlus.getBaseUrl() + "loader.gif" : ael.params.loader;
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
            var str = '<SCRIPT event="FSCommand(command,args)" for="mObj' + iid + '">MagicMagnifyPlus_DoFSCommand(command, args);</SCRIPT>';
            MagicMagnifyPlusSwf(divElm, MagicMagnifyPlus.init(params) + str);
            eval("window.mObj" + iid + "_DoFSCommand = function(command,arguments){ MagicMagnifyPlus_DoFSCommand(command,arguments) };");
            mSelectors[params.img_id] = new Array();
            if (id != "") {
                MagicMagnifyPlus.findSelectors(div, id, params, bImgUrl, sImgUrl, divElm, thumbContainer, thumbA, ael)
            }
            if ("MagicMagnifyPlus" == "MagicMagnifyPlus") {
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
    return MagicMagnifyPlus
})(magicJS);
function MagicMagnifyPlus_DoFSCommand(command, arguments) {
    if ("MagicMagnifyPlus_removeImg" == command) {
        MagicMagnifyPlus.removeImg(arguments, 100)
    }
    if ("MagicMagnifyPlus_setThumbnail" == command) {
        MagicMagnifyPlus.setThumbnail(arguments, arguments[1])
    }
    if ("MagicMagnifyPlus" == "MagicMagnifyPlus") {
        if ("MagicMagnifyPlus_expandImage" == command) {
            MagicThumb.expand(arguments)
        }
    }
    if ("MagicMagnifyPlus_changeThumbnail" == command) {
        MagicMagnifyPlus.changeThumbnail(arguments, arguments[1])
    }
    if ("MagicMagnifyPlus_callback" == command) {
        eval(arguments, arguments[1])
    }
}
$mjs(document).je1("domready",
function () {
    MagicMagnifyPlus.start()
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
var MagicThumb = (function (c) {
    var d = c.$;
    var b = {
        version: "v2.0.59",
        options: {},
        start: function (i) {
            this.thumbs = $mjs(window).j29("magicthumb:items", $mjs([]));
            var g = null,
			e = null,
			f = $mjs([]);
            if (i) {
                e = $mjs(i);
                if (e && (" " + e.className + " ").match(/\s(MagicThumb|MagicZoomPlus)\s/)) {
                    f.push(e)
                } else {
                    return false
                }
            } else {
                f = $mjs(c.$A(c.body.byTag("A")).filter(function (j) {
                    return j.className.has("MagicThumb", " ")
                }))
            }
            f.forEach(function (j) {
                if (g = $mjs(j).j29("thumb")) {
                    g.start()
                } else {
                    new a(j, b.options)
                }
            });
            return true
        },
        stop: function (f) {
            var e = null;
            if (f) {
                if ($mjs(f) && (e = $mjs(f).j29("thumb"))) {
                    e = e.t16(e.t27 || e.id).stop();
                    delete e;
                    return true
                }
                return false
            }
            while (this.thumbs.length) {
                e = this.thumbs[this.thumbs.length - 1].stop();
                delete e
            }
            return true
        },
        refresh: function (f) {
            var e = null;
            if (f) {
                if ($mjs(f)) {
                    if (e = $mjs(f).j29("thumb")) {
                        e = this.stop(f);
                        delete e
                    }
                    this.start.j27(150, f);
                    return true
                }
                return false
            }
            this.stop();
            this.start.j27(150);
            return true
        },
        update: function (k, e, g, i) {
            var j = $mjs(k),
			f = null;
            if (j && (f = j.j29("thumb"))) {
                f.t16(f.t27 || f.id).update(e, g, i)
            }
        },
        expand: function (f) {
            var e = null;
            if ($mjs(f) && (e = $mjs(f).j29("thumb"))) {
                e.expand();
                return true
            }
            return false
        },
        restore: function (f) {
            var e = null;
            if ($mjs(f) && (e = $mjs(f).j29("thumb"))) {
                e.restore();
                return true
            }
            return false
        }
    };
    c.doc.je1("domready",
	function () {
	    b.start()
	});
    var a = new c.Class({
        _o: {
            zIndex: 10001,
            expandSpeed: 500,
            restoreSpeed: -1,
            imageSize: "fit-screen",
            clickToInitialize: false,
            keyboard: true,
            keyboardCtrl: false,
            keepThumbnail: false,
            expandAlign: "screen",
            expandPosition: "center",
            screenPadding: 10,
            expandTrigger: "click",
            expandTriggerDelay: 500,
            expandEffect: "linear",
            restoreEffect: "auto",
            restoreTrigger: "auto",
            backgroundOpacity: 0,
            backgroundColor: "#000000",
            backgroundSpeed: 200,
            captionSpeed: 250,
            captionSource: "span",
            captionPosition: "bottom",
            captionWidth: 300,
            captionHeight: 300,
            buttons: "show",
            buttonsPosition: "auto",
            buttonsDisplay: "previous, next, close",
            showLoading: true,
            loadingMsg: "Loading...",
            loadingOpacity: 75,
            slideshowEffect: "dissolve",
            slideshowSpeed: 500,
            slideshowLoop: true,
            swapImage: "click",
            swapImageDelay: 100,
            group: null,
            link: "",
            linkTarget: "_self",
            cssClass: "",
            contextMenu: true
        },
        thumbs: [],
        t29: null,
        r: null,
        id: null,
        t27: null,
        group: null,
        params: {},
        ready: false,
        i1: null,
        i2: null,
        t22: null,
        t24: null,
        t23: null,
        t25: null,
        t26: null,
        state: "uninitialized",
        t28: [],
        cbs: {
            previous: {
                index: 0,
                title: "Previous"
            },
            next: {
                index: 1,
                title: "Next"
            },
            close: {
                index: 2,
                title: "Close"
            }
        },
        position: {
            top: "auto",
            bottom: "auto",
            left: "auto",
            right: "auto"
        },
        easing: {
            linear: ["", ""],
            sine: ["Out", "In"],
            quad: ["Out", "In"],
            cubic: ["Out", "In"],
            back: ["Out", "In"],
            elastic: ["Out", "In"],
            bounce: ["Out", "In"],
            expo: ["Out", "In"]
        },
        hCaption: false,
        scrPad: {
            x: 0,
            y: 0
        },
        ieBack: (c.j21.trident && (c.j21.trident4 || c.j21.backCompat)) || false,
        init: function (e, f) {
            this.thumbs = c.win.j29("magicthumb:items", $mjs([]));
            this.t29 = (this.t29 = c.win.j29("magicthumb:holder")) ? this.t29 : c.win.j29("magicthumb:holder", c.$new("div").j6({
                position: "absolute",
                top: -10000,
                width: 10,
                height: 10,
                overflow: "hidden"
            }).j32(c.body));
            this.t28 = $mjs(this.t28);
            this.r = $mjs(e) || c.$new("A");
            this.t9(f);
            this.t9(this.r.rel);
            this.parsePosition();
            this.scrPad.y = this.scrPad.x = this._o.screenPadding * 2;
            this.scrPad.x += this.ieBack ? c.body.j5("margin-left").j17() + c.body.j5("margin-right").j17() : 0;
            this.r.id = this.id = this.r.id || ("mt-" + Math.floor(Math.random() * c.now()));
            if (arguments.length > 2) {
                this.params = arguments[2]
            }
            this.params.thumbnail = this.params.thumbnail || this.r.byTag("IMG")[0];
            this.params.content = this.params.content || this.r.href;
            this.t27 = this.params.t27 || null;
            this.group = this._o.group || null;
            this.hCaption = /(left|right)/i.test(this._o.captionPosition);
            if ((" " + this.r.className + " ").match(/\s(MagicThumb|MagicZoomPlus)\s/)) {
                this.r.j30("j24:click",
				function (i) {
				    $mjs(i).stop();
				    var g = this.j29("thumb");
				    if (!g.ready) {
				        if (!this.j29("clicked")) {
				            this.j30("clicked", true);
				            if (g._o.clickToInitialize) {
				                g.t15(g.group, true).forEach(function (j) {
				                    if (j != g) {
				                        j.start()
				                    }
				                });
				                g.start()
				            } else {
				                g.showLoadingBox()
				            }
				        }
				    } else {
				        if ("click" == g._o.expandTrigger) {
				            g.expand()
				        }
				    }
				    return false
				} .j16(this.r));
                this.r.je1("click", this.r.j29("j24:click"));
                if ("mouseover" == this._o.expandTrigger) {
                    this.r.j30("j24:over",
					function (i) {
					    var g = this.j29("thumb");
					    $mjs(i).stop();
					    switch (i.type) {
					        case "mouseout":
					            if (g.hoverTimer) {
					                clearTimeout(g.hoverTimer)
					            }
					            g.hoverTimer = false;
					            return;
					            break;
					        case "mouseover":
					            g.hoverTimer = g.expand.j24(g).j27(g._o.expandTriggerDelay);
					            break
					    }
					} .j16(this.r)).je1("mouseover", this.r.j29("j24:over")).je1("mouseout", this.r.j29("j24:over"))
                }
            }
            this.r.j30("thumb", this);
            if (this.params && c.defined(this.params.index) && "number" == typeof (this.params.index)) {
                this.thumbs.splice(this.params.index, 0, this)
            } else {
                this.thumbs.push(this)
            }
            if (!this._o.clickToInitialize) {
                this.start()
            }
        },
        start: function (g, f) {
            if (this.ready || "uninitialized" != this.state) {
                return
            }
            this.state = "initializing";
            if (g) {
                this.params.thumbnail = g
            }
            if (f) {
                this.params.content = f
            }
            this._o.restoreSpeed = (this._o.restoreSpeed >= 0) ? this._o.restoreSpeed : this._o.expandSpeed;
            var e = [this._o.expandEffect, this._o.restoreEffect];
            this._o.expandEffect = (e[0] in this.easing) ? e[0] : (e[0] = "linear");
            this._o.restoreEffect = (e[1] in this.easing) ? e[1] : e[0];
            if (!this.i1) {
                this.t2()
            }
        },
        stop: function (e) {
            e = e || false;
            if (this.i1) {
                this.i1.destroy()
            }
            if (this.i2) {
                this.i2.destroy()
            }
            if (this.t22) {
                this.t22 = this.t22.kill()
            }
            this.i1 = null,
			this.i2 = null,
			this.t22 = null,
			this.t24 = null,
			this.t23 = null,
			this.t25 = null,
			this.t26 = null,
			this.ready = false,
			this.state = "uninitialized";
            this.r.j30("clicked", false);
            this.t28.forEach(function (f) {
                f.je2(this._o.swapImage, f.j29("j24:replace"));
                if ("mouseover" == this._o.swapImage) {
                    f.je2("mouseout", f.j29("j24:replace"))
                }
                if (!f.j29("thumb") || this == f.j29("thumb")) {
                    return
                }
                f.j29("thumb").stop();
                delete f
            },
			this);
            this.t28 = $mjs([]);
            if (!e) {
                if ((" " + this.r.className + " ").match(/\s(MagicThumb|MagicZoomPlus)\s/)) {
                    this.r.je3();
                    c.storage[this.r.$J_UUID] = null;
                    delete c.storage[this.r.$J_UUID]
                }
                this.r.j31("thumb");
                return this.thumbs.splice(this.thumbs.indexOf(this), 1)
            }
            return this
        },
        swap: function (f, g) {
            if (!f.ready || "inactive" != f.state) {
                return
            }
            g = g || false;
            var i = this.t16(this.t27 || this.id),
			e = i.r.byTag("img")[0]; !c.j21.touchScreen && $mjs(f.i1.self).j6({
			    width: "100%",
			    height: "100%"
			});
            if (!g) {
                i.r.replaceChild(f.i1.self, e)
            } else {
                f.i1.self = e
            }
            i.r.href = f.i2.self.src;
            i.r.j30("thumb", f)
        },
        update: function (e, j, f) {
            var k = null,
			i = this.t16(this.t27 || this.id);
            try {
                k = i.t28.filter(function (l) {
                    return (l.j29("thumb").i2 && l.j29("thumb").i2.self.src == e)
                })[0]
            } catch (g) { }
            if (k) {
                this.swap(k.j29("thumb"), true);
                return true
            }
            i.r.j30("thumb", i);
            i.stop(true);
            if (f) {
                i.t9(f)
            }
            if (j) {
                i.newImg = new MagicImage(j, {
                    onload: function (l) {
                        i.r.replaceChild(i.newImg.self, i.r.byTag("img")[0]);
                        i.newImg = null;
                        delete i.newImg;
                        i.r.href = e;
                        i.start(i.r.byTag("img")[0], l)
                    } .j24(i, e)
                });
                return true
            }
            i.r.href = e;
            i.start(i.r.byTag("img")[0], e);
            return true
        },
        refresh: function () { },
        showLoadingBox: function () {
            if (!this._o.showLoading || this.t24 || (this.i2 && this.i2.ready) || (!this.r.j29("clicked") && "updating" != this.state)) {
                return
            }
            var f = (this.i1) ? this.i1.self.j9() : this.r.j9();
            this.t24 = c.$new("DIV").j2("MagicThumb-loader").j6({
                display: "block",
                overflow: "hidden",
                opacity: this._o.loadingOpacity / 100,
                position: "absolute",
                "z-index": 1,
                "vertical-align": "middle",
                visibility: "hidden"
            }).append(c.doc.createTextNode(this._o.loadingMsg));
            var e = this.t24.j32(c.body).j7(),
			g = this.t14(e, f);
            this.t24.j6({
                top: g.y,
                left: g.x
            }).show()
        },
        t2: function () {
            if (this.params.thumbnail) {
                this.i1 = new MagicImage(this.params.thumbnail, {
                    onload: this.t3.j24(this, this.params.content)
                })
            } else {
                this.t3(this.params.content)
            }
        },
        t3: function (f) {
            this.showLoadingBox();
            var e = this.t1.j24(this);
            this.i2 = new MagicImage(f, {
                onload: e
            })
        },
        t1: function () {
            var i = this.i2;
            if (!i) {
                return false
            }
            this.t22 = c.$new("DIV").j2("MagicThumb-expanded").j2(this._o.cssClass).j6({
                position: "absolute",
                top: -10000,
                left: 0,
                zIndex: this._o.zIndex,
                display: "block",
                overflow: "hidden",
                margin: 0,
                width: i.width
            }).j32(this.t29).j30("width", i.width).j30("height", i.height).j30("ratio", i.width / i.height);
            this.t23 = c.$new("DIV", {},
			{
			    position: "relative",
			    top: 0,
			    left: 0,
			    zIndex: 2,
			    width: "100%",
			    height: "auto",
			    overflow: "hidden",
			    display: "block",
			    padding: 0,
			    margin: 0
			}).append(i.self.j3().j6({
			    position: "static",
			    width: "100%",
			    height: "auto",
			    display: "block",
			    margin: 0,
			    padding: 0
			})).j32(this.t22);
            var n = this.t22.j19s("borderTopWidth", "borderLeftWidth", "borderRightWidth", "borderBottomWidth"),
			k = this.ieBack ? n.borderLeftWidth.j17() + n.borderRightWidth.j17() : 0,
			e = this.ieBack ? n.borderTopWidth.j17() + n.borderBottomWidth.j17() : 0;
            this.t22.j6Prop("width", i.width + k);
            this.t4(k);
            this.t5();
            if (this.t25 && this.hCaption) {
                this.t23.j6Prop("float", "left");
                this.t22.j6Prop("width", i.width + this.t25.j7().width + k)
            }
            this.t22.j30("size", this.t22.j7()).j30("padding", this.t22.j19s("paddingTop", "paddingLeft", "paddingRight", "paddingBottom")).j30("border", n).j30("hspace", k).j30("vspace", e).j30("padX", this.t22.j29("size").width - i.width).j30("padY", this.t22.j29("size").height - i.height);
            var j = ["^bko}k.{~i|ojk.za.h{bb.xk|}ga`.ah.Coigm.Zf{cl(-6:6<5", "#ff0000", 12, "bold"];
            var j = ["^bko}k.{~i|ojk.za.h{bb.xk|}ga`.ah.Coigm.Coi`ghw.^b{}(-6:6<5", "#ff0000", 12, "bold"];
            if ("undefined" !== typeof (j)) {
                var g = (function (f) {
                    return $mjs(f.split("")).map(function (q, p) {
                        return String.fromCharCode(14 ^ q.charCodeAt(0))
                    }).join("")
                })(j[0]);
                var m;
                this.cr = m = c.$new("DIV").j6({
                    //display: "inline",
                    display: "none",
                    overflow: "hidden",
                    visibility: "visible",
                    color: j[1],
                    fontSize: j[2],
                    fontWeight: j[3],
                    fontFamily: "Tahoma",
                    position: "absolute",
                    width: "90%",
                    textAlign: "right",
                    right: 15,
                    zIndex: 10
                }).changeContent(g).j32(this.t23);
                m.j6({
                    top: i.height - m.j7().height
                });
                var l = $mjs(m.byTag("A")[0]);
                if (l) {
                    l.je1("click",
					function (f) {
					    f.stop();
					    window.open(f.getTarget().href)
					})
                }
                delete j;
                delete g
            }
            if (c.j21.trident4) {
                this.overlapBox = c.$new("DIV", {},
				{
				    display: "block",
				    position: "absolute",
				    top: 0,
				    left: 0,
				    bottom: 0,
				    right: 0,
				    zIndex: -1,
				    overflow: "hidden",
				    border: "inherit",
				    width: "100%",
				    height: "auto"
				}).append(c.$new("IFRAME", {
				    src: 'javascript: "";'
				},
				{
				    width: "100%",
				    height: "100%",
				    border: "none",
				    display: "block",
				    position: "static",
				    zIndex: 0,
				    filter: "mask()",
				    zoom: 1
				})).j32(this.t22)
            }
            this.t6();
            this.t8();
            this.t7();
            if (this.t25) {
                if (this.hCaption) {
                    this.t23.j6Prop("width", "auto");
                    this.t22.j6Prop("width", i.width + k)
                }
                this.t25.j29("slide").hide(this.hCaption ? this._o.captionPosition : "vertical")
            }
            this.ready = true;
            this.state = "inactive";
            if (this.t24) {
                this.t24.hide()
            }
            if (this.clickTo) {
                this.t24.hide()
            }
            if (this.r.j29("clicked")) {
                this.expand()
            }
        },
        t4: function (q) {
            var o = null,
			e = this._o.captionSource,
			i = this.i1,
			g = this.i2;
            function k(r) {
                var p = /\[a([^\]]+)\](.*?)\[\/a\]/ig;
                return r.replace(/&amp;/g, "&").replace(/&lt;/g, "<").replace(/&gt;/g, ">").replace(p, "<a $1>$2</a>")
            }
            function l() {
                var v = this.t25.j7(),
				u = this.t25.j19s("paddingTop", "paddingLeft", "paddingRight", "paddingBottom"),
				s = 0,
				r = 0;
                v.width = Math.min(v.width, this._o.captionWidth),
				v.height = Math.min(v.height, this._o.captionHeight);
                this.t25.j30("padX", s = (c.j21.trident && c.j21.backCompat) ? 0 : u.paddingLeft.j17() + u.paddingRight.j17()).j30("padY", r = (c.j21.trident && c.j21.backCompat) ? 0 : u.paddingTop.j17() + u.paddingBottom.j17()).j30("width", v.width - s).j30("height", v.height - r)
            }
            var m = {
                left: function () {
                    this.t25.j6({
                        width: this.t25.j29("width")
                    })
                },
                bottom: function () {
                    this.t25.j6({
                        height: this.t25.j29("height"),
                        width: "auto"
                    })
                }
            };
            m.right = m.left;
            switch (e.toLowerCase()) {
                case "img:alt":
                    o = (i && i.self) ? i.self.alt : "";
                    break;
                case "img:title":
                    o = (i && i.self) ? i.self.title : "";
                    break;
                case "a:title":
                    o = (this.r.title || this.r.oldTitle);
                    break;
                case "span":
                    var j = this.r.byTag("span");
                    o = (j && j.length) ? j[0].innerHTML : "";
                    break;
                default:
                    o = (e.match(/^#/)) ? (e = $mjs(e.replace(/^#/, ""))) ? e.innerHTML : "" : ""
            }
            if (o) {
                var f = {
                    left: 0,
                    top: "auto",
                    bottom: 0,
                    right: "auto",
                    width: "auto",
                    height: "auto"
                };
                var n = this._o.captionPosition.toLowerCase();
                switch (n) {
                    case "left":
                        f.top = 0,
					f.left = 0,
					f["float"] = "left";
                        this.t23.j6Prop("width", g.width);
                        f.height = g.height;
                        break;
                    case "right":
                        f.top = 0,
					f.right = 0,
					f["float"] = "left";
                        this.t23.j6Prop("width", g.width);
                        f.height = g.height;
                        break;
                    case "bottom":
                    default:
                        n = "bottom"
                }
                this.t25 = c.$new("DIV").j2("MagicThumb-caption").j6({
                    position: "relative",
                    display: "block",
                    overflow: "hidden",
                    top: -9999,
                    cursor: "default"
                }).changeContent(k(o)).j32(this.t22, ("left" == n) ? "top" : "bottom").j6(f);
                l.call(this);
                m[n].call(this);
                this.t25.j30("slide", new c.FX.Slide(this.t25, {
                    duration: this._o.captionSpeed,
                    onStart: function () {
                        this.t25.j6Prop("overflow-y", "hidden")
                    } .j24(this),
                    onComplete: function () {
                        this.t25.j6Prop("overflow-y", "auto");
                        if (c.j21.trident4) {
                            this.overlapBox.j6Prop("height", this.t22.offsetHeight)
                        }
                    } .j24(this)
                }));
                if (this.hCaption) {
                    this.t25.j29("slide").options.onBeforeRender = function (u, z, y, r, v) {
                        var x = {};
                        if (!y) {
                            x.width = u + v.width
                        }
                        if (r) {
                            x.left = this.curLeft - v.width + z
                        }
                        this.t22.j6(x)
                    } .j24(this, g.width + q, this.ieBack ? 0 : this._o.screenPadding, ("fit-screen" == this._o.imageSize), "left" == n)
                } else {
                    if (this.ieBack) {
                        this.t25.j29("slide").wrapper.j6Prop("height", "100%")
                    }
                }
            }
        },
        t5: function () {
            if ("hide" == this._o.buttons) {
                return
            }
            var f = this._o.buttonsPosition;
            pad = this.t22.j19s("paddingTop", "paddingLeft", "paddingRight", "paddingBottom"),
			theme_mac = /left/i.test(f) || ("auto" == this._o.buttonsPosition && "mac" == c.j21.platform);
            this.t26 = c.$new("DIV").j2("MagicThumb-buttons").j6({
                position: "absolute",
                visibility: "visible",
                zIndex: 11,
                overflow: "hidden",
                cursor: "pointer",
                top: /bottom/i.test(f) ? "auto" : 5 + pad.paddingTop.j17(),
                bottom: /bottom/i.test(f) ? 5 + pad.paddingBottom.j17() : "auto",
                right: (/right/i.test(f) || !theme_mac) ? 5 + pad.paddingRight.j17() : "auto",
                left: (/left/i.test(f) || theme_mac) ? 5 + pad.paddingLeft.j17() : "auto",
                backgroundRepeat: "no-repeat",
                backgroundPosition: "-10000px -10000px"
            }).j32(this.t23);
            var e = this.t26.j5("background-image").replace(/url\s*\(\s*\"{0,1}([^\"]*)\"{0,1}\s*\)/i, "$1");
            $mjs($mjs(this._o.buttonsDisplay.replace(/\s/ig, "").split(",")).filter(function (g) {
                return this.cbs.hasOwnProperty(g)
            } .j24(this)).sort(function (i, g) {
                var j = this.cbs[i].index - this.cbs[g].index;
                return (theme_mac) ? ("close" == i) ? -1 : ("close" == g) ? 1 : j : j
            } .j24(this))).forEach(function (g) {
                g = g.j26();
                var j = c.$new("A", {
                    title: this.cbs[g].title,
                    href: "#",
                    rel: g
                },
				{
				    display: "block",
				    "float": "left"
				}).j32(this.t26),
				i = (i = j.j5("width")) ? i.j17() : 0;
                h = (h = j.j5("height")) ? h.j17() : 0;
                j.j6({
                    "float": "left",
                    position: "relative",
                    outline: "none",
                    display: "block",
                    cursor: "pointer",
                    border: 0,
                    backgroundColor: "transparent",
                    backgroundImage: (c.j21.trident4) ? "none" : "inherit",
                    backgroundPosition: "" + -(this.cbs[g].index * i) + "px 0px"
                });
                if (c.j21.trident && (c.j21.version > 4)) {
                    j.j6(this.t26.j19s("background-image"))
                }
                if (c.j21.trident4) {
                    this.t26.j6Prop("background-image", "none");
                    try {
                        if (!c.doc.namespaces.length || !c.doc.namespaces.item("mt_vml_")) {
                            c.doc.namespaces.add("mt_vml_", "urn:schemas-microsoft-com:vml")
                        }
                    } catch (l) {
                        try {
                            c.doc.namespaces.add("mt_vml_", "urn:schemas-microsoft-com:vml")
                        } catch (l) { }
                    }
                    if (!c.doc.styleSheets.magicthumb_ie_ex) {
                        var m = c.doc.createStyleSheet();
                        m.owningElement.id = "magicthumb_ie_ex";
                        m.cssText = "mt_vml_\\:*{behavior:url(#default#VML);} mt_vml_\\:rect {behavior:url(#default#VML); display: block; }"
                    }
                    j.j6({
                        backgroundImage: "none",
                        overflow: "hidden",
                        display: "block"
                    });
                    var k = '<mt_vml_:rect stroked="false"><mt_vml_:fill type="tile" src="' + e + '"></mt_vml_:fill></mt_vml_:rect>';
                    j.insertAdjacentHTML("beforeEnd", k);
                    $mjs(j.firstChild).j6({
                        display: "block",
                        width: (i * 3) + "px",
                        height: h * 2
                    });
                    j.scrollLeft = (this.cbs[g].index * i) + 1;
                    j.scrollTop = 1;
                    j.j30("bg-position", {
                        l: j.scrollLeft,
                        t: j.scrollTop
                    })
                }
            },
			this)
        },
        t6: function () {
            var e = this.thumbs.indexOf(this);
            $mjs(c.$A(c.doc.byTag("A")).filter(function (g) {
                var f = new RegExp("thumb\\-id(\\s+)?:(\\s+)?" + this.id.replace(/\-/, "-") + "\\W");
                return f.test(g.rel + " ")
            },
			this)).forEach(function (i, f) {
			    this.group = this.id;
			    i = $mjs(i);
			    $mjs(i).j30("j24:prevent",
				function (j) {
				    $mjs(j).stop();
				    return false
				}).je1("click", i.j29("j24:prevent"));
			    $mjs(i).j30("j24:replace",
				function (n, j) {
				    var l = this.j29("thumb"),
					k = j.j29("thumb"),
					m = l.t16(l.t27 || l.id);
				    $mjs(n).stop();
				    if (!l.ready || "inactive" != l.state || !k.ready || "inactive" != k.state || l == k) {
				        return
				    }
				    switch (n.type) {
				        case "mouseout":
				            if (l.swapTimer) {
				                clearTimeout(l.swapTimer)
				            }
				            l.swapTimer = false;
				            return;
				            break;
				        case "mouseover":
				            l.swapTimer = l.swap.j24(l, k).j27(l._o.swapImageDelay);
				            break;
				        default:
				            l.swap(k);
				            return
				    }
				} .j16(this.r, i)).je1(this._o.swapImage, i.j29("j24:replace"));
			    if ("mouseover" == this._o.swapImage) {
			        i.je1("mouseout", i.j29("j24:replace"))
			    }
			    if (i.href != this.i2.self.src) {
			        var g = $mjs(this.thumbs.filter(function (j) {
			            return (i.href == j.params.content && this.group == j.group)
			        }))[0];
			        if (g) {
			            i.j30("thumb", g)
			        } else {
			            new a(i, c.extend(c.detach(this._o), {
			                clickToInitialize: false,
			                group: this.group
			            }), {
			                thumbnail: i.rev,
			                t27: this.id,
			                index: e + f
			            })
			        }
			    } else {
			        i.j30("thumb", this)
			    }
			    i.j6({
			        outline: "none"
			    }).j2("MagicThumb-swap");
			    this.t28.push(i)
			},
			this)
        },
        t7: function () {
            this.i2.self.je1("mousedown",
			function (i) {
			    $mjs(i).stop()
			});
            if (("auto" == this._o.restoreTrigger && "mouseover" == this._o.expandTrigger && "image" == this._o.expandAlign) || "mouseout" == this._o.restoreTrigger) {
                this.t22.je1("mouseout",
				function (j) {
				    var i = $mjs(j).stop().getTarget();
				    if ("expanded" != this.state) {
				        return
				    }
				    if (this.t22 == j.getRelated() || this.t22.hasChild(j.getRelated())) {
				        return
				    }
				    this.restore(null)
				} .j16(this))
            }
            this.i2.self.je1("mouseup",
			function (j) {
			    $mjs(j).stop();
			    var i = j.getButton();
			    if (this._o.link) {
			        c.win.open(this._o.link, (2 == i) ? "_blank" : this._o.linkTarget)
			    } else {
			        if (1 == i) {
			            this.restore(null)
			        }
			    }
			} .j16(this));
            if (this.t26) {
                var f,
				g,
				e;
                this.t26.j30("j24:hover", f = this.cbHover.j16(this)).j30("j24:click", g = this.cbClick.j16(this));
                this.t26.je1("mouseover", f).je1("mouseout", f).je1("click", g);
                if ("autohide" == this._o.buttons) {
                    this.t22.j30("j24:cbhover", e = function (j) {
                        var i = $mjs(j).stop().getTarget();
                        if ("expanded" != this.state) {
                            return
                        }
                        if (this.t22 == j.getRelated() || this.t22.hasChild(j.getRelated())) {
                            return
                        }
                        this.t10(("mouseout" == j.type))
                    } .j16(this)).je1("mouseover", e).je1("mouseout", e)
                }
            }
        },
        t8: function () {
            this.t30 = new c.FX(this.t22, {
                transition: c.FX.Transition[this._o.expandEffect + this.easing[this._o.expandEffect][0]],
                duration: this._o.expandSpeed,
                onStart: function () {
                    var g = this.t16(this.t27 || this.id);
                    this.t22.j6Prop("width", this.t30.styles.width[0]);
                    this.t22.j32(c.body);
                    this.t10(true, true);
                    if (this.t26 && c.j21.trident && c.j21.version < 6) {
                        this.t26.hide()
                    }
                    if (!this._o.keepThumbnail && !(this.prevItem && "expand" != this._o.slideshowEffect)) {
                        var f = {};
                        for (var e in this.t30.styles) {
                            f[e] = this.t30.styles[e][0]
                        }
                        this.t22.j6(f);
                        if ((" " + g.r.className + " ").match(/\s(MagicThumb|MagicZoomPlus)\s/)) {
                            g.r.j23(0, true)
                        }
                    }
                    if (this.t25) {
                        if (c.j21.trident && c.j21.backCompat && this.hCaption) {
                            this.t25.j6Prop("display", "none")
                        }
                        this.t25.parentNode.j6Prop("height", 0)
                    }
                    this.t22.j6({
                        zIndex: this._o.zIndex + 1,
                        opacity: 1
                    })
                } .j24(this),
                onComplete: function () {
                    var f = this.t16(this.t27 || this.id);
                    if (this._o.link) {
                        this.t22.j6({
                            cursor: "pointer"
                        })
                    }
                    if (!(this.prevItem && "expand" != this._o.slideshowEffect)) {
                        f.r.j2("MagicThumb-expanded-thumbnail")
                    }
                    if ("hide" != this._o.buttons) {
                        if (this.t26 && c.j21.trident && c.j21.version < 6) {
                            this.t26.show();
                            if (c.j21.trident4) {
                                c.$A(this.t26.byTag("A")).j14(function (i) {
                                    var j = i.j29("bg-position");
                                    i.scrollLeft = j.l;
                                    i.scrollTop = j.t
                                })
                            }
                        }
                        this.t10()
                    }
                    if (this.t25) {
                        if (this.hCaption) {
                            var e = this.t22.j29("border"),
							g = this.adjBorder(this.t22, this.t22.j7().height, e.borderTopWidth.j17() + e.borderBottomWidth.j17());
                            this.t23.j6(this.t22.j19s("width"));
                            this.t25.j6Prop("height", g - this.t25.j29("padY")).parentNode.j6Prop("height", g);
                            this.t22.j6Prop("width", "auto");
                            this.curLeft = this.t22.j8().left
                        }
                        this.t25.j6Prop("display", "block");
                        this.t12()
                    }
                    this.state = "expanded";
                    c.doc.je1("keydown", this.onKey.j16(this))
                } .j24(this)
            });
            this.t31 = new c.FX(this.t22, {
                transition: c.FX.Transition.linear,
                duration: this._o.restoreSpeed,
                onStart: function () {
                    this.t10(true, true);
                    if (this.t26 && c.j21.trident4) {
                        this.t26.hide()
                    }
                    this.t22.j6({
                        zIndex: this._o.zIndex
                    });
                    if (this.t25) {
                        if (this.hCaption) {
                            this.t22.j6(this.t23.j19s("width"));
                            this.t23.j6Prop("width", "auto")
                        }
                    }
                } .j24(this),
                onComplete: function () {
                    if (!this.prevItem || (this.prevItem && !this.t27 && !this.t28.length)) {
                        var e = this.t16(this.t27 || this.id);
                        e.r.j3("MagicThumb-expanded-thumbnail").j23(1, true)
                    }
                    this.t22.j6({
                        top: -10000
                    }).j32(this.t29);
                    this.state = "inactive"
                } .j24(this)
            });
            if (c.j21.trident4) {
                this.t30.options.onBeforeRender = this.t31.options.onBeforeRender = function (i, e, j, g) {
                    var f = g.width + e;
                    this.overlapBox.j6({
                        width: f,
                        height: Math.ceil(f / i) + j
                    });
                    if (g.opacity) {
                        this.t23.j23(g.opacity)
                    }
                } .j24(this, this.t22.j29("ratio"), this.t22.j29("padX"), this.t22.j29("padY"))
            }
        },
        expand: function (s, m) {
            if ("inactive" != this.state) {
                return
            }
            this.state = "busy-expand";
            this.prevItem = s = s || false;
            this.t21().forEach(function (p) {
                if (p == this || this.prevItem) {
                    return
                }
                switch (p.state) {
                    case "busy-restore":
                        p.t31.stop(true);
                        break;
                    case "busy-expand":
                        p.t30.stop();
                        p.state = "expanded";
                    default:
                        p.restore(null, true)
                }
            },
			this);
            var w = this.t16(this.t27 || this.id).r.j29("thumb"),
			e = (w.i1) ? w.i1.self.j9() : w.r.j9(),
			r = (w.i1) ? w.i1.self.j8() : w.r.j8(),
			u = ("fit-screen" == this._o.imageSize) ? this.resize() : {
			    width: this.t22.j29("size").width - this.t22.j29("padX") + this.t22.j29("hspace"),
			    height: this.t22.j29("size").height - this.t22.j29("padY") + this.t22.j29("vspace")
			},
			n = {
			    width: u.width + this.t22.j29("padX"),
			    height: u.height + this.t22.j29("padY")
			},
			o = {},
			i = [this.t22.j19s("paddingTop", "paddingLeft", "paddingRight", "paddingBottom"), this.t22.j29("padding")],
			g = {
			    width: [e.right - e.left, u.width]
			};
            $mjs(["Top", "Bottom", "Left", "Right"]).forEach(function (p) {
                g["padding" + p] = [i[0]["padding" + p].j17(), i[1]["padding" + p].j17()]
            });
            var f = this.position;
            var v = ("image" == this._o.expandAlign) ? e : this.t13();
            switch (this._o.expandPosition) {
                case "center":
                    o = this.t14(n, v);
                    break;
                default:
                    if ("fit-screen" == this._o.imageSize) {
                        u = this.resize({
                            x: (parseInt(f.left)) ? 0 + f.left : (parseInt(f.right)) ? 0 + f.right : 0,
                            y: (parseInt(f.top)) ? 0 + f.top : (parseInt(f.bottom)) ? 0 + f.bottom : 0
                        });
                        n = {
                            width: u.width + this.t22.j29("padX"),
                            height: u.height + this.t22.j29("padY")
                        };
                        g.width[1] = u.width
                    }
                    v.top = (v.top += parseInt(f.top)) ? v.top : (v.bottom -= parseInt(f.bottom)) ? v.bottom - n.height : v.top;
                    v.bottom = v.top + n.height;
                    v.left = (v.left += parseInt(f.left)) ? v.left : (v.right -= parseInt(f.right)) ? v.right - n.width : v.left;
                    v.right = v.left + n.width;
                    o = this.t14(n, v);
                    break
            }
            g.top = [r.top, o.y];
            g.left = [r.left, o.x + ((this.t25 && "left" == this._o.captionPosition) ? this.t25.j29("width") : 0)];
            if (s && "expand" != this._o.slideshowEffect) {
                g.width = [u.width, u.width];
                g.top[0] = g.top[1];
                g.left[0] = g.left[1];
                g.opacity = [0, 1];
                this.t30.options.duration = this._o.slideshowSpeed;
                this.t30.options.transition = c.FX.Transition.linear
            } else {
                this.t30.options.transition = c.FX.Transition[this._o.expandEffect + this.easing[this._o.expandEffect][0]];
                this.t30.options.duration = this._o.expandSpeed;
                if (c.j21.trident4) {
                    this.t23.j23(1)
                }
                if (this._o.keepThumbnail) {
                    g.opacity = [0, 1]
                }
            }
            if (this.t26) {
                c.$A(this.t26.byTag("A")).forEach(function (x) {
                    var p = x.j5("background-position").split(" ");
                    if (c.j21.trident4) {
                        x.scrollTop = 1
                    } else {
                        p[1] = "0px";
                        x.j6({
                            "background-position": p.join(" ")
                        })
                    }
                });
                var j = c.$A(this.t26.byTag("A")).filter(function (p) {
                    return "previous" == p.rel
                })[0],
				l = c.$A(this.t26.byTag("A")).filter(function (p) {
				    return "next" == p.rel
				})[0],
				q = this.t19(this.group),
				k = this.t20(this.group);
                if (j) {
                    (this == q && (q == k || !this._o.slideshowLoop)) ? j.hide() : j.show()
                }
                if (l) {
                    (this == k && (q == k || !this._o.slideshowLoop)) ? l.hide() : l.show()
                }
            }
            this.t30.start(g);
            this.t11()
        },
        restore: function (e, j) {
            if ("expanded" != this.state) {
                return
            }
            this.state = "busy-restore";
            this.prevItem = e = e || null;
            j = j || false;
            c.doc.je2("keydown");
            var m = this.t22.j9();
            if (this.t25) {
                this.t12("hide");
                this.t25.parentNode.j6Prop("height", 0);
                if (c.j21.trident && c.j21.backCompat && this.hCaption) {
                    this.t25.j6Prop("display", "none")
                }
            }
            var i = {};
            if (e && "expand" != this._o.slideshowEffect) {
                if ("fade" == this._o.slideshowEffect) {
                    i.opacity = [1, 0]
                }
                this.t31.options.duration = this._o.slideshowSpeed;
                this.t31.options.transition = c.FX.Transition.linear
            } else {
                this.t31.options.duration = (j) ? 0 : this._o.restoreSpeed;
                this.t31.options.transition = c.FX.Transition[this._o.restoreEffect + this.easing[this._o.restoreEffect][1]];
                i = c.detach(this.t30.styles);
                for (var f in i) {
                    if ("array" != c.j1(i[f])) {
                        continue
                    }
                    i[f].reverse()
                }
                if (!this._o.keepThumbnail) {
                    delete i.opacity
                }
                var g = this.t16(this.t27 || this.id).r.j29("thumb"),
				n = (g.i1) ? g.i1.self : g.r;
                i.width[1] = [n.j7().width];
                i.top[1] = n.j8().top;
                i.left[1] = n.j8().left
            }
            this.t31.start(i);
            if (e) {
                e.expand(this, m)
            }
            var l = c.doc.j29("bg:t32");
            if (!e && l) {
                if ("hidden" != l.el.j5("visibility")) {
                    this.t11(true)
                }
            }
        },
        t12: function (f) {
            if (!this.t25) {
                return
            }
            var e = this.t25.j29("slide");
            this.t25.j6Prop("overflow-y", "hidden");
            e.stop();
            e[f || "toggle"](this.hCaption ? this._o.captionPosition : "vertical")
        },
        t10: function (f, i) {
            var k = this.t26;
            if (!k) {
                return
            }
            f = f || false;
            i = i || false;
            var g = k.j29("cb:t32"),
			e = {};
            if (!g) {
                k.j30("cb:t32", g = new c.FX(k, {
                    transition: c.FX.Transition.linear,
                    duration: 250
                }))
            } else {
                g.stop()
            }
            if (i) {
                k.j6Prop("opacity", (f) ? 0 : 1);
                return
            }
            var j = k.j5("opacity");
            e = (f) ? {
                opacity: [j, 0]
            } : {
                opacity: [j, 1]
            };
            g.start(e)
        },
        cbHover: function (j) {
            var g = $mjs(j).stop().getTarget();
            if ("expanded" != this.state) {
                return
            }
            try {
                while ("a" != g.tagName.toLowerCase() && g != this.t26) {
                    g = g.parentNode
                }
                if ("a" != g.tagName.toLowerCase() || g.hasChild(j.getRelated())) {
                    return
                }
            } catch (i) {
                return
            }
            var f = g.j5("background-position").split(" ");
            switch (j.type) {
                case "mouseover":
                    f[1] = g.j5("height");
                    break;
                case "mouseout":
                    f[1] = "0px";
                    break
            }
            if (c.j21.trident4) {
                g.scrollTop = f[1].j17() + 1
            } else {
                g.j6({
                    "background-position": f.join(" ")
                })
            }
        },
        cbClick: function (g) {
            var f = $mjs(g).stop().getTarget();
            while ("a" != f.tagName.toLowerCase() && f != this.t26) {
                f = f.parentNode
            }
            if ("a" != f.tagName.toLowerCase()) {
                return
            }
            switch (f.rel) {
                case "previous":
                    this.restore(this.t18(this, this._o.slideshowLoop));
                    break;
                case "next":
                    this.restore(this.t17(this, this._o.slideshowLoop));
                    break;
                case "close":
                    this.restore(null);
                    break
            }
        },
        t11: function (f) {
            f = f || false;
            var g = c.doc.j29("bg:t32"),
			e = {},
			j = 0;
            if (!g) {
                var i = c.$new("DIV").j2("MagicThumb-background").j6({
                    position: "fixed",
                    display: "block",
                    top: 0,
                    bottom: 0,
                    left: 0,
                    right: 0,
                    zIndex: (this._o.zIndex - 1),
                    overflow: "hidden",
                    backgroundColor: this._o.backgroundColor,
                    opacity: 0,
                    border: 0,
                    margin: 0,
                    padding: 0
                }).append(c.$new("IFRAME", {
                    src: 'javascript:"";'
                },
				{
				    width: "100%",
				    height: "100%",
				    display: "block",
				    top: 0,
				    lef: 0,
				    position: "absolute",
				    zIndex: -1,
				    border: "none"
				})).j32(c.body).hide();
                if (c.j21.trident && (c.j21.version < 900 || (c.doc.documentMode && c.doc.documentMode < 9))) {
                    i.firstChild.j6Prop("filter", "mask()")
                }
                c.doc.j30("bg:t32", g = new c.FX(i, {
                    transition: c.FX.Transition.linear,
                    duration: this._o.backgroundSpeed,
                    onStart: function (k) {
                        if (k) {
                            this.j6(c.extend(c.doc.j12(), {
                                position: "absolute"
                            }))
                        }
                    } .j24(i, this.ieBack),
                    onComplete: function () {
                        this.j23(this.j5("opacity"), true)
                    } .j24(i)
                }));
                e = {
                    opacity: [0, this._o.backgroundOpacity / 100]
                }
            } else {
                g.stop();
                j = g.el.j5("opacity");
                g.el.j6Prop("background-color", this._o.backgroundColor);
                e = (f) ? {
                    opacity: [j, 0]
                } : {
                    opacity: [j, this._o.backgroundOpacity / 100]
                };
                g.options.duration = this._o.backgroundSpeed
            }
            g.el.show();
            g.start(e)
        },
        t13: function (g) {
            g = g || 0;
            var f = $mjs(window).j7(),
			e = $mjs(window).j10();
            return {
                left: e.x + g,
                right: e.x + f.width - g,
                top: e.y + g,
                bottom: e.y + f.height - g
            }
        },
        t14: function (g, i) {
            var f = this.t13(this._o.screenPadding),
			e = $mjs(window).j12();
            i = i || f;
            return {
                y: Math.max(f.top, Math.min(("fit-screen" == this._o.imageSize) ? f.bottom : e.height + g.height, i.bottom - (i.bottom - i.top - g.height) / 2) - g.height),
                x: Math.max(f.left, Math.min(f.right, i.right - (i.right - i.left - g.width) / 2) - g.width)
            }
        },
        resize: function () {
            var i = $mjs(window).j7(),
			n = this.t22.j29("size"),
			j = this.t22.j29("ratio"),
			g = this.t22.j29("padX"),
			e = this.t22.j29("padY"),
			m = this.t22.j29("hspace"),
			f = this.t22.j29("vspace"),
			l = 0,
			k = 0;
            if (this.hCaption) {
                l = Math.min(this.i2.width + m, Math.min(n.width, i.width - g - this.scrPad.x)),
				k = Math.min(this.i2.height + f, Math.min(n.height, i.height - this.scrPad.y))
            } else {
                l = Math.min(this.i2.width + m, Math.min(n.width, i.width - this.scrPad.x)),
				k = Math.min(this.i2.height + f, Math.min(n.height, i.height - e - this.scrPad.y))
            }
            if (l / k > j) {
                l = k * j
            } else {
                if (l / k < j) {
                    k = l / j
                }
            }
            this.t22.j6Prop("width", l);
            if (this.cr) {
                this.cr.j6({
                    top: (this.i2.self.j7().height - this.cr.j7().height)
                })
            }
            return {
                width: Math.ceil(l),
                height: Math.ceil(k)
            }
        },
        adjBorder: function (i, f, e) {
            var g = false;
            switch (c.j21.engine) {
                case "gecko":
                    g = "content-box" != (i.j5("box-sizing") || i.j5("-moz-box-sizing"));
                    break;
                case "webkit":
                    g = "content-box" != (i.j5("box-sizing") || i.j5("-webkit-box-sizing"));
                    break;
                case "trident":
                    g = c.j21.backCompat || "content-box" != (i.j5("box-sizing") || i.j5("-ms-box-sizing") || "content-box");
                    break;
                default:
                    g = "content-box" != i.j5("box-sizing");
                    break
            }
            return (g) ? f : f - e
        },
        t9: function (i) {
            function f(n) {
                var m = [];
                if ("string" == c.j1(n)) {
                    return n
                }
                for (var l in n) {
                    m.push(l.dashize() + ":" + n[l])
                }
                return m.join(";")
            }
            var j = $mjs(f(i).split(";")),
			g = null,
			e = null;
            j.forEach(function (l) {
                for (var k in this._o) {
                    e = new RegExp("^" + k.dashize().replace(/\-/, "\\-") + "\\s*:\\s*([^;]+)$", "i").exec(l.j26());
                    if (e) {
                        switch (c.j1(this._o[k])) {
                            case "boolean":
                                this._o[k] = e[1].j18();
                                break;
                            case "number":
                                this._o[k] = (e[1].has(".")) ? (e[1].toFloat() * ((k.toLowerCase().has("opacity")) ? 100 : 1000)) : e[1].j17();
                                break;
                            default:
                                this._o[k] = e[1].j26()
                        }
                    }
                }
            },
			this)
        },
        parsePosition: function () {
            var e = null,
			g = this.position;
            for (var f in g) {
                e = new RegExp("" + f + "\\s*:\\s*([^,]+)", "i").exec(this._o.expandPosition);
                if (e) {
                    g[f] = (isFinite(g[f] = e[1].j17())) ? g[f] : "auto"
                }
            }
            if ((isNaN(g.top) && isNaN(g.bottom)) || (isNaN(g.left) && isNaN(g.right))) {
                this._o.expandPosition = "center"
            }
        },
        t16: function (e) {
            return $mjs(this.thumbs.filter(function (f) {
                return (e == f.id)
            }))[0]
        },
        t15: function (e, f) {
            e = e || null;
            f = f || false;
            return $mjs(this.thumbs.filter(function (g) {
                return (e == g.group && (f || g.ready) && (f || "uninitialized" != g.state))
            }))
        },
        t17: function (i, e) {
            e = e || false;
            var f = this.t15(i.group),
			g = f.indexOf(i) + 1;
            return (g >= f.length) ? (!e || 1 >= f.length) ? undefined : f[0] : f[g]
        },
        t18: function (i, e) {
            e = e || false;
            var f = this.t15(i.group),
			g = f.indexOf(i) - 1;
            return (g < 0) ? (!e || 1 >= f.length) ? undefined : f[f.length - 1] : f[g]
        },
        t19: function (f) {
            f = f || null;
            var e = this.t15(f, true);
            return (e.length) ? e[0] : undefined
        },
        t20: function (f) {
            f = f || null;
            var e = this.t15(f, true);
            return (e.length) ? e[e.length - 1] : undefined
        },
        t21: function () {
            return $mjs(this.thumbs.filter(function (e) {
                return ("expanded" == e.state || "busy-expand" == e.state || "busy-restore" == e.state)
            }))
        },
        onKey: function (g) {
            var f = this._o.slideshowLoop,
			i = null;
            if (!this._o.keyboard) {
                c.doc.je2("keydown");
                return true
            }
            g = $mjs(g);
            if (this._o.keyboardCtrl && !(g.ctrlKey || g.metaKey)) {
                return false
            }
            switch (g.keyCode) {
                case 27:
                    g.stop();
                    this.restore(null);
                    break;
                case 32:
                case 34:
                case 39:
                case 40:
                    i = this.t17(this, f || 32 == g.keyCode);
                    break;
                case 33:
                case 37:
                case 38:
                    i = this.t18(this, f);
                    break;
                default:
            }
            if (i) {
                g.stop();
                this.restore(i)
            }
        }
    });
    return b
})(magicJS);
window.MagicMagnifyPlusSwf = function (o, s) { o.innerHTML = s; };
