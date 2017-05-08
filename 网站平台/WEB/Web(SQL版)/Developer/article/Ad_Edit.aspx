<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Ad_Edit.aspx.cs" Inherits="WebSite.Web.Ad_Edit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>添加或修改广告</title>
    <meta name="Author" content="http://www.wieui.com" />
    <meta name="viewport" content="width=device-width" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../skin/css.css" rel="stylesheet" type="text/css" />
    <script src="../skin/Plugins/mootools-1.4.5.js" type="text/javascript"></script>
    <script src="../skin/Plugins/jquery-1.11.0.min.js" type="text/javascript"></script>
    <script src="../skin/Plugins/jQuery-ui/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../skin/Plugins/public.js" type="text/javascript"></script>
</head>
<body scroll="no">
<form id="form1" runat="server">
    <table border="0" width="280" cellpadding="4" align="center" cellspacing="0">
        <tr>
            <th width="70px">广告名称：</th>
            <td>
                <aspx:TextBox ID="title" runat="server" CssClass="text" Width="150"></aspx:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="title" CssClass="line1px_5"  runat="server" BorderStyle="None" />
            </td>
        </tr>
         <tr>
            <th>广告分类：</th>
            <td>
                <aspx:DropDownList ID="groupid" runat="server" Width="162"></aspx:DropDownList>
            </td>
        </tr>
        <tr>
            <th>广告地址：</th>
            <td>
                <aspx:TextBox ID="linkurl" runat="server" CssClass="text" Width="150"></aspx:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="linkurl" CssClass="line1px_5"  runat="server" BorderStyle="None" />
            </td>
        </tr>
        <tr>
            <th>广告媒体：</th>
            <td>
                <aspx:BrowseFile ID="linksrc" runat="server" CssClass="text" Width="150" ResourceType="广告文件" />
            </td>
        </tr>
        <tr>
            <th>打开窗口：</th>
            <td>
                <aspx:DropDownList ID="target" runat="server" Width="162"></aspx:DropDownList>
            </td>
        </tr>
        <tr>
            <th>广告宽度：</th>
            <td>
                <aspx:TextBox ID="width" runat="server" CssClass="text" Width="150" Text="100"></aspx:TextBox>
            </td>
        </tr>
        <tr>
            <th>广告高度：</th>
            <td>
                <aspx:TextBox ID="height" runat="server" CssClass="text" Width="150" Text="100"></aspx:TextBox>
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