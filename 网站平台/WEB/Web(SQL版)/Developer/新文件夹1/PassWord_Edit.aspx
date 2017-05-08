<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PassWord_Edit.aspx.cs" Inherits="WebSite.Web.PassWord_Edit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>修改密码</title>
    <link href="images/css.css" rel="stylesheet" type="text/css" />
    <!--[if lte IE 7]><script src="Plugins/fontsicon/lte-ie7.js"></script><![endif]-->
    <script src="Plugins/mootools/mootools-core-1.4.5.js" type="text/javascript"></script>
    <script src="Plugins/jQuery/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="Plugins/jQuery-ui/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="Plugins/public.js" type="text/javascript"></script>
</head>
<body>
<form id="form1" runat="server">
    <div class="head"><i class="icon0-key"></i>修改密码</div>
    <table width="450" border="0" align="center" cellpadding="4" cellspacing="0" style="margin-top:30px;">
        <tr>
            <th width="100">
                登录名称：
            </th>
            <td>
                <b><%= Admin.UserName%></b>
            </td>
        </tr>
        <tr>
            <th>
                原始密码：
            </th>
            <td>
                <asp:TextBox ID="txtOldpwd" Width="200" TextMode="Password" runat="server" MaxLength="15" CssClass="text"></asp:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="txtOldpwd" CssClass="line1px_2" Width="80" runat="server" Text="不能为空!" />
            </td>
        </tr>
        <tr>
            <th>
                新密码：
            </th>
            <td>
                <asp:TextBox ID="txtNewpwd" Width="200" TextMode="Password" runat="server" MaxLength="15" CssClass="text"></asp:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="txtNewpwd" CssClass="line1px_2" Width="80" runat="server" Text="不能为空!" />
            </td>
        </tr>
        <tr>
            <th>
                确认新密码：
            </th>
            <td>
                <asp:TextBox ID="txtRepwd" Width="200" TextMode="Password" runat="server" MaxLength="15" CssClass="text"></asp:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="txtRepwd" CssClass="line1px_2" Width="80" runat="server" Text="不能为空!" />
            </td>
        </tr>
        <tr>
            <th></th>
            <td>
                <asp:CompareValidator runat="server" CssClass="line1px_2" ErrorMessage="两次密码输入不一致！" ControlToValidate="txtRepwd" Display="Static" ControlToCompare="txtNewpwd" Width="185"></asp:CompareValidator>
                <asp:Label ID="Label1" runat="server" Width="185"></asp:Label>
            </td>
        </tr>
        <tr>
            <th></th>
            <td>
                <asp:Button ID="Button1" hidefocus="hidefocus" runat="server" Text="保存" Width="80" onclick="btnSave_Click" />
                <input type="button" value="重置" style="width:80px;" />
            </td>
        </tr>
    </table>
</form>
</body>
</html>