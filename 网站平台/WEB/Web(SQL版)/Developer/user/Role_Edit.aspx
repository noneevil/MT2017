<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Role_Edit.aspx.cs" Inherits="WebSite.Web.Role_Edit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>创建或修改角色</title>
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
                    ele: "#Name",
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
    <table border="0" align="center" cellpadding="4" cellspacing="0">
        <tr>
            <th width="70px">角色名称：</th>
            <td>
                <aspx:TextBox ID="Name" runat="server" CssClass="text" Width="200"></aspx:TextBox>
                <em class="Validform_checktip">*</em>
                <%--<asp:RequiredFieldValidator ControlToValidate="Name" CssClass="line1px_5" runat="server" BorderStyle="None" />--%>
            </td>
        </tr>
        <tr>
            <th>角色说明：</th>
            <td>
                <aspx:TextBox ID="Remark" runat="server" CssClass="text" Width="200"></aspx:TextBox>
            </td>
        </tr>
        <tr>
            <th>系统默认</th>
            <td>
                <aspx:RadioButtonList ID="issystem" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="1" Selected="True">是</asp:ListItem>
                    <asp:ListItem Value="0">否</asp:ListItem>
                </aspx:RadioButtonList>
            </td>
        </tr>
        <tr>
            <th></th>
            <td>
                <%--<asp:RequiredFieldValidator ControlToValidate="Name" Width="205" CssClass="line1px_2" runat="server" Text="角色名称不能为空!" />--%>
                <asp:Label ID="Label1" runat="server" Width="185"></asp:Label>
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