var save2local = new Class({
    Extends: Swiff,
    initialize: function(src, arg) {
        Object.append(arg, { callBacks: this.callBacks });
        this.parent(src, arg);
        //        var parent = this.object.getParent();
        //        parent.getParent().replaceChild(this.object, parent);
        document.addEvent('dblclick', this.GetBrowser.bind(this));
    },
    write: function(key, val) {
        this.remote('write', key, val);
    },
    read: function(key) {
        return this.remote('read', key);
    },
    clear: function() {
        this.remote('clear');
    },
    send: function(key) {
        this.remote('send', key);
    },
    GetBrowser: function(event) {
        event.stop();
        this.write('browser', event.target.get('text'));
        this.send('browser');
        //alert(this.read("browser"));
        //this.clear();
    },
    callBacks: {
        UploadComplete: function(data) {
            alert(data);
        }
    }
});