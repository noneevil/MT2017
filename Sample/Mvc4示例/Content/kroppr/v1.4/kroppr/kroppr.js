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
            } .bind(this)
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
            } .bind(this)
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
        } .bind(this));
        $('k_reset').setStyles({
            top: this.coords.height - 30,
            left: this.coords.width / 2 + 10,
            opacity: .6
        }).addEvent('click', function () {
            this.cropper.set_reset()
        } .bind(this));
        $('k_accept').addEvent('mouseenter', function () { this.setStyle('opacity', 1); });
        $('k_reset').addEvent('mouseenter', function () { this.setStyle('opacity', 1); });
        $('k_accept').addEvent('mouseleave', function () { this.setStyle('opacity', .6); });
        $('k_reset').addEvent('mouseleave', function () { this.setStyle('opacity', .6); });
        $(document.body).addEvent('mouseleave', function () {
            this.cropper.drdr.stop();
            this.cropper.res.stop();
            this.cropper.set_mode('move')
        } .bind(this));

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
            } .bind(this),
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
            } .bind(this),
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
                    } .bind(this))
                }
            } .bind(this),
            'mouseup': function (e) {
                e = new DOMEvent(e);
                e.stop();
                this.cropper.set_mode('move');
                if (this.down) {
                    this.down = false;
                    this.canvas.update()
                }
            } .bind(this)
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
        this.width = options.width;
        this.height = options.height;
        this.id = options.id;
        this.root = this.container.clone().setStyles({ overflow: 'hidden', position: 'relative', border: 0, margin: 0, top: 0, left: 0 }).inject(this.container);
        this.root.id = this.id;
        this.rotation = 0;
        this.container.setStyle('overflow', 'hidden');
        this.container.setStyles({ 'overflow': 'hidden' });
        this.xfact = 1;
        if (!document.namespaces['v']) {
            //document.namespaces.add("v", "urn:schemas-microsoft-com:vml");
            //document.createStyleSheet().addRule("v\\:*", "behavior:url(#default#VML)")
            document.namespaces.add("v", "urn:schemas-microsoft-com:vml", "#default#VML");
        }
    },
    create_image: function (src, left, top, width, height, id) {
        vml = document.createElement('v:image');
        vml.id = 'kroprimg' + id;
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
        svg.setAttributeNS(null, 'id', "kroprimg" + id);
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
        this.mode = 'move';
        this.render = options.render;
        this.kropr = options.kropr;
        this.x = options.x || null;
        this.y = options.y || null;
        this.opacity = options.opacity || 0.6;
        this.color = options.color || '#000000';
        this.container = options.render.container;
        this.container_coords = this.container.getCoordinates();
        this.width = options.width || parseInt(this.container_coords.width / 2);
        this.height = options.height || parseInt(this.container_coords.height / 2);
        this.top = options.top || parseInt((this.container_coords.height - this.height) / 2);
        this.left = options.left || parseInt((this.container_coords.width - this.width) / 2);
        this.t = new Element('div#t', {
            'class': 'op',
            'styles': {
                top: 0,
                left: 0,
                opacity: .6,
                overflow: 'hidden',
                height: this.top + 1,
                width: this.container_coords.width
            }
        }).inject(this.container);

        this.b = new Element('div#b', {
            'class': 'op',
            'styles': {
                left: 0,
                opacity: .6,
                overflow: 'hidden',
                top: this.top + this.height + 1,
                height: this.top + this.height + 1,
                width: this.container_coords.width
            }
        }).inject(this.container);

        this.l = new Element('div#l', {
            'class': 'op',
            'styles': {
                left: 0,
                opacity: .6,
                overflow: 'hidden',
                top: this.top + 1,
                height: this.height,
                width: this.left + 1
            }
        }).inject(this.container);

        this.r = new Element('div#r', {
            'class': 'op',
            'styles': {
                opacity: .6,
                overflow: 'hidden',
                top: this.top + 1,
                height: this.height,
                width: this.left + 1,
                left: this.width + this.left + 1
            }
        }).inject(this.container);

        this.set_mode(this.mode);
        this.cropper = new Element('div', {
            'styles': {
                top: this.top,
                left: this.left,
                cursor: 'move',
                width: this.width,
                height: this.height,
                position: 'absolute',
                overflow: 'hidden',
                background: 'url(about:blank)',
                border: '1px dashed #fff'
            }
        }).inject(this.container);

        this.cropper.addEvents({
            'mouseenter': function () {
                this.cropper.setStyle('border', '1px dashed #f00');
            } .bind(this),
            'mouseleave': function () {
                this.cropper.setStyle('border', '1px dashed #fff');
            } .bind(this),
            'mousedown': function () {
                this.set_mode('crop');
            } .bind(this)
        });

        this.resizer = new Element('div', {
            'styles': {
                width: 6,
                height: 6,
                zIndex: 2,
                top: this.cropper.getStyle('top').toInt() + this.height - 7,  // this.height * 2 - 7,
                left: this.cropper.getStyle('left').toInt() + this.width - 7, //this.width * 2 - 7,
                background: '#fff',
                overflow: 'hidden',
                cursor: 'se-resize',
                position: 'absolute',
                border: '1px solid #333'
            }
        }).inject(this.container);

        this.show_size = new Element('div', {
            text: this.width + 'x' + this.height,
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
        }).inject(this.cropper);
        this.resizer.addEvents({
            'mouseenter': function () {
                this.cropper.setStyle('border', '1px dashed #f00');
            } .bind(this),
            'mouseleave': function () {
                this.cropper.setStyle('border', '1px dashed #fff');
            } .bind(this),
            'mousedown': function () {
                this.set_mode('crop');
            } .bind(this)
        });

        this.drdr = this.cropper.makeDraggable({
            container: this.container,
            onStart: function () {
                this.old_top = this.cropper.getStyle('top').toInt();
                this.old_left = this.cropper.getStyle('left').toInt();
                this.old_height = this.cropper.getStyle('height').toInt();
                this.old_width = this.cropper.getStyle('width').toInt()
            } .bind(this),
            onDrag: function () {
                var d_top = this.cropper.getStyle('top').toInt();
                var d_left = this.cropper.getStyle('left').toInt();
                var d_height = this.cropper.getStyle('height').toInt();
                var d_width = this.cropper.getStyle('width').toInt();
                this.t.setStyles({ 'height': d_top + 1 });
                this.l.setStyles({ 'width': d_left + 1, 'top': d_top + 1 });
                this.r.setStyles({ 'left': d_left + d_width + 1, 'width': this.container_coords.width - d_left - d_width + 1, 'top': d_top + 1 });
                this.b.setStyles({ 'top': d_top + d_height + 1, 'height': this.container_coords.height - d_top - d_height });
                this.resizer.setStyles({ 'top': d_top + d_height - 7, 'left': d_left + d_width - 7 })
            } .bind(this),
            onComplete: function () {
                this.res.options.limit = {
                    x: [10, this.old_width + this.r.getStyle('width').toInt()],
                    y: [10, this.old_height + this.b.getStyle('height').toInt()]
                }
            } .bind(this)
        });
        this.res = this.cropper.makeResizable({
            container: this.container,
            handle: this.resizer,
            onDrag: function () {
                var d_top = this.cropper.getStyle('top').toInt();
                var d_left = this.cropper.getStyle('left').toInt();
                var d_height = this.cropper.getStyle('height').toInt();
                var d_width = this.cropper.getStyle('width').toInt();
                this.t.setStyles({ 'height': d_top + 1 });
                this.l.setStyles({ 'width': d_left + 1, 'top': d_top + 1, 'height': d_height });
                this.r.setStyles({ 'left': d_left + d_width + 1, 'width': this.container_coords.width - d_left - d_width, 'top': d_top + 1, 'height': d_height });
                this.b.setStyles({ 'top': d_top + d_height + 1, 'height': this.container_coords.height - d_top - d_height });
                this.resizer.setStyles({ 'top': d_top + d_height - 7, 'left': d_left + d_width - 7 });
                this.show_size.set('text', d_width + 'x' + d_height);
            } .bind(this)
        })
    },
    set_mode: function (mode) {
        if (mode == 'crop') {
            display = 'block'
        } else if (mode = 'move') {
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
        this.resizer.setStyles({ height: 6, width: 6, top: this.cropper.getStyle('top').toInt() + this.height - 7, left: this.cropper.getStyle('left').toInt() + this.width - 7 });
        this.show_size.set('text', this.width + 'x' + this.height);
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