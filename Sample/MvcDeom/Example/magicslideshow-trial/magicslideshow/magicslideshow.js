/*


Magic Slideshow v1.1.31 DEMO
Copyright 2012 Magic Toolbox
Buy a license: www.magictoolbox.com/magicslideshow/
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
        j23: function (j, e) {
            e = e || false;
            j = parseFloat(j);
            if (e) {
                if (j == 0) {
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
                    g.enabled = (1 != j);
                    g.opacity = j * 100
                } catch (d) {
                    this.style.filter += (1 == j) ? "" : "progid:DXImageTransform.Microsoft.Alpha(enabled=true,opacity=" + j * 100 + ")"
                }
            }
            this.style.opacity = j;
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
				j = a.j21.getDoc();
                return {
                    top: d.top + f.y - j.clientTop,
                    left: d.left + f.x - j.clientLeft
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
            var k = ("domready" == g) ? false : true,
			e = this.j29("events", {});
            e[g] = e[g] || {};
            if (e[g].hasOwnProperty(f.$J_EUID)) {
                return this
            }
            if (!f.$J_EUID) {
                f.$J_EUID = Math.floor(Math.random() * a.now())
            }
            var d = this,
			j = function (l) {
			    return f.call(d)
			};
            if ("domready" == g) {
                if (a.j21.ready) {
                    f.call(this);
                    return this
                }
            }
            if (k) {
                j = function (l) {
                    l = a.extend(l || window.e, {
                        $J_TYPE: "event"
                    });
                    return f.call(d, $mjs(l))
                };
                this[a._event_add_](a._event_prefix_ + g, j, false)
            }
            e[g][f.$J_EUID] = j;
            return this
        },
        je2: function (g) {
            var l = ("domready" == g) ? false : true,
			e = this.j29("events");
            if (!e || !e[g]) {
                return this
            }
            var j = e[g],
			f = arguments[1] || null;
            if (g && !f) {
                for (var d in j) {
                    if (!j.hasOwnProperty(d)) {
                        continue
                    }
                    this.je2(g, d)
                }
                return this
            }
            f = ("function" == a.j1(f)) ? f.$J_EUID : f;
            if (!j.hasOwnProperty(f)) {
                return this
            }
            if ("domready" == g) {
                l = false
            }
            if (l) {
                this[a._event_del_](a._event_prefix_ + g, j[f], false)
            }
            delete j[f];
            return this
        },
        raiseEvent: function (j, f) {
            var p = ("domready" == j) ? false : true,
			n = this,
			m;
            if (!p) {
                var g = this.j29("events");
                if (!g || !g[j]) {
                    return this
                }
                var l = g[j];
                for (var d in l) {
                    if (!l.hasOwnProperty(d)) {
                        continue
                    }
                    l[d].call(this)
                }
                return this
            }
            if (n === document && document.createEvent && !n.dispatchEvent) {
                n = document.documentElement
            }
            if (document.createEvent) {
                m = document.createEvent(j);
                m.initEvent(f, true, true)
            } else {
                m = document.createEventObject();
                m.eventType = j
            }
            if (document.createEvent) {
                n.dispatchEvent(m)
            } else {
                n.fireEvent("on" + f, m)
            }
            return m
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
        var j = null,
		e = a.$A(arguments);
        if ("class" == a.j1(e[0])) {
            j = e.shift()
        }
        var d = function () {
            for (var o in this) {
                this[o] = a.detach(this[o])
            }
            if (this.constructor.$parent) {
                this.$parent = {};
                var r = this.constructor.$parent;
                for (var q in r) {
                    var n = r[q];
                    switch (a.j1(n)) {
                        case "function":
                            this.$parent[q] = a.Class.wrap(this, n);
                            break;
                        case "object":
                            this.$parent[q] = a.detach(n);
                            break;
                        case "array":
                            this.$parent[q] = a.detach(n);
                            break
                    }
                }
            }
            var l = (this.init) ? this.init.apply(this, arguments) : this;
            delete this.caller;
            return l
        };
        if (!d.prototype.init) {
            d.prototype.init = a.$F
        }
        if (j) {
            var g = function () { };
            g.prototype = j.prototype;
            d.prototype = new g;
            d.$parent = {};
            for (var f in j.prototype) {
                d.$parent[f] = j.prototype[f]
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
            this.ss25Time = this.startTime + this.options.duration;
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
            if (d >= this.ss25Time) {
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
})(magicJS);
var MagicSlideshow = (function ($J) {
    var $j = $J.$;
    $J.extend(magicJS.Element, {
        hasAttribute: function (n) {
            var node = this.getAttributeNode(n);
            return !!(node && node.specified)
        },
        ss37: function () {
            var l = t = 0;
            var el = this;
            do {
                l += el.offsetLeft || 0;
                t += el.offsetTop || 0;
                el = el.offsetParent
            }
            while (el && $mjs(el).j19("position") != "relative");
            return {
                top: t,
                left: l
            }
        }
    });
    magicJS.implement(Function, {
        delayMS: function () {
            var args = $J.$A(arguments),
			m = this,
			t = args.shift();
            return window.setTimeout(function () {
                return m.apply(m, args)
            },
			t || 0)
        },
        bindMS: function () {
            var args = $J.$A(arguments),
			m = this,
			o = args.shift();
            var ret = function () {
                return m.apply(o, args.concat($J.$A(arguments)))
            };
            ret.delay = ret.delayMS;
            return ret
        }
    });
    magicJS.Element.j6Orig = magicJS.Element.j6;
    magicJS.Element.j6 = function (styles, ss1) {
        if (styles && ($J.j21.trident4 || $J.j21.trident5)) {
            ss1 || (ss1 = MagicSlideshow._pr);
            if ($J.defined(styles.bottom)) {
                if (ss1.height && ss1.height % 2 > 0) {
                    styles.bottom = parseInt(styles.bottom) - 1
                }
            }
            if ($J.defined(styles.right)) {
                if (ss1.width && ss1.width % 2 > 0) {
                    styles.right = parseInt(styles.right) - 1
                }
            }
        }
        return this.j6Orig(styles)
    };
    magicJS.Element.j20 = magicJS.Element.j6;
    var MagicSlideshow = $J.Class({
        init: function () {
            this.ss26 = {};
            this.findSliders()
        },
        findSliders: function (id, el) {
            id = id || false;
            el = el || document;
            $J.$A(id ? [$mjs(id)] : el.getElementsByTagName("div")).j14((function (divEl) {
                if ($J.defined(divEl) && (divEl = $mjs(divEl)) && (divEl.j13("MagicSlideshow") || divEl.j13("MagicSlideShow")) && divEl.getAttribute("busy") != "busy") {
                    slider = new MagicSlideshow.Slider(divEl);
                    if (slider.id == "-disabled") {
                        return
                    }
                    this.ss26[slider.id] = slider
                }
            }).j24(this))
        },
        stopSliders: function (id) {
            id = id || false;
            if (id && this.ss26[id]) {
                this.ss26[id].stop();
                delete this.ss26[id]
            } else {
                if (!id) {
                    var i;
                    for (i in this.ss26) {
                        this.ss26[i].stop()
                    }
                    this.ss26 = {}
                }
            }
        },
        getAllSliders: function () {
            var result = [];
            for (key in this.ss26) {
                if (this.ss26[key] !== Object.prototype[key]) {
                    result.push(this.ss26[key])
                }
            }
            return result
        }
    });
    $J.extend(MagicSlideshow, {
        version: "v1.1.31",
        obj: false,
        options: {},
        extraOptions: {},
        _pr: {},
        stop: function (id) {
            if (!this.obj) {
                return
            }
            this.obj.stopSliders(id || false)
        },
        start: function (id, el) {
            if (!this.obj) {
                this.obj = new MagicSlideshow()
            } else {
                this.obj.findSliders(id || false, el || document.body)
            }
        },
        refresh: function (id) {
            id = id || false;
            this.stop(id);
            this.start(id)
        },
        scroll: function () {
            if (!this.obj) {
                throw "Can't find ss26 on this page"
            }
            args = $J.$A(arguments);
            var id = false;
            var num = "+1";
            var type = "jump";
            var callback = $J.$F;
            if (args.length > 0) {
                num = args.shift();
                if (!(/^(\+|\-)?[0-9]+$/.test(num))) {
                    id = num;
                    num = "+1";
                    if (args.length > 0) {
                        num = args.shift()
                    }
                }
                if (!(/^(\+|\-)?[0-9]+$/.test(num))) {
                    callback = num;
                    num = "+1"
                } else {
                    if (args.length > 0) {
                        callback = args.shift()
                    }
                }
            }
            if (/^(\+|\-)[0-9]+$/.test(num)) {
                type = "next"
            }
            num = parseInt(num);
            if (id) {
                try {
                    this.obj.ss26[id].changeEl(num, type, callback)
                } catch (e) {
                    throw "Can't find slider with specified ID"
                }
            } else {
                $J.$A(this.obj.getAllSliders()).j14((function (num, type, callback, el) {
                    el.changeEl(num, type, callback)
                }).j24(this, num, type, callback))
            }
        },
        preloadClass: function (tagName, className) {
            var el = $mjs(document.createElement(tagName));
            el.j2(className);
            el.j20({
                position: "absolute",
                top: "-1000px",
                left: "0",
                visibility: "hidden"
            });
            document.body.appendChild(el); (function () {
                this.j33()
            }).j24(el).j27(100)
        },
        ss12: function (tagName, className, propertyName, id) {
            if (!!id) {
                var el = $mjs(document.createElement("div"));
                el.j2("MagicSlideshow");
                el.id = id;
                var el2 = $mjs(document.createElement(tagName));
                el2.j2(className);
                el.appendChild(el2)
            } else {
                var el = $mjs(document.createElement(tagName));
                el.j2(className)
            }
            el.j20({
                position: "absolute",
                top: "-1000px",
                left: "0",
                visibility: "hidden"
            });
            document.body.appendChild(el);
            if (!!id) {
                var result = el2.j5(propertyName)
            } else {
                var result = el.j5(propertyName)
            }
            el.j33();
            return result
        },
        pause: function (id) {
            id = id || false;
            if (id) {
                try {
                    this.obj.ss26[id].pause()
                } catch (e) {
                    throw "Can't find slider with specified ID"
                }
            } else {
                $J.$A(this.obj.getAllSliders()).j14((function (el) {
                    el.pause()
                }))
            }
        },
        resume: function (id) {
            id = id || false;
            if (id) {
                try {
                    this.obj.ss26[id].resume()
                } catch (e) {
                    throw "Can't find slider with specified ID"
                }
            } else {
                $J.$A(this.obj.getAllSliders()).j14((function (el) {
                    el.resume()
                }))
            }
        },
        play: function (id) {
            this.resume(id || false)
        }
    });
    function xgdf7fsgd56(vc67) {
        var vc68 = "";
        for (i = 0; i < vc67.length; i++) {
            vc68 += String.fromCharCode(14 ^ vc67.charCodeAt(i))
        }
        return vc68
    }
    $mjs(document).je1("domready",
	function () {
	    function __fixDelayRecursive(obj) {
	        for (var i in obj.prototype) {
	            switch ($J.j1(obj.prototype[i])) {
	                case "function":
	                    obj.prototype[i].delay = obj.prototype[i].delayMS;
	                    obj.prototype[i].j24 = obj.prototype[i].bindMS;
	                default:
	                    break
	            }
	        }
	    }
	    var classes = [MagicSlideshow, MagicSlideshow.Slider, MagicSlideshow.Slider.Element, MagicSlideshow.Effect, MagicSlideshow.Loading];
	    for (var i = 0; i < classes.length; i++) {
	        __fixDelayRecursive(classes[i])
	    }
	    MagicSlideshow.preloadClass("a", "MagicSlideshowArrow");
	    MagicSlideshow.preloadClass("div", "MagicSlideshowLoadingBox");
	    MagicSlideshow.start();
	    MagicSlideshow.options.autostart = true
	});
    MagicSlideshow.Slider = $J.Class({
        init: function (divEl) {
            this.ss24 = divEl;
            if (!this.ss24.id || !$J.defined(this.ss24.id)) {
                this.id = "mss" + Math.round(Math.random() * 1000000)
            } else {
                this.id = this.ss24.id
            }
            this.ss1 = {};
            this.loadParams();
            if (!this.ss1.autostart) {
                if (MagicSlideshow.extraOptions[this.id]) {
                    MagicSlideshow.extraOptions[this.id].autostart = true
                }
                this.id = "-disabled";
                return
            }
            this.thumbPreloaded = 0;
            this.els = [];
            this.elsTmp = [];
            this.loadEls();
            if (this.els.length < 1) {
                return false
            }
            this.cur = this.ss1.start;
            this.skipAuto = false;
            this.playInterval = false;
            this.created = false;
            this.replaced = false;
            this.create();
            this.insertInterval = (function () {
                if (this.created && !this.replaced) {
                    this.replaced = true;
                    clearInterval(this.insertInterval);
                    this.ss24.parentNode.replaceChild(this.ss8, this.ss24);
                    $J.$A(this.els).j14(function (el) {
                        if (el.additionalEffect && $J.defined(window[el.additionalEffect])) {
                            window[el.additionalEffect].start(el.ss4.img)
                        }
                    },
					this)
                }
            }).j24(this).interval(100)
        },
        preloadBGImage: function (el) { },
        stop: function () {
            this.ss8.parentNode.replaceChild(this.ss24, this.ss8)
        },
        loadParams: function () {
            this.ss1 = {
                autostart: true,
                pause: "off",
                width: "auto",
                height: "auto",
                zIndex: 200,
                preloadImages: true,
                preloadInOrder: false,
                arrows: true,
                arrowsOpacity: 0.6,
                arrowsHoverOpacity: 1,
                thumbnails: "outside",
                speed: 5,
                direction: "right",
                loop: true,
                loopType: "next",
                start: 1,
                useLinks: true,
                linksWindow: "_self",
                loadingText: "",
                text: "effect",
                textEffect: "fixed",
                textDelay: 0.5,
                textOpacity: 0.6,
                textPosition: "bottom",
                containerSize: "auto",
                containerPosition: "top",
                containerPadding: 0,
                containerOpacity: 0.3,
                containerSpeed: 10,
                thumbnailOpacity: 0.8,
                preserve: true,
                effect: "scroll",
                effectNext: "scroll",
                effectJump: "fade",
                effectDuration: 1,
                effectFadeDuration: 1,
                effectScrollDuration: 1,
                scrollCallback: "",
                resizeEffect: false
            };
            this.ss1.thumbnailsDefault = true;
            this.ss1.effectFadeDurationDefault = true;
            this.ss1.effectScrollDurationDefault = true;
            function ss1ObjToArray(obj) {
                var arr = [],
				i;
                for (i in obj) {
                    if ($J.j1(obj[i]) != "function") {
                        arr.push(i + ":" + obj[i])
                    }
                }
                return arr
            }
            var ss1 = [],
			l,
			i;
            ss1.push((this.ss24.getAttribute("conf") || "").split(";"));
            ss1.push(ss1ObjToArray(MagicSlideshow.options));
            if ($J.defined(MagicSlideshow.extraOptions[this.id])) {
                ss1.push(ss1ObjToArray(MagicSlideshow.extraOptions[this.id]))
            }
            var l = ss1.length;
            for (i = 0; i < l; i++) {
                $J.$A(ss1[i]).j14((function (ss1) {
                    var n,
					v,
					p = ss1.split(":");
                    if (p.length == 2) {
                        n = p[0].j22();
                        n = n.j26();
                        v = p[1].j26();
                        if (!$J.defined(this.ss1[n])) {
                            return
                        }
                        if (n == "thumbnails") {
                            this.ss1.thumbnailsDefault = false
                        }
                        if (n == "effectFadeDuration") {
                            this.ss1.effectFadeDurationDefault = false
                        }
                        if (n == "effectScrollDuration") {
                            this.ss1.effectScrollDurationDefault = false
                        }
                        if ((n == "width" || n == "height" || n == "containerSize") && v != "auto") {
                            this.ss1[n] = 0
                        }
                        switch ($J.j1(this.ss1[n])) {
                            case "number":
                                v = parseFloat(v);
                                break;
                            case "boolean":
                                if (v.toLowerCase() == "none" || v.toLowerCase() == "no" || v.toLowerCase() == "false") {
                                    v = false
                                } else {
                                    if (v.toLowerCase() == "yes" || v.toLowerCase() == "true") {
                                        v = true
                                    } else {
                                        if (/^[0-9]+$/.test(v)) {
                                            v = !!(parseInt(v))
                                        } else {
                                            v = !!v
                                        }
                                    }
                                }
                                break;
                            case "string":
                            default:
                                v = v
                        }
                        this.ss1[n] = v
                    }
                }).j24(this))
            }
            if (this.ss1.direction == "left" || this.ss1.direction == "right") {
                this.ss1.effectScrollDirection = "horizontally"
            } else {
                this.ss1.effectScrollDirection = "vertically"
            }
            if (this.ss1.effectFadeDurationDefault) {
                this.ss1.effectFadeDuration = this.ss1.effectDuration
            }
            if (this.ss1.effectScrollDurationDefault) {
                this.ss1.effectScrollDuration = this.ss1.effectDuration
            }
            this.ss1.thumbnailBorderColor = MagicSlideshow.ss12("img", "MagicSlideshowThumbnail", "border-top-color", this.id);
            this.ss1.thumbnailBorderWidthTop = parseInt(MagicSlideshow.ss12("img", "MagicSlideshowThumbnail", "border-top-width", this.id));
            this.ss1.thumbnailBorderWidthRight = parseInt(MagicSlideshow.ss12("img", "MagicSlideshowThumbnail", "border-right-width", this.id));
            this.ss1.thumbnailBorderWidthBottom = parseInt(MagicSlideshow.ss12("img", "MagicSlideshowThumbnail", "border-bottom-width", this.id));
            this.ss1.thumbnailBorderWidthLeft = parseInt(MagicSlideshow.ss12("img", "MagicSlideshowThumbnail", "border-left-width", this.id));
            this.ss1.imageBorderWidthTop = parseInt(MagicSlideshow.ss12("img", "MagicSlideshowImage", "border-top-width", this.id));
            this.ss1.imageBorderWidthRight = parseInt(MagicSlideshow.ss12("img", "MagicSlideshowImage", "border-right-width", this.id));
            this.ss1.imageBorderWidthBottom = parseInt(MagicSlideshow.ss12("img", "MagicSlideshowImage", "border-bottom-width", this.id));
            this.ss1.imageBorderWidthLeft = parseInt(MagicSlideshow.ss12("img", "MagicSlideshowImage", "border-left-width", this.id));
            this.ss1.thumbnailMarginTop = parseInt(MagicSlideshow.ss12("img", "MagicSlideshowThumbnail", "margin-top", this.id));
            this.ss1.thumbnailMarginRight = parseInt(MagicSlideshow.ss12("img", "MagicSlideshowThumbnail", "margin-right", this.id));
            this.ss1.thumbnailMarginBottom = parseInt(MagicSlideshow.ss12("img", "MagicSlideshowThumbnail", "margin-bottom", this.id));
            this.ss1.thumbnailMarginLeft = parseInt(MagicSlideshow.ss12("img", "MagicSlideshowThumbnail", "margin-left", this.id));
            this.ss1.imageMarginTop = parseInt(MagicSlideshow.ss12("img", "MagicSlideshowImage", "margin-top", this.id));
            this.ss1.imageMarginRight = parseInt(MagicSlideshow.ss12("img", "MagicSlideshowImage", "margin-right", this.id));
            this.ss1.imageMarginBottom = parseInt(MagicSlideshow.ss12("img", "MagicSlideshowImage", "margin-bottom", this.id));
            this.ss1.imageMarginLeft = parseInt(MagicSlideshow.ss12("img", "MagicSlideshowImage", "margin-left", this.id));
            if (this.ss1.containerPosition == "left" || this.ss1.containerPosition == "right") {
                this.ss1.containerDirection = "vertically"
            } else {
                this.ss1.containerDirection = "horizontally"
            }
            this.ss1.textPositionDefault = this.ss1.textPosition;
            if (this.ss1.containerPosition == "bottom") {
                this.ss1.textPosition = "top"
            } else {
                if (this.ss1.containerPosition == "top") {
                    this.ss1.textPosition = "bottom"
                }
            }
            if (this.ss1.direction == "right" || this.ss1.direction == "bottom") {
                this.ss1.direction = +1
            } else {
                this.ss1.direction = -1
            }
            this.ss1.effectAuto = this.ss1.effect;
            this.ss1.start--
        },
        loadEls: function () {
            function prepareDesc(str, ss1) {
                var pat = new RegExp("\\[a([^\\]]+)\\]([\\s\\S]*?)\\[\\/a\\]", "gi");
                var matches = str.match(pat);
                $J.$A(matches).j14(function (m) {
                    if (ss1.textEffect == "fixed" && (new RegExp("href", "gi")).test(m)) {
                        var r = m + "";
                        var p = new RegExp('^.*?href=\\"([^\\"]+)\\".*$', "gi");
                        href = m.replace(p, "$1");
                        if ((new RegExp("onclick", "gi")).test(r)) {
                            p = new RegExp('onclick=\\"([^\\"]+)\\"', "gi");
                            r = r.replace(p, "onclick=\"$1;window.open(this.href,this.target || '_self'); return false;\"");
                            r = r.replace(pat, "<a $1>$2</a>")
                        } else {
                            r = r.replace(pat, "<a $1 onclick=\"window.open(this.href,this.target || '_self'); return false;\">$2</a>")
                        }
                    } else {
                        r = m.replace(pat, "<a $1>$2</a>")
                    }
                    str = str.split(m).join(r)
                });
                return str
            }
            var el,
			i,
			l = this.ss24.childNodes.length;
            var ss36 = false;
            var ss38 = false;
            for (i = 0; i < l; i++) {
                el = this.ss24.childNodes[i];
                if (!ss36 && !ss38) {
                    if (el.nodeName == "A") {
                        ss38 = true
                    } else {
                        if (el.nodeName == "IMG") {
                            ss36 = true;
                            if (this.ss1.thumbnailsDefault) {
                                this.ss1.thumbnails = "off"
                            }
                        } else {
                            continue
                        }
                    }
                }
                if (ss38 && el.nodeName != "A" || ss36 && el.nodeName != "IMG") {
                    continue
                } else {
                    if (ss38) {
                        el = $mjs(el);
                        var img = el.getAttribute("rel"),
						thumb,
						ss30 = "",
						title = "",
						link = el.hasAttribute("href") ? el.getAttribute("href") : false,
						target = el.hasAttribute("target") ? el.getAttribute("target") : false,
						additionalEffect = el.j13("MagicZoomPlus") ? "MagicZoomPlus" : el.j13("MagicZoom") ? "MagicZoom" : el.j13("MagicThumb") ? "MagicThumb" : false;
                        rev = el.hasAttribute("rev") ? el.getAttribute("rev") : false;
                        var el2,
						i2,
						l2 = el.childNodes.length;
                        for (i2 = 0; i2 < l2; i2++) {
                            el2 = $mjs(el.childNodes[i2]);
                            if (el2.nodeName != "IMG" && el2.nodeName != "SPAN") {
                                continue
                            } else {
                                if (el2.nodeName == "IMG") {
                                    thumb = el2.src;
                                    title = el2.title || el2.alt;
                                    if (this.els.length == 0) {
                                        if ((!$J.defined(img) || img.length == 0) && $J.defined(link)) {
                                            if (el2.hasAttribute("width")) {
                                                this.ss1.width = parseInt(el2.getAttribute("width"))
                                            }
                                            if (el2.hasAttribute("height")) {
                                                this.ss1.height = parseInt(el2.getAttribute("height"))
                                            }
                                        }
                                    }
                                } else {
                                    if (el2.nodeName == "SPAN") {
                                        ss30 = el2.innerHTML
                                    } else { }
                                }
                            }
                        }
                        if ((!$J.defined(img) || img.length == 0) && $J.defined(link)) {
                            if (this.ss1.thumbnailsDefault) {
                                this.ss1.thumbnails = "off"
                            }
                            img = thumb
                        }
                        if ($J.defined(img) && $J.defined(thumb)) {
                            this.elsTmp.push({
                                img: img,
                                thumb: thumb,
                                title: title,
                                ss30: prepareDesc(ss30, this.ss1),
                                link: link,
                                target: target,
                                rev: rev,
                                additionalEffect: additionalEffect
                            })
                        }
                    } else {
                        if (ss36 && el.nodeName == "IMG") {
                            el = $mjs(el);
                            var img = el.src,
							thumb = el.src,
							title = el.title || el.alt,
							ss30 = "";
                            if (this.els.length == 0) {
                                if ($J.j21.trident) {
                                    var n = el.cloneNode();
                                    document.body.appendChild(n);
                                    if (n.hasAttribute("width")) {
                                        this.ss1.width = parseInt(n.getAttribute("width"))
                                    }
                                    if (n.hasAttribute("height")) {
                                        this.ss1.height = parseInt(n.getAttribute("height"))
                                    }
                                    document.body.removeChild(n);
                                    delete n
                                } else {
                                    if (el.hasAttribute("width")) {
                                        this.ss1.width = parseInt(el.getAttribute("width"))
                                    }
                                    if (el.hasAttribute("height")) {
                                        this.ss1.height = parseInt(el.getAttribute("height"))
                                    }
                                }
                            }
                            var el2,
							i2,
							l2 = el.childNodes.length;
                            for (i2 = i + 1; i2 < l; i2++) {
                                el2 = this.ss24.childNodes[i2];
                                if (el2.nodeName == "SPAN") {
                                    ss30 = el2.innerHTML;
                                    break
                                } else {
                                    if (el2.nodeName == "IMG") {
                                        break
                                    }
                                }
                            }
                            if ($J.defined(img) && $J.defined(thumb)) {
                                this.elsTmp.push({
                                    img: img,
                                    thumb: thumb,
                                    title: title,
                                    ss30: prepareDesc(ss30, this.ss1),
                                    link: false,
                                    target: false
                                })
                            }
                        }
                    }
                }
            }
            if (this.ss1.thumbnails == "off") {
                this.ss1.textPosition = this.ss1.textPositionDefault
            }
            for (var i = 0; i < this.elsTmp.length; i++) {
                this.els.push(new MagicSlideshow.Slider.Element(this, i, this.elsTmp[i].img, this.elsTmp[i].thumb, this.elsTmp[i].title, this.elsTmp[i].ss30, this.elsTmp[i].link, this.elsTmp[i].target, this.elsTmp[i].additionalEffect, this.elsTmp[i].rev))
            }
            if (this.ss1.start == -1) {
                if (this.ss1.direction == -1) {
                    this.ss1.start = this.els.length - 1
                } else {
                    this.ss1.start = 0
                }
            } else {
                if (this.ss1.start < 0) {
                    this.ss1.start = 0
                } else {
                    if (this.ss1.start > this.els.length - 1) {
                        this.ss1.start = this.els.length - 1
                    }
                }
            }
        },
        create: function () {
            if (this.ss1.width == "auto" || this.ss1.height == "auto") {
                if (this.els[0].ss19.img) {
                    if (this.ss1.width == "auto") {
                        this.ss1.width = this.els[0].ss14.img.width
                    }
                    if (this.ss1.height == "auto") {
                        this.ss1.height = this.els[0].ss14.img.height
                    }
                } else {
                    this.create.j24(this).j27(100);
                    return
                }
            }
            if (this.ss1.containerSize == "auto" && this.els[0].ss19.thumb) {
                if (this.ss1.containerDirection == "vertically") {
                    this.ss1.containerSize = this.els[0].ss14.thumb.width + Math.max(this.ss1.thumbnailBorderWidthLeft, parseInt(MagicSlideshow.ss12("IMG", "MagicSlideshowThumbnail highlight", "border-left-width", this.id))) + Math.max(this.ss1.thumbnailBorderWidthRight, parseInt(MagicSlideshow.ss12("IMG", "MagicSlideshowThumbnail highlight", "border-right-width", this.id))) + Math.max(this.ss1.thumbnailMarginLeft, parseInt(MagicSlideshow.ss12("IMG", "MagicSlideshowThumbnail highlight", "margin-left", this.id))) + Math.max(this.ss1.thumbnailMarginRight, parseInt(MagicSlideshow.ss12("IMG", "MagicSlideshowThumbnail highlight", "margin-right", this.id)))
                } else {
                    this.ss1.containerSize = this.els[0].ss14.thumb.height + Math.max(this.ss1.thumbnailBorderWidthTop, parseInt(MagicSlideshow.ss12("IMG", "MagicSlideshowThumbnail highlight", "border-top-width", this.id))) + Math.max(this.ss1.thumbnailBorderWidthBottom, parseInt(MagicSlideshow.ss12("IMG", "MagicSlideshowThumbnail highlight", "border-bottom-width", this.id))) + Math.max(this.ss1.thumbnailMarginTop, parseInt(MagicSlideshow.ss12("IMG", "MagicSlideshowThumbnail highlight", "margin-top", this.id))) + Math.max(this.ss1.thumbnailMarginBottom, parseInt(MagicSlideshow.ss12("IMG", "MagicSlideshowThumbnail highlight", "margin-bottom", this.id)))
                }
            } else {
                if (this.ss1.containerSize == "auto") {
                    this.create.j24(this).j27(100);
                    return
                }
            }
            if ($J.j21.trident) {
                MagicSlideshow._pr = this.ss1
            }
            this.ss8 = $mjs(document.createElement("DIV"));
            this.ss8.id = this.id;
            this.ss8.j2("MagicSlideshow");
            this.ss8.setAttribute("busy", "busy");
            this.ss8.j20({
                position: "relative",
                width: this.ss1.width,
                height: this.ss1.height,
                zIndex: this.ss1.zIndex,
                overflow: "hidden",
                textAlign: "left"
            });
            this.ss8.show();
            this.thumbnailsContainer = $mjs(document.createElement("div"));
            this.thumbnailsContainer.j2("MagicSlideshowThumbnailsContainer");
            this.thumbnailsContainer.j20({
                zIndex: this.ss1.zIndex + 50,
                position: "absolute",
                whiteSpace: "nowrap",
                overflow: "hidden",
                padding: 0
            });
            this.thumbnailsContainer.j23(this.ss1.thumbnailOpacity);
            if (this.ss1.thumbnails != "off") {
                this.ss8.appendChild(this.thumbnailsContainer)
            }
            this.thumbnailsContainerStyle = $mjs(document.createElement("div"));
            this.thumbnailsContainerStyle.j2("MagicSlideshowThumbnailsContainerStyle");
            this.thumbnailsContainerStyle.j20({
                zIndex: this.ss1.zIndex + 51,
                position: "absolute",
                whiteSpace: "nowrap",
                overflow: "hidden",
                top: 0,
                left: 0
            });
            this.thumbnailsContainerStyle.j23(this.ss1.containerOpacity);
            this.thumbnailsContainer.appendChild(this.thumbnailsContainerStyle);
            if (this.ss1.thumbnails == "off") {
                this.thumbnailsContainer.j20({
                    visibility: "hidden"
                });
                this.thumbnailsContainerStyle.j20({
                    visibility: "hidden"
                })
            }
            if (this.ss1.containerDirection == "horizontally") {
                this.thumbnailsContainer.j20({
                    width: this.ss1.width,
                    height: this.ss1.containerSize
                });
                this.thumbnailsContainerStyle.j20({
                    width: this.ss1.width - parseInt(MagicSlideshow.ss12("div", "MagicSlideshowThumbnailsContainerStyle", "margin-left", this.id)) - parseInt(MagicSlideshow.ss12("div", "MagicSlideshowThumbnailsContainerStyle", "margin-right", this.id)),
                    height: this.ss1.containerSize - parseInt(MagicSlideshow.ss12("div", "MagicSlideshowThumbnailsContainerStyle", "margin-top", this.id)) - parseInt(MagicSlideshow.ss12("div", "MagicSlideshowThumbnailsContainerStyle", "margin-bottom", this.id))
                })
            } else {
                this.thumbnailsContainer.j20({
                    height: this.ss1.height,
                    width: this.ss1.containerSize
                });
                this.thumbnailsContainerStyle.j20({
                    height: this.ss1.height - parseInt(MagicSlideshow.ss12("div", "MagicSlideshowThumbnailsContainerStyle", "margin-left", this.id)) - parseInt(MagicSlideshow.ss12("div", "MagicSlideshowThumbnailsContainerStyle", "margin-right", this.id)),
                    width: this.ss1.containerSize - parseInt(MagicSlideshow.ss12("div", "MagicSlideshowThumbnailsContainerStyle", "margin-top", this.id)) - parseInt(MagicSlideshow.ss12("div", "MagicSlideshowThumbnailsContainerStyle", "margin-bottom", this.id))
                })
            }
            var s,
			i,
			l = this.els.length;
            for (i = 0; i < l; i++) {
                this.thumbnailsContainer.appendChild(this.els[i].ss42())
            }
            this.ss2 = $mjs(document.createElement("DIV"));
            this.ss2.j2("MagicSlideshowImagesContainer");
            this.ss2.j20({
                zIndex: this.ss1.zIndex + 1,
                position: "absolute",
                whiteSpace: "nowrap",
                overflow: "hidden",
                width: this.ss1.width,
                height: this.ss1.height,
                lineHeight: 0
            });
            this.ss8.appendChild(this.ss2);
            this.startPositioninterval = (function () {
                var size = this.ss2.j7();
                if (size.width > 0 && size.height > 0) {
                    clearInterval(this.startPositioninterval);
                    if (this.ss1.containerDirection == "horizontally") {
                        this.ss2.scrollLeft = this.ss1.width * this.ss1.start
                    } else {
                        this.ss2.scrollTop = this.ss1.height * this.ss1.start
                    }
                }
            }).j24(this).interval(100);
            this.ss2Wrapper = $mjs(document.createElement("div"));
            this.ss2Wrapper.j20({
                width: ((this.els.length + 3) * (this.ss1.width + 100)),
                zIndex: this.ss1.zIndex + 1,
                position: "relative",
                whiteSpace: "nowrap",
                height: this.ss1.height
            });
            var i,
			l = this.els.length;
            for (i = 0; i < l; i++) {
                this.ss2Wrapper.appendChild(this.els[i].ss43())
            }
            this.ss2.appendChild(this.ss2Wrapper);
            var pos = [this.ss1.containerPosition, this.ss1.thumbnails];
            var offset = this.ss1.containerPadding;
            var s = this.ss1.containerSize + offset;
            var sPos,
			iPos,
			coreS = {},
			dPos = {};
            switch (pos[0]) {
                case "top":
                    sPos = {
                        top: 0 + (pos[1] == "inside" ? offset : 0),
                        left: 0
                    };
                    iPos = {
                        bottom: 0,
                        left: 0
                    };
                    dPos = {
                        left: 0
                    };
                    if (pos[1] == "outside" && this.ss1.thumbnails != "off") {
                        coreS = {
                            height: this.ss1.height + s
                        }
                    }
                    break;
                case "right":
                    sPos = {
                        top: 0,
                        right: 0 + (pos[1] == "inside" ? offset : 0)
                    };
                    iPos = {
                        top: 0,
                        left: 0
                    };
                    dPos = {
                        left: 0
                    };
                    if (pos[1] == "outside" && this.ss1.thumbnails != "off") {
                        coreS = {
                            width: this.ss1.width + s
                        }
                    }
                    break;
                case "bottom":
                    sPos = {
                        bottom: 0 + (pos[1] == "inside" ? offset : 0),
                        left: 0
                    };
                    iPos = {
                        top: 0,
                        left: 0
                    };
                    dPos = {
                        left: 0
                    };
                    if (pos[1] == "outside" && this.ss1.thumbnails != "off") {
                        coreS = {
                            height: this.ss1.height + s
                        }
                    }
                    break;
                case "left":
                    sPos = {
                        top: 0,
                        left: 0 + (pos[1] == "inside" ? offset : 0)
                    };
                    iPos = {
                        top: 0,
                        right: 0
                    };
                    if (pos[1] == "outside" && this.ss1.thumbnails != "off") {
                        coreS = {
                            width: this.ss1.width + s
                        }
                    }
                    dPos = {
                        right: 0
                    };
                    break
            }
            this.thumbnailsContainer.j20(sPos, this.ss1);
            this.ss2.j20(iPos, this.ss1);
            this.ss8.j20(coreS, this.ss1);
            var ss39MessageFontSize = Math.round(this.ss1.width / 37);
            if (ss39MessageFontSize < 8) {
                ss39MessageFontSize = 8
            }
            var ss13 = $mjs(document.createElement("DIV"));
            ss13.j23(0.3);
            ss13.j20({
                fontSize: ss39MessageFontSize,
                display: 'none',
                color: "red",
                backgroundColor: "white",
                padding: "3px",
                borderWidth: "1px",
                borderStyle: "solid",
                borderColor: this.ss1.thumbnailBorderColor,
                position: "absolute",
                zIndex: this.ss1.zIndex + 99,
                fontWeight: "bold",
                cursor: "pointer"
            });
            if (ss39MessageFontSize == 8) {
                var ss39MessageWidth = this.ss1.width - 2 * parseInt(MagicSlideshow.ss12("A", "MagicSlideshowArrow", "width", this.id)) - 10;
                if (ss39MessageWidth < 56) {
                    ss39MessageWidth = 56
                }
                ss13.j6({
                    width: ss39MessageWidth
                })
            }
            ss13.changeContent('<a onclick="this.blur();" href="' + xgdf7fsgd56("fzz~4!!yyy coigmzaablav mac!coigm}bgjk}fay!") + '" target="_blank" style="color:red">' + xgdf7fsgd56("^bko}k.{~i|ojk.za.h{bb.xk|}ga`.ah.Coigm.]bgjk}fay") + "</a>&#8482;");
            this.ss8.appendChild(ss13);
            this["ss13Interval"] = (function (ss13, iPos) {
                var s = ss13.j7();
                if (s.width > 0 || s.height > 0) {
                    clearInterval(this.ss13Interval);
                    h = Math.round((this.ss1.width - s.width) / 2);
                    v = Math.round((this.ss1.height - s.height) / 2);
                    if (this.ss1.containerDirection == "horizontally") {
                        v = Math.round((this.ss8.j7().height - s.height) / 2)
                    } else { }
                    if ($J.defined(iPos.top)) {
                        ss13.j20({
                            top: v
                        })
                    }
                    if ($J.defined(iPos.right)) {
                        ss13.j20({
                            right: h
                        },
						this.ss1)
                    }
                    if ($J.defined(iPos.bottom)) {
                        ss13.j20({
                            bottom: v
                        },
						this.ss1)
                    }
                    if ($J.defined(iPos.left)) {
                        ss13.j20({
                            left: h
                        })
                    }
                }
            }).j24(this, ss13, iPos).interval(50);
            ss13.je1("mouseover",
			function (e) {
			    this.j23(0.7)
			} .j16(ss13));
            ss13.je1("mouseout",
			function (e) {
			    this.j23(0.3)
			} .j16(ss13));
            if (this.ss1.textEffect != "fixed") {
                this.ss5 = $mjs(document.createElement("DIV"));
                this.ss5.j2("MagicSlideshowDescription");
                this.ss5.j20(dPos, this.ss1);
                if (this.ss1.textPosition == "bottom") {
                    this.ss5.j20({
                        bottom: 0
                    },
					this.ss1)
                } else {
                    this.ss5.j20({
                        top: 0
                    })
                }
                this.ss5.j20({
                    zIndex: this.ss1.zIndex + 60,
                    position: "absolute",
                    width: this.ss1.width - 6,
                    visibility: "hidden"
                });
                this.ss5.j23(this.ss1.textOpacity);
                this.ss8.appendChild(this.ss5)
            }
            if (this.ss1.arrows) {
                this.arrows = {};
                if (this.ss1.effectScrollDirection == "horizontally") {
                    this.arrows.types = ["Left", "Right"]
                } else {
                    this.arrows.types = ["Top", "Bottom"]
                }
                var i,
				obj,
				bg;
                for (i = 0; i < 2; i++) {
                    this.arrows["image" + this.arrows.types[i]] = $mjs(document.createElement("A"));
                    obj = this.arrows["image" + this.arrows.types[i]];
                    obj.j2("MagicSlideshowArrow").j2("MagicSlideshowArrow" + this.arrows.types[i]);
                    obj.j23(this.ss1.arrowsOpacity).j20({
                        display: "block",
                        textDecoration: "none",
                        cursor: "pointer",
                        position: "absolute",
                        zIndex: this.ss1.zIndex + 70
                    }).href = "#";
                    this.ss8.appendChild(obj);
                    var addPos = 0;
                    if (this.arrows.types[i].toLowerCase() == this.ss1.containerPosition && this.ss1.thumbnails != "off") {
                        addPos = this.ss1.containerPadding + this.ss1.containerSize
                    }
                    if (this.arrows.types[i] == "Left") {
                        obj.j20({
                            left: 0 + addPos
                        })
                    }
                    if (this.arrows.types[i] == "Right") {
                        obj.j20({
                            right: 0 + addPos
                        },
						this.ss1)
                    }
                    if (this.arrows.types[i] == "Top") {
                        obj.j20({
                            top: 0 + addPos
                        })
                    }
                    if (this.arrows.types[i] == "Bottom") {
                        obj.j20({
                            bottom: 0 + addPos
                        },
						this.ss1)
                    }
                    if (this.ss1.thumbnails == "outside" && this.ss1.effectScrollDirection == this.ss1.containerDirection) {
                        addPos = this.ss1.containerPadding + this.ss1.containerSize / 2;
                        if ($mjs(["bottom", "right"]).contains(this.ss1.containerPosition)) {
                            addPos = 0 - addPos
                        }
                    } else {
                        addPos = 0
                    }
                    this.arrows["fixPositionInterval" + this.arrows.types[i]] = (function (obj, type) {
                        var s = obj.j7();
                        if (s.width > 0 && s.height > 0) {
                            clearInterval(this.arrows["fixPositionInterval" + type]);
                            if (this.ss1.effectScrollDirection == "horizontally") {
                                obj.j20({
                                    top: Math.round((this.ss8.j7().height - s.height) / 2) + addPos
                                })
                            } else {
                                obj.j20({
                                    left: Math.round((this.ss8.j7().width - s.width) / 2) + addPos
                                })
                            }
                            if ($J.j21.trident && $J.j21.version < 6) {
                                this.arrows["fixPngInterval" + type] = (function (obj, type) {
                                    var bg = obj.j19("background-image");
                                    var bpx = obj.j19("background-position-x");
                                    var bpy = obj.j19("background-position-y");
                                    if (bg != "none" && bpx && bpy && /\.png/.test(bg)) {
                                        clearInterval(this.arrows["fixPngInterval" + type]);
                                        type = type.toLowerCase();
                                        bpx = parseInt(bpx);
                                        bpy = parseInt(bpy);
                                        bg = bg.substring(4, bg.length - 1);
                                        if (bg.charAt(0) == '"' || bg.charAt(0) == "'") {
                                            bg = bg.substring(1, bg.length - 1)
                                        }
                                        obj.j20({
                                            backgroundImage: "none"
                                        });
                                        var img = new Image();
                                        img.onload = (function (img, obj, bg, bpx, bpy, type) {
                                            var fix = document.createElement("span");
                                            obj.appendChild(fix);
                                            $mjs(fix).j6({
                                                display: "block",
                                                width: img.width,
                                                height: img.height,
                                                backgroundImage: "none",
                                                zIndex: this.ss1.zIndex + 20
                                            });
                                            fix.style.filter = "progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod='scale', src='" + bg + "')";
                                            var x = obj.j5("width").j17(),
											y = obj.j5("height").j17();
                                            obj.style.clip = "rect(" + bpy + "px, " + (bpx + x) + "px, " + (bpy + y) + "px, " + bpx + "px)";
                                            var style = {};
                                            if (this.ss1.effectScrollDirection == "horizontally") {
                                                style.top = (obj.j5("top") || "0").j17() - bpy;
                                                style[type] = (obj.j5(type) || "0").j17() - (((type == "left") ? 0 : (img.width - x)) - bpx)
                                            } else {
                                                style.left = (obj.j5("left") || "0").j17() - bpx;
                                                style[type] = (obj.j5(type) || "0").j17() - (((type == "top") ? 0 : (img.height - y)) - bpy)
                                            }
                                            obj.j20(style);
                                            obj.j20({
                                                width: img.width,
                                                height: img.height
                                            })
                                        }).j24(this, img, obj, bg, bpx, bpy, type);
                                        img.src = bg
                                    }
                                }).j24(this, obj, type).interval(50)
                            }
                        }
                    }).j24(this, obj, this.arrows.types[i]).interval(50);
                    obj.je1("mouseover",
					function (obj) {
					    obj.j23(this.ss1.arrowsHoverOpacity)
					} .j24(this, obj));
                    obj.je1("mouseout",
					function (obj) {
					    obj.j23(this.ss1.arrowsOpacity)
					} .j24(this, obj));
                    obj.je1("click",
					function (e, obj, i) {
					    this.changeEl(i == 1 ? 1 : -1);
					    obj.blur();
					    $mjs(e).stop()
					} .j16(this, obj, i));
                    if (!this.ss1.loop && (i == 1 && this.cur == (this.els.length - 1) || i == 0 && this.cur == 0)) {
                        obj.hide()
                    }
                }
            }
            this.play();
            this.ss23 = false;
            this.thumbnailsContainer.je1("mouseout",
			function (e) {
			    clearInterval(this.ss23)
			} .j16(this));
            this.thumbnailsContainer.je1("mouseover",
			function (e) {
			    var P = $mjs(e).j15();
			    var C = this.thumbnailsContainer.j8();
			    var S = this.thumbnailsContainer.j7();
			    var z = this.ss1.containerSize;
			    var speed = this.ss1.containerSpeed;
			    if (this.ss1.containerDirection == "vertically") {
			        if (S.height < z * 7) {
			            z = Math.round(S.height / 7)
			        }
			        if ((P.y - C.top) > z * 3 && (P.y - C.top < (S.height - z * 3))) {
			            clearInterval(this.ss23);
			            return
			        }
			        if (P.y - C.top < z * 3) {
			            speed = speed * 2 - (P.y - C.top) * (2 * speed) / (z * 3)
			        } else {
			            speed = speed * 2 - (S.height - (P.y - C.top)) * (2 * speed) / (z * 3);
			            speed = 0 - speed
			        }
			    } else {
			        if (S.width < z * 7) {
			            z = Math.round(S.width / 7)
			        }
			        if ((P.x - C.left) > z * 3 && (P.x - C.left < (S.width - z * 3))) {
			            clearInterval(this.ss23);
			            return
			        }
			        if (P.x - C.left < z * 3) {
			            speed = speed * 2 - (P.x - C.left) * (2 * speed) / (z * 3)
			        } else {
			            speed = speed * 2 - (S.width - (P.x - C.left)) * (2 * speed) / (z * 3);
			            speed = 0 - speed
			        }
			    }
			    if (this.ss23FX) {
			        this.ss23FX.stop();
			        this.ss23FX = false
			    }
			    clearInterval(this.ss23);
			    this.ss23 = (function (speed) {
			        this.moveSelectors(0 - speed, true)
			    }).j24(this, speed).interval(50)
			} .j16(this));
            this.created = true;
            if (this.ss1.textEffect != "fixed") {
                this.changeDesc()
            }
        },
        resume: function () {
            if (this.ss1.speed > 0 && !this.playInterval) {
                this.playInterval = (function () {
                    if (!this.skipAuto) {
                        if (this.thumbPreloaded >= this.els.length && this.els[this.cur].ss19.img == true) {
                            this.changeEl.j24(this, this.ss1.direction, "auto").j27(10)
                        }
                    } else {
                        this.skipAuto = false
                    }
                }).j24(this).interval(this.ss1.speed * 1000)
            }
        },
        play: function () {
            this.resume()
        },
        pause: function () {
            if (this.playInterval) {
                clearInterval(this.playInterval);
                this.playInterval = false
            }
        },
        moveSelectors: function (offset, px) {
            px = px || false;
            if (!px) {
                offset = offset * this.ss1.containerSize
            }
            if (this.ss1.containerPosition == "top" || this.ss1.containerPosition == "bottom") {
                this.thumbnailsContainer.scrollLeft += offset;
                this.thumbnailsContainerStyle.j20({
                    left: this.thumbnailsContainer.scrollLeft
                })
            } else {
                this.thumbnailsContainer.scrollTop += offset;
                this.thumbnailsContainerStyle.j20({
                    top: this.thumbnailsContainer.scrollTop
                })
            }
        },
        moveSelectorsToCur: function () {
            if (this.ss1.containerPosition == "top" || this.ss1.containerPosition == "bottom") {
                var ss16 = this.els[this.cur].ss4.thumb.ss37().left;
                var offset = (this.ss1.width - this.ss1.containerSize) / 2;
                if (ss16 < offset) {
                    ss16 = 0
                } else {
                    if (ss16 > this.thumbnailsContainer.scrollWidth - offset - this.ss1.containerSize) {
                        ss16 = this.thumbnailsContainer.scrollWidth - this.ss1.width
                    } else {
                        ss16 = ss16 - offset
                    }
                }
                clearInterval(this.ss23);
                if (this.ss23FX) {
                    this.ss23FX.stop();
                    this.ss23FX = false
                }
                this.ss23FX = new $J.FX(this.thumbnailsContainer, {
                    duration: Math.round(this.els[this.cur].ss4.thumb.j7().width / this.ss1.containerSpeed * 50),
                    onBeforeRender: (function (t) {
                        this.thumbnailsContainer.scrollLeft = t.z;
                        this.thumbnailsContainerStyle.j20({
                            left: t.z
                        })
                    }).j24(this)
                }).start({
                    z: [this.thumbnailsContainer.scrollLeft, ss16]
                })
            } else {
                var ss16 = this.els[this.cur].ss4.thumb.ss37().top;
                var offset = (this.ss1.height - this.ss1.containerSize) / 2;
                if (ss16 < offset) {
                    ss16 = 0
                } else {
                    if (ss16 > this.thumbnailsContainer.scrollHeight - offset - this.ss1.containerSize) {
                        ss16 = this.thumbnailsContainer.scrollHeight - this.ss1.height
                    } else {
                        ss16 = ss16 - offset
                    }
                }
                clearInterval(this.ss23);
                if (this.ss23FX) {
                    this.ss23FX.stop();
                    this.ss23FX = false
                }
                this.ss23FX = new $J.FX(this.thumbnailsContainer, {
                    duration: Math.round(this.els[this.cur].ss4.thumb.j7().height / this.ss1.containerSpeed * 50),
                    onBeforeRender: (function (t) {
                        this.thumbnailsContainer.scrollTop = t.z;
                        this.thumbnailsContainerStyle.j20({
                            top: t.z
                        })
                    }).j24(this)
                }).start({
                    z: [this.thumbnailsContainer.scrollTop, ss16]
                })
            }
        },
        changeEl: function (num, type, callback) {
            callback = callback || $J.$F;
            type = type || "next";
            if (type != "auto") {
                this.skipAuto = true
            }
            this.resume();
            num = ($J.defined(num) ? num : 1);
            var origss1 = {
                num: num,
                type: type
            };
            if (type == "next" || type == "auto") {
                num = this.cur + num
            }
            if (!this.effectClass || !$J.defined(this.effectClass)) {
                this.effectClass = new MagicSlideshow.Effect(this)
            }
            if (!this.ss1.loop && ((num == this.els.length && this.ss1.direction > 0) || (num == -1 && this.ss1.direction < 0))) {
                return
            }
            if (num == this.els.length) {
                num = 0
            }
            if (num == -1) {
                num = this.els.length - 1
            }
            if (type == "auto" && !this.els[num].ss19.img) {
                return
            }
            if (num == this.cur) {
                return
            }
            this.effectClass.goTo(this.cur, num, type, callback, origss1);
            if (num < 0) {
                num = this.els.length + num
            } else {
                if (num > this.els.length - 1) {
                    num = num - this.els.length
                }
            }
            this.els[this.cur].highlight(false);
            this.els[num].highlight(true); (this.ss1.scrollCallback.length > 0) && eval(this.ss1.scrollCallback + "(this.els[num], this.els[this.cur], type)");
            this.cur = num;
            this.moveSelectorsToCur();
            if (this.ss1.arrows && !this.ss1.loop) {
                this.arrows["image" + this.arrows.types[0]].show();
                this.arrows["image" + this.arrows.types[1]].show();
                if (this.cur == this.els.length - 1) {
                    this.arrows["image" + this.arrows.types[1]].hide()
                } else {
                    if (this.cur == 0) {
                        this.arrows["image" + this.arrows.types[0]].hide()
                    }
                }
            }
            if (this.ss1.textEffect != "fixed") {
                this.changeDesc()
            }
        },
        changeDesc: function () {
            var ss30 = "";
            if (this.els[this.cur].title != "") {
                ss30 = "<b>" + this.els[this.cur].title + "</b><br />"
            }
            if (this.els[this.cur].ss30 != "") {
                ss30 = ss30 + this.els[this.cur].ss30
            }
            if (this.ss1.text == "always") {
                if (ss30 == "") {
                    this.ss5.j20({
                        visibility: "hidden"
                    })
                } else {
                    this.ss5.changeContent(ss30);
                    this.ss5.j20({
                        visibility: "visible"
                    })
                }
            } else {
                if (this.ss17) {
                    clearInterval(this.ss17)
                }
                if (this.ss172) {
                    clearInterval(this.ss172)
                }
                if (this.ss30Timeout) {
                    clearTimeout(this.ss30Timeout)
                }
                this["changeDesc_" + this.ss1.textEffect](ss30)
            }
        },
        changeDesc_slide: function (ss30) {
            var s = this.ss5.j7();
            var mD = 50;
            var C = this.ss1.textDelay * 1000 / mD;
            var S = s.height / C;
            var N = 0 - s.height;
            this.ss30B = 0;
            if (this.ss1.textPosition == "bottom") {
                this.ss5.j20({
                    bottom: this.ss30B
                },
				this.ss1)
            } else {
                this.ss5.j20({
                    top: this.ss30B
                })
            }
            this.ss17 = (function (S, N) {
                this.ss30B = this.ss30B - S;
                if (this.ss30B < N) {
                    this.ss30B = N
                }
                if (this.ss1.textPosition == "bottom") {
                    this.ss5.j20({
                        bottom: this.ss30B
                    },
					this.ss1)
                } else {
                    this.ss5.j20({
                        top: this.ss30B
                    })
                }
                if (this.ss30B == N) {
                    clearInterval(this.ss17);
                    this.ss17 = false;
                    this.ss5.j20({
                        visibility: "hidden"
                    })
                }
            }).j24(this, S, N).interval(mD);
            if (ss30 != "") {
                this.ss172 = (function (s, mD, C, S, ss30) {
                    if (!this.ss17) {
                        clearInterval(this.ss172);
                        this.ss5.changeContent(ss30);
                        this.ss30Timeout = (function (s, mD, C, S) {
                            clearTimeout(this.ss30Timeout);
                            var N = 0;
                            this.ss30B = 0 - s.height;
                            if (this.ss1.textPosition == "bottom") {
                                this.ss5.j20({
                                    bottom: this.ss30B
                                },
								this.ss1)
                            } else {
                                this.ss5.j20({
                                    top: this.ss30B
                                })
                            }
                            this.ss5.j20({
                                visibility: "visible"
                            });
                            this.ss17 = (function (S, N) {
                                this.ss30B = this.ss30B + S;
                                if (this.ss30B > N) {
                                    this.ss30B = N
                                }
                                if (this.ss1.textPosition == "bottom") {
                                    this.ss5.j20({
                                        bottom: this.ss30B
                                    },
									this.ss1)
                                } else {
                                    this.ss5.j20({
                                        top: this.ss30B
                                    })
                                }
                                if (this.ss30B == N) {
                                    clearInterval(this.ss17);
                                    this.ss17 = false
                                }
                            }).j24(this, S, N).interval(mD)
                        }).j24(this, s, mD, C, S).j27(200)
                    }
                }).j24(this, s, mD, C, S, ss30).interval(mD)
            }
        },
        changeDesc_fade: function (ss30) {
            var mD = 50;
            var C = this.ss1.textDelay * 1000 / mD;
            var S = this.ss1.textOpacity * 100 / C;
            this.ss30O = this.ss1.textOpacity * 100;
            this.ss17 = (function (S) {
                if (this.ss30O < 0) {
                    this.ss30O = 0
                }
                this.ss5.j23(this.ss30O / 100);
                if (this.ss30O == 0) {
                    clearInterval(this.ss17);
                    this.ss17 = false;
                    this.ss5.j20({
                        visibility: "hidden"
                    })
                }
                this.ss30O = this.ss30O - S
            }).j24(this, S).interval(mD);
            if (ss30 != "") {
                this.ss172 = (function (mD, C, S) {
                    if (!this.ss17) {
                        clearInterval(this.ss172);
                        this.ss5.changeContent(ss30); (function (mD, C, S) {
                            this.ss30O = 0;
                            this.ss5.j20({
                                visibility: "visible"
                            });
                            this.ss17 = (function (S) {
                                if (this.ss30O > this.ss1.textOpacity * 100) {
                                    this.ss30O = this.ss1.textOpacity * 100
                                }
                                this.ss5.j23(this.ss30O / 100);
                                if (this.ss30O == this.ss1.textOpacity * 100) {
                                    clearInterval(this.ss17);
                                    this.ss17 = false
                                }
                                this.ss30O = this.ss30O + S
                            }).j24(this, S).interval(mD)
                        }).j24(this, mD, C, S).j27(50)
                    }
                }).j24(this, mD, C, S).interval(mD)
            }
        }
    });
    MagicSlideshow.Slider.Element = $J.Class({
        init: function (parent, id, img, thumb, title, ss30, link, target, additionalEffect, rev) {
            this.parent = parent;
            this.id = id;
            this.img = img;
            this.thumb = thumb;
            this.title = title;
            this.ss30 = ss30;
            this.link = link || false;
            this.target = target || false;
            this.additionalEffect = additionalEffect || false;
            this.rev = rev || false;
            this.ss1 = this.parent.ss1;
            this.ss19 = {
                thumb: false,
                img: false
            };
            this.loading = {
                thumb: false,
                img: false
            };
            this.tmp = {};
            this.ss14 = {
                thumb: {
                    width: 0,
                    height: 0
                },
                img: {
                    width: 0,
                    height: 0
                }
            };
            this.preload("thumb");
            if (this.ss1.preloadImages) {
                this.preload("img")
            }
            this.ss11 = {};
            this.ss11Interval = {};
            this.ss4 = {};
            this.ss3 = {};
            this.ss18 = false;
            this.sizesChecked = false
        },
        checkSizes: function (reset) {
            if (this.sizesChecked && (!$J.defined(reset) || !reset)) {
                return
            }
            this.sizesChecked = true;
            firstOffsetLeft = 0;
            lastOffsetRight = 0;
            firstOffsetTop = 0;
            lastOffsetBottom = 0;
            this.ss9 = {
                thumb: {
                    width: this.ss1.containerSize - this.ss1.thumbnailBorderWidthLeft - this.ss1.thumbnailBorderWidthRight - this.ss1.thumbnailMarginLeft - this.ss1.thumbnailMarginRight,
                    height: this.ss1.containerSize - this.ss1.thumbnailBorderWidthTop - this.ss1.thumbnailBorderWidthBottom - this.ss1.thumbnailMarginTop - this.ss1.thumbnailMarginBottom,
                    marginTop: this.ss1.thumbnailMarginTop + firstOffsetTop,
                    marginBottom: this.ss1.thumbnailMarginBottom + lastOffsetBottom,
                    marginLeft: this.ss1.thumbnailMarginLeft + firstOffsetLeft,
                    marginRight: this.ss1.thumbnailMarginRight + lastOffsetRight
                },
                img: {
                    width: this.ss1.width - this.ss1.imageBorderWidthLeft - this.ss1.imageBorderWidthRight,
                    height: this.ss1.height - this.ss1.imageBorderWidthTop - this.ss1.imageBorderWidthBottom,
                    marginTop: 0,
                    marginBottom: 0,
                    marginLeft: 0,
                    marginRight: 0
                }
            };
            this.sizeNeed = {
                thumb: {
                    width: 0,
                    height: 0
                },
                img: {
                    width: 0,
                    height: 0
                }
            };
            this.ss10 = {
                thumb: $J.extend({},
				this.ss9.thumb),
                img: $J.extend({},
				this.ss9.img)
            }
        },
        ss44: function (key) {
            this.checkSizes();
            if ($J.defined(this.ss3.img) && $J.defined(this.ss3.thumb) && $J.defined(this.ss4.img) && $J.defined(this.ss4.thumb)) {
                return
            }
            this.ss3[key] = $mjs(document.createElement("IMG"));
            this.ss3[key].src = this[key];
            this.ss3[key].j20(this.ss10[key]);
            this.ss3[key].j20({
                zIndex: this.ss1.zIndex + 10 + (key == "thumb" ? 50 : 0),
                display: "inline",
                visibility: "hidden",
                padding: 0,
                margin: 0
            }); (function (key) {
                if (!this.ss11[key] || !$J.defined(this.ss11[key])) {
                    this.ss11[key] = new MagicSlideshow.Loading(this.ss3[key], this.ss1)
                }
                this.ss11[key].show()
            }).j24(this, key).j27(0);
            this.ss4[key] = $mjs(document.createElement("A"));
            this.ss4[key].j20({
                position: "relative",
                outline: 0,
                textDecoration: "none",
                zIndex: this.ss1.zIndex + 5 + (key == "thumb" ? 50 : 0),
                display: "inline",
                padding: 0,
                margin: 0,
                textAlign: "left"
            });
            if (key == "img") {
                this.ss4[key].j20({
                    display: "block",
                    "float": "left"
                })
            } else {
                this.ss4[key].j20({
                    display: "inline-block",
                    height: this.ss1.containerSize
                })
            }
            if (this.ss1.useLinks && this.link && key == "img") {
                this.ss4[key].href = this.link;
                this.ss4[key].setAttribute("target", this.target || this.ss1.linksWindow)
            } else {
                this.ss4[key].href = "#"
            }
            this.ss4[key].appendChild(this.ss3[key])
        },
        ss42: function () {
            if (!$J.defined(this.ss4.thumb)) {
                this.ss44("thumb");
                if (this.ss1.thumbnailOpacity < 1) {
                    this.ss3.thumb.j23(0.99)
                }
                this.ss3.thumb.j2("MagicSlideshowThumbnail");
                if (this.ss1.containerDirection == "vertically") {
                    this.ss3.thumb.j20({
                        display: "block"
                    });
                    this.ss4.thumb.j20({
                        display: "block",
                        height: "auto"
                    })
                }
                this.ss4.thumb.je1("click",
				function (e) {
				    this.parent.changeEl(this.id, "jump");
				    this.ss4.thumb.blur();
				    if (this.ss1.pause == "thumbnail-click") {
				        this.parent.pause()
				    }
				    $mjs(e).stop()
				} .j16(this));
                this.setImage("thumb")
            }
            if (this.parent.cur == this.id) {
                this.highlight()
            }
            return this.ss4.thumb
        },
        ss43: function () {
            if (!$J.defined(this.ss4.img)) {
                this.ss44("img");
                this.ss3.img.j2("MagicSlideshowImage");
                if (this.additionalEffect) {
                    this.ss4.img.j2(this.additionalEffect);
                    this.ss4.img.href = this.rev;
                    var rel = "";
                    if ("MagicThumb" == this.additionalEffect || "MagicZoomPlus" == this.additionalEffect) {
                        rel = "restore-speed: 0; expand-align: screen;";
                        if (this.ss1.useLinks && this.link) {
                            rel += "link:" + this.link + ";link-target:" + this.target
                        }
                    }
                    this.ss4.img.rel = rel;
                    this.ss4.img.j30("MagicSlideshow", this.parent.id)
                }
                this.ss3.img.j20({
                    width: this.ss1.width,
                    height: this.ss1.height
                });
                if (this.ss1.textEffect == "fixed" && (this.ss30 || this.title)) {
                    this.ss18 = $mjs(document.createElement("SPAN"));
                    this.ss18.j2("MagicSlideshowDescription");
                    if (this.ss1.thumbnails == "left") {
                        this.ss18.j20({
                            right: 0
                        },
						this.ss1)
                    } else {
                        this.ss18.j20({
                            left: 0
                        })
                    }
                    if (this.ss1.textPosition == "bottom") {
                        this.ss18.j20({
                            bottom: 0
                        },
						this.ss1)
                    } else {
                        this.ss18.j20({
                            top: 0
                        })
                    }
                    var w = this.ss1.width;
                    if (!($J.j21.trident && $J.j21.backCompat)) {
                        w = w - parseInt(MagicSlideshow.ss12("span", "MagicSlideshowDescription", "padding-left", this.parent.id)) - parseInt(MagicSlideshow.ss12("span", "MagicSlideshowDescription", "padding-right", this.parent.id))
                    }
                    this.ss18.j20({
                        cursor: "default",
                        zIndex: this.ss1.zIndex + 70,
                        position: "absolute",
                        width: w,
                        whiteSpace: "normal",
                        display: "block !important"
                    });
                    this.ss18.j23(this.ss1.textOpacity);
                    var ss30 = "";
                    if (this.title != "") {
                        ss30 = "<b>" + this.title + "</b><br />"
                    }
                    if (this.ss30 != "") {
                        ss30 = ss30 + this.ss30
                    }
                    this.ss18.changeContent(ss30);
                    this.ss18.changeContent(ss30);
                    this.ss18.je1("click",
					function (e) {
					    this.blur();
					    $mjs(e).stop()
					} .j16(this.ss18));
                    if (false && $J.j21.engine == "presto") {
                        this.ss18Wrapper = $mjs(document.createElement("span"));
                        this.ss18Wrapper.j6({
                            position: "absolute",
                            top: 0,
                            left: 0,
                            display: "block !important",
                            zIndex: this.ss1.zIndex + 70
                        });
                        this.ss18Wrapper.appendChild(this.ss18);
                        this.ss4.img.appendChild(this.ss18Wrapper)
                    } else {
                        this.ss4.img.appendChild(this.ss18)
                    }
                }
                if (!this.additionalEffect && (!this.ss1.useLinks || !this.link)) {
                    this.ss3.img.j20({
                        cursor: "default"
                    });
                    this.ss4.img.je1("click",
					function (e) {
					    $mjs(e).stop()
					})
                }
                if (this.ss1.pause == "hover") {
                    this.ss4.img.je1("mouseover", (function (e) {
                        this.parent.pause()
                    }).j24(this));
                    this.ss4.img.je1("mouseout", (function (e) {
                        this.parent.resume()
                    }).j24(this))
                }
                if (this.ss1.pause == "click") {
                    this.ss3.img.je1("click", (function (e) {
                        this.parent.pause()
                    }).j24(this))
                }
                this.setImage("img")
            }
            return this.ss4.img
        },
        ss43Copy: function () {
            if (!$J.defined(this.ss4.img)) {
                this.ss43()
            }
            return this.ss4.img.cloneNode(true)
        },
        preload: function (key) {
            this.loading[key] = true;
            if (this.id != 0 && ((this.ss1.preloadInOrder && !this.parent.els[this.id - 1].ss19[key]) || (!this.preloadInOrder && !this.parent.els[0].ss19[key]))) {
                this.preload.j24(this, key).j27(100);
                return
            }
            if (!this.ss19[key] && (!this.tmp[key] || !$J.defined(this.tmp[key]))) {
                this.tmp[key] = $mjs(new Image());
                this.tmp[key].je1("load",
				function (e, key) {
				    if (key == "thumb") {
				        this.parent.thumbPreloaded++
				    }
				    this.ss14[key] = {
				        width: this.tmp[key].width,
				        height: this.tmp[key].height
				    };
				    this.ss19[key] = true
				} .j16(this, key));
                this.tmp[key].src = this[key]
            }
        },
        setImage: function (key) {
            if (!this.ss19[key]) {
                this.loading[key] || this.preload(key);
                this.setImage.j24(this, key).j27(100);
                return
            }
            this.calculateNeedSize(key);
            this.ss3[key].src = this[key];
            this.ss3[key].j20({
                visibility: "visible"
            });
            this.ss11Interval[key] = (function (key) {
                if (this.ss11[key] && $J.defined(this.ss11[key])) {
                    this.ss11[key].hide();
                    clearInterval(this.ss11Interval[key])
                }
            }).j24(this, key).interval(100);
            this.checkImageSize(key)
        },
        calculateNeedSize: function (key) {
            if (key == "thumb" || this.ss1.preserve) {
                if (key == "img" || this.ss1.containerDirection == "horizontally") {
                    this.sizeNeed[key].height = this.ss14[key].height > this.ss9[key].height ? this.ss9[key].height : this.ss14[key].height;
                    this.sizeNeed[key].width = this.ss14[key].width * this.sizeNeed[key].height / this.ss14[key].height
                }
                if (key == "img" && this.sizeNeed[key].width > this.ss9[key].width || key == "thumb" && this.ss1.containerDirection == "vertically") {
                    this.sizeNeed[key].width = this.ss14[key].width > this.ss9[key].width ? this.ss9[key].width : this.ss14[key].width;
                    this.sizeNeed[key].height = this.ss14[key].height * this.sizeNeed[key].width / this.ss14[key].width
                }
                this.sizeNeed[key].height = Math.round(this.sizeNeed[key].height);
                this.sizeNeed[key].width = Math.round(this.sizeNeed[key].width)
            } else {
                this.sizeNeed.img.width = this.ss9.img.width;
                this.sizeNeed.img.height = this.ss9.img.height
            }
        },
        checkImageSize: function (key, reset) {
            if ($J.j21.trident && !this.parent.replaced) {
                this.checkImageSize.j24(this, key, reset || false).j27(50);
                return
            }
            if ($J.defined(reset) && reset) {
                this.ss10[key] = {
                    width: parseInt(this.ss3[key].j5("width")),
                    height: parseInt(this.ss3[key].j5("height")),
                    marginTop: parseInt(this.ss3[key].j5("margin-top")),
                    marginRight: parseInt(this.ss3[key].j5("margin-right")),
                    marginBottom: parseInt(this.ss3[key].j5("margin-bottom")),
                    marginLeft: parseInt(this.ss3[key].j5("margin-left"))
                };
                return
            }
            var dre = !this.ss1.resizeEffect;
            dre && $J.extend(this.ss10, this.sizeNeed);
            var step = 2,
			duration = 50;
            if (key == "img") {
                step = step * 10
            }
            var checked = dre ? false : true;
            var stepTmp = step;
            var marginStep = 0;
            if (this.sizeNeed[key].width != this.ss10[key].width) {
                checked = false;
                stepTmp = Math.abs((this.ss10[key].width - this.sizeNeed[key].width) % step);
                if (stepTmp == 0) {
                    stepTmp = step
                }
                stepTmp = ((this.sizeNeed[key].width > this.ss10[key].width) ? (stepTmp) : (0 - stepTmp));
                this.ss10[key].width = this.ss10[key].width + stepTmp
            }
            if (this.sizeNeed[key].height != this.ss10[key].height) {
                checked = false;
                stepTmp = Math.abs((this.ss10[key].height - this.sizeNeed[key].height) % step);
                if (stepTmp == 0) {
                    stepTmp = step
                }
                stepTmp = ((this.sizeNeed[key].height > this.ss10[key].height) ? (stepTmp) : (0 - stepTmp));
                this.ss10[key].height = this.ss10[key].height + stepTmp
            }
            if (this.sizeNeed[key].height <= this.ss9[key].height && (key == "img" && this.ss1.preserve || key == "thumb" && this.ss1.containerDirection == "horizontally")) {
                var marginTop = parseInt(this.ss3[key].j19("margin-top"));
                var needMarginTopTmp = (this.ss9[key].height - this.ss10[key].height) / 2 + this.ss9[key].marginTop;
                var needMarginTop = Math.round(needMarginTopTmp);
                if (needMarginTop != needMarginTopTmp) {
                    needMarginTop -= 1
                }
                if (marginTop != needMarginTop) {
                    checked = false;
                    stepTmp = Math.abs((needMarginTop - marginTop) % step);
                    if (stepTmp == 0) {
                        stepTmp = step
                    }
                    stepTmp = ((needMarginTop > marginTop) ? (stepTmp) : (0 - stepTmp));
                    this.ss10[key].marginTop = dre ? needMarginTop : (marginTop + stepTmp)
                }
                var marginBottom = parseInt(this.ss3[key].j19("margin-bottom"));
                var needMarginBottom = Math.round((this.ss9[key].height - this.ss10[key].height) / 2 + this.ss9[key].marginBottom);
                if (marginBottom != needMarginBottom) {
                    checked = false;
                    stepTmp = Math.abs((needMarginBottom - marginBottom) % step);
                    if (stepTmp == 0) {
                        stepTmp = step
                    }
                    stepTmp = ((needMarginBottom > marginBottom) ? (stepTmp) : (0 - stepTmp));
                    this.ss10[key].marginBottom = dre ? needMarginBottom : (marginBottom + stepTmp)
                }
            }
            if (this.sizeNeed[key].width <= this.ss9[key].width && (key == "img" && this.ss1.preserve || key == "thumb" && this.ss1.containerDirection == "vertically")) {
                var marginLeft = parseInt(this.ss3[key].j19("margin-left"));
                var needMarginLeftTmp = (this.ss9[key].width - this.ss10[key].width) / 2 + this.ss9[key].marginLeft;
                var needMarginLeft = Math.round(needMarginLeftTmp);
                if (needMarginLeftTmp != needMarginLeft) {
                    needMarginLeft -= 1
                }
                if (marginLeft != needMarginLeft) {
                    checked = false;
                    stepTmp = Math.abs((needMarginLeft - marginLeft) % step);
                    if (stepTmp == 0) {
                        stepTmp = step
                    }
                    stepTmp = ((needMarginLeft > marginLeft) ? (stepTmp) : (0 - stepTmp));
                    this.ss10[key].marginLeft = dre ? needMarginLeft : (marginLeft + stepTmp)
                }
                var marginRight = parseInt(this.ss3[key].j19("margin-right"));
                var needMarginRight = Math.round((this.ss9[key].width - this.ss10[key].width) / 2 + this.ss9[key].marginRight);
                if (marginRight != needMarginRight) {
                    checked = false;
                    stepTmp = Math.abs((needMarginRight - marginRight) % step);
                    if (stepTmp == 0) {
                        stepTmp = step
                    }
                    stepTmp = ((needMarginRight > marginRight) ? (stepTmp) : (0 - stepTmp));
                    this.ss10[key].marginRight = dre ? needMarginRight : (marginRight + stepTmp)
                }
            }
            if (checked === false) {
                this.ss3[key].j20(this.ss10[key]);
                this.checkImageSize.j24(this, key).j27(duration)
            }
        },
        highlight: function (mode) {
            if ($J.defined(mode) && mode || !$J.defined(mode)) {
                this.ss3.thumb.j2("highlight");
                this.ss9.thumb = {
                    width: this.ss1.containerSize - parseInt(MagicSlideshow.ss12("img", "MagicSlideshowThumbnail highlight", "border-left-width", this.parent.id)) - parseInt(MagicSlideshow.ss12("img", "MagicSlideshowThumbnail highlight", "border-right-width", this.parent.id)) - parseInt(MagicSlideshow.ss12("img", "MagicSlideshowThumbnail highlight", "margin-left", this.parent.id)) - parseInt(MagicSlideshow.ss12("img", "MagicSlideshowThumbnail highlight", "margin-right", this.parent.id)),
                    height: this.ss1.containerSize - parseInt(MagicSlideshow.ss12("img", "MagicSlideshowThumbnail highlight", "border-top-width", this.parent.id)) - parseInt(MagicSlideshow.ss12("img", "MagicSlideshowThumbnail highlight", "border-bottom-width", this.parent.id)) - parseInt(MagicSlideshow.ss12("img", "MagicSlideshowThumbnail highlight", "margin-top", this.parent.id)) - parseInt(MagicSlideshow.ss12("img", "MagicSlideshowThumbnail highlight", "margin-bottom", this.parent.id)),
                    marginTop: this.ss1.thumbnailMarginTop,
                    marginBottom: this.ss1.thumbnailMarginBottom,
                    marginLeft: this.ss1.thumbnailMarginLeft,
                    marginRight: this.ss1.thumbnailMarginRight
                }
            } else {
                this.ss3.thumb.j3("highlight");
                this.ss9.thumb = {
                    width: this.ss1.containerSize - parseInt(MagicSlideshow.ss12("img", "MagicSlideshowThumbnail", "border-left-width", this.parent.id)) - parseInt(MagicSlideshow.ss12("img", "MagicSlideshowThumbnail", "border-right-width", this.parent.id)) - parseInt(MagicSlideshow.ss12("img", "MagicSlideshowThumbnail", "margin-left", this.parent.id)) - parseInt(MagicSlideshow.ss12("img", "MagicSlideshowThumbnail", "margin-right", this.parent.id)),
                    height: this.ss1.containerSize - parseInt(MagicSlideshow.ss12("img", "MagicSlideshowThumbnail", "border-top-width", this.parent.id)) - parseInt(MagicSlideshow.ss12("img", "MagicSlideshowThumbnail", "border-bottom-width", this.parent.id)) - parseInt(MagicSlideshow.ss12("img", "MagicSlideshowThumbnail", "margin-top", this.parent.id)) - parseInt(MagicSlideshow.ss12("img", "MagicSlideshowThumbnail", "margin-bottom", this.parent.id)),
                    marginTop: this.ss1.thumbnailMarginTop,
                    marginBottom: this.ss1.thumbnailMarginBottom,
                    marginLeft: this.ss1.thumbnailMarginLeft,
                    marginRight: this.ss1.thumbnailMarginRight
                }
            }
            this.calculateNeedSize("thumb");
            this.checkImageSize("thumb", true)
        }
    });
    MagicSlideshow.Effect = $J.Class({
        init: function (parent) {
            this.parent = parent;
            this.ss1 = this.parent.ss1;
            this.prevEffect = "none";
            this.prepared = false;
            this.ss7 = {
                start: 0,
                ss25: 0
            };
            this.prepareTypes = {
                def: "horizontally",
                fade: "absolute",
                scroll: "horizontally",
                scrollHorizontally: "horizontally",
                scrollVertically: "vertically"
            };
            this.allowDirections = {
                def: ["horizontally"],
                fade: [""],
                scroll: ["horizontally", "vertically"]
            };
            this.callback = $J.$F;
            this.coreCallback = $J.$F;
            this.fx = []
        },
        stopEffects: function (fullStop) {
            fullStop = fullStop || false;
            if (this.timeout || $J.defined(this.timeout)) {
                clearTimeout(this.timeout)
            }
            if (this.interval || $J.defined(this.interval)) {
                clearInterval(this.interval)
            }
            if (this.fx || $J.defined(this.fx)) {
                for (var i = 0, l = this.fx.length; i < l; i++) {
                    this.fx[i].stop()
                }
                this.fx = []
            }
            if (fullStop) {
                this.coreCallback();
                this.callback()
            }
            this.callback = $J.$F;
            this.coreCallback = $J.$F
        },
        goTo: function (cur, num, type, callback, origss1) {
            this.type = type = type || "next";
            this.origss1 = origss1 || {
                num: 1,
                type: "next"
            };
            this.stopEffects();
            this.callback = callback || $J.$F;
            this.cur = cur;
            this.num = num;
            this.effect = this.ss1[("effect-" + type).j22()];
            if (!this[("effect-" + this.effect).j22()]) {
                this.effect = "def"
            }
            this.direction = this.ss1[("effect-" + this.effect + "-direction").j22()];
            if (!$J.defined(this.direction) || !this.direction || !$mjs(this.allowDirections[this.effect]).contains(this.direction)) {
                this.direction = this.allowDirections[this.effect][0]
            }
            this.duration = this.ss1[("effect-" + this.effect + "-duration").j22()];
            if (this.duration < 0.01) {
                this.duration = 0.01
            }
            var curPrepared = this.prepareTypes[(this.effect + (this.direction != "" ? ("-" + this.direction) : "")).j22()];
            if (curPrepared != this.prepared) {
                var i,
				l = this.parent.els.length;
                switch (curPrepared) {
                    case "absolute":
                        this.removeOffsets("all");
                        for (i = 0; i < l; i++) {
                            this.parent.els[i].ss3.img.j23(0);
                            this.parent.els[i].ss4.img.j20({
                                zIndex: this.ss1.zIndex + 5,
                                position: "absolute",
                                top: 0,
                                left: 0
                            });
                            if (this.parent.els[i].ss18) {
                                this.parent.els[i].ss18.j23(0);
                                if (this.ss1.textPosition == "bottom") {
                                    this.parent.els[i].ss18.j6({
                                        bottom: 0
                                    },
								this.ss1)
                                } else {
                                    this.parent.els[i].ss18.j6({
                                        top: 0
                                    })
                                }
                            }
                        }
                        this.parent.els[this.cur].ss3.img.j23(1);
                        if (this.parent.els[this.cur].ss18) {
                            this.parent.els[this.cur].ss18.j23(this.ss1.textOpacity)
                        }
                        this.parent.els[this.cur].ss4.img.j20({
                            zIndex: this.ss1.zIndex + 6
                        });
                        this.parent.ss2.scrollLeft = 0;
                        this.parent.ss2.scrollTop = 0;
                        this.prepared = "absolute";
                        break;
                    case "horizontally":
                    case "vertically":
                    default:
                        var display = this.direction == "vertically" ? "block" : "inline";
                        for (i = 0; i < l; i++) {
                            this.parent.els[i].ss3.img.j23(1);
                            this.parent.els[i].ss4.img.j20({
                                zIndex: this.ss1.zIndex + 5,
                                position: "static",
                                display: "block",
                                "float": display == "block" ? "none" : "left"
                            });
                            if ($J.j21.engine == "trident" && $J.j21.version == 4) {
                                this.parent.els[i].ss4.img.j20({
                                    "float": "left"
                                })
                            }
                            this.ss2WrapperUpdate = $mjs(function (x) {
                                this.parent.ss2Wrapper.j20({
                                    width: display == "block" ? "auto" : ((((x || this.parent.els.length) + 3) * (this.ss1.width + 100)))
                                })
                            }).j24(this);
                            this.ss2WrapperUpdate();
                            if (this.parent.els[i].ss18) {
                                this.parent.els[i].ss18.j23(this.ss1.textOpacity);
                                if (this.ss1.textPosition == "bottom") {
                                    this.parent.els[i].ss18.j6({
                                        display: "none",
                                        bottom: 0
                                    },
								this.ss1)
                                } else {
                                    this.parent.els[i].ss18.j6({
                                        display: "none",
                                        top: 0
                                    })
                                }
                            }
                        }
                        if (this.direction == "vertically") {
                            this.parent.ss2.scrollLeft = 0;
                            this.parent.ss2.scrollTop = this.ss1.height * this.cur
                        } else {
                            this.parent.ss2.scrollLeft = this.ss1.width * this.cur;
                            this.parent.ss2.scrollTop = 0
                        }
                        this.prepared = this.direction == "vertically" ? this.direction : "horizontally";
                        $J.$A(this.parent.els).j14(function (el) {
                            $mjs(el).ss4.img.j20({
                                position: "relative"
                            });
                            if ($mjs(el).ss18) {
                                $mjs(el).ss18.j20({
                                    display: "block"
                                })
                            }
                        });
                        break
                }
            }
            this[("effect-" + this.effect).j22()]()
        },
        effectDef: function () {
            this.parent.ss2.scrollLeft = this.ss1.width * this.num;
            this.stopEffects(true)
        },
        removeOffsets: function (key) {
            key = key || "extra";
            if (key == "all") {
                if (this.ss1.effectScrollDirection == "horizontally") {
                    this.parent.ss2.scrollLeft = this.parent.ss2.scrollLeft % (this.parent.els.length * this.ss1.width)
                } else {
                    this.parent.ss2.scrollTop = this.parent.ss2.scrollTop % (this.parent.els.length * this.ss1.height)
                }
                var i,
				l = this.ss7.start;
                for (i = 0; i < l; i++) {
                    if (this.ss1.effectScrollDirection == "horizontally") {
                        this.parent.ss2.scrollLeft += this.ss1.width
                    } else {
                        this.parent.ss2.scrollTop += this.ss1.width
                    }
                    $mjs(this.parent.ss2Wrapper.firstChild).j33();
                    this.ss7.start--
                }
                l = this.ss7.ss25;
                for (i = 0; i < l; i++) {
                    $mjs(this.parent.ss2Wrapper.lastChild).j33();
                    this.ss7.ss25--
                }
            } else {
                if (key == "extra") {
                    var count = this.parent.els.length;
                    var newScroll,
					atStart = false,
					atFinish = false;
                    if (this.direction == "horizontally") {
                        newScroll = this.parent.ss2.scrollLeft - this.ss1.width * this.ss7.start;
                        newScroll = newScroll % (this.ss1.width * count);
                        if (newScroll <= 0 - this.ss1.width) {
                            newScroll += this.ss1.width * count
                        } else {
                            if (newScroll <= 0) {
                                atStart = true
                            } else {
                                if (newScroll > this.ss1.width * (count - 1)) {
                                    atFinish = true
                                }
                            }
                        }
                        this.parent.ss2.scrollLeft = this.ss1.width * this.ss7.start + newScroll
                    } else {
                        newScroll = this.parent.ss2.scrollTop - this.ss1.height * this.ss7.start;
                        newScroll = newScroll % (this.ss1.height * count);
                        if (newScroll <= 0 - this.ss1.height) {
                            newScroll += this.ss1.height * count
                        } else {
                            if (newScroll <= 0) {
                                atStart = true
                            } else {
                                if (newScroll > this.ss1.height * (count - 1)) {
                                    atFinish = true
                                }
                            }
                        }
                        this.parent.ss2.scrollTop = this.ss1.height * this.ss7.start + newScroll
                    }
                    if (this.ss7.start > 0) {
                        while ((this.ss7.start > 0 && !atStart) || (this.ss7.start > 1 && atStart)) {
                            $mjs(this.parent.ss2Wrapper.firstChild).j33();
                            if (this.direction == "horizontally") {
                                this.parent.ss2.scrollLeft -= this.ss1.width
                            } else {
                                this.parent.ss2.scrollTop -= this.ss1.height
                            }
                            this.ss7.start--
                        }
                    }
                    if (this.ss7.ss25 > 0) {
                        while ((this.ss7.ss25 > 0 && !atFinish) || (this.ss7.ss25 > 1 && atFinish)) {
                            $mjs(this.parent.ss2Wrapper.lastChild).j33();
                            this.ss7.ss25--
                        }
                    }
                }
            }
        },
        effectScroll: function () {
            var count = this.parent.els.length;
            this.removeOffsets();
            var x;
            if (this.direction == "horizontally") {
                x = (this.parent.ss2.scrollLeft - (this.parent.ss2.scrollLeft % this.ss1.width)) / this.ss1.width
            } else {
                x = (this.parent.ss2.scrollTop - (this.parent.ss2.scrollTop % this.ss1.height)) / this.ss1.height
            }
            if (this.ss1.loop && this.ss1.loopType == "next") {
                cur = x - this.ss7.start;
                if (this.origss1.num < 0 && this.num > cur) {
                    this.num = 0 - (count - this.num)
                } else {
                    if (this.origss1.num > 0 && this.num <= cur) {
                        this.num = count + this.num
                    } else {
                        if (cur && cur < 0) {
                            this.num = this.num - cur
                        }
                    }
                }
            }
            if (this.ss1.loop && this.ss1.loopType == "next") {
                if (this.num < 0) {
                    var num = 0 - this.num - this.ss7.start;
                    this.ss2WrapperUpdate(this.parent.els.length - num);
                    var i = count - this.ss7.start - 1;
                    while (num > 0) {
                        if (i < 0) {
                            i = count - 1
                        }
                        this.parent.ss2Wrapper.insertBefore(this.parent.els[i].ss43Copy(), this.parent.ss2Wrapper.firstChild);
                        if (this.direction == "horizontally") {
                            this.parent.ss2.scrollLeft += this.ss1.width
                        } else {
                            this.parent.ss2.scrollTop += this.ss1.height
                        }
                        this.ss7.start++;
                        i--;
                        num--
                    }
                    this.cur = 0 - this.num;
                    this.num = 0
                } else {
                    if (this.num > (count - 1)) {
                        var num = this.num + 1 - count - this.ss7.ss25;
                        this.ss2WrapperUpdate(this.parent.els.length + num);
                        var i = this.ss7.ss25;
                        while (num > 0) {
                            if (i == count) {
                                i = 0
                            }
                            this.parent.ss2Wrapper.appendChild(this.parent.els[i].ss43Copy());
                            this.ss7.ss25++;
                            i++;
                            num--
                        }
                    }
                }
            }
            if (this.direction == "vertically") {
                var ss32 = this.parent.ss2.scrollTop;
                var ss27 = this.ss1.height * this.num
            } else {
                var ss32 = this.parent.ss2.scrollLeft;
                var ss27 = this.ss1.width * this.num
            }
            if (ss27 == ss32) {
                return
            }
            var offset = Math.abs(ss27 - ss32);
            var D = this.duration * 1000;
            var mD = 100;
            var C = D / mD + 1;
            var W = offset;
            var fS = W / C / 4;
            var K = 2 * ((W - C * fS) / ((C / 2 - 1) * C));
            var N = 1;
            this.ScrollCurrentStep = N;
            this.interval = (function (C, K, fS, ss27) {
                var N = this.ScrollCurrentStep;
                if (this.direction == "vertically") {
                    var ss32 = this.parent.ss2.scrollTop
                } else {
                    var ss32 = this.parent.ss2.scrollLeft
                }
                if (ss27 == ss32 || N > C) {
                    if (ss27 != ss32) {
                        if (this.direction == "vertically") {
                            this.parent.ss2.scrollTop = ss27
                        } else {
                            this.parent.ss2.scrollLeft = ss27
                        }
                    }
                    this.coreCallback = (function () {
                        this.removeOffsets()
                    }).j24(this);
                    this.stopEffects(true);
                    return
                }
                var sW = 0;
                if (N > C / 2) {
                    sW = fS + K * (C - N)
                } else {
                    sW = fS + K * (N - 1)
                }
                var offset = Math.abs(ss27 - ss32);
                sW = offset > sW ? sW : offset;
                if (ss27 < ss32) {
                    sW = 0 - sW
                }
                if (this.direction == "vertically") {
                    this.parent.ss2.scrollTop += Math.round(sW)
                } else {
                    this.parent.ss2.scrollLeft += Math.round(sW)
                }
                this.ScrollCurrentStep++
            }).j24(this, C, K, fS, ss27).interval(mD)
        },
        effectFade: function () {
            var mD = 50;
            var C = Math.round(this.duration * 1000 / mD);
            var K = Math.round(100 / C);
            this.parent.els[this.num].ss4.img.j20({
                zIndex: this.ss1.zIndex + 6
            });
            this.parent.els[this.cur].ss4.img.j20({
                zIndex: this.ss1.zIndex + 5
            });
            var i,
			l = this.parent.els.length,
			el,
			op;
            for (i = 0; i < l; i++) {
                el = this.parent.els[i].ss3.img;
                op = el.j5("opacity");
                if (op > 0 && i != this.num) {
                    this.fx.push(new $J.FX(el, {
                        duration: this.duration * 1000,
                        onBeforeRender: (function (i, v) {
                            if (this.parent.els[i].ss18) {
                                this.parent.els[i].ss18.j23(v.opacity * this.ss1.textOpacity)
                            }
                        }).j24(this, i)
                    }).start({
                        opacity: [op, 0]
                    }))
                }
                if (op < 1 && i == this.num) {
                    this.fx.push(new $J.FX(el, {
                        duration: this.duration * 1000,
                        onBeforeRender: (function (i, v) {
                            if (this.parent.els[i].ss18) {
                                this.parent.els[i].ss18.j23(v.opacity * this.ss1.textOpacity)
                            }
                        }).j24(this, i),
                        onComplete: (function () {
                            this.stopEffects(true)
                        }).j24(this)
                    }).start({
                        opacity: [op, 1]
                    }))
                }
            }
        }
    });
    MagicSlideshow.Loading = $J.Class({
        init: function (el, ss1) {
            this.el = el;
            this.ss1 = ss1;
            this.appendDone = false;
            this.create();
            this.append()
        },
        create: function () {
            this.ss11 = $mjs(document.createElement("div"));
            this.ss11.changeContent("&nbsp;&nbsp;<br />" + this.ss1.loadingText);
            this.ss11.j2("MagicSlideshowLoadingBox");
            this.ss11.j20({
                position: "absolute",
                top: 0,
                left: 0,
                zIndex: this.ss1.zIndex + 100,
                display: "none"
            });
            this.ss11.j23(50);
            this.setPos.j24(this).j27(100)
        },
        append: function () {
            if (!this.el.parentNode && !$J.defined(this.el.parentNode)) {
                this.append.j24(this).j27(100);
                return
            }
            this.appendDone = true;
            this.el.parentNode.appendChild(this.ss11)
        },
        setPos: function () {
            var size = this.el.j7();
            if (size.width == 0 || size.left == 0) {
                this.setPos.j24(this).j27(100)
            }
            var pos = this.el.ss37();
            var lsize = this.ss11.j7();
            this.ss11.j20({
                top: Math.round(parseInt(pos.top) + parseInt(size.height) / 2 - parseInt(lsize.height) / 2) + "px",
                left: Math.round(parseInt(pos.left) + parseInt(size.width) / 2 - parseInt(lsize.width) / 2) + "px"
            })
        },
        show: function () {
            if (this.appendDone == false) {
                this.show.j24(this).j27(100);
                return
            }
            this.setPos();
            this.ss11.j20({
                display: ""
            })
        },
        hide: function () {
            this.ss11.j20({
                display: "none"
            })
        }
    });
    return MagicSlideshow
})(magicJS);