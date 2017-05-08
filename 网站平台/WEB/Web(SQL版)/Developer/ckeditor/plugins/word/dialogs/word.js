CKEDITOR.dialog.add('Word', function (editor) {
    return {
        title: 'Word 文档转换',
        minWidth: 350,
        minHeight: 150,
        resizable: false,
        contents: [
			{
			    id: 'Upload',
			    elements:
				[
                    {
                        type: "html",
                        html: "<iframe id='uploadword' width='100%' height='100%' src='about:blank'></iframe>",
                        style: "width:100%;height:150px;padding:0;"
                    }
				]
			}
		],
        onOk: function () {
            var win = document.getElementById('uploadword').contentWindow;
            var txt = win.document.getElementById('txt').value;
            editor.insertHtml(txt);
        },
        onLoad: function () {

        },
        onShow: function () {
            document.getElementById('uploadword').contentDocument.location.href = "/Developer/Plugin-S/Word.aspx";
        }
    };
});