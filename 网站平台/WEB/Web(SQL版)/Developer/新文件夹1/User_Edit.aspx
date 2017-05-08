<%@ Page Language="C#" AutoEventWireup="true" CodeFile="User_Edit.aspx.cs" Inherits="WebSite.Web.User_Edit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>创建或修改用户</title>
    <link href="images/css.css" rel="stylesheet" type="text/css" />
    <script src="Plugins/mootools/mootools-core-1.4.5.js" type="text/javascript"></script>
    <script src="Plugins/jQuery/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="Plugins/jQuery-ui/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="Plugins/public.js" type="text/javascript"></script>
</head>
<body scroll="no">
<form id="form1" runat="server">
    <table width="90%" border="0" align="center" cellpadding="4" cellspacing="0">
        <tr>
            <th>登录名称：</th>
            <td>
                <aspx:TextBox ID="UserName" runat="server" CssClass="text" Width="200"></aspx:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="UserName" CssClass="line1px_5" runat="server" BorderStyle="None" />
            </td>
        </tr>
        <tr>
            <th>登录密码：</th>
            <td>
                <aspx:TextBox ID="txtpwd" runat="server" CssClass="text" Width="200" TextMode="Password"></aspx:TextBox>
                <asp:RequiredFieldValidator ID="Req1" ControlToValidate="txtpwd" CssClass="line1px_5" runat="server" BorderStyle="None" />
            </td>
        </tr>
        <tr>
            <th>确认密码：</th>
            <td>
                <aspx:TextBox ID="PassWord" runat="server" CssClass="text"  Width="200" TextMode="Password"></aspx:TextBox>
                <asp:RequiredFieldValidator ID="Req2" ControlToValidate="PassWord" CssClass="line1px_5" runat="server" BorderStyle="None" />
            </td>
        </tr>
        <tr>
            <th>联系邮箱：</th>
            <td>
                <aspx:TextBox ID="Email" runat="server" CssClass="text"  Width="200"></aspx:TextBox>
            </td>
        </tr>
        <tr>
            <th>用户角色：</th>
            <td>
                <aspx:DropDownList ID="RoleID" runat="server" Width="212"></aspx:DropDownList>
                <asp:CompareValidator runat="server" ControlToValidate="RoleID" CssClass="line1px_5" BorderStyle="None" Type="Integer" Operator="GreaterThan" ValueToCompare="0"></asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <th>创建日期：</th>
            <td>
                <aspx:TextBox ID="JoinDate" runat="server" FormatString="{0:yyyy-MM-dd HH:mm:ss}" Enabled="false" CssClass="text" Width="200"></aspx:TextBox>
            </td>
        </tr>
        <tr>
            <th>用户状态：</th>
            <td>
                <aspx:CheckBox ID="Status" runat="server" Text="启用/停用" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:CompareValidator ID="Req3" runat="server" CssClass="line1px_2" ErrorMessage="两次密码输入不一致！" ControlToValidate="PassWord"  ControlToCompare="txtpwd"></asp:CompareValidator>
                <asp:Label ID="Label1" runat="server"></asp:Label>
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