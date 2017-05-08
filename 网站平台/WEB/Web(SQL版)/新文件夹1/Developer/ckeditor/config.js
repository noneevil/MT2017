/**
* @license Copyright (c) 2003-2013, CKSource - Frederico Knabben. All rights reserved.
* For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function (config) {
    config.skin = 'moono';
    //config.uiColor = '#C4E0DE';
    config.language = 'zh-cn';

    //上传配置
    config.filebrowserBrowseUrl = location.hash + '/ckfinder/ckfinder.html';
    config.filebrowserImageBrowseUrl = location.hash + '/ckfinder/ckfinder.html?Type=Images';
    config.filebrowserFlashBrowseUrl = location.hash + '/ckfinder/ckfinder.html?Type=Flash';
    config.filebrowserUploadUrl = location.hash + '/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files';
    config.filebrowserImageUploadUrl = location.hash + '/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images';
    config.filebrowserFlashUploadUrl = location.hash + '/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash';
    config.filebrowserWindowWidth = '950';
    config.filebrowserWindowHeight = '650';

    //工具栏配置
    config.toolbar = "Basic";
    config.toolbar_Full = [
        ['Source', '-', 'Preview'],
        ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'],
        ['Find', 'Replace', '-', 'SelectAll', '-', 'Scayt'],
        ['Form', 'Checkbox', 'Radio', 'TextField', 'Textarea', 'Select', 'Button', 'ImageButton', 'HiddenField'],
        ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-', 'RemoveFormat'],
        ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote', 'CreateDiv', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', '-', 'BidiLtr', 'BidiRtl'],
        ['Link', 'Unlink', 'Anchor'],
        ['Image', 'Flash', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar', 'PageBreak', 'Iframe'],
        ['Styles', 'Format', 'Font', 'FontSize'],
        ['TextColor', 'BGColor'],
        ['Maximize', 'ShowBlocks']
    ];

    config.toolbar_Basic = [
        ['Source'],
        ['Bold', 'Italic', 'Underline', 'Strike', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', '-',
        'NumberedList', 'BulletedList', 'Outdent', 'Indent', '-', 'Subscript', 'Superscript', '-', 'Cut', 'Copy', 'PasteText', 'PasteFromWord', '-', '-', 'Find', 'RemoveFormat', 'SelectAll', 'Maximize'], '/', ['Font', 'FontSize', 'Format', 'Styles', 'TextColor', 'BGColor', 'Table', 'Image', 'Flash', 'Link', 'Unlink', 'Anchor', 'SpecialChar', 'Blockquote', 'HorizontalRule', 'PageBreak']];

    config.toolbar_Min = [['Source', '-', 'Bold', 'Italic', 'Underline', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', '-', 'TextColor', 'BGColor', '-', 'Maximize']];

    // “拖拽以改变尺寸”功能
    config.resize_enabled = false;
    //工具栏是否可以被收缩
    config.toolbarCanCollapse = true;
    //工具栏的位置
    config.toolbarLocation = 'top'; //可选：bottom
    //工具栏默认是否展开
    config.toolbarStartupExpanded = true;
    //改变大小的最大高度
    config.resize_maxHeight = 3000;
    //改变大小的最大宽度
    config.resize_maxWidth = 3000;
    //改变大小的最小高度
    config.resize_minHeight = 250;
    //改变大小的最小宽度
    config.resize_minWidth = 400;
    // 当提交包含有此编辑器的表单时，是否自动更新元素内的资料
    config.autoUpdateElement = true;

    //设置编辑内元素的背景色的取值plugins/colorbutton/plugin.js.
    config.colorButton_backStyle = {
        element: 'span',
        styles: { 'background-color': '#(color)' }
    }
    //是否对编辑区域进行渲染plugins/editingblock/plugin.js
    config.editingBlock = true;
    //编辑器中回车产生的标签
    config.enterMode = CKEDITOR.ENTER_P; //可选：CKEDITOR.ENTER_BR或CKEDITOR.ENTER_DIV
    //是否使用HTML实体进行输出plugins/entities/plugin.js
    config.entities = true;
    //定义更多的实体plugins/entities/plugin.js
    config.entities_additional = '#39'; //其中#代替了&
    //是否转换一些难以显示的字元为相应的HTML字元plugins/entities/plugin.js
    config.entities_greek = true;
    //是否转换一些拉丁字元为HTML plugins/entities/plugin.js
    config.entities_latin = true;
    //是否转换一些特殊字元为ASCII字元如"This is Chinese: 汉语."转换为"This is Chinese: 汉语." plugins/entities/plugin.js
    config.entities_processNumerical = false;

    //默认的字体名 plugins/font/plugin.js
    config.font_defaultLabel = '宋体';
    //字体编辑时的字元集可以添加常用的中文字元：宋体、楷体、黑体等plugins/font/plugin.js
    config.font_names = 'Arial;Times New Roman;Verdana;宋体;微软雅黑;黑体;隶书;楷体;华文彩云;新宋体;幼圆;华文新魏;华文琥珀;华文细黑;华文行楷';
    //文字的默认式样 plugins/font/plugin.js
    config.font_style = {
        element: 'span',
        styles: { 'font-family': '#(family)' },
        overrides: [{ element: 'font', attributes: { 'face': null}}]
    };
    //字体默认大小 plugins/font/plugin.js
    config.fontSize_defaultLabel = '12px';
    //字体编辑时可选的字体大小plugins/font/plugin.js
    config.fontSize_sizes = '8/8px;9/9px;10/10px;11/11px;12/12px;14/14px;16/16px;18/18px;20/20px;22/22px;24/24px;26 /26px;28/28px;36/36px;48/48px;72/72px'
    //设置字体大小时使用的式样plugins/font/plugin.js
    config.fontSize_style = {
        element: 'span',
        styles: { 'font-size': '#(size)' },
        overrides: [{ element: 'font', attributes: { 'size': null}}]
    };
    //自定义插件
    //config.extraPlugins = 'syntaxhighlight,mediaplayer,music';
    //config.extraPlugins = 'control';
};