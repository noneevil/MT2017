window.addEvent('domready', function () {
    var div = new Widget({});

    //    $(div).setStyles({
    //        border: '1px solid #000',
    //        width: 300,
    //        height: 400
    //    });

});
var Widget = new Class({
    Implements: [Options, Events],
    options:
    {

    },
    initialize: function (options) {
        this.setOptions(options);
        this.render();

        alert(document.body.getOffsetParent);
    },
    render: function () {
        this.Mask = new Element('div', {
            id: 'mask-' + String.uniqueID(),
            styles:
            {
                top: 0,
                left: 0,
                opacity: .1,
                'z-index': 10000,
                position: 'absolute',
                'text-align': 'center',
                'background-color': '#000'
            }
        }).inject(document.body);
    },
    resize: function () {

    },
    toElement: function () {
        //        return this.Mask;
    }
});