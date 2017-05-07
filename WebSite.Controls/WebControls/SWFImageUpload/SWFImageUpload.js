var SWFImageUpload = new Class({
    Extends: Swiff,
    version: "2.5.0 2010-01-15 Beta 2",
    element: null,
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

    initialize: function (src, arg) {
        this.element = $(arg.id);
        var holder = $(arg.id + "_holder");
        this.size = holder.getCoordinates();
        var containerid = "SWFImage_" + String.uniqueID();
        new Element('div', { id: containerid }).inject(holder);

        var events = {};
        for (var property in this.callBacks) {
            events[property] = this.callBacks[property].bind(this);
        }
        Object.append(arg, {
            id: null,
            width: this.size.width,
            height: this.size.height,
            container: containerid,
            params: { wMode: 'transparent' },
            callBacks: events
        });
        this.parent(src, arg);

        this.img = holder.getElement('img');
        this.img.setStyles({ width: '100%', height: '100%' });

        var div = holder.getElement('div');
        div.setStyles({ position: 'absolute', left: 0, top: 0, zIndex: 1 });

    },
    set: function (to, file) {
        var _width = (this.box.getStyle('width').replace('px', '') * (to.toInt() / 100)).toInt();
        this.perc.set('morph', { duration: this.speed, link: 'cancel' }).morph({ width: _width });
        var procc = to.toInt() + '%';
        if (file)
            procc = '文件：' + file.name + '      ' + procc;
        this.txt.set('text', procc);
    },
    callBacks: {
        flashReady: function () {

        },
        fileDialogStart: function () {

        },
        mouseClick: function () {

        },
        mouseOver: function () {

        },
        mouseOut: function () {

        },
        fileQueued: function (file) {

        },
        fileQueueError: function (file, errorCode, message) {
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
        fileDialogComplete: function (numFilesSelected, numFilesQueued, numFilesInQueue) {
            this.remote('StartUpload');
        },
        uploadResizeStart: function (file, resizeSettings) {

        },
        uploadStart: function (file) {
            this.remote('ReturnUploadStart', true);
        },
        uploadProgress: function (file, bytesComplete, bytesTotal) {
            var percent = Math.ceil((bytesComplete / bytesTotal) * 100);
        },
        uploadError: function (file, errorCode, message) {

        },
        uploadSuccess: function (file, serverData, responseReceived) {
            this.element.value = serverData;
            var src = 'url(/Developer/Plugin-S/Thumbnail.aspx?w=' + this.size.width + '&h=' + this.size.height + '&file=' + serverData + ')';
            this.img.setStyles({ 'background-image': src });
        },
        uploadComplete: function (file) {
            if (this.remote('GetStats').files_queued > 0) {
                (function () {
                    this.remote('StartUpload');
                }.bind(this)).delay(100);
            }
        },
        debug: function (message) {
            try {
                if (!this.console) {
                    this.console = new Element('textarea', { id: this.id + "_Console", wrap: 'off', styles: { 'width': '100%', 'height': 200 } }).inject(document.body, 'top');
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