<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    uploadify组件上传测试
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .uploadify-button-text { display: block; font-size: 18px; text-align: center; color: #fff; text-indent: 0; }
    </style>
    <script src="/Scripts/lib/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="/Scripts/uploadify/jquery.uploadify.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        jQuery(function () {
            jQuery('#file').uploadify({
                width: 400,
                height: 300,
                queueID: true,
                buttonImage: '/Content/NewFolder1/159130308.h264_1.jpg',
                buttonText: '上传封面图',
                fileTypeExts: '*.jpg;*.jpeg;*.bmp;*.gif',
                fileTypeDesc: '图片文件',
                fileObjName: 'upload',//服务器端脚本使用的文件对象的名称 $_FILES个['upload']
                formData: {
                    //categoryid: 10,
                    //name: '文件名称' + Date.parse(new Date()),
                    //tags: '标签',
                    //description: '图片说明',
                    //"RoadsoftID": "xftz3m115xaurnxwjimd2x0v",
                    //thumbsize: JSON.stringify(thumbsize2)
                },//附带值
                //multi: false,
                //queueSizeLimit: 1,//上传数量
                fileSizeLimit: '300MB', //上传文件的大小限制
                swf: '/Scripts/uploadify/uploadify.swf',
                uploader: '/api/Images/Upload',//上传处理程序
                //debug: true,//开启调试
                //auto: false,//是否自动上传
                successTimeout: 99999,//超时时间
                onUploadSuccess: function (file, data, response) {
                    $('#txtResult').val(data);
                }
            });
        });
    </script>
    <input id="file" name="file" type="file" />

</asp:Content>
