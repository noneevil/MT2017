<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Comment_Edit.aspx.cs" Inherits="WebSite.Web.Comment_Edit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>添加或修改会员资料</title>
    <link href="images/css.css" rel="stylesheet" type="text/css" />
    <script src="Plugins/mootools/mootools-core-1.4.5.js" type="text/javascript"></script>
    <script src="Plugins/jQuery/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="Plugins/jQuery-ui/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="Plugins/jQuery-ui/jquery.ui.datepicker-zh-CN.min.js" type="text/javascript"></script>
    <script src="Plugins/public.js" type="text/javascript"></script>
    <script type="text/javascript">
        jq(function () {
            jq.datepicker.setDefaults(jq.datepicker.regional['zh-TW']);
            var dates = jq("#birthday");
            dates.datepicker({
                changeMonth: true,
                changeYear: true,
                showAnim: 'clip',
                dateFormat: "yy-mm-dd"
            });
            var src = 'url(system/Thumbnail.aspx?w=165&h=100&file=' + jq('#picture').attr('value') + ')';
            jq('#photo').css({ 'background-image': src });
        });
    </script>
</head>
<body scroll="no">
<form id="Form1" runat="server">
    <table border="0" width="96%" cellpadding="4" align="center" cellspacing="0">
        <tr>
            <th width="70px">会员名称：</th>
            <td>
                <aspx:Label ID="MemberName" runat="server"></aspx:Label>
            </td>
        </tr>
        <tr>
            <th>用户名称：</th>
            <td>
                <aspx:Label ID="UserName" runat="server"></aspx:Label>
            </td>
        </tr>
        <tr>
            <th>评论标题：</th>
            <td>
                <aspx:Label ID="Title" runat="server"></aspx:Label>
            </td>
        </tr>
        <tr>
            <th>评论内容：</th>
            <td>
                <aspx:Label ID="Content" runat="server"></aspx:Label>
            </td>
        </tr>
        
        <tr>
            <th></th>
            <td>
                支持:(<aspx:Label ID="Plus" runat="server"></aspx:Label>)
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                反对:(<aspx:Label ID="Minus" runat="server"></aspx:Label>)
            </td>
        </tr>
        <tr>
            <th>日期：</th>
            <td>
                <aspx:Label ID="JoinDate" runat="server" FormatString="{0:yyyy年MM月dd日 HH:mm:ss}"></aspx:Label>
            </td>
        </tr>
        <tr>
            <th>IP地址:</th>
            <td>
                <aspx:Label ID="ClientIP" runat="server"></aspx:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align:center;">
                <input type="button" onclick="parent.view.dialog('close');" value="关闭" style="width:80px;" />
            </td>
        </tr>
    </table>
</form>
</body>
</html>