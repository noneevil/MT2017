CKEDITOR.plugins.add('Word', {
    requires: ['dialog'],
    init: function (editor) {
        CKEDITOR.dialog.add('Word', this.path + 'dialogs/Word.js');
        editor.addCommand('Word', new CKEDITOR.dialogCommand('Word'));
        editor.ui.addButton('Word', {
            label: "Word转换插件",
            command: 'Word',
            icon: this.path + 'images/Word.png'
        });
    }
});