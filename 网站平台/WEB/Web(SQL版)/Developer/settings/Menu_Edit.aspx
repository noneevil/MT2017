<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Menu_Edit.aspx.cs" Inherits="WebSite.Web.Menu_Edit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>添加或修改菜单</title>
    <link href="../skin/css.css" rel="stylesheet" type="text/css" />
    <script src="../skin/Plugins/mootools-1.4.5.js" type="text/javascript"></script>
    <script src="../skin/Plugins/jquery-1.11.0.min.js" type="text/javascript"></script>
    <script src="../skin/Plugins/jQuery-ui/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../skin/Plugins/Validform/Validform_v5.3.2.js" type="text/javascript"></script>
    <script src="../skin/Plugins/public.js" type="text/javascript"></script>
    <script type="text/javascript">
        jQuery(function () {
            var form = jQuery('#form1').Validform({ tiptype: 3 });
            form.tipmsg.c = ' ';
            form.addRule([
                {
                    ele: "#title",
                    datatype: "*3-20",
                    nullmsg: " ",
                    sucmsg: " ",
                    errormsg: " "
                }
            ]);
        });
    </script>
</head>
<body scroll="no">
<form id="form1" runat="server">
    <table border="0" width="98%" cellpadding="4" align="center" cellspacing="0">
        <tr>
            <th width="70px">菜单名称：</th>
            <td>
                <aspx:TextBox ID="title" runat="server" CssClass="text" Width="300"></aspx:TextBox>
                <em class="Validform_checktip">*</em>
                <%--<asp:RequiredFieldValidator ControlToValidate="title" CssClass="line1px_5"  runat="server" BorderStyle="None" />--%>
            </td>
        </tr>
        <tr>
            <th>上级菜单：</th>
            <td>
                <aspx:DropDownList ID="parentid" runat="server" Width="312"></aspx:DropDownList>
            </td>
        </tr>
        <tr>
            <th>链接页面：</th>
            <td><aspx:TextBox ID="link_url" runat="server" CssClass="text" Width="300"></aspx:TextBox></td>
        </tr>
        <tr>
            <th>权限资源：</th>
            <td>
                <aspx:CheckBoxList ID="actiontype" runat="server" RepeatColumns="5" Width="100%" RepeatDirection="Horizontal"></aspx:CheckBoxList>
            </td>
        </tr>
        <tr>
            <th></th>
            <td>
                <aspx:CheckBox ID="isenable" runat="server"  Text="启用"/>
                <aspx:CheckBox ID="isopen" runat="server"  Text="展开"/>
                <aspx:CheckBox ID="issystem" runat="server"  Text="系统默认"/>
            </td>
        </tr>
        <tr>
            <th></th>
            <td>
                <%--<asp:RequiredFieldValidator ControlToValidate="title" CssClass="line1px_2" runat="server" ErrorMessage="菜单名称不能为空！" />--%>
                <asp:Label ID="Label1" runat="server" Width="280"></asp:Label>
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