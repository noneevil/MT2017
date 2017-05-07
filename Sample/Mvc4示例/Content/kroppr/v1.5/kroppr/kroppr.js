/// <reference path="../../Scripts/mootools-core-1.5.1.js" />
var Kropr2 = new Class({
    Implements: [Chain, Events, Options],
    /*版本号*/
    version: 1.5,
    /*设置参数*/
    options: {
        /*裁剪前的事件*/
        onCrop: function () { },
        /*取消事件*/
        onCancel: function () { },
        /*完成事件*/
        onSuccess: function (result) { },
        /*图片地址*/
        url: null,
        /*图片的width, height, left, right, top, bottom值*/
        coords: {},
        /*缩略图背景色*/
        background: '#000',
        /*处理地址*/
        handler: 'Kroppr.ashx?k=c'
    },
    initialize: function (img, options) {
        img = img || '.kroppr';
        if (typeOf(img) == 'string') img = $$(img);
        if (typeOf(img) == 'elements') {
            img.each(function (o) {
                return new Kropr2(o, options);
            });
        }
        if (typeOf(img) != 'element') return this;

        this.setOptions(options);
        this.json = {};
        this.ie = Browser.name == 'ie' && Browser.version < 9;
        this.id = String.uniqueID();

        this.Create(img);
    },
    /*创建显示界面*/
    Create: function (img) {
        var o = this.options;
        o.url = img.get('src');
        o.coords = img.getCoordinates();
        var w = o.coords.width, h = o.coords.height;
        /*包装容器*/
        this.wrapper = new Element('div', { 'class': 'kroppr', styles: { width: w, height: h } }).inject(img, 'after');
        /*旋转*/
        var rotate = new Element('div', { title: '旋转', 'class': 'k_tips k_rotate' }).inject(this.wrapper);
        new Element('span', { html: '&nbsp;0&deg;' }).inject(rotate);
        new Element('div', { html: '<i />' }).inject(rotate);
        new Element('span', { html: '360&deg;' }).inject(rotate);
        /*放大*/
        var zoom = new Element('div', { title: '放大', 'class': 'k_tips k_zoom' }).inject(this.wrapper);
        new Element('span', { html: '+' }).inject(zoom);
        new Element('div', { html: '<i />' }).inject(zoom);
        new Element('span', { html: '-' }).inject(zoom);
        /*按钮*/
        var buttons = new Element('div', { 'class': 'k_buttons' }).inject(this.wrapper);
        new Element('span', { 'class': 'k_button k_accept', text: '裁剪' }).inject(buttons);
        new Element('span', { 'class': 'k_button k_cancel', text: '重置' }).inject(buttons);
        /*动画*/
        var loading = new Element('div', { 'class': 'k_loading' }).inject(this.wrapper);
        new Element('span', { 'class': 'k_message', text: 'Kropping...' }).inject(loading);
        //new Element('div', { 'class': 'k_download' }).inject(loading);
        new Element('span', { 'class': 'k_button k_cancel', text: 'Crop again' }).inject(loading);
        /*画布容器*/
        var div_canvas = new Element('div', { 'class': 'k_canvas' }).inject(this.wrapper);
        img.destroy();

        /*创建画布*/
        if (this.ie) {
            this.canvas = new Vml_render({ id: this.id, width: w, height: h, container: div_canvas })
        }
        else {
            this.canvas = new Svg_render({ id: this.id, width: w, height: h, container: div_canvas })
        }
        this.canvas.create_image(o.url, 0, 0, w, h, this.id);
        this.cropper = new Cropper({ render: this.canvas, kropr: this });
        this.bindEvents(o);
    },
    /*发送裁剪请求*/
    Request: function () {
        var self = this;
        var data = this.json;

        data.quality = 70;
        data.image = self.options.url;
        data.background = self.options.background,
        data.rotation = self.canvas.rotation;
        data.original = self.canvas.original;
        data.offset = { x: self.canvas.posx, y: self.canvas.posy };
        data.xfact = self.canvas.xfact;
        data.cropper = self.cropper.cropper.getCoordinates();
        data.cropper.height = data.cropper.height - 2;
        data.cropper.width = data.cropper.width - 2;
        //alert(JSON.stringify(data));
        new Request.JSON({
            url: self.options.handler,// "/Content/kroppr/Kroppr.ashx?k=c",
            data: JSON.encode(data),
            onRequest: function () {
                //                    $('k_load').setStyles({
                //                        'display': 'block',
                //                        'background': 'url("/kroppr/images/load.gif") no-repeat center center #000000'
                //                    });
                //                    $('k_download').empty();
                //                    $('k_message').set('text', "Kropping...")
            },
            onComplete: function () {
                self.fireEvent('success', arguments)
                //$('result').empty();
                //new Element('img', { src: response.image }).inject($('result'));
                //                    $('k_load').setStyle('background', '#000000');
                //                    $('k_message').set('text', "Click on the image to download!");
                //                    $('k_download').set('html', '<br /><a href="kroppr_s.php?k=d&img=' + response.real_image + '&image_type=' + response.image_type + '" ><img src="' + response.image + '?' + Math.random() + '" style="border:1px solid #ffffff;margin:0 auto 0 auto;" /></a>')
            }
        }).send();
    },
    /*绑定事件*/
    bindEvents: function (o) {
        var self = this;
        /*滑杆提示信息*/
        new Tips(this.wrapper.getElements('.k_tips'), { className: 'k_tips' })
        /*旋转滑杆*/
        var rotate = this.wrapper.getElement('div.k_rotate>div');
        var rotate_nob = rotate.getElement('i');
        this.rotation_slide = new Slider(rotate, rotate_nob, {
            steps: 360,
            initialStep: 0,
            mode: 'vertical',
            onChange: function (deg) {
                self.canvas.set_rotation(deg)
            }
        });
        /*放大滑杆*/
        var zoom = this.wrapper.getElement('div.k_zoom>div');
        var zoom_nob = zoom.getElement('i');
        this.zoom_slide = new Slider(zoom, zoom_nob, {
            steps: 50,
            initialStep: 40,
            mode: 'vertical',
            onChange: function (step) {
                if (step == 50) step = 49;
                if (step >= 40) {
                    step = 50 - step;
                    xfact = (step / 10)
                } else {
                    xfact = (50 - step) / 10
                }
                self.canvas.zoom(xfact)
            }
        });
        /*裁剪按钮事件*/
        var btn_accept = this.wrapper.getElement('div.k_buttons>span.k_accept');
        btn_accept.addEvent('click', this.Request.bind(this));
        /*重置按钮事件*/
        var btn_reset = this.wrapper.getElement('div.k_buttons>span.k_cancel');
        btn_reset.addEvent('click', function () {
            self.cropper.set_reset();
        });

        /*包装容器鼠标事件*/
        this.wrapper.addEvents({
            /*滚轮事件*/
            mousewheel: function (e) {
                e = new DOMEvent(e);
                e.stop();
                if (!e.alt && !e.control && !e.shift) {
                    /*旋转*/
                    if (e.wheel < 0)
                        self.zoom_slide.set(self.zoom_slide.step + 1);
                    else
                        self.zoom_slide.set(self.zoom_slide.step - 1);
                }
                else {
                    /*放大*/
                    if (e.wheel < 0)
                        self.rotation_slide.set(self.rotation_slide.step + 1);
                    else
                        self.rotation_slide.set(self.rotation_slide.step - 1);
                }
            },
            mouseleave: function () {
                self.down = false;
                self.canvas.update();
                self.cropper.drdr.stop();
                self.cropper.res.stop();
                self.cropper.set_mode('move')
            }
        });
        /*画布鼠标事件*/
        this.canvas.container.addEvents({
            mousemove: function (e) {
                e = new DOMEvent(e);
                if (self.down && self.target) {
                    self.trans_x = parseFloat(e.client.x - self.orig_x);
                    self.trans_y = parseFloat(e.client.y - self.orig_y);
                    if (self.ie) {
                        self.target.style.left = self.img_x + self.trans_x + 'px';
                        self.target.style.top = self.img_y + self.trans_y + 'px'
                    }
                    else {
                        self.target.setAttributeNS(null, 'transform', 'translate(' + self.trans_x + ',' + self.trans_y + ') rotate(' + self.curAngle + ',' + self.cx + ',' + self.cy + ')')
                    }
                    e.stop();
                }
            },
            mousedown: function (e) {
                e = new DOMEvent(e);
                if (e.target.id == 'kroprimg_' + self.id) {
                    var image = self.canvas.image;
                    self.down = true;
                    self.target = e.target;
                    self.orig_x = e.client.x;
                    self.orig_y = e.client.y;
                    self.curAngle = self.canvas.rotation;
                    if (self.ie) {
                        self.img_x = image.style.left.toInt();
                        self.img_y = image.style.top.toInt();
                        self.cx = image.style.top.toInt() + image.style.width.toInt() / 2;
                        self.cy = image.style.left.toInt() + image.style.height.toInt() / 2
                    }
                    else {
                        self.cx = image.getAttributeNS(null, 'x').toInt() + image.getAttributeNS(null, 'width').toInt() / 2;
                        self.cy = image.getAttributeNS(null, 'y').toInt() + image.getAttributeNS(null, 'height').toInt() / 2
                    }
                    e.stop();
                }
            },
            mouseup: function (e) {
                e = new DOMEvent(e);
                self.cropper.set_mode('move');
                if (self.down) {
                    self.down = false;
                    self.canvas.update()
                    e.stop();
                }
            }
        });
    }
});

var Kropr = new Class({
    options: {},
    initialize: function (options) {
        this.coords = $(options.el).getCoordinates();
        this.json = {};
        this.ie = Browser.ie && Browser.version < 9;
        if (this.ie) {
            this.canvas = new Vml_render({ 'id': 'k', width: this.coords.width, height: this.coords.height, 'container': options.el })
        }
        else {
            this.canvas = new Svg_render({ 'id': 'k', width: this.coords.width, height: this.coords.height, 'container': options.el })
        }
        this.canvas.create_image(options.img, 0, 0, this.coords.width, this.coords.height, '');
        this.cropper = new Cropper({ 'render': this.canvas, 'kropr': this });

        this.rotation_slide = new Slider($('slider_rotate'), $('knob_rotate'), {
            steps: 360,
            mode: 'vertical',
            onChange: function (deg) {
                this.canvas.set_rotation(deg)
            }.bind(this)
        }).set(0);
        this.zoom_slide = new Slider($('slider_zoom'), $('knob_zoom'), {
            steps: 50,
            mode: 'vertical',
            onChange: function (step) {
                if (step == 50) step = 49;
                if (step >= 40) {
                    step = 50 - step;
                    xfact = (step / 10)
                } else {
                    xfact = (50 - step) / 10
                }
                this.canvas.zoom(xfact)
            }.bind(this)
        }).set(40);
        $('kdiv_rotate').setStyles({ top: 5, left: 5, opacity: .6 });
        $('kdiv_zoom').setStyles({ top: 5, left: this.coords.width - 23, opacity: .6 });
        $('kdiv_zoom').addEvent('mouseenter', function () { this.setStyle('opacity', 1); });
        $('kdiv_rotate').addEvent('mouseenter', function () { this.setStyle('opacity', 1); });
        $('kdiv_zoom').addEvent('mouseleave', function () { this.setStyle('opacity', .6); });
        $('kdiv_rotate').addEvent('mouseleave', function () { this.setStyle('opacity', .6); });
        $('k_load').setStyles({ opacity: .95, height: this.coords.height });

        $('k_accept').setStyles({
            top: this.coords.height - 30,
            left: this.coords.width / 2 - 80,
            opacity: .6
        }).addEvent('click', function () {
            this.json.quality = 70;
            this.json.image = options.img;
            this.json.background = $(options.el).getStyle('background-color'),
            this.json.rotation = this.canvas.rotation;
            this.json.original = this.canvas.original;
            this.json.offset = { x: this.canvas.posx, y: this.canvas.posy };
            this.json.xfact = this.canvas.xfact;
            this.json.cropper = this.cropper.cropper.getCoordinates();
            this.json.cropper.height = this.json.cropper.height - 2;
            this.json.cropper.width = this.json.cropper.width - 2;
            new Request.JSON({
                url: "Kroppr.axd?k=c",
                data: JSON.encode(this.json),
                onRequest: function () {
                    //                    $('k_load').setStyles({
                    //                        'display': 'block',
                    //                        'background': 'url("/kroppr/images/load.gif") no-repeat center center #000000'
                    //                    });
                    //                    $('k_download').empty();
                    //                    $('k_message').set('text', "Kropping...")
                },
                onComplete: function (response) {
                    $('result').empty();
                    new Element('img', { src: response.image }).inject($('result'));
                    //                    $('k_load').setStyle('background', '#000000');
                    //                    $('k_message').set('text', "Click on the image to download!");
                    //                    $('k_download').set('html', '<br /><a href="kroppr_s.php?k=d&img=' + response.real_image + '&image_type=' + response.image_type + '" ><img src="' + response.image + '?' + Math.random() + '" style="border:1px solid #ffffff;margin:0 auto 0 auto;" /></a>')
                }
            }).send();
        }.bind(this));
        $('k_reset').setStyles({
            top: this.coords.height - 30,
            left: this.coords.width / 2 + 10,
            opacity: .6
        }).addEvent('click', function () {
            this.cropper.set_reset()
        }.bind(this));
        $('k_accept').addEvent('mouseenter', function () { this.setStyle('opacity', 1); });
        $('k_reset').addEvent('mouseenter', function () { this.setStyle('opacity', 1); });
        $('k_accept').addEvent('mouseleave', function () { this.setStyle('opacity', .6); });
        $('k_reset').addEvent('mouseleave', function () { this.setStyle('opacity', .6); });
        $(document.body).addEvent('mouseleave', function () {
            this.cropper.drdr.stop();
            this.cropper.res.stop();
            this.cropper.set_mode('move')
        }.bind(this));

        document.addEvents({
            'mousewheel': function (e) {/*this.canvas.container.addEvent*/
                e = new DOMEvent(e);
                if (!e.alt && !e.control && !e.shift) {
                    if (e.wheel < 0)
                        this.zoom_slide.set(this.zoom_slide.step + 1);
                    else
                        this.zoom_slide.set(this.zoom_slide.step - 1);
                }
                else {
                    if (e.wheel < 0)
                        this.rotation_slide.set(this.rotation_slide.step + 1);
                    else
                        this.rotation_slide.set(this.rotation_slide.step - 1);
                }
                e.stop();
            }.bind(this),
            'mousemove': function (e) {
                e = new DOMEvent(e);
                e.stop();
                if (this.down && this.target) {
                    this.trans_x = parseFloat(e.client.x - this.orig_x);
                    this.trans_y = parseFloat(e.client.y - this.orig_y);
                    if (this.ie) {
                        this.target.style.left = this.img_x + this.trans_x + 'px';
                        this.target.style.top = this.img_y + this.trans_y + 'px'
                    }
                    else {
                        this.target.setAttributeNS(null, 'transform', 'translate(' + this.trans_x + ',' + this.trans_y + ') rotate(' + this.curAngle + ',' + this.cx + ',' + this.cy + ')')
                    }
                }
            }.bind(this),
            'mousedown': function (e) {
                e = new DOMEvent(e);
                e.stop();
                if (e.target.id == 'kroprimg') {
                    this.down = true;
                    this.target = e.target;
                    this.orig_x = e.client.x;
                    this.orig_y = e.client.y;
                    this.curAngle = this.canvas.rotation;
                    if (this.ie) {
                        this.img_x = this.canvas.image.style.left.toInt();
                        this.img_y = this.canvas.image.style.top.toInt();
                        this.cx = this.canvas.image.style.top.toInt() + this.canvas.image.style.width.toInt() / 2;
                        this.cy = this.canvas.image.style.left.toInt() + this.canvas.image.style.height.toInt() / 2
                    }
                    else {
                        this.cx = this.canvas.image.getAttributeNS(null, 'x').toInt() + this.canvas.image.getAttributeNS(null, 'width').toInt() / 2;
                        this.cy = this.canvas.image.getAttributeNS(null, 'y').toInt() + this.canvas.image.getAttributeNS(null, 'height').toInt() / 2
                    }
                    $(document.body).addEvent('mouseleave', function () {
                        this.down = false;
                        this.canvas.update()
                    }.bind(this))
                }
            }.bind(this),
            'mouseup': function (e) {
                e = new DOMEvent(e);
                e.stop();
                this.cropper.set_mode('move');
                if (this.down) {
                    this.down = false;
                    this.canvas.update()
                }
            }.bind(this)
        });
        new Tips($$('.tip'))
    },
    set_rotation: function (deg) {
        this.rotation_slide.set(parseInt(deg))
    },
    set_zoom: function (xzoom) {
        if (isNaN(xzoom)) xzoom = 0;
        this.zoom_slide.set(parseFloat(xzoom) * 1)
    }
});
var Vml_render = new Class({
    options: {},
    initialize: function (options) {
        this.container = $(options.container) || document.body;
        this.xfact = 1;
        this.rotation = 0;
        this.id = options.id;
        this.width = options.width;
        this.height = options.height;
        this.container.setStyles({
            top: 0,
            left: 0,
            border: 0,
            margin: 0,
            width: this.width,
            height: this.height,
            overflow: 'hidden',
            position: 'absolute'
        });
        this.root = this.container;
        this.root.id = this.id;
        if (!document.namespaces['v']) {
            //document.namespaces.add("v", "urn:schemas-microsoft-com:vml");
            //document.createStyleSheet().addRule("v\\:*", "behavior:url(#default#VML)")
            document.namespaces.add("v", "urn:schemas-microsoft-com:vml", "#default#VML");
        }
    },
    create_image: function (src, left, top, width, height, id) {
        vml = document.createElement('v:image');
        vml.id = 'kroprimg_' + id;
        vml.style.position = 'absolute';
        vml.style.left = left;
        vml.style.top = top;
        vml.style.width = width;
        vml.style.height = height;
        vml.style.cursor = 'move';
        vml.src = src;
        this.root.appendChild(vml);
        this.image = vml;
        this.original = { w: width, h: height, x: left, y: top };
        this.posx = 0;
        this.posy = 0
    },
    get_bbox: function () {
        return {
            x: this.image.offsetLeft,
            y: this.image.offsetTop,
            width: this.image.offsetWidth,
            height: this.image.offsetHeight
        }
    },
    set_rotation: function (deg) {
        this.image.style.rotation = deg;
        this.rotation = deg;
        this.update()
    },
    zoom: function (xfact) {
        act = this.get_bbox();
        w_diff = (this.original.w * xfact - act.width) / 2;
        h_diff = (this.original.h * xfact - act.height) / 2;
        this.image.style.left = act.x - w_diff;
        this.image.style.top = act.y - h_diff;
        this.image.style.width = this.original.w * xfact;
        this.image.style.height = this.original.h * xfact;
        this.xfact = xfact;
        this.update()
    },
    update: function () {
        this.posx = this.image.style.left.toInt();
        this.posy = this.image.style.top.toInt();
    },
    set_reset: function () {
        this.set_rotation(0);
        this.image.style.left = this.original.x;
        this.image.style.top = this.original.y;
        this.image.style.width = this.original.w;
        this.image.style.height = this.original.h;
    }
});
var Svg_render = new Class({
    options: {},
    initialize: function (options) {
        this.container = $(options.container) || document.body;
        this.width = options.width;
        this.height = options.height;
        this.id = options.id;
        this.SVG = "http://www.w3.org/2000/svg";
        this.XLINK = "http://www.w3.org/1999/xlink";
        var svg = document.createElementNS(this.SVG, "svg");
        this.root = svg;
        svg.setAttributeNS(null, 'id', this.id);
        svg.setAttributeNS(null, 'width', this.width);
        svg.setAttributeNS(null, 'height', this.height);
        svg.setAttributeNS(null, 'preserveAspectRatio', "none");
        this.container.appendChild(this.root);
        this.rotation = 0;
        this.container.setStyles({ 'overflow': 'hidden' });
        this.xfact = 1
    },
    get_bbox: function (el) {
        return (el.getBBox())
    },
    create_image: function (src, left, top, width, height, id) {
        var svg = document.createElementNS(this.SVG, "image");
        svg.setAttributeNS(null, 'id', "kroprimg_" + id);
        svg.setAttributeNS(null, 'y', top);
        svg.setAttributeNS(null, 'x', left);
        svg.setAttributeNS(null, 'width', width);
        svg.setAttributeNS(null, 'height', height);
        svg.setAttributeNS(this.XLINK, 'href', src);
        svg.setAttributeNS(null, 'preserveAspectRatio', "none");
        svg.setAttributeNS(null, 'style', "cursor:move;");
        this.root.appendChild(svg);
        this.image = svg;
        this.original = { 'w': width, 'h': height, 'x': left, 'y': top };
        this.posx = 0;
        this.posy = 0
    },
    set_rotation: function (deg) {
        var cx = this.image.getAttributeNS(null, 'x').toInt() + this.image.getAttributeNS(null, 'width').toInt() / 2;
        var cy = this.image.getAttributeNS(null, 'y').toInt() + this.image.getAttributeNS(null, 'height').toInt() / 2;
        this.image.setAttributeNS(null, 'transform', 'rotate(' + deg + ',' + cx + ',' + cy + ')');
        this.rotation = deg;
        this.update()
    },
    zoom: function (xfact) {
        act = this.image.getBBox();
        w_diff = (this.original.w * xfact - act.width) / 2;
        h_diff = (this.original.h * xfact - act.height) / 2;
        this.image.setAttributeNS(null, 'x', act.x - w_diff);
        this.image.setAttributeNS(null, 'y', act.y - h_diff);
        this.image.setAttributeNS(null, 'width', this.original.w * xfact);
        this.image.setAttributeNS(null, 'height', this.original.h * xfact);
        this.xfact = xfact;
        this.update()
    },
    get_transform: function (transform) {
        if (transform == 'all') var re = new RegExp('(.*)');
        if (transform == 'translate') {
            var re = new RegExp("translate\((.*)\)");
            transform = this.image.getAttributeNS(null, 'transform');
            var m = re.exec(transform);
            if (m == null) {
                var t = new Array();
                t[0] = 0;
                t[1] = 0;
                return t
            }
            else {
                var str = m[1].split(')')[0].replace('(', '').trim();
                if (Browser.ie9) {
                    return str.split(' ');
                }
                else {
                    return str.split(',');
                }
            }
        }
        if (transform == 'rotate') {
            var re = new RegExp("rotate\((.*)\)");
            transform = this.image.getAttributeNS(null, 'transform');
            var m = re.exec(transform);
            if (m == null) {
                var rot = new Array();
                rot[0] = 0;
                return rot
            } else {
                var str = m[1].replace('(', '').replace(')', '');
                if (Browser.ie9) {
                    return str.split(' ');
                }
                else {
                    return str.split(',');
                }
            }
        }
    },
    update: function () {
        tr = this.get_transform('translate');
        ro = this.get_transform('rotate');
        var bx = this.image.getBBox();
        if (!tr[0]) { tr[0] = 0 }
        this.image.setAttributeNS(null, 'x', parseFloat(tr[0]) + parseFloat(bx.x));
        this.posx = parseFloat(tr[0]) + parseFloat(bx.x);
        if (!tr[1]) { tr[1] = 0 }
        this.image.setAttributeNS(null, 'y', parseFloat(tr[1]) + parseFloat(bx.y));
        this.posy = parseFloat(tr[1]) + parseFloat(bx.y);
        if (ro[1]) {
            rot_x = parseFloat(ro[1]) + parseFloat(tr[0]);
            rot_y = parseFloat(ro[2]) + parseFloat(tr[1]);
            this.image.setAttributeNS(null, 'transform', 'rotate(' + ro[0] + ',' + rot_x + ',' + rot_y + ')')
        }
        else {
            this.image.setAttributeNS(null, 'transform', 'rotate(' + ro[0] + ')')
        }
    },
    set_reset: function () {
        this.set_rotation(0);
        this.image.setAttributeNS(null, 'x', this.original.x);
        this.image.setAttributeNS(null, 'y', this.original.y);
        this.image.setAttributeNS(null, 'width', this.original.w);
        this.image.setAttributeNS(null, 'height', this.original.h)
    }
});
var Cropper = new Class({
    options: {},
    initialize: function (options) {
        var self = this;
        self.render = options.render;
        self.kropr = options.kropr;
        self.mode = 'move';
        self.x = options.x || null;
        self.y = options.y || null;
        self.opacity = options.opacity || 0.6;
        self.color = options.color || '#000000';
        self.container = options.render.container;
        self.container_coords = self.container.getCoordinates();
        self.width = options.width || parseInt(self.container_coords.width / 2);
        self.height = options.height || parseInt(self.container_coords.height / 2);
        self.top = options.top || parseInt((self.container_coords.height - self.height) / 2);
        self.left = options.left || parseInt((self.container_coords.width - self.width) / 2);

        this.t = new Element('div.op', {
            'styles': {
                top: 0,
                left: 0,
                height: self.top + 1,
                width: self.container_coords.width
            }
        }).inject(self.container);

        this.b = new Element('div.op', {
            'styles': {
                left: 0,
                top: self.top + self.height + 1,
                height: self.top + self.height + 1,
                width: self.container_coords.width
            }
        }).inject(self.container);

        this.l = new Element('div.op', {
            'styles': {
                left: 0,
                top: self.top + 1,
                height: self.height,
                width: self.left + 1
            }
        }).inject(self.container);

        this.r = new Element('div.op', {
            'styles': {
                top: self.top + 1,
                height: self.height,
                width: self.left + 1,
                left: self.width + self.left + 1
            }
        }).inject(self.container);

        self.set_mode(self.mode);
        this.cropper = new Element('div', {
            'styles': {
                top: self.top,
                left: self.left,
                cursor: 'move',
                width: self.width,
                height: self.height,
                position: 'absolute',
                overflow: 'hidden',
                background: 'url(about:blank)',
                border: '1px dashed #fff'
            }
        }).inject(self.container);

        this.cropper.addEvents({
            mouseenter: function () {
                self.cropper.setStyle('border', '1px dashed #f00');
            },
            mouseleave: function () {
                self.cropper.setStyle('border', '1px dashed #fff');
            },
            mousedown: function () {
                self.set_mode('crop');
            }
        });

        this.resizer = new Element('div', {
            'styles': {
                width: 6,
                height: 6,
                zIndex: 2,
                top: self.cropper.getStyle('top').toInt() + self.height - 7,  // self.height * 2 - 7,
                left: self.cropper.getStyle('left').toInt() + self.width - 7, //self.width * 2 - 7,
                background: '#fff',
                overflow: 'hidden',
                cursor: 'se-resize',
                position: 'absolute',
                border: '1px solid #333'
            }
        }).inject(self.container);

        this.text = new Element('div', {
            text: self.width + 'x' + self.height,
            'styles': {
                top: 0,
                zIndex: 1,
                color: '#fff',
                fontSize: '12px',
                background: '#000',
                textAlign: 'right',
                overflow: 'hidden',
                position: 'absolute',
                fontFamily: 'verdana',
                padding: 3,
                opacity: .7
            }
        }).inject(self.cropper);
        this.resizer.addEvents({
            mouseenter: function () {
                self.cropper.setStyle('border', '1px dashed #f00');
            },
            mouseleave: function () {
                self.cropper.setStyle('border', '1px dashed #fff');
            },
            mousedown: function () {
                self.set_mode('crop');
            }
        });

        this.drdr = this.cropper.makeDraggable({
            container: self.container,
            onStart: function () {
                self.old_top = self.cropper.getStyle('top').toInt();
                self.old_left = self.cropper.getStyle('left').toInt();
                self.old_height = self.cropper.getStyle('height').toInt();
                self.old_width = self.cropper.getStyle('width').toInt()
            },
            onDrag: function () {
                var d_top = self.cropper.getStyle('top').toInt();
                var d_left = self.cropper.getStyle('left').toInt();
                var d_height = self.cropper.getStyle('height').toInt();
                var d_width = self.cropper.getStyle('width').toInt();
                self.t.setStyles({ 'height': d_top + 1 });
                self.l.setStyles({ 'width': d_left + 1, 'top': d_top + 1 });
                self.r.setStyles({ 'left': d_left + d_width + 1, 'width': self.container_coords.width - d_left - d_width + 1, 'top': d_top + 1 });
                self.b.setStyles({ 'top': d_top + d_height + 1, 'height': self.container_coords.height - d_top - d_height });
                self.resizer.setStyles({ 'top': d_top + d_height - 7, 'left': d_left + d_width - 7 })
            },
            onComplete: function () {
                self.res.options.limit = {
                    x: [10, self.old_width + self.r.getStyle('width').toInt()],
                    y: [10, self.old_height + self.b.getStyle('height').toInt()]
                }
            }
        });
        this.res = this.cropper.makeResizable({
            container: self.container,
            handle: self.resizer,
            onDrag: function () {
                var d_top = self.cropper.getStyle('top').toInt();
                var d_left = self.cropper.getStyle('left').toInt();
                var d_height = self.cropper.getStyle('height').toInt();
                var d_width = self.cropper.getStyle('width').toInt();
                self.t.setStyles({ 'height': d_top + 1 });
                self.l.setStyles({ 'width': d_left + 1, 'top': d_top + 1, 'height': d_height });
                self.r.setStyles({ 'left': d_left + d_width + 1, 'width': self.container_coords.width - d_left - d_width, 'top': d_top + 1, 'height': d_height });
                self.b.setStyles({ 'top': d_top + d_height + 1, 'height': self.container_coords.height - d_top - d_height });
                self.resizer.setStyles({ 'top': d_top + d_height - 7, 'left': d_left + d_width - 7 });
                self.text.set('text', d_width + 'x' + d_height);
            }
        })
    },
    set_mode: function (mode) {
        if (mode == 'crop') {
            display = 'block'
        }
        else if (mode = 'move') {
            display = 'none'
        }
        this.mode = mode;
        this.t.setStyle('display', display);
        this.b.setStyle('display', display);
        this.r.setStyle('display', display);
        this.l.setStyle('display', display)
    },
    set_reset: function () {
        this.cropper.setStyles({ height: this.height, width: this.width, top: this.top, left: this.left });
        this.t.setStyles({ height: this.top + 1, width: this.container_coords.width, top: 0, left: 0 });
        this.l.setStyles({ height: this.height, width: this.left + 1, top: this.top + 1, left: 0 });
        this.r.setStyles({ height: this.height, width: this.left + 1, top: this.top + 1, left: this.width + this.left + 1 });
        this.b.setStyles({ height: this.top + this.height + 1, width: this.container_coords.width, top: this.top + this.height + 1, left: 0 });
        this.resizer.setStyles({ top: this.cropper.getStyle('top').toInt() + this.height - 7, left: this.cropper.getStyle('left').toInt() + this.width - 7 });
        this.text.set('text', this.width + 'x' + this.height);
        this.render.set_reset();
        this.kropr.rotation_slide.set(0);
        this.kropr.zoom_slide.set(40)
    }
});
Element.extend({
    disableSelection: function () {
        if (window.ActiveXObject) this.onselectstart = function () {
            return false
        };
        this.style.MozUserSelect = "none";
        return this
    }
});