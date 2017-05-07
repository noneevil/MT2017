var SWFUpload = new Class({
    Extends: Swiff,
    version: "2.5.0 2010-01-15 Beta 2",
    Values: [],
    element: null,
    movieElement: null,
    console: null,
    speed: 10,
    QUEUE_ERROR: { QUEUE_LIMIT_EXCEEDED: -100, FILE_EXCEEDS_SIZE_LIMIT: -110, ZERO_BYTE_FILE: -120, INVALID_FILETYPE: -130 },
    UPLOAD_ERROR: {
        HTTP_ERROR: -200,
        MISSING_UPLOAD_URL: -210,
        IO_ERROR: -220,
        SECURITY_ERROR: -230,
        UPLOAD_LIMIT_EXCEEDED: -240,
        UPLOAD_FAILED: -250,
        SPECIFIED_FILE_ID_NOT_FOUND: -260,
        FILE_VALIDATION_FAILED: -270,
        FILE_CANCELLED: -280,
        UPLOAD_STOPPED: -290,
        RESIZE: -300
    },
    FILE_STATUS: { QUEUED: -1, IN_PROGRESS: -2, ERROR: -3, COMPLETE: -4, CANCELLED: -5 },
    UPLOAD_TYPE: { NORMAL: -1, RESIZED: -2 },
    BUTTON_ACTION: { SELECT_FILE: -100, SELECT_FILES: -110, START_UPLOAD: -120, JAVASCRIPT: -130, NONE: -130 },
    CURSOR: { ARROW: -1, HAND: -2 },
    RESIZE_ENCODING: { JPEG: -1, PNG: -2 },

    initialize: function(src, arg) {
        this.element = $(arg.id);
        var events = {};
        for (var property in this.callBacks) {
            events[property] = this.callBacks[property].bind(this);
        }
        Object.append(arg, { id: null, params: { wMode: 'transparent' }, callBacks: events });
        this.parent(src, arg);

        var size = this.element.getCoordinates();
        var _border = this.element.getStyle('border').toInt() * 2;
        this.box = new Element('div').inject(this.element, 'after');
        this.box.setProperties(this.element.getProperties('style', 'class'));
        this.box.setStyles({
            width: size.width - _border,
            height: size.height - _border,
            background: '#fff',
            overflow: 'hidden',
            border: 0,
            position: 'absolute',
            left: 0 - _border / 2,
            top: 0 - _border / 2
        });
        this.box.setStyles({ top: this.element.getPosition().y - this.box.getPosition().y, left: this.element.getPosition().x - this.box.getPosition().x, display: 'none' });
        this.perc = new Element('div', { styles: { background: 'url(<%=WebResource("WebSite.Controls.SWFUpload.ProgressBar.gif") %>) 0 center no-repeat', width: 0, height: '100%'} }).inject(this.box);
        this.txt = new Element('div', { styles: { width: '100%', height: '100%', 'text-align': 'center', overflow: 'hidden', 'line-height': 21, 'white-space': 'nowrap', 'z-index': 10, position: 'absolute', left: 0, top: 0} }).inject(this.box);

        this.console = $(this.id + "_Console");
    },
    set: function(to, file) {
        var _width = (this.box.getStyle('width').replace('px', '') * (to.toInt() / 100)).toInt();
        this.perc.set('morph', { duration: this.speed, link: 'cancel' }).morph({ width: _width });
        var procc = to.toInt() + '%';
        if (file)
            procc = '文件：' + file.name + '      ' + procc;
        this.txt.set('text', procc);
    },
    callBacks: {
        flashReady: function() {

        },
        fileDialogStart: function() {

        },
        mouseClick: function() {

        },
        mouseOver: function() {

        },
        mouseOut: function() {

        },
        fileQueued: function(file) {

        },
        fileQueueError: function(file, errorCode, message) {
            switch (errorCode) {
                case this.QUEUE_ERROR.FILE_EXCEEDS_SIZE_LIMIT:
                case this.QUEUE_ERROR.ZERO_BYTE_FILE:
                case this.QUEUE_ERROR.INVALID_FILETYPE:
                    alert(file.name + ' 文件无效！');
                default:
                    alert(message);
                    break;
            }
        },
        fileDialogComplete: function(numFilesSelected, numFilesQueued, numFilesInQueue) {
            this.remote('StartUpload');
        },
        uploadResizeStart: function(file, resizeSettings) {

        },
        uploadStart: function(file) {
            this.set(0);
            this.box.setStyles({ display: 'block', opacity: 1 });
            this.remote('ReturnUploadStart', true);
        },
        uploadProgress: function(file, bytesComplete, bytesTotal) {
            var percent = Math.ceil((bytesComplete / bytesTotal) * 100);
            this.set(percent, file);
        },
        uploadError: function(file, errorCode, message) {

        },
        uploadSuccess: function(file, serverData, responseReceived) {
            this.Values.include(serverData);
            this.set(100, file);
        },
        uploadComplete: function(file) {
            if (this.remote('GetStats').files_queued > 0) {
                (function() {
                    this.remote('StartUpload');
                } .bind(this)).delay(100);
            }
            else {
                this.element.value = this.Values.join("@");
                this.txt.set('text', " 上传完成!");
                (function() {
                    new Fx.Morph(this.box, {
                        duration: 500,
                        transition: Fx.Transitions.linear,
                        onComplete: function() {
                            this.box.setStyles({ display: 'none' });
                        } .bind(this)
                    }).start({ opacity: 0 });
                } .bind(this)).delay(1000);
                if (this.options.vars.buttonAction == this.BUTTON_ACTION.SELECT_FILE) this.Values.empty();
            }
        },
        debug: function(message) {
            try {
                if (!this.console) {
                    this.console = new Element('textarea', { id: this.id + "_Console", wrap: 'off', styles: { 'width': '100%', 'height': 200} }).inject(document.body, 'top');
                }
                this.console.value += message + "\n";
                this.console.scrollTop = this.console.scrollHeight - this.console.clientHeight;
            }
            catch (ex) {
                alert("Exception: " + ex.name + " Message: " + ex.message);
            }
        }
    }
});