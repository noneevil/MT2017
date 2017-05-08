<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Menu_Edit.aspx.cs" Inherits="WebSite.Web.Menu_Edit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>添加或修改菜单</title>
    <link href="images/css.css" rel="stylesheet" type="text/css" />
    <script src="Plugins/mootools/mootools-core-1.4.5.js" type="text/javascript"></script>
    <script src="Plugins/jQuery/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="Plugins/jQuery-ui/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="Plugins/public.js" type="text/javascript"></script>
</head>
<body scroll="no">
<form runat="server">
    <table border="0" width="280" cellpadding="4" align="center" cellspacing="0">
        <tr>
            <th width="70px">菜单名称：</th>
            <td>
                <aspx:TextBox ID="menuname" runat="server" CssClass="text" Width="150"></aspx:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="menuname" CssClass="line1px_5"  runat="server" BorderStyle="None" />
            </td>
        </tr>
        <tr>
            <th>上级菜单：</th>
            <td>
                <aspx:DropDownList ID="parentid" runat="server" Width="162"></aspx:DropDownList>
            </td>
        </tr>
        <tr>
            <th>链接页面：</th>
            <td><aspx:TextBox ID="action" runat="server" CssClass="text" Width="150"></aspx:TextBox></td>
        </tr>
        <tr>
            <th></th>
            <td>
                <aspx:CheckBox ID="status" runat="server"  Text="启用"/>
                <aspx:CheckBox ID="open" runat="server"  Text="展开"/>
                <aspx:CheckBox ID="internal" runat="server"  Text="内置功能"/>
            </td>
        </tr>
        <tr>
            <th></th>
            <td>
                <asp:RequiredFieldValidator ControlToValidate="menuname" CssClass="line1px_2" runat="server" ErrorMessage="菜单名称不能为空！"  Width="135"/>
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