var SlideList = new Class({
    Implements: [Options],
    options: {
        transition: Fx.Transitions.Back.easeOut,
        duration: 500
    },
    initialize: function (menu, options) {
        this.setOptions(options);

        this.menu = $(menu);
        this.current = this.menu.getElement('li.on');

        this.menu.getElements('li').each(function (item, index) {
            item.addEvents({
                mouseenter: function () {
                    this.moveBg(item);
                }.bind(this),
                mouseleave: function () {
                    this.moveBg(this.current);
                }.bind(this),
                click: function () {
                    this.clickItem(item, index);
                }.bind(this)
            });
        }.bind(this));

        this.back = new Element('li').addClass('background').inject(this.menu);
        this.back.set('tween', this.options);
        if (this.current) this.setCurrent(this.current);
    },
    setCurrent: function (el, effect) {
        this.back.setStyle('left', this.getLeft(el));
        this.current = el;
    },
    clickItem: function (item, index) {
        if (!this.current) this.setCurrent(item, true);
        this.current = item;

        document.title = item.get('text');
        /*tabs*/
        var contents = $('menu').getElements('ul');
        contents.addClass('hide');
        if (contents[index]) contents[index].removeClass('hide');
    },
    moveBg: function (to) {
        if (!this.current) return;
        this.back.tween('left', this.getLeft(to));
    },
    getLeft: function (el) {
        return el.offsetLeft - el.getStyle('margin-left').toInt();
    }
});