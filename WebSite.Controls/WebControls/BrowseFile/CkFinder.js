window.addEvent('domready', function () {
    var browsefile = new IFrame({
        src: '<%=WebResource("WebSite.Controls.BrowseFile.CkFinder.html") %>',
        styles: {
            width: '100%',
            height: 100,
            border: 1
        },
        events: {
            mouseenter: function () {
                //alert('Welcome aboard.');
            },
            mouseleave: function () {
                //alert('Goodbye!');
            },
            load: function () {
                alert('The iframe has finished loading.');
            }
        }
    }).inject(top.document.body);
});