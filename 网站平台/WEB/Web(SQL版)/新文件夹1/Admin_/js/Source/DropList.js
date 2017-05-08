var DropList = new Class({
    Implements: [Options],
    options: { container: '', multiple: false, parent: false },
    element: null,
    coord: {},
    size: {},
    loadurl: false,
    initialize: function(options) {
        this.setOptions(options);
        this.element = $(options.container + '_txt');
        if (!this.element) return;
        this.src = '/Developer/DropMenu.aspx?container=' + options.container + '&multiple=' + options.multiple + '&parent=' + options.parent;

        this.element.setStyle('background', 'url(/Developer/images/ico/DropDownList.gif) no-repeat right bottom');
        this.element.addEvents({
            click: this.Show.bind(this),
            mouseenter: function() {
                this.element.setStyle('background', 'url(/Developer/images/ico/DropDownList.gif) no-repeat right top');
            } .bind(this),
            mouseleave: function() {
                this.element.setStyle('background', 'url(/Developer/images/ico/DropDownList.gif) no-repeat right bottom');
            } .bind(this)
        });

        this.mask = new Element('div', {
            styles: {
                top: 0,
                left: 0,
                opacity: 0,
                width: '100%',
                display: 'none',
                background: '#000',
                position: 'absolute'
            },
            events: {
                click: this.Hide.bind(this)
            }
        }).inject(document.body, 'top');

        var size = this.element.getSize();
        this.iframe = new IFrame(
        {
            //src: this.src,
            frameborder: 0,
            styles: {
                top: 0,
                left: 0,
                width: size.x - (this.element.getStyle('border').toInt() * 2),
                height: 100,
                position: 'absolute',
                border: '1px solid #add2da',
                'background-color': '#fff'
            }
        }).inject(this.element, 'after');
        this.iframe.setStyles(
        {
            top: (this.element.getPosition().y - this.iframe.getPosition().y) + size.y + 1,
            left: this.element.getPosition().x - this.iframe.getPosition().x,
            height: 0,
            opacity: 0,
            display: 'none'
        });
    },
    Hide: function(event) {
        event.stop();
        if (this.transition) this.transition.cancel();
        this.transition = new Fx.Morph(this.iframe, {
            duration: 50,
            transition: Fx.Transitions.linear,
            onComplete: function() {
                this.iframe.setStyles({ display: 'none' });
                this.mask.setStyles({ height: 0, display: 'none' });
            } .bind(this)
        }).start({ height: 0, opacity: 0 });
    },
    Show: function(event) {
        event.stop();
        if (!this.loadurl) {
            this.loadurl = true;
            this.iframe.set('src', this.src);
        }
        this.size = window.getScrollSize();
        this.mask.setStyles({ height: this.size.y, display: 'block' });

        this.iframe.setStyles({ display: 'block' });
        if (this.transition) this.transition.cancel();
        this.transition = new Fx.Morph(this.iframe, {
            duration: 300,
            transition: Fx.Transitions.linear
        }).start({
            height: 300,
            opacity: 1
        });
    }
});