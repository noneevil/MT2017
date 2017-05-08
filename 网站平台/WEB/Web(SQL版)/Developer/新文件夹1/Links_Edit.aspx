<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Links_Edit.aspx.cs" Inherits="WebSite.Web.Links_Edit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>添加或修改友情连接</title>
    <meta name="Author" content="http://www.wieui.com" />
    <meta name="viewport" content="width=device-width" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="images/css.css" rel="stylesheet" type="text/css" />
    <script src="Plugins/mootools/mootools-core-1.4.5.js" type="text/javascript"></script>
    <script src="Plugins/jQuery/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="Plugins/jQuery-ui/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="Plugins/public.js" type="text/javascript"></script>
    <script type="text/javascript">
        jq(function () {
            jq('#groupid').change(function () {
                var val = jq(this).val() == '图片';
                if (val) {
                    jq('#tr_pic').show();
                } 
                else {
                    jq('#tr_pic').hide();
                }
            });

            if (jq('#groupid').val() == '图片') {
                jq('#tr_pic').show();
            }
        });
    </script>
</head>
<body scroll="no">
<form id="form1" runat="server">
    <table border="0" width="280" cellpadding="4" align="center" cellspacing="0">
        <tr>
            <th width="70px">连接名称：</th>
            <td>
                <aspx:TextBox ID="linkname" runat="server" CssClass="text" Width="150"></aspx:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="linkname" CssClass="line1px_5"  runat="server" BorderStyle="None" />
            </td>
        </tr>
         <tr>
            <th>连接分类：</th>
            <td>
                <aspx:DropDownList ID="groupid" runat="server" Width="162"></aspx:DropDownList>
            </td>
        </tr>
        <tr>
            <th>连接地址：</th>
            <td>
                <aspx:TextBox ID="linkurl" runat="server" CssClass="text" Width="150"></aspx:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="linkurl" CssClass="line1px_5"  runat="server" BorderStyle="None" />
            </td>
        </tr>
        <tr id="tr_pic" style="display:none;">
            <th>连接图片：</th>
            <td>
                <aspx:BrowseFile ID="linkimage" runat="server" CssClass="text" Width="150"></aspx:BrowseFile>
            </td>
        </tr>
        <tr>
            <th></th>
            <td>
                <asp:Label ID="Label1" runat="server" Width="135"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align:center;">
                <asp:Button ID="btnSave" runat="server" Text="保存" Width="80" onclick="btnSave_Click" />
                <input type="button" onclick="parent.view.dialog('close');" value="取消" style="width:80px;" />
            </td>
        </tr>
    </table>
</form>
</body>
</html>